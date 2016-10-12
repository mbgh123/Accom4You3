using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using A4U3.Web.Helpers;
using A4U3.Domain.Interfaces;
using A4U3.Domain.Models;
using A4U3.Web.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using cloudscribe.Web.Pagination;

namespace A4U3.Web.Controllers
{
	public class AuditController : Controller
	{
	  protected IRepository rep;

	  public AuditController(IRepository rep)
		{
			this.rep = rep;
		}

      public ActionResult Index(string currentFilter, string searchString, bool? excludeRobots = false, 
                                bool? excludeMe = false, int? page = 1)
		{
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            const int pageSize = 20;
          
			if (searchString != null)
			{
				page = 1;
			}
			else
			{
				searchString = currentFilter;
			}

			ViewBag.CurrentFilter = searchString;
            ViewBag.ExcludeRobots = excludeRobots;
            ViewBag.ExcludeMe = excludeMe;

            IEnumerable<string> robots = rep.StaticData.GetRobotList();
		    
            IPagedList<AuditVM> audits
                = rep.GetAuditsAll()
                     .Select(a => a.ToAuditVM(robots))
                     .Where(a => excludeRobots == false || !a.IsRobot)
                     .Where(a => excludeMe == false || !a.IsMe) 
                     .Where(a => searchString == null || a.UserAgent.ToLower().Contains(searchString.ToLower()))
                     // Magic here
                     .ToPagedList(currentPageIndex, pageSize);
          
			return View(audits);
		}

        public ActionResult Index2(string searchString, bool? excludeRobots = false,
                                bool? excludeMe = false, int? page = 1, string order = "asc")
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            const int pageSize = 20;

            if (searchString != null)
            {
                page = 1;
            }

            // Copy params to ViewBag so the pager control can inject them into Next/Prev actions
            ViewBag.SearchString = searchString;
            ViewBag.ExcludeRobots = excludeRobots;
            ViewBag.ExcludeMe = excludeMe;
            ViewBag.Order = order;

            IEnumerable<string> robots = rep.StaticData.GetRobotList();

            IPagedList<AuditVM> audits
                = rep.GetAuditsFiltered(excludeRobots.Value, excludeMe.Value, searchString, order)
                     .Select(a => a.ToAuditVM(robots))
                     .ToPagedList(currentPageIndex, pageSize);

            return View(audits);
        }

        public ActionResult Details(int id)
		{
			Audit audit = rep.GetAuditById(id);

		    if (audit == null)
		    {
		        //return new HttpStatusCodeResult(HttpStatusCode.NotFound);
		        return NotFound();
		    }

			return View(audit.ToAuditVM(rep.StaticData.GetRobotList()));
		}
	}
}