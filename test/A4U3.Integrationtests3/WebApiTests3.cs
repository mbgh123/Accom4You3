using A4U3.Domain.Models;
using A4U3.IntegrationTests3.Extensions;
using A4U3.IntegrationTests3.TestFixtures;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace A4U3.Integrationtests3
{
    /// <summary>
    /// The item under test is the Web Api functionality.
    /// This class is based on the previous A4U2.IntegrationTest[X] projects.
    /// But here we combine xunit testfixture setup with in memory database (see appsettings).
    /// 
    /// The tests rely on a fixed order of execution. That can't be guaranteed ???
    /// For example, the first test expects the database to be empty, the later tests populate the db.
    /// If the first test doesn't run first, it will find data in the db and fail.
    /// 
    /// Things to consider:
    /// xunit (since version 2) will run tests from different test classes in parallel.
    /// We are using asynch on the tests.
    /// 
    /// </summary>
    [Collection("Test Collection #1")]  // Classes in same collection are not run in parallel: https://xunit.github.io/docs/running-tests-in-parallel.html
    public class WebApiTests3 : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;

        public WebApiTests3(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async void Get_Properties_Returns_Empty()
        {
            // Arrange (see TestFixture for setup)
            // NB We're starting with an in memory database. So no data.


            // Act GET ALL
            var response = await _client.GetAsync("/api/Properties/");
            var getAllResult = await response.Content.ReadAsJsonAsync<List<Property>>();


            //Assert 
            getAllResult.Should().NotBeNull();
            getAllResult.Should().BeEmpty();
        }

        [Fact]
        public async void Add_Then_GetAll_Properties()
        {
            // Arrange (see TestFixture for setup)
            var originProp = new Property {
                    Description = "new property",
                    Address = "Address is a required field"
            };


            // Act ADD
            var postResponse = await _client.PostAsJsonAsync("/api/Properties/", originProp);
            var created = await postResponse.Content.ReadAsJsonAsync<Property>();

            // Act GET ALL
            var getResponse = await _client.GetAsync("/api/Properties/");
            var getAllResult = await getResponse.Content.ReadAsJsonAsync<List<Property>>();


            //Assert            
            postResponse.IsSuccessStatusCode.Should().BeTrue();
            getResponse.IsSuccessStatusCode.Should().BeTrue();

            created.PropertyId.Should().BePositive("A PropertyId should have been set");
            created.Description.Should().Equals(originProp.Description);

            getAllResult.Should().NotBeEmpty();
            var newProperty = getAllResult.Single(x => x.PropertyId == created.PropertyId);

            newProperty.Should().NotBeNull("The newly created property should be in the getAll result set");
            newProperty.Description.Should().Equals(originProp.Description);
        }

        [Fact]
        public async void Add_Then_Get_Specific_Property()
        {
            // Arrange (see TestFixture for setup)
            var originProp = new Property
            {
                Description = "new property",
                Address = "Address is a required field"
            };


            // Act ADD
            var postResponse = await _client.PostAsJsonAsync("/api/Properties/", originProp);
            var created = await postResponse.Content.ReadAsJsonAsync<Property>();

            // Act GET using Id
            var getResponse = await _client.GetAsync($"/api/Properties/{created.PropertyId}");
            Property getPropertyResult = await getResponse.Content.ReadAsJsonAsync<Property>();


            //Assert
            Assert.True(postResponse.IsSuccessStatusCode);
            Assert.True(getResponse.IsSuccessStatusCode);

            Assert.True(created.PropertyId > 0);    // Id should have been set
            Assert.Equal(originProp.Description, created.Description);

            Assert.NotNull(getPropertyResult);
            Assert.Equal(originProp.Description, getPropertyResult.Description);
        }

        [Fact]
        public async void Add_Update_Get_Specific_Property()
        {
            // Arrange (see TestFixture for setup)
            var prop = new Property
            {
                Description = "ORIGINAL VALUE",
                Address = "Address is a required field"
            };

            // Act ADD
            var postResponse = await _client.PostAsJsonAsync("/api/Properties/", prop);
            var created = await postResponse.Content.ReadAsJsonAsync<Property>();

            // Act UPDATE
            created.Description = "CHANGED";
            var putResponse = await _client.PutAsJsonAsync($"/api/Properties/{created.PropertyId}", created);

            //TODO Put does not return a resource at the moment (in the case a Property )
            //But there is opinion on that net that it should.
            //var updated = await putResponse.Content.ReadAsJsonAsync<Property>();

            // Act GET using Id
            var getResponse = await _client.GetAsync($"/api/Properties/{created.PropertyId}");
            Property getPropertyResult = await getResponse.Content.ReadAsJsonAsync<Property>();


            //Assert
            Assert.True(postResponse.IsSuccessStatusCode);
            Assert.True(getResponse.IsSuccessStatusCode);
            Assert.True(putResponse.IsSuccessStatusCode);

            //Assert.Equal("CHANGED", updated.Description);   
            Assert.Equal("CHANGED", getPropertyResult.Description);
        }

        [Fact]
        public async void Add_Delete_Get_Specific_Property()
        {
            // Arrange (see TestFixture for setup)
            var prop = new Property
            {
                Description = "new property",
                Address = "Address is a required field"
            };


            // Act ADD
            var postResponse = await _client.PostAsJsonAsync("/api/Properties/", prop);
            var created = await postResponse.Content.ReadAsJsonAsync<Property>();

            // Act DELETE it
            var deleteResponse = await _client.DeleteAsync($"/api/Properties/{created.PropertyId}");
            // no resource is returned from delete operation
            var deletedResource = await deleteResponse.Content.ReadAsJsonAsync<Property>();

            // Act - GET it. Should be null
            var getResponse = await _client.GetAsync($"/api/Properties/{created.PropertyId}");
            Property getPropertyResult = await getResponse.Content.ReadAsJsonAsync<Property>();


            //Assert
            Assert.True(postResponse.IsSuccessStatusCode);
            Assert.True(deleteResponse.IsSuccessStatusCode);

            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
            Assert.Null(getPropertyResult);
        }
    }
}

