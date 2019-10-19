using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using JHMS.Models;
using JHMS.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JHMS.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private Entities db = new Entities();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Include(b => b.Branch);
            return View(bookings.ToList());
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            ViewBag.branchId = new SelectList(db.Branches, "Id", "name");
            


            return View();
        }
        [HttpPost]
        public ActionResult getRoomPrice(String id)
        {
            Branch branch = db.Branches.Find(id);
            //if (branch != null)
            //    return PartialView("userDetails", branch.roomPrice);
            //else
            //    return PartialView("userDetails", "0");

            return Json("a=2", JsonRequestBehavior.AllowGet);
        }


        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult Create([Bind(Include = "Id,fromDate,toDate,roomCount,visitorCount,branchId")] Booking booking)
        {
            ViewBag.branchId = new SelectList(db.Branches, "Id", "name", booking.branchId);
           
            if (booking.toDate < booking.fromDate)
            {
                ViewBag.Message = "To date cannot be smaller than from date";
                return View(booking);
            }
           

            ViewBag.branchId = new SelectList(db.Branches, "Id", "name", booking.branchId);

            var branch = db.Branches.Single(m => m.Id == booking.branchId);
            // ViewBag.myBranch = Branch;
            int? roomPrice = branch.roomPrice;

            ViewBag.roomPrice = roomPrice;
            DateTime d1 = Convert.ToDateTime( booking.toDate);
            DateTime d2 = Convert.ToDateTime(booking.fromDate);

           

            int days = (int)(d1-d2).TotalDays;

            if (roomPrice == null)
            {
                booking.totalPrice = 0;
            }
            else
            {
                booking.totalPrice = booking.roomCount * roomPrice* days;

            }

            if (ModelState.IsValid)
            {
                booking.userId = User.Identity.GetUserId();

                db.Bookings.Add(booking);
                db.SaveChanges();

              ApplicationDbContext identityDb = new ApplicationDbContext();
              var a=  identityDb.Users.Find(booking.userId);



                String toEmail =   a.Email;
                String subject = "Booking Created";
                String contents = "The booking for Jetwing hotels " + booking.Branch.name + " branch from " + d2.ToString("dd/MM/yyyy") + " to " + d1.ToString("dd/MM/yyyy") + " for " +
                      booking.roomCount + " rooms is confirmed.";

                EmailSender es = new EmailSender();
                es.SendSingleEmail(toEmail, subject, contents);




            }

            //var Branch = (from branch in db.Branches select booking).ToList();




            return RedirectToAction("Confirm/"+ booking.Id);
        }


       

        // GET: Bookings/Edit/5
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);

         

            if (booking == null)
            {
                return HttpNotFound(booking.userId);
            }
            ViewBag.branchId = new SelectList(db.Branches, "Id", "name", booking.branchId);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.branchId = new SelectList(db.Branches, "Id", "name", booking.branchId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fromDate,toDate,roomCount,visitorCount,totalPrice,userId,branchId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.branchId = new SelectList(db.Branches, "Id", "name", booking.branchId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }

        public ActionResult WeeklyReport()
        {
            string query = "select branchId,count(*) from booking group by branchId ";
            var result = db.Database.SqlQuery<List<String>>(query);
            //var res = result.ToList();
            // var jsonResult = JsonConvert.SerializeObject(res);

            //var studentsGroupByStandard = from b in db.Bookings
            //group b by b.cdate into sg
            
            //select new { sg.Key, sg };
                return View();
        }


        [Authorize(Roles = "admin, manager")]
        public ActionResult Chart()
        {
            var bookings = db.Bookings.ToList();

            int melBookingCount=0;

            int geeBookingCount=0;
            int fraBookingCount=0;

            for (int i = 0; i < bookings.Count; i++) {

                Booking b = bookings[i];

                if (b.branchId == 1) {
                    melBookingCount++;
                }
               else if (b.branchId ==2)
                {
                    geeBookingCount++;
                }
                else if (b.branchId ==3)
                {
                    fraBookingCount++;
                }
            }


            String[] arr = {  };


            new Chart(width: 400, height: 200, theme: ChartTheme.Green)
                .AddSeries(
                     chartType: "column",
                  xValue: new[] { "Melbourne", "Geelong", "Frankston" },
                    yValues: new[] { melBookingCount.ToString(), geeBookingCount.ToString(), fraBookingCount.ToString() })
                  .Write("png");
            return null;


        }




        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            db.Bookings.Remove(booking);
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
