using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace A4U3.TestTools
{
    public static class Utility
    {
        public static void PopulateDB(IRepository rep)
        {
            // NB when adding data to EF, don't specify the keys !
            for (int i = 0; i < 10; i++)
            {
                var prop = CreateProperty(i);
                rep.AddPropertyAndSave(prop);
            }
        }

        /// <summary>
        ///  NB we are not using propertyId to set the propertyId.
        ///  That is done by EF.
        ///  But it is used in the address field
        /// <returns></returns>
        static Property CreateProperty(int number)
        {
            var features = new List<Feature>
            {
                CreateFeature(featureId: 1),
                CreateFeature(featureId: 2)
            };

            var pictures = new List<Picture>
            {
                CreatePicture(pictureId: 1)
            };

            return new Property
            {
                PropertyId = 0, //propertyId,
                Address = $"Property {number} Address",
                PostCode = "NE34 6RN",
                Description = "Description",
                Bedrooms = 2,
                RatePCM = 500,
                isEnabled = true,
                NextAvailability = null,
                Latitude = null,
                Longitude = null,
                Furnishing = Furnishing.Furnished,
                Pictures = pictures,
                Features = features
            };
        }

        static Feature CreateFeature(int featureId) => new Feature
        {
            Description = $"Feature {featureId} ",
            FeatureId = 0, //featureId,
            PropertyId = 0 //propertyId
        };

        static Picture CreatePicture(int pictureId) => new Picture
        {
            Description = $"Picture {pictureId}",
            PictureId = 0, //pictureId,
            PropertyId = 0, //propertyId,
            OriginalName = $"pic{pictureId}.jpg",
            ThumbName = "",
            UniqueName = ""
        };

        static public string ReturnRoot()
        {
            //HACK Directory.GetCurrentDirectory() returns 
            // "C:\\Projects\\A4U3B\\test\\A4U3.IntegrationTest\\bin\\Debug\\Net452"
            //
            // The apsettings.json lives several directory levels below.
            //

            var path = System.IO.Directory.GetCurrentDirectory();

            if (path.Contains("\\bin\\"))
            {
                path = System.IO.Directory.GetParent(path).ToString();
                path = System.IO.Directory.GetParent(path).ToString();
                path = System.IO.Directory.GetParent(path).ToString();
            }            

            return path;
        }

    }
    
}
