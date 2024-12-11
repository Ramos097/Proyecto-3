using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GestionSeguridad.Controllers
{
    public class UsuariosController : Controller
    {
        private SeguridadMejorada2Entities db = new SeguridadMejorada2Entities();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        // GET: Usuarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // GET: Usuarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase file, [Bind(Include = "UsuarioID,NombreUsuario,Email,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Direccion,Telefono,Clave,FechaRegistro")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                // Asignar la fecha de registro si no es proporcionada (no es necesario si el valor se asigna por defecto en la DB)
                if (usuarios.FechaRegistro == null)
                {
                    usuarios.FechaRegistro = DateTime.Now;
                }

                // Manejo de la foto
                if (file != null && file.ContentLength > 0)
                {
                    string ruta = Server.MapPath("~/Content/img");

                    // Crear directorio si no existe
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(ruta, fileName);

                    file.SaveAs(filePath);

                    // Guardar ruta en la base de datos
                    usuarios.Foto = "/Content/img/" + fileName;
                }

                db.Usuarios.Add(usuarios);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(usuarios);
        }

        // GET: Usuarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "UsuarioID,NombreUsuario,Email,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Direccion,Telefono,Clave,FechaRegistro")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string ruta = Server.MapPath("~/Content/img");

                    // Crear directorio si no existe
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(ruta, fileName);

                    file.SaveAs(filePath);

                    // Actualizar la ruta en la base de datos
                    usuarios.Foto = "/Content/img/" + fileName;
                }

                db.Entry(usuarios).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuarios);
        }


        // GET: Usuarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Usuarios usuarios = db.Usuarios.Find(id);
            if (usuarios == null)
            {
                return HttpNotFound();
            }
            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Usuarios usuarios = db.Usuarios.Find(id);
            db.Usuarios.Remove(usuarios);
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
