using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{

    public class SellItemsController : Controller
    {
        private ShopDBEntities1 db = new ShopDBEntities1();

        // GET: SellItems
        public ActionResult List(int CatigoryId = 0, int PageNum = 1)
        {

            int pagesize = 12;
            
            ItemListViewListModel model = new ItemListViewListModel()
            {

                Items = db.Items.Where(i => (CatigoryId == 0 && i.IsDeleted == false) || (i.Catigory.id == CatigoryId && i.IsDeleted == false)).ToList()
                .OrderBy(b => b.id).Skip((PageNum - 1) * pagesize).Take(pagesize).ToList(),

                paginginfo = new PageInfo()
                {
                    CurrentPage = PageNum,
                    ItemsPerPage = pagesize,
                    TotalItems = CatigoryId == 0 ? db.Items.Count() : (db.Items.Where(b => b.CatigoryId == CatigoryId)).Count()
                }
                //لية لازم نيو هنا ؟
                ,
                CurrentCatigoryId = CatigoryId,

            };

            ViewBag.SelectedCat = CatigoryId;

            return View("List", model);
        }
        [HttpPost]
        public ActionResult SearchByName(int CatigoryId = 0, int PageNum = 1, string SearchString = "")
        {
            if (SearchString != "")
            {
                int pagesize = 12;
                List<Item> Items = db.Items.Where(i => ((CatigoryId == 0 && i.IsDeleted == false) || (i.Catigory.id == CatigoryId && i.IsDeleted == false)) && i.Name.Contains(SearchString)).
                    OrderBy(b => b.id).Skip((PageNum - 1) * pagesize).Take(pagesize).ToList();


                ItemListViewListModel model = new ItemListViewListModel()
                {

                    Items = Items,
                    paginginfo = new PageInfo()
                    {
                        CurrentPage = PageNum,
                        ItemsPerPage = pagesize,
                        TotalItems = Items.Count()
                    }
                    //لية لازم نيو هنا ؟
                    ,
                    CurrentCatigoryId = CatigoryId
                };

                return View("List", model);

            }
            return List(CatigoryId, PageNum);
        }



        //  ViewBag.SelectedCat = CatigoryId;


    }
}

