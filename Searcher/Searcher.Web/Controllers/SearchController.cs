using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Searcher.Data;

namespace Searcher.Web.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SaveTerms()
        {
            var data = Request["data"];
            var model = Newtonsoft.Json.JsonConvert.DeserializeObject<Search>(data);
            return null;
        }
	}


}