using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Searcher.Data;

namespace Searcher.Web.Controllers
{
    public class SearchResultsController : Controller
    {
        
        //
        // GET: /SearchResults/
        public ActionResult Index()
        {
            var model = new List<SearchResultData>();
            using (var ctx = new SearcherEntities())
            {

                var created = System.DateTime.Now.AddDays(-5);
                model.AddRange(
                    ctx.SearchResultDatas.Where(
                        x =>
                            x.Hidden == false &&
                            x.PostDateTime>created &&
                            x.KeywordScore > 105 &&  
                            x.AppliedDateTime.HasValue == false).ToList());
            }
            return View(model);
        }

        public ActionResult SaveChanges(FormCollection form)
        {
            using (var context = new SearcherEntities())
            {
                form.AllKeys.Where(x => x.Contains("PostId_")).ToList().ForEach(x =>
                {
                    var id = Guid.Parse(x.Replace("PostId_", ""));
                    context.SearchResultDatas.Single(z => z.Id == id).AppliedDateTime = System.DateTime.Now;
                });
                form.AllKeys.Where(x => x.Contains("Hidden_")).ToList().ForEach(x =>
                {
                    var id = Guid.Parse(x.Replace("Hidden_", ""));
                    context.SearchResultDatas.Single(z => z.Id == id).Hidden= true;
                });
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
	}
}