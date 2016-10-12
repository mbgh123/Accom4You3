using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using A4U3.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;


namespace A4U3.Web.Helpers
{
    public static class Utility
    {
        #region appSettings

        //TODO is show shearch used?
        //public static bool ShowSearch
        //{
        //    get
        //    {
        //        bool result = false;
        //        Boolean.TryParse(ConfigurationManager.AppSettings["ShowSearch"], out result);

        //        return result;
        //    }
        //} 
        #endregion

        

        public static IEnumerable<SelectListItem> FurnishingSelectList()
        {
            IEnumerable<Furnishing> values = Enum.GetValues(typeof(Furnishing)).Cast<Furnishing>();

            IEnumerable<SelectListItem> temp = 
                values.Select(t => new SelectListItem { Text = t.Description(), Value = t.ToString() });
            
            return temp;
        }

        public static IEnumerable<SelectListItem> BedroomsSelectList(Bedrooms bedroomsSelected)
        {
            IEnumerable<Bedrooms> values = Enum.GetValues(typeof(Bedrooms)).Cast<Bedrooms>();

            IEnumerable<SelectListItem> temp = 
                values.Select(t => new SelectListItem
                {    Text = t.Description()
                    ,Value = t.ToString()
                    ,Selected = (t == bedroomsSelected)
                });

            return temp;
        }

        public static IEnumerable<SelectListItem> SortOrderSelectList(SortOrder sortorderSelected)
        {
            IEnumerable<SortOrder> values = Enum.GetValues(typeof(SortOrder)).Cast<SortOrder>();

            IEnumerable<SelectListItem> temp =
                values.Select(t => new SelectListItem
                {
                    Text = t.Description(),
                    Value = t.ToString(),
                    Selected = (t == sortorderSelected)
                });

            return temp;
        }

        public static IEnumerable<SelectListItem> SortOrderOptions()
        {
            //yield return new SelectListItem { Value = "asc", Text = "Ascending", Selected = (order == "asc") };
            //yield return new SelectListItem { Value = "desc", Text = "Decending", Selected = (order == "desc") };

            yield return new SelectListItem { Value = "asc", Text = "Ascending" };
            yield return new SelectListItem { Value = "desc", Text = "Decending" };
        }
    }
}