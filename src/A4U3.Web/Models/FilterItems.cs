using System;
using A4U3.Domain.Models;

namespace A4U3.Web.Models
{
    public class FilterItems
    {
        public Bedrooms Bedrooms { get; set; }
        public Decimal? RentMax { get; set; }
        public Decimal? RentMin { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}