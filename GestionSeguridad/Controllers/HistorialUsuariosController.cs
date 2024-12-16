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
