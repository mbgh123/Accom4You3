using System;
using System.Collections.Generic;
using System.Linq;
//using System.Web;
using A4U3.Web.Models.ViewModel;
using A4U3.Domain.Models;
using A4U3.Web.Models;

namespace A4U3.Web.Helpers
{
    public static class ModelMapping
    {
        public static PropertyVM ToPropertyVM(this Property prop, int shortDescriptionLength = 60)
        {
            if (prop == null)
            {
                return null;
            }

            var result = new PropertyVM (shortDescriptionLength) {
                isEnabled = prop.isEnabled,

                PropertyId = prop.PropertyId,
                Address = prop.Address,
                PostCode = prop.PostCode,
                Furnishing = prop.Furnishing.Description(),
                Description = prop.Description,
                Features = prop.Features,
                Pictures = prop.Pictures,
                RatePCM = prop.RatePCM,
                Bedrooms = prop.Bedrooms
            };

            if (prop.Latitude == null)
            {
                result.Location = null;
            }
            else
            {
                result.Location = new GeoLocation { Lat = Decimal.Parse(prop.Latitude),
                                                    Long = Decimal.Parse(prop.Longitude) };
            }

            return result;
        }

        public static Property ToProperty(this PropertyVM propVM)
        {
            if (propVM == null)
            {
                return null;
            }

            var result = new Property()
            {
                isEnabled = propVM.isEnabled,

                PropertyId = propVM.PropertyId,
                Address = propVM.Address,
                PostCode = propVM.PostCode,

                //TODO Having added a space to "Part Furnished" so it looks nice on the UI.
                //converting it back to the enum is a pain.
                Furnishing = (Furnishing)Enum.Parse(typeof(Furnishing),  propVM.Furnishing.Replace(" ", String.Empty)),

                Description = propVM.Description,
                Features = propVM.Features,
                Pictures = propVM.Pictures,
                RatePCM = propVM.RatePCM,
                Bedrooms = propVM.Bedrooms
            };

            if (propVM.Location == null)
            {
                result.Latitude = "";
                result.Longitude = "";
            }
            else
            {
                result.Latitude = propVM.Location.Lat.ToString();
                result.Longitude = propVM.Location.Long.ToString();
            }

            return result;
        }

        /// <summary>
        /// We need the robots list to determine if its a robot
        /// </summary>
        public static AuditVM ToAuditVM(this Audit audit, IEnumerable<string> robots )
        {
            if (audit == null)
            {
                return null;
            }

            return new AuditVM(audit.UserAgent, robots)
            {
                Id = audit.Id,
                DateTimeRaw = audit.DateTime,
                Url = audit.Url,
                //UserAgent = audit.UserAgent,
                UserHostAddress = audit.UserHostAddress
            };
        }
    }
}