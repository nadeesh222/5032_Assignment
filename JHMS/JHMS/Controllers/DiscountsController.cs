using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JHMS.Models;
using JHMS.Utils;

namespace JHMS.Controllers
{
    [Authorize(Roles = "admin,manager")]
    public class DiscountsController : Controller
    {
        private DEntities db = new DEntities();

        // GET: Discounts
        public ActionResult Index()
        {
            return View(db.Discounts.ToList());
        }

        // GET: Discounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // GET: Discounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Discounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,value,fromdate,todate")] Discount discount)
        {
            if (discount.todate < discount.fromdate)
            {
                ViewBag.Message = "To date cannot be smaller than from date";
                return View(discount);
            }

            if (ModelState.IsValid)
            {
                db.Discounts.Add(discount);
                db.SaveChanges();


                ApplicationDbContext identityDb = new ApplicationDbContext();
                var a = identityDb.Users.ToList();
                String toEmail = "";
                for (int i = 0; i < a.Count; i++)
                {

                    if (toEmail.Trim().Length != 0)
                    {
                        toEmail = toEmail+"|" + a[i].Email;
                    }
                    else {
                        toEmail = a[i].Email;
                    }

                }


               
                String subject = "Jetwing hotels discounts";
                String contents = "You will get  " + discount.value + "% discount on your booking from " + discount.fromdate + " to " + discount.todate + " for all the branches in Jetwing hotels.";

                EmailSender es = new EmailSender();
                es.SendBulkEmail(toEmail, subject, contents);



                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: Discounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,value,fromdate,todate")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discount);
        }

        // GET: Discounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discount discount = db.Discounts.Find(id);
            db.Discounts.Remove(discount);
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
