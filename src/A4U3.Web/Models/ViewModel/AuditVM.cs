using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
//using System.Web;
using A4U3.Web.Helpers;
using System.Collections.Generic;

namespace A4U3.Web.Models.ViewModel

{
    public class AuditVM
    {
        /// <summary>
        /// We need the UserAgent and robots list up front to determine if its a robot
        /// </summary>
        public AuditVM(string userAgent, IEnumerable<string> robots)
        {
            UserAgent = userAgent;
            if (UserAgent == null)
            {
                UserAgent = "[NULL]";
            }

            IsRobot = robots.Any(robot => UserAgent.ToLower().Contains(robot.ToLower()));
        }

        public int Id { get; set; }
     
        public DateTime DateTimeRaw { get; set; }
        /// <summary>
        /// Format ?
        /// </summary>
        public string Date => DateTimeRaw.ToShortDateString(); 
        
        /// <summary>
        /// Format DD/MM/YY hh:mm
        /// </summary>
        public string DateTimeDisplayShort => DateTimeRaw.ToString("dd/MM/yy hh:mm tt"); 
        public string DateTimeDisplayFull => DateTimeRaw.ToString("dd/MM/yy hh:mm:ss tt"); 

        public string Url { get; set; }
        public string UrlDisplay => Url.TrimStart("http://"); //.Shorten(40,true); 
        public string UserAgent { get; set; }

        public string UserAgentDisplay => UserAgent.Shorten(30, true); 

        public string UserHostAddress { get; set; }

        public bool IsMe2 => (bool)UserHostAddress?.Contains("62.31.191.80")
                           || Url.ToLower().Contains("localhost");

        public bool IsMe { get
            {
                if (Url.ToLower().Contains("localhost"))
                {
                    return true;
                }
                if (UserHostAddress == null )
                {
                    return false;
                }

                return UserHostAddress.Contains("62.31.191.80");
            }
        }

        public bool IsRobot { get; private set; }
    }
}
