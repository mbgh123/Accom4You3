using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using A4U3.Domain.Models;
using A4U3.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace A4U3.Web.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/features")]
    [Authorize]
    public class FeaturesController : Controller
    {
        private IRepository _rep;

        public FeaturesController(IRepository rep)
        {
            _rep = rep;

        }

        [HttpPost]
        public IActionResult Post([FromBody] Feature feature)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //TODO again, no return codes, how handle failure 
            _rep.AddFeatureAndSave(feature);
            
            return Ok(feature);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            // In EF we have to retrieve the entity before it can be deleted !

            var feature = _rep.GetFeatureById(id);

            _rep.RemoveFeatureAndSave(feature);
        }
    }
}
