using System.Collections.Generic;
using A4U3.Domain.Models;

namespace A4U3.Web.Models.ViewModel
{
    public class WrapperVM
    {
        public IEnumerable<Property> Properties { get; set; }
        public FilterItems Filter { get; set; }
    }
}