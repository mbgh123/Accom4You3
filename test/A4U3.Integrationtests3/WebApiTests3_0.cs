﻿using A4U3.Domain.Models;
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
    /// This class is based on the previous A4U3.IntegrationTest[X] projects.
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
    [Collection("Test Collection #2")]  // Classes in same collection are not run in parallel: https://xunit.github.io/docs/running-tests-in-parallel.html
    public class WebApiTests3_0 : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient _client;

        public WebApiTests3_0(TestServerFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public void Get_Properties_Returns_Empty()
        {
            // Arrange (see TestFixture for setup)
            // NB We're starting with an in memory database. So no data.


            // Act GET ALL
            var response = _client.GetAsync("/api/Properties/").Result;
            var getAllResult =  response.Content.ReadAsJsonAsync<List<Property>>().Result;
                       

            //Assert 
            getAllResult.Should().NotBeNull();
            getAllResult.Should().BeEmpty();
        }

        //[Fact]
        //public void Add_Update_Get_Specific_Property()
        //{
        //    // Arrange (see TestFixture for setup)
        //    var prop = new Property
        //    {
        //        Description = "ORIGINAL VALUE",
        //        Address = "Address is a required field"
        //    };

        //    // Act ADD
        //    var postResponse = _client.PostAsJsonAsync("/api/Properties/", prop).Result;
        //    var created = postResponse.Content.ReadAsJsonAsync<Property>().Result;

        //    // Act UPDATE
        //    created.Description = "CHANGED";
        //    var putResponse = _client.PutAsJsonAsync($"/api/Properties/{created.PropertyId}", created).Result;

        //    //TODO Put does not return a resource at the moment (in the case a Property )
        //    //But there is opinion on that net that it should.
        //    //var updated = await putResponse.Content.ReadAsJsonAsync<Property>();

        //    // Act GET using Id
        //    var getResponse = _client.GetAsync($"/api/Properties/{created.PropertyId}").Result;
        //    Property getPropertyResult = getResponse.Content.ReadAsJsonAsync<Property>().Result;


        //    //Assert
        //    Assert.True(postResponse.IsSuccessStatusCode);
        //    Assert.True(getResponse.IsSuccessStatusCode);
        //    Assert.True(putResponse.IsSuccessStatusCode);

        //    //Assert.Equal("CHANGED", updated.Description);   
        //    Assert.Equal("CHANGED", getPropertyResult.Description);
        //}

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
    }
}

