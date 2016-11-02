using System;
using System.Collections.Generic;
//using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using A4U3.Web.Helpers;
using A4U3.Web.Infrastructure;
using A4U3.Web.Models;
using A4U3.Web.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http.Features;
using System.Xml.Linq;

namespace A4U3.Web.Controllers
{
    [TypeFilter(typeof(MyAudit))]
    public class RentalController : Controller
    {
        protected IRepository _rep;
        protected IGeoService _geoService;

        public RentalController(IRepository rep, IGeoService geoService)
        {
            _rep = rep;
            _geoService = geoService;
        }

        #region List and Details
        /// <summary>
        /// Public view of properties to rent.
        /// </summary>
        public ActionResult Index(FilterItems fi)
        {
            HandleFilterPersistence(ref fi);

            var props = _rep.GetPropertiesAndChildren(enabledOnly: true).AsEnumerable();

            // NB: For high volume data I should I pass the filter to the repository 
            // rather than getting all data and filtering here.
            props = props.Where(x => x.IsFilterMatch(fi));


            // sorting !
            if (fi.SortOrder != SortOrder.Default)
            {
                props = Sort(props, fi.SortOrder);
            }

            var wrapper = new WrapperVM { Properties = props, Filter = fi };

            return View(wrapper); // return an entire page
        }

        /// <summary>
        /// Based on Index action, but checks for Ajax call and returns a partial view when required.
        /// </summary>
        public ActionResult Index2(FilterItems fi)
        {
            HandleFilterPersistence(ref fi);

            var test = Request.IsAjaxRequest();

            var props = _rep.GetPropertiesAndChildren(enabledOnly: true).AsEnumerable();

            // NB: For high volume data I should I pass the filter to the repository 
            // rather than getting all data and filtering here.
            props = props.Where(x => x.IsFilterMatch(fi));

            // sorting !

            if (fi.SortOrder != SortOrder.Default)
            {
                props = Sort(props, fi.SortOrder);
            }

            if (Request.IsAjaxRequest() == false) // initial request, return entire page
            {
                var wrapper = new WrapperVM { Properties = props, Filter = fi };

                return View(wrapper); // return an entire page
            }

            // We've received an ajax call, we're refreshing the page
            // return just the appropriate partial view

            Thread.Sleep(1000);               // to test ajax-progress indicator

            return PartialView("_Details", props); // return to ajax
        }

        private IEnumerable<Property> Sort(IEnumerable<Property> props, SortOrder so)
        {
            if (so == SortOrder.PriceHighest)
            {
                return props.OrderByDescending(x => x.RatePCM);
            }
            if (so == SortOrder.PriceLowest)
            {
                return props.OrderBy(x => x.RatePCM);
            }

            return props;
        }


        public ActionResult Search(Bedrooms bedrooms)
        {
            IEnumerable<Property> temp = _rep.GetPropertiesAndChildren(enabledOnly: true);

            FilterItems fi = new FilterItems { Bedrooms = bedrooms };

            var filtered = temp.Where(x => x.IsFilterMatch(fi));

            if (!filtered.Any())
            {
                return Content("Sorry, no matching properties");
            }

            return PartialView("_Details", filtered);
        }

        /// <summary>
        /// CMS view of properties to rent
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult List()
        {
            var result = _rep.GetPropertiesAndChildren(enabledOnly: false)
                           .Select(x => x.ToPropertyVM())
                           .AsEnumerable();

            return View(result);
        }

        public ActionResult DetailsCMS(int id = 0)
        {
            Property property = _rep.GetPropertyAndChildrenById(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }


        public ActionResult DetailsPublic(int id)
        {
            Property prop = _rep.GetPropertyAndChildrenById(id);

            return View(prop);
        }

        #endregion

        #region Create
        [Authorize]
        public ActionResult Create()
        {
            var prop = new Property
            {
                isEnabled = false,
                Address = "",
                Description = "",
                Features = new List<Feature>() {
                    new Feature { Description = "feature 1" },
                    new Feature { Description = "feature 2" } }
            };

            return View(prop);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Property property)
        {
            if (ModelState.IsValid)
            {
                SetGeoPoint(property);

                _rep.AddPropertyAndSave(property);

                return RedirectToAction("List");
            }

            return View(property);
        }
        #endregion

        #region Edit
        [Authorize]
        public ActionResult Edit(int id = 0)
        {
            Property property = _rep.GetPropertyAndChildrenById(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(Property property)
        {
            if (ModelState.IsValid)
            {
                SetGeoPoint(property);

                _rep.UpdatePropertyAndSave(property);

                return RedirectToAction("List");
            }

            return View(property);
        }
        #endregion

        #region Delete
        [Authorize]
        public ActionResult Delete(int id = 0)
        {
            Property property = _rep.GetPropertyAndChildrenById(id);

            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            // Features table is foreign keyed to property table.
            // It has a ON DELETE CASCADE so when Property is deleted, so are related Features.

            Property property = _rep.GetPropertyById(id);
            _rep.RemovePropertyAndSave(property);

            return RedirectToAction("List");
        }
        #endregion

        public ActionResult Map(FilterItems fi)
        {
            HandleFilterPersistence(ref fi);

            var temp = _rep.GetPropertiesAndChildren(enabledOnly: true);

            var result = temp.Where(x => x.Latitude != null && x.IsFilterMatch(fi))
                             .Select(x => new
                             {
                                 Id = x.PropertyId,
                                 Address = x.Address,
                                 Lng = x.Longitude,
                                 Lat = x.Latitude,
                                 Bedrooms = x.Bedrooms,
                                 Rate = x.RatePCM,
                                 Description = x.Description.Shorten(200),
                                 Furnishing = x.Furnishing.Description()
                             });

            var json = new MapJson
            {
                DataToMap = JsonConvert.SerializeObject(result),
                Filter = fi,
                PropertyCount = result.Count()
            };

            return View(json);
        }

        /// <summary>
        /// Check if  on a GET we have filter items saved in Session to carry over
        /// </summary>
        /// <param name="fi"></param>
        void HandleFilterPersistence(ref FilterItems fi)
        {
            if (Request.Method.ToLower() == "get")
            {
                var temp = HttpContext.Session.GetObjectFromJson<FilterItems>("Filter");
                if (temp != null)
                {
                    fi = temp;
                }
            }
            else // post
            {
                HttpContext.Session.SetObjectAsJson("Filter", fi);
            }
        }

        /// <summary>
        /// This method was used by an document ready ajax call in the map page.
        /// But that method is no longer in use. Instead, we embed the json data in the page.
        /// </summary>
        ActionResult PropertiesToMap(FilterItems fi)
        {
            var temp = _rep.GetPropertiesAndChildren(enabledOnly: true);

            var result = temp.Where(x => x.Latitude != null && x.IsFilterMatch(fi))
                             .Select(x => new
                             {
                                 Id = x.PropertyId
                                    ,
                                 Address = x.Address
                                    ,
                                 Lng = x.Longitude
                                    ,
                                 Lat = x.Latitude
                                    ,
                                 Bedrooms = x.Bedrooms
                                    ,
                                 Rate = x.RatePCM
                                    ,
                                 Description = x.Description.Shorten(170)
                             });

            //return Json(result, JsonRequestBehavior.AllowGet);  // returning to ajax call
            return Json(result);
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

        protected override void Dispose(bool disposing)
        {
            _rep.Dispose();
            base.Dispose(disposing);
        }
    }
}