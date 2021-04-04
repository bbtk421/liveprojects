using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS3.Areas.Prod.Models;
using TheatreCMS3.Models;
using System.IO;


namespace TheatreCMS3.Areas.Prod.Controllers
{
    public class ProductionPhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //GET: Production/ProductionPhotos
        public ActionResult Index()
        {


            return View(db.ProductionPhotoes.ToList());
        }

        // GET: Production/ProductionPhotoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhoto productionPhoto = db.ProductionPhotoes.Find(id);
            if (productionPhoto == null)
            {
                return HttpNotFound();
            }
            return View(productionPhoto);
        }

        // GET: Production/ProductionPhotoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Production/ProductionPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProPhotoId,Title, PhotoFile, Description")] ProductionPhoto productionPhoto, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            productionPhoto.PhotoFile = bytes;
            if (ModelState.IsValid)
            {
                db.ProductionPhotoes.Add(productionPhoto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productionPhoto);
        }

        // GET: Production/ProductionPhotoes/Edit/5
        public ActionResult Edit(int? id, Byte[] photoF)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhoto productionPhoto = db.ProductionPhotoes.Find(id);
            if (productionPhoto == null)
            {
                return HttpNotFound();
            }
            return View(productionPhoto);
        }

        // POST: Production/ProductionPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProPhotoId,Title, PhotoFile, Description")] ProductionPhoto productionPhoto, HttpPostedFileBase postedFile)
        {
            if (postedFile == null)
            {
                var foto = db.ProductionPhotoes.AsNoTracking().First(prod => productionPhoto.ProPhotoId == prod.ProPhotoId);
                productionPhoto.PhotoFile = foto.PhotoFile;
            }
           
            else
            {
                byte[] bytes;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                }
                productionPhoto.PhotoFile = bytes;
            }
            if (ModelState.IsValid)
            {
                db.Entry(productionPhoto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productionPhoto);
        }

        // GET: Production/ProductionPhotoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhoto productionPhoto = db.ProductionPhotoes.Find(id);
            if (productionPhoto == null)
            {
                return HttpNotFound();
            }
            return View(productionPhoto);
        }

        // POST: Production/ProductionPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductionPhoto productionPhoto = db.ProductionPhotoes.Find(id);
            db.ProductionPhotoes.Remove(productionPhoto);
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
