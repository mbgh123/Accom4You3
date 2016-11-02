using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using A4U3.Domain.Models;
using A4U3.EFContext;
using A4U3.Domain.Interfaces;
using A4U3.Web.Helpers;
using A4U3.Web.Models.ViewModel;
using System.Threading;
using System;

namespace A4U3.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Properties")]
    public class PropertiesController : Controller
    {
        private IRepository _rep;
        protected IGeoService _geoService;


        public PropertiesController (IRepository rep, IGeoService geoService)
        {
            _rep = rep;
            _geoService = geoService;
        }

        // GET: api/Properties
        [HttpGet]
        public IEnumerable<PropertyVM> GetProperties()
        {
            Thread.Sleep(2000); 

            var result = _rep.GetPropertiesAndChildren()
                             .Select(x => x.ToPropertyVM(shortDescriptionLength: 200));

            return result;
        }

        // GET: api/Properties/5
        [HttpGet("{id}", Name = "GetProperty")]
        public IActionResult GetProperty([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PropertyVM property = _rep.GetPropertyAndChildrenById(id)
                                      .ToPropertyVM();

            if (property == null)
            {
                return NotFound();
            }

            return Ok(property);
        }

        // POST: api/Properties
        [HttpPost]
        public IActionResult PostProperty([FromBody] Property property)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SetGeoPoint(property);

            //TODO again, no return codes, how handle failure 
            _rep.AddPropertyAndSave(property);
         
            // The CreatedAtRoute method returns a 201 response, which is the standard response for an 
            // HTTP POST method that creates a new resource on the server.
            // CreateAtRoute also adds a Location header to the response.
            // The Location header specifies the URI of the newly created item.
            //  eg Header: Location: http://localhost/:53292/api/Properties/2003

            return CreatedAtRoute("GetProperty", new { id = property.PropertyId }, property);
        }

        // This is an UPDATE operation
        // PUT: api/Properties/5
        [HttpPut("{id}")]
        public IActionResult PutProperty(int id, [FromBody] PropertyVM propertyVM)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != propertyVM.PropertyId)
            {
                return BadRequest();
            }

            var property = propertyVM.ToProperty();

            SetGeoPoint(property);


            //TODO There is no status returned from my Update. What if it fails? Add return codes?
            _rep.UpdatePropertyAndSave(property);

            //TODO I'm not returning the Property, but should I?
            //see http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api
            //return new StatusCodeResult(StatusCodes.Status204NoContent);

            return Ok(propertyVM);
        }

        // DELETE: api/Properties/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProperty(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Property property = _rep.GetPropertyById(id);

            if (property == null)
            {
                return NotFound();
            }

            _rep.RemovePropertyAndSave(property);

            return Ok(property);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _rep.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Sets Lat and Long on property using PostCode and google apis
        /// </summary>
        private void SetGeoPoint(Property property)
        {
            if (string.IsNullOrEmpty(property.PostCode))
            {
                property.Latitude = null;
                property.Longitude = null;
                return;
            }

            var geo = _geoService.GetGeoPoint(property.PostCode);

            if (geo == null)    // invalid postcode, no location found
            {
                property.Latitude = null;
                property.Longitude = null;
                return;
            }

            property.Latitude = geo.Lat.ToString();
            property.Longitude = geo.Long.ToString();

        }
    }
}