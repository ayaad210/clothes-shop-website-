using Shop.Helper;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class CartController : Controller
    {


        // GET: Cart
        ShopDBEntities1 db = new ShopDBEntities1();
           // IOrderProcessor OrderProcessor;
            public CartController()
            {
               
            }
            public RedirectToRouteResult AddToCart(Cart cart, int id, string returnUrl)
            {
                Item item = db.Items.FirstOrDefault(i => i.id == id);
                if (item != null)
                {
                    cart.AddItem(item);

                };

                return RedirectToAction("Index", new { returnUrl });//index
            }

     

        public RedirectToRouteResult MinusItem(Cart cart, int id, string returnUrl)
        {

            if (id!=0)
            {
                cart.MinusItem( id);
            }

            return RedirectToAction("Index", new { returnUrl });//index
        }
        public RedirectToRouteResult RemoveFromCart(Cart cart, int id, string returnUrl)
            {
                Item item = db.Items.FirstOrDefault(b => b.id == id);
                if (item != null)
                {
                    cart.RemoveLine(item);
                };

                return RedirectToAction("Index", new { returnUrl });//index
            }

       
        [HttpGet]
            public ViewResult Index(Cart cart, string returnUrl)
            {
            ViewBag.DropItems =new SelectList(db.Customers.Where(c=>c.IsDeleted==false).OrderBy(c=>c.StartDate), "id", "Name",cart.CustomerId);
            //cart.SellPrice = (cart.SellPrice <= 0 ? cart.ComputeTotalValue() : cart.SellPrice);
          //  cart.SellPrice = cart.ComputeTotalValue();
            return View(new CartIndexViewModel { Cart = cart, ReternUrl = returnUrl });
            }
        [HttpPost]
        public RedirectToRouteResult Save(DateTime DateTime,decimal SellPrice,int CustomerId, string command, Cart cart ,string returnUrl)
        {
            cart.DateTime = DateTime;
            cart.SellPrice = SellPrice;
            cart.CustomerId = CustomerId;
            if (ModelState.IsValid)
            {
            }
            else
            {

                return RedirectToAction("Index");
            }
            if (command== "اضافة"&&cart.OldCart==null)
            {
                
   
                    EventsClass.InsertEvent(cart, 2);
                    

                    

                    cart.Clear();
                    return RedirectToAction("Index", controllerName: "Orders");
                
              
            }
            else if (command== "حفظ التعديلات"&&cart.OldCart!=null)
            {
                EventsClass.EditEvent(cart, 2);
                cart.Clear();


                return RedirectToAction("Index", controllerName: "Orders");

            }
            else if (command=="حذف"&&cart.OrderId!=0)
            {
                EventsClass.DeleteEvent(cart.OrderId, 2);
                cart.Clear();

                return RedirectToAction("Index", controllerName: "Orders");
            }
            else if (command=="الغاء")
            {
                cart.Clear();
                return RedirectToAction("List", "SellItems");

            }
            else
            {

            return RedirectToAction("List","SellItems");

            }

        }
       
        public PartialViewResult Summery(Cart cart)
        {


            return PartialView("Summery",cart);
        }

    

    }
    }