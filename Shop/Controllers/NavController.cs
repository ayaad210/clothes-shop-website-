using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class NavController : Controller
    {
        // GET: Nav
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult FlixableMenu(int CatigoryId = 0, bool MobileLayout = false)
        {
            ViewBag.SelectedCat = CatigoryId;
            IEnumerable<Catigory> Categories = new ShopDBEntities1().Catigories.Where(c => c.IsDeleted == false).ToList();



            return PartialView("FlixableMenu", Categories);
            //note menue view and horizontial not used 
        }

    }
}