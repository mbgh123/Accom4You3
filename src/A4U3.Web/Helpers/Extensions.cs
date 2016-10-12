using System;
using System.ComponentModel;
using A4U3.Domain.Models;
using A4U3.Web.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace A4U3.Web.Helpers
{
    public static class Extensions
    {
        public static string AddBreaks(this string input) =>  input.Replace("\r\n", "<br />");

        public static string TrimStart(this string str, string sStartValue)
        {
            if (str.StartsWith(sStartValue))
            {
                str = str.Remove(0, sStartValue.Length);
            }
            return str;
        }
      
        /// <summary>
        /// Assuming the input contains sentences. ie chars separated by a space and
        /// perhaps ending with a full stop, this method shortens the sentence but
        /// tries to avoid truncating the final word.
        /// </summary>
        /// <returns></returns>
        public static string Shorten(this string input, int length, bool addDots = true)
        {
            // validate that the requested length is not longer than the input
            if (length >= input.Length)
            {
                return input;
            }

            if (length == 0)
            {
                return String.Empty;
            }

            // Look for an end point

            // If the char following the current end point is a space or period then
            // we have a good end point. If not, move back until we do.

            string temp = input;

            while (true)
            {
                //NB remember that substring start index is zero based.
                //So if I want to examine the character after char 3 I would
                //use Substring (2, 1) NOT Substring(2+1, 1)

                string ep = temp.Substring(length, 1);

                if (ep == " " || ep == ".")
                {
                    temp = temp.Substring(0, length);
                    break;
                }

                if (length > 0)
                {
                    length--;
                    temp = temp.Substring(0, length + 1);
                }
                else
                {
                    break;
                }
            }

            return addDots ? temp + "..." : temp;
        }

       

        /// <summary>
        /// Extension method to return Description attribute on the enum
        /// </summary>
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }

        public static bool IsFilterMatch(this Property prop, FilterItems fi)
        {
            //TODO-low, this is become harder to read than before. Not at all clear.

            if (fi == null)
            {
                return true; // there's no filter, so match
            }

            // Rent
            if (fi.RentMax != null)
            {
                if (fi.RentMax != 0 && prop.RatePCM > fi.RentMax.Value)
                {
                    return false;
                }
            }

            // Bedrooms
            if (fi.Bedrooms != Bedrooms.Any) // indicates any number of bedrooms will do
            {
                if (fi.Bedrooms == Bedrooms.OneOnly) // Must have 1 bedroom
                {
                    if (prop.Bedrooms != 1)
                    {
                        return false;
                    }
                } 
                else if (prop.Bedrooms < (int)fi.Bedrooms) // must have n or more bedrooms
                {
                    return false;
                }
            }
           

            //Other filter items tests go here.

            return true;
        }

        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return false;
        }

        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        /// <summary>
        /// Example of a HtmlHelper. Returns an anchor wrapped in an image.
        /// </summary>
        public static IHtmlContent AnchorImage(this IHtmlHelper helper, Picture pic, string group = "")
        {
            // TODO how get the base path within a static extension class?
            // can't use constructor injection for IAplicationEnvionment
            string root = ""; // appEnv.ApplicationBasePath + "/uploads/";

            TagBuilder a = new TagBuilder("a");

            a.Attributes.Add("rel", group);

            a.Attributes.Add("href", root + pic.UniqueName);

            TagBuilder img = new TagBuilder("img");
            
            img.Attributes.Add("class", "img-rounded");

            img.Attributes.Add("src", root + pic.ThumbName);
            img.Attributes.Add("title", pic.Description);


            a.InnerHtml.Append(img.ToString());

            return  a;
        }
    }
}