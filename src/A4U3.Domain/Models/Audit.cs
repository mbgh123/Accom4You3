using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A4U3.Domain.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Url { get; set; }
        public string UserAgent { get; set; }
        public string UserHostAddress { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
