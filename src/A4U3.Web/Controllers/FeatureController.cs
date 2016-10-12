using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace A4U3.Web.Controllers
{
    [Authorize]            
    public class FeatureController : Controller
    {
        protected IRepository rep;

        public FeatureController(IRepository rep)
        {
            this.rep = rep;
        }
        
        #region Create
        public ActionResult Create(int id)
        {
            Feature feature = new Feature { PropertyId = id };

            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feature feature)
        {
            if (ModelState.IsValid)
            {
                rep.AddFeatureAndSave(feature);

                return RedirectToAction("Edit", "Rental", new { id = feature.PropertyId });
            }

            return RedirectToAction("Edit", "Rental", new { id = feature.PropertyId });
        } 
        #endregion

        #region Edit    
        [HttpGet]        
        public ActionResult Edit(int id)
        {
            Feature feature = rep.GetFeatureById(id);

           return View(feature);
        }

        [HttpPost]
        public ActionResult Edit(Feature feature)
        {
            if (ModelState.IsValid)
            {
                rep.UpdateFeatureAndSave(feature);

                return RedirectToAction("Edit", "Rental", new { id = feature.PropertyId });
            }
                        
            return View(feature);
        }
        #endregion

        #region Delete
        public ActionResult Delete(int id = 0)
        {
            Feature feature = rep.GetFeatureById(id);
            if (feature == null)
            {
                return NotFound();
            }

            return View(feature);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feature feature = rep.GetFeatureById(id);

            rep.RemoveFeatureAndSave(feature);

            return RedirectToAction("Edit", "Rental", new { id = feature.PropertyId });
        } 
        #endregion

        protected override void Dispose(bool disposing)
        {
            rep.Dispose();
            base.Dispose(disposing);
        }
    }
}