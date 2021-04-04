using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TheatreCMS3.Areas.Prod.Models;
using TheatreCMS3.Models;
using PagedList;

namespace TheatreCMS3.Areas.Prod.Controllers
{
    public class ProductionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Prod/Productions
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (page <= 0)
            {
                page = 1;
            }

            // Retain sort order during page changes.
            ViewBag.CurrentSort = sortOrder;

            // If search string is changed set "page" to 1.
            if (searchString != null)
            {
                page = 1;
            }
            // Otherwise retain search string during page changes.
            else
            {
                searchString = currentFilter;
            }
            // Provide view with current filter string.
            ViewBag.CurrentFilter = searchString;

            // Query database for table rows.
            var productions = from p in db.Productions select p;

            // If search string is not empty get search results.
            if (!String.IsNullOrEmpty(searchString))
            {
                productions = productions.Where(p => p.Title.Contains(searchString));
            }

            // Sort results ascending by title
            productions = productions.OrderBy(p => p.Title);

            // Set page size.
            int pageSize = 10;            
            
            // Retain value of "page" or set to 1 if "page" is null.
            int pageNumber = (page ?? 1);
            

            // Return list to view with page number and page size.
            return View(productions.ToPagedList(pageNumber, pageSize));
        }

        // Render Partial View to Details Modal.
        [HttpPost]
        public ActionResult DetailsModal(string id)
        {
            // Convert query string to integer for record search.
            int record = Convert.ToInt32(id);

            // Find matching database record.
            Production production = db.Productions.Find(record);

            // Return error if record not located.
            if (production == null)
            {
                return HttpNotFound();
            }

            // Return partial view with matching record.
            return PartialView("_DetailsModal", production);
        }

        // GET: Prod/Productions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: Prod/Productions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prod/Productions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductionID,Title,Description,Playwright,Runtime,OpeningDay,ClosingDay,ShowTimeEve,ShowTimeMat,Season,IsWorldPremier,TicketLink")] Production production)
        {
            if (ModelState.IsValid)
            {
                db.Productions.Add(production);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(production);
        }

        // GET: Prod/Productions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Prod/Productions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductionID,Title,Description,Playwright,Runtime,OpeningDay,ClosingDay,ShowTimeEve,ShowTimeMat,Season,IsWorldPremier,TicketLink")] Production production)
        {
            if (ModelState.IsValid)
            {
                db.Entry(production).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(production);
        }

        // GET: Prod/Productions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Prod/Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Production production = db.Productions.Find(id);
            db.Productions.Remove(production);
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
