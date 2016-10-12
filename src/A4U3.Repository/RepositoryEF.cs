using A4U3.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4U3.Domain.Models;
using A4U3.EFContext;
using Microsoft.EntityFrameworkCore;

namespace A4U3.Repository
{
    /// <summary>
    /// Purpose: provide means to add/del/upate the entities
    /// Pattern: Should be no other reason to change.
    /// 
    /// This implmentation of IRepository  uses EF (There may come a time when
    /// another implmentation uses another datastore) As it uses a specific EF
    /// is there any need to loosely couple it to the EF? Probalby not.
    /// 
    /// Should retieval login go in here or the context class?
    /// 
    /// Not much Business login in this app. But if there was shouldn't it be separate
    /// from this repository class? YES: Single reason for change principle.
    /// 
    /// </summary>
    public class RepositoryEF : IRepository
    {
        private IApplicationDbContext _db;
        public IStaticData StaticData { get; set; }

        public RepositoryEF(IApplicationDbContext applicationDbContext, IStaticData staticData)
        {
            StaticData = staticData;

            // We're tied to a concrete context
            // We could use a context interface (must have SaveChanges, Entry methods etc)
            _db = applicationDbContext;
        }


        public void AddFeatureAndSave(Feature feat)
        {
            _db.Features.Add(feat);
            _db.SaveChanges();
        }

        public void AddPictureAndSave(Picture pic)
        {
            _db.Pictures.Add(pic);
            _db.SaveChanges();
        }

        public void AddPropertyAndSave(Property prop)
        {
            _db.Properties.Add(prop);
            _db.SaveChanges();
            // After SaveChanges the PropertyId is now populated. 
            
            // As property is passed by reference, the change to PropertyId
            // is available to the caller. No need to return property.
        }

        public void AddUserProfileAndSave(UserProfile up)
        {
            _db.UserProfiles.Add(up);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public Audit GetAuditById(int id)
        {
            return _db.Audit.FirstOrDefault(x => x.Id == id);
        }

        public List<Audit> GetAuditsAll()
        {
            return _db.Audit.OrderBy(a => a.DateTime).ToList();
        }

        /// <summary>
        /// Instead of returning all audit data and filtering in the web controller,
        /// filter here and return only qualifying data.
        /// Would would be better again to pass the criteria into the SQL DB via a sproc?
        /// Or is that effectively already happening? 
        /// </summary>
        public List<Audit> GetAuditsFiltered(bool excludeRobots, bool excludeMe, string searchTerm, string order)
        {
            //TODO determine the point of filtering. Here or in the DB?

            var robots = StaticData.GetRobotList();

            
            // Build filters
            Func<string, bool> IsRobot = (userAgent) =>
            {
                return robots.Any(robot => userAgent.ToLower().Contains(robot.ToLower()));
            };

            Func<string, string, bool> IsMe  = (url, userHostAddress) => {
                if (url.ToLower().Contains("localhost"))
                {
                    return true;
                }
                if (userHostAddress == null)
                {
                    return false;
                }

                return userHostAddress.Contains("62.31.191.80");
            };

            Func<string, bool> SimpleTest = (url) =>
            {
                return url != null;
            };

            // Get data and apply filters
            var result = _db.Audit.Where(x => searchTerm == null || x.UserAgent.ToLower().Contains(searchTerm))
                                 
                                //.Where(x => x.Url != null)        // This was passed into SQL query  
                                //.Where( x => SimpleTest(x.Url))   // This was not passed into SQL quesy
                                //                                  // Likewise, the filters below were not passsed into SQL query
                                  .Where(x => excludeRobots == false || !IsRobot(x.UserAgent))
                                  .Where(x => excludeMe == false || !IsMe(x.Url, x.UserHostAddress));


            // The Orderby IS passed into SQL despite the elements above that aren't!
            if (order == "asc")
            {
                return result.OrderBy(a => a.DateTime).ToList();
            }

            return result.OrderByDescending(a => a.DateTime).ToList();
        }

        public Feature GetFeatureById(int id)
        {
            return _db.Features.SingleOrDefault(x => x.FeatureId == id);
        }

        public Picture GetPictureById(int id)
        {
            return _db.Pictures.SingleOrDefault(x => x.PictureId == id);
        }

        public ICollection<Property> GetPropertiesAndChildren(bool enabledOnly = false)
        {
            var result = _db.Properties.Include(x => x.Features)
                                       .Include(x => x.Pictures);

            if (enabledOnly)
            {
                return result.Where(x => x.isEnabled).ToList();
            }
            else
            {
                return result.ToList();
            }
        }

        public Property GetPropertyAndChildrenById(int id)
        {
            return _db.Properties.Include(x => x.Features)
                      .Include(x => x.Pictures)
                      .FirstOrDefault(x => x.PropertyId == id);
        }

        public Property GetPropertyById(int id)
        {
            return _db.Properties.SingleOrDefault(x => x.PropertyId == id);

        }

        public UserProfile GetUserProfile(string username)
        {
            return _db.UserProfiles.FirstOrDefault(u => u.UserName.ToLower() == username);
        }

        public void Log(string url, string userAgent, string userHostAddress, string controller, string action)
        {
            Audit audit = new Audit() { DateTime = DateTime.Now };

            audit.Url = url;
            audit.UserAgent = userAgent;
            audit.UserHostAddress = userHostAddress;
            audit.Controller = controller;
            audit.Action = action;

            _db.Audit.Add(audit);
            _db.SaveChanges();
        }

        public void RemoveFeatureAndSave(Feature feat)
        {
            _db.Features.Remove(feat);
            _db.SaveChanges();

        }

        public void RemovePictureAndSave(Picture pic)
        {
            _db.Pictures.Remove(pic);
            _db.SaveChanges();
        }

        public void RemovePropertyAndSave(Property prop)
        {
            _db.Properties.Remove(prop);
            _db.SaveChanges();
        }

        public void UpdateFeatureAndSave(Feature feat)
        {
            _db.Entry(feat).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void UpdatePictureAndSave(Picture pic)
        {
            _db.Entry(pic).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void UpdatePropertyAndSave(Property prop)
        {
            _db.Entry(prop).State = EntityState.Modified;
            _db.SaveChanges();
        }

        
    }
}
