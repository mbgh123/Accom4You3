using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A4U3.Domain.Models;
using A4U3.Domain.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace A4U3.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/features")]
    public class FeaturesController : Controller
    {
        private IRepository _rep;

        public FeaturesController(IRepository rep)
        {
            _rep = rep;

        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Feature
        [HttpPost]
        public IActionResult PostFeature([FromBody] Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //TODO again, no return codes, how handle failure 
            _rep.AddFeatureAndSave(feature);

            // The CreatedAtRoute method returns a 201 response, which is the standard response for an 
            // HTTP POST method that creates a new resource on the server.
            // CreateAtRoute also adds a Location header to the response.
            // The Location header specifies the URI of the newly created item.
            //  eg Header: Location: http://localhost/:53292/api/Properties/2003

            return Ok(feature);

            //return CreatedAtRoute("GetProperty", new { id = feature.PropertyId }, feature);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
