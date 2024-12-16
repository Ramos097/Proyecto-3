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
        private readonly HistorialService historialService;

        public UsuariosController()
        {
            historialService = new HistorialService(db);
        }

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
                if (usuarios.FechaRegistro == null)
                {
                    usuarios.FechaRegistro = DateTime.Now;
                }

                if (file != null && file.ContentLength > 0)
                {
                    string ruta = Server.MapPath("~/Content/img");
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(ruta, fileName);

                    file.SaveAs(filePath);

                    usuarios.Foto = "/Content/img/" + fileName;
                }

                db.Usuarios.Add(usuarios);
                db.SaveChanges();

                // Registrar acción con el usuario logueado
                string usuarioLogueadoID = Session["UsuarioID"]?.ToString();
                if (string.IsNullOrEmpty(usuarioLogueadoID))
                {
                    usuarioLogueadoID = "Usuario desconocido"; 
                }
                historialService.RegistrarAuditoria("Agregar", $"Usuario {usuarios.NombreUsuario} creado", usuarioLogueadoID);
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
        public ActionResult Edit(HttpPostedFileBase file, [Bind(Include = "UsuarioID,NombreUsuario,Email,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,Direccion,Telefono,Clave")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = db.Usuarios.Find(usuarios.UsuarioID);
                if (usuarioExistente == null)
                {
                    return HttpNotFound();
                }

                // Mantener la fecha de registro existente
                usuarios.FechaRegistro = usuarioExistente.FechaRegistro;

                if (file != null && file.ContentLength > 0)
                {
                    string ruta = Server.MapPath("~/Content/img");
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }

                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(ruta, fileName);

                    file.SaveAs(filePath);

                    usuarios.Foto = "/Content/img/" + fileName;
                }
                else
                {
                    // Mantener la foto existente si no se sube una nueva
                    usuarios.Foto = usuarioExistente.Foto;
                }

                // Actualizar las propiedades del usuario existente
                db.Entry(usuarioExistente).CurrentValues.SetValues(usuarios);
                db.SaveChanges();

                // Registrar acción con el usuario logueado
                string usuarioLogueadoID = Session["UsuarioID"]?.ToString();
                if (string.IsNullOrEmpty(usuarioLogueadoID))
                {
                    usuarioLogueadoID = "Usuario desconocido";
                }
                historialService.RegistrarAuditoria("Editar", $"Usuario {usuarios.NombreUsuario} editado", usuarioLogueadoID);

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
            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario != null)
            {
                string nombreUsuarioEliminado = usuario.NombreUsuario;

                // Registrar acción con el usuario logueado
                string usuarioLogueadoID = Session["UsuarioID"]?.ToString();
                if (string.IsNullOrEmpty(usuarioLogueadoID))
                {
                    usuarioLogueadoID = "Usuario desconocido";
                }
                historialService.RegistrarAuditoria("Eliminar", $"Usuario {nombreUsuarioEliminado} eliminado", usuarioLogueadoID);

                var historialUsuarios = db.HistorialUsuarios.Where(h => h.UsuarioID == id).ToList();
                foreach (var historial in historialUsuarios)
                {
                    historial.UsuarioID = null;
                    db.Entry(historial).State = EntityState.Modified;
                }

                db.Usuarios.Remove(usuario);
                db.SaveChanges();
            }
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

        public class HistorialService
        {
            private readonly SeguridadMejorada2Entities db;

            public HistorialService(SeguridadMejorada2Entities context)
            {
                db = context;
            }

            public void RegistrarAuditoria(string accion, string detalle, string nombreUsuario)
            {
                var auditoria = new AuditoriaCambios
                {
                    Accion = accion,
                    Detalles = detalle,
                    FechaHora = DateTime.Now,
                    NombreUsuario = nombreUsuario
                };

                db.AuditoriaCambios.Add(auditoria);
                db.SaveChanges();
            }
        }
    }
}