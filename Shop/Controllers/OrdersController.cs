using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop;
using Shop.Models;
using Shop.Helper;

namespace Shop.Controllers
{
    public class OrdersController : Controller
    {
        private ShopDBEntities1 db = new ShopDBEntities1();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Where(o=>o.IsDeleted==false).Include(o => o.Customer).Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create(Cart cart)
        {
            ViewBag.Customerid = new SelectList(db.Customers, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Customerid,DateTime,TotalPrice,SellPrice,Userid,IsDeleted")] Order order,Cart cart)
        {
            if (cart==null||cart.LineCollecrtion.Count<=0)
            {
                ModelState.AddModelError("", "Sorry ,Your Cart Is Empty");

            }
         
            if (ModelState.IsValid)
            {
                order.IsDeleted = false;
                order.Userid = 2;
                
            string    Event = " " + db.Users.Where(u => u.id == order.Userid).First().Name + ":بواسطة";
                foreach (CartLine line in cart.LineCollecrtion)
                {
                    Item item = db.Items.Where(i => i.id == line.item.id).First();
                    item.Count -= line.Quantity;
                    Event += " , "+line.item.Name + " " + line.Quantity;
                   
                }

                db.Orders.Add(order);
                Event += " " + db.Customers.Where(c=>c.Id==order.Customerid).First().Name+ ":تم بيع ل";

                db.Events.Add(new Shop.Event() { EventDateTime = order.DateTime, Text = Event });
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.Customerid = new SelectList(db.Customers, "Id", "Name", order.Customerid);
            ViewBag.Userid = new SelectList(db.Users, "id", "Name", order.Userid);
            return View(order);
        }
        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customerid = new SelectList(db.Customers, "Id", "Name", order.Customerid);
            ViewBag.Userid = new SelectList(db.Users, "id", "Name", order.Userid);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Customerid,DateTime,TotalPrice,SellPrice,Userid,IsDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customerid = new SelectList(db.Customers, "Id", "Name", order.Customerid);
            ViewBag.Userid = new SelectList(db.Users, "id", "Name", order.Userid);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            EventsClass.DeleteEvent(order.id, 2);
            //db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        /// <summary>
        /// 
        /// </summary>

        public RedirectToRouteResult EditOrder(Cart cart,int ?id)
        {
            cart.Clear();
            if (id != null)
            {
                Order order = db.Orders.Find(id);


                if (order != null)
                {
                    ViewBag.Customerid = new SelectList(db.Customers, "Id", "Name", order.Customerid);
                    cart.OrderId = order.id;
                    cart.CustomerId = order.Customerid??1;
                    cart.UserId = order.Userid??1;
                    cart.SellPrice = order.SellPrice??0;
                    cart.DateTime = order.DateTime ?? DateTime.Now;
                    foreach (OrderDetail o in order.OrderDetails)
                    {
                        cart.LineCollecrtion.Add(new CartLine() { item = o.Item, Quantity = o.Count });
                    }

                    cart.OldCart = new Cart() { CustomerId=order.Customerid??0,UserId=order.Userid??0,DateTime=cart.DateTime,SellPrice=cart.SellPrice};
                    foreach (OrderDetail o in order.OrderDetails)
                    {
                        cart.OldCart.LineCollecrtion.Add(new CartLine() { item = o.Item, Quantity = o.Count });
                    }
                }

                return    RedirectToAction("Index",controllerName: "Cart");

            }
            return RedirectToAction("Index");
        }

    }
}
