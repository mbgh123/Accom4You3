using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using A4U3.IntegrationTests3.Extensions;
using A4U3.Repository;
using Microsoft.Extensions.DependencyInjection;
using A4U3.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace A4U3.IntegrationTests3.TestFixtures
{
    public class TestServerFixture : IDisposable
    {
        public TestServer TestServer { get; }
        public HttpClient Client { get; }

        public TestServerFixture()
        {
            // We know we can reuse the web site's startup file config. But we want an in memory
            // database. So we'll copy the web site's startup into this project and modify it.
            // (I'd prefer to use the web's and just override the db config, but how?)
            //
            // Answer: Move the assignment of sqlDB/inMemory from the startup to the
            // OnConfiguring on the DbContext. Supply OnConfiguring with options set from
            // a local appsettings file.
            // Start a local SQL Server container for tests using Docker (mcr.microsoft.com/mssql/server:2019-latest)
            // Requires Docker to be available on the host running tests. The container will be started with a short
            // lifetime for each test run.
            var dockerImage = "mcr.microsoft.com/mssql/server:2019-latest";
            var saPassword = "Your_strong_P@ssw0rd";
            var namedContainer = "a4u3_test_sql";
            var containerName = $"{namedContainer}_{Guid.NewGuid():N}";

            // If a named container 'a4u3_test_sql' already exists and is running,
            // prefer using it instead of launching a new container. This prevents
            // port conflicts and uses containers the developer may have started
            // manually with a different lifetime.
            try
            {
                var psInfo = new ProcessStartInfo("docker", $"ps --filter name={namedContainer} --format \"{{{{.Names}}}}\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                var psProc = Process.Start(psInfo);
                if (psProc != null)
                {
                    var outText = psProc.StandardOutput.ReadToEnd().Trim();
                    psProc.WaitForExit(2000);
                    if (!string.IsNullOrEmpty(outText) && outText.Split('\n').Any(n => n.Trim().Equals(namedContainer, StringComparison.OrdinalIgnoreCase)))
                    {
                        // Use the existing named container
                        containerName = namedContainer;
                    }
                }
            }
            catch (System.Exception)
            {
                // Ignore failures querying docker; we'll attempt to start a container below.
            }

            // Launch container if we didn't pick an existing named container
            var startInfo = new ProcessStartInfo("docker", $"run -e \"ACCEPT_EULA=Y\" -e \"SA_PASSWORD={saPassword}\" -p 14333:1433 --name {containerName} -d {dockerImage}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                // Only attempt to start if we selected a generated container name
                if (!containerName.Equals(namedContainer, StringComparison.OrdinalIgnoreCase))
                {
                    var proc = Process.Start(startInfo);
                    if (proc != null)
                    {
                        proc.WaitForExit(10000);
                        // If docker run failed quickly, capture the error for diagnostics
                        if (proc.ExitCode != 0)
                        {
                            var err = proc.StandardError.ReadToEnd();
                            // swallow and continue; we'll attempt to connect to the mapped port
                        }
                        Thread.Sleep(5000); // give SQL Server time to initialize
                    }
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                // Docker CLI not available on PATH for this process. Assume the
                // test environment has a SQL Server container already running
                // and accessible on localhost:14333 (e.g., a4u3_test_sql).
                // Fall back to using the default mapped port rather than
                // failing the fixture. This makes the tests resilient when
                // developers start the container manually.
            }

            // Create a fresh database for this test run to ensure isolation.
            var dbName = $"a4u3_test_db_{Guid.NewGuid():N}";

            // Build a master connection to create the new database if needed
            var masterConnBuilder = new SqlConnectionStringBuilder
            {
                DataSource = "127.0.0.1,14333",
                InitialCatalog = "master",
                UserID = "sa",
                Password = saPassword,
                TrustServerCertificate = true,
                Encrypt = false
            };

            try
            {
                using var masterConn = new SqlConnection(masterConnBuilder.ConnectionString);
                masterConn.Open();
                using var cmd = masterConn.CreateCommand();
                cmd.CommandText = $"IF DB_ID(N'{dbName}') IS NULL CREATE DATABASE [{dbName}];";
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                // Ignore and attempt to continue; later DB operations will surface errors if DB is not available.
            }

            // Build connection string to the local Docker SQL Server using the new database
            var connectionString = new SqlConnectionStringBuilder
            {
                DataSource = "127.0.0.1,14333",
                InitialCatalog = dbName,
                UserID = "sa",
                Password = saPassword,
                TrustServerCertificate = true,
                Encrypt = false
            }.ConnectionString;

            // Export connection string to env var so web startup can pick it up if it reads config from environment
            Environment.SetEnvironmentVariable("TEST_SQL_CONNECTION", connectionString);
            // Also set the hierarchical configuration key so ASP.NET Configuration binder
            // will find it as ConfigOptions:ConnectionString when building IConfiguration.
            Environment.SetEnvironmentVariable("ConfigOptions__ConnectionString", connectionString);

            // Wait for SQL Server to accept connections. SQL Server in a container
            // can take several seconds to become available. Poll with a short
            // timeout and fail early if it never becomes ready.
            const int maxAttempts = 30; // ~60s total
            var attempt = 0;
            var dbReady = false;
            while (attempt < maxAttempts)
            {
                try
                {
                    using var testConn = new SqlConnection(connectionString);
                    testConn.Open();
                    testConn.Close();
                    dbReady = true;
                    break;
                }
                catch (Exception)
                {
                    attempt++;
                    Thread.Sleep(2000);
                }
            }

            if (!dbReady)
            {
                throw new InvalidOperationException($"SQL Server did not become available after {maxAttempts * 2} seconds. Check that the container is running and accepting connections on the mapped port.");
            }

            TestServer = new TestServer(new WebHostBuilder().UseStartup<A4U3.Web.Startup>());

            // Ensure database schema exists and apply migrations/seeding for tests
            try
            {
                using (var scope = TestServer.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var db = services.GetRequiredService<A4U3.EFContext.ApplicationDbContext>();
                    // Use Migrate to apply any pending migrations. If migrations are not present,
                    // EnsureCreated will create the schema based on the model.
                    try
                    {
                        db.Database.Migrate();
                    }
                    catch (System.InvalidOperationException)
                    {
                        // Migrations not available; fall back to EnsureCreated
                        db.Database.EnsureCreated();
                    }

                    // Do not seed data here. Tests should explicitly create required data
                    // so their expectations are deterministic. EnsureCreated/Migrate above
                    // guarantees the schema exists.
                }
            }
            catch (Exception ex)
            {
                // If DB initialization fails, surface a helpful exception to the test runner.
                throw new InvalidOperationException("Failed to initialize test database: " + ex.Message, ex);
            }

            Client = TestServer.CreateClient().AcceptJson();
        }

        public void Dispose()
        {
            TestServer.Dispose();
            Client.Dispose();
        }
    }
}
