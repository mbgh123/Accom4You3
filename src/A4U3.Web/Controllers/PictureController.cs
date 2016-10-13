using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace A4U3.Web.Controllers
{
    [Authorize]                    
    public class PictureController : Controller
    {
        protected IRepository rep;
        private IHostingEnvironment env;

        public PictureController(IRepository rep, IHostingEnvironment appEnvironment)
        {
            this.rep = rep;
            this.env = appEnvironment;
        }

        #region Create
        public ActionResult Create(int id)
        {
            ViewBag.Comment = "";
            
            Picture picture = new Picture { PropertyId = id };

            return View(picture);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Picture picture, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Comment = "Please specify a file to upload";
                return View(picture);
            }


            // NB for file.FileName IE returns the path AND filename, whereas Firefox returns just the filname.
            // Need Path.GetFileName() to discard possible user path info.
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            picture.OriginalName = Path.GetFileName(fileName);

            string guid = Guid.NewGuid().ToString();
            picture.UniqueName = string.Format("{0}_{2}_{1}", picture.PropertyId, picture.OriginalName, guid);

            var basePath = env.ContentRootPath;

            // TODO: Are we on Azure? must be a better way of detecting this
            if (basePath.StartsWith("D:\\home\\site\\")) 
            {
                // We have probably have D:\\home\\site\\approot\\src\\A4U3.Web
                // drop the approot portion
                basePath = "D:\\home\\site\\";
            }

            basePath = Path.Combine(basePath, "wwwroot");
            var uploadPath = Path.Combine(basePath, "uploads");

            var path = Path.Combine(uploadPath, picture.UniqueName);
            
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }


            // Create thumbnail image
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(300, 300, () => false, IntPtr.Zero);
            string thumbName = string.Format("{0}_{2}_thumb_{1}", picture.PropertyId, picture.OriginalName, guid);
            picture.ThumbName = thumbName;
            thumbName = Path.Combine(uploadPath, thumbName);
            thumb.Save(thumbName);


            if (ModelState.IsValid)
            {
                rep.AddPictureAndSave(picture);
                
                return RedirectToAction("Edit", "Rental", new { id = picture.PropertyId });
            }

            return RedirectToAction("Edit", "Rental", new { id = picture.PropertyId });
        } 
        #endregion

        #region Edit
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Picture picture = rep.GetPictureById(id);

            return View(picture);
        }

        [HttpPost]
        public ActionResult Edit(Picture picture)
        {
            // Not all the picture fields are populated by the form (we didn't shown UniqueName for example)
            // We are just interested in the the Description

            string desc = picture.Description;
            picture = rep.GetPictureById(picture.PictureId);
            picture.Description = desc;

            if (ModelState.IsValid)
            {
                rep.UpdatePictureAndSave(picture);

                return RedirectToAction("Edit", "Rental", new { id = picture.PropertyId });
            }
            
            return View(picture);
        }
        #endregion

        #region Delete
        private void RemovePictureFile(Picture pic)
        {
            DeleteFile(pic.UniqueName);
            DeleteFile(pic.ThumbName);
        }

        private void DeleteFile(string name)
        {
            if (name == null)
            {
                return;
            }

            var rootPath = env.ContentRootPath;
            
            string path = Path.Combine(rootPath, "wwwroot");
            path = Path.Combine(path, "uploads");
            path = Path.Combine(path, name);

            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.IO.File.Delete(path);
                }
                catch (Exception)
                {
                    // TODO-high  I once got a "can't delete file, in use by another process" 
                    // Catch all for now
                }
            }
        }

        public ActionResult Delete(int id = 0)
        {
            Picture pic = rep.GetPictureById(id);
            if (pic == null)
            {
                return NotFound();
            }

            return View(pic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture pic = rep.GetPictureById(id);

            RemovePictureFile(pic);

            rep.RemovePictureAndSave(pic);

            return RedirectToAction("Edit", "Rental", new { id = pic.PropertyId });
        }
                
        #endregion

        protected override void Dispose(bool disposing)
        {
            rep.Dispose();
            base.Dispose(disposing);
        }
    }
}
