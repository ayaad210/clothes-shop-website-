using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop;

namespace Shop.Controllers
{
    public class CatigoriesController : Controller
    {
        private ShopDBEntities1 db = new ShopDBEntities1();

        // GET: Catigories
        public ActionResult Index()
        {
            return View((db.Catigories.Where(c => !c.IsDeleted).ToList()));
        }

        // GET: Catigories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catigory catigory = db.Catigories.Find(id);
            if (catigory == null)
            {
                return HttpNotFound();
            }
            return View(catigory);
        }

        // GET: Catigories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catigories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Name,Desc,IsDeleted")] Catigory catigory)
        {
            if (ModelState.IsValid)
            {
                catigory.IsDeleted = false;

                db.Catigories.Add(catigory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catigory);
        }

        // GET: Catigories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catigory catigory = db.Catigories.Find(id);
            if (catigory == null)
            {
                return HttpNotFound();
            }
            return View(catigory);
        }

        // POST: Catigories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Name,Desc,IsDeleted")] Catigory catigory)
        {
            if (ModelState.IsValid)
            {
                catigory.IsDeleted = false;

                db.Entry(catigory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catigory);
        }

        // GET: Catigories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catigory catigory = db.Catigories.Find(id);
            if (catigory == null)
            {
                return HttpNotFound();
            }
            return View(catigory);
        }

        // POST: Catigories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Catigory catigory = db.Catigories.Find(id);
            db.Catigories.Remove(catigory);
            db.SaveChanges();
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
    }
}
