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
    public class HistorialUsuariosController : Controller
    {
        private SeguridadMejorada2Entities db = new SeguridadMejorada2Entities();

        // GET: HistorialUsuarios
        public ActionResult Index()
        {
            var historialUsuarios = db.HistorialUsuarios.Include(h => h.Usuarios);
            return View(historialUsuarios.ToList());
        }

        // GET: HistorialUsuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialUsuarios historialUsuarios = db.HistorialUsuarios.Find(id);
            if (historialUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(historialUsuarios);
        }

        // GET: HistorialUsuarios/Create
        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario");
            return View();
        }

        // POST: HistorialUsuarios/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "HistorialID,UsuarioID,Accion,FechaAccion")] HistorialUsuarios historialUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.HistorialUsuarios.Add(historialUsuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", historialUsuarios.UsuarioID);
            return View(historialUsuarios);
        }

        // GET: HistorialUsuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialUsuarios historialUsuarios = db.HistorialUsuarios.Find(id);
            if (historialUsuarios == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", historialUsuarios.UsuarioID);
            return View(historialUsuarios);
        }

        // POST: HistorialUsuarios/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HistorialID,UsuarioID,Accion,FechaAccion")] HistorialUsuarios historialUsuarios)
        {
            if (ModelState.IsValid)
            {
                db.Entry(historialUsuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.Usuarios, "UsuarioID", "NombreUsuario", historialUsuarios.UsuarioID);
            return View(historialUsuarios);
        }

        // GET: HistorialUsuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HistorialUsuarios historialUsuarios = db.HistorialUsuarios.Find(id);
            if (historialUsuarios == null)
            {
                return HttpNotFound();
            }
            return View(historialUsuarios);
        }

        // POST: HistorialUsuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HistorialUsuarios historialUsuarios = db.HistorialUsuarios.Find(id);
            db.HistorialUsuarios.Remove(historialUsuarios);
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
