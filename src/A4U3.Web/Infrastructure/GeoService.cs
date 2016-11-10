using A4U3.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace A4U3.Web.Infrastructure
{
    public class GeoService : IGeoService
    {
        /// <summary>
        /// Here we're calling google geodata from the server. This may not be strictly
        /// allowed by google.
        /// </summary>
        public GeoLocation GetGeoPoint(string postcode)
        {
            decimal lat;
            decimal lng;

            string url = @"http://maps.googleapis.com/maps/api/geocode/json?address="
                            + postcode + @",uk&sensor=false";

            var request = HttpWebRequest.Create(url);

            request.Method = "GET";

            var response = request.GetResponse();
            using (var stream = response.GetResponseStream())
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = JObject.Parse(reader.ReadToEnd());
                    XDocument resultX = new XDocument(result);


                    //TODO An invalid postcode will return no results.
                    //Would be better to check the status field.
                    //Consider a geo library instead of this code.
                    var latRaw = result.SelectToken("results[0].geometry.location.lat");

                    if (latRaw != null)
                    {
                        lat = (decimal)latRaw;
                        lng = (decimal)result.SelectToken("results[0].geometry.location.lng");
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            //return DbGeography.PointFromText(string.Format("POINT({0} {1})", lng, lat), 4326);

            return new GeoLocation { Lat = lat, Long = lng };
        }
    }
}
