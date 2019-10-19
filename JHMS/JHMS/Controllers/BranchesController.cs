using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JHMS.Models;

namespace JHMS.Controllers
{
     public class BranchesController : Controller
    {
        private Entities db = new Entities();

        // GET: Branches
        public ActionResult Index()
        {
            return View(db.Branches.ToList());
        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        [Authorize(Roles = "admin")]

        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "admin")]

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,name,address,latitude,longitude,roomPrice,availableRooms")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                //Prevent Cross site scriptinh#########################

                branch.name = getWithoutCrossScript(branch.name);
                branch.address = getWithoutCrossScript(branch.address);
               



                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branch);
        }


        public String getWithoutCrossScript(String s)
        {

            if (s != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HttpUtility.HtmlEncode(s));


                sb.Replace("&lt;script&gt", "");
                sb.Replace("&lt;div&gt", "");
                sb.Replace("&lt;p&gt", "");
                sb.Replace("&b;p&gt", "");
                sb.Replace("&b;style&gt", "");
                return sb.ToString();
            }

            return "";
        }


        [Authorize(Roles = "admin")]

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }
        [Authorize(Roles = "admin")]

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,address,latitude,longitude,roomPrice,availableRooms")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
        }
        [Authorize(Roles = "admin")]

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
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

        public ActionResult Melbourne()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }

        public ActionResult Attractions()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }
        
        public ActionResult GeeAttractions()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }
        public ActionResult BranchNavigate()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }
        public ActionResult MelAttractions()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }

        public ActionResult FraAttractions()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }

        public ActionResult Frankston()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }
        public ActionResult Geelong()
        {
            ViewBag.Message = "Your Branch page.";

            return View();
        }
    }
}
