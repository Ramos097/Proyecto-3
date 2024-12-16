using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GestionSeguridad.Controllers
{
    public class AuditoriaCambiosController : Controller
    {
        private SeguridadMejorada2Entities db = new SeguridadMejorada2Entities();

        // GET: AuditoriaCambios
        public ActionResult Index()
        {
            var auditoriaCambios = db.AuditoriaCambios.Include(a => a.Usuarios);
            return View(auditoriaCambios.ToList());
        }

        // GET: AuditoriaCambios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditoriaCambios auditoriaCambios = db.AuditoriaCambios.Find(id);
            if (auditoriaCambios == null)
            {
                return HttpNotFound();
            }
            return View(auditoriaCambios);
        }

        // GET: AuditoriaCambios/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario");
            return View();
        }

        // POST: AuditoriaCambios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuditoriaID,UsuarioID,Accion,Detalles,FechaHora")] AuditoriaCambios auditoriaCambios)
        {
            if (ModelState.IsValid)
            {
                db.AuditoriaCambios.Add(auditoriaCambios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", auditoriaCambios.UsuarioID);
            return View(auditoriaCambios);
        }

        // GET: AuditoriaCambios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditoriaCambios auditoriaCambios = db.AuditoriaCambios.Find(id);
            if (auditoriaCambios == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", auditoriaCambios.UsuarioID);
            return View(auditoriaCambios);
        }

        // POST: AuditoriaCambios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuditoriaID,UsuarioID,Accion,Detalles,FechaHora")] AuditoriaCambios auditoriaCambios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auditoriaCambios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", auditoriaCambios.UsuarioID);
            return View(auditoriaCambios);
        }

        // GET: AuditoriaCambios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuditoriaCambios auditoriaCambios = db.AuditoriaCambios.Find(id);
            if (auditoriaCambios == null)
            {
                return HttpNotFound();
            }
            return View(auditoriaCambios);
        }

        // POST: AuditoriaCambios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AuditoriaCambios auditoriaCambios = db.AuditoriaCambios.Find(id);
            db.AuditoriaCambios.Remove(auditoriaCambios);
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
