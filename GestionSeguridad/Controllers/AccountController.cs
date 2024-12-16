using GestionSeguridad.Controllers;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

public class AccountController : Controller
{
    private SeguridadMejorada2Entities db = new SeguridadMejorada2Entities();

    // GET: Account/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Login(string username, string password)
    {

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ViewBag.ErrorMessage = "Por favor, ingrese ambos campos.";
            return View();
        }

        // Validar usuario y contraseña
        var user = db.Usuarios.FirstOrDefault(u => u.NombreUsuario == username && u.Clave == password);
        if (user != null)
        {
            // Crear una cookie de autenticación
            FormsAuthentication.SetAuthCookie(user.NombreUsuario, false);
            return RedirectToAction("Index", "Home");
        }
        else
        {
            ViewBag.ErrorMessage = "Correo o contraseña incorrectos.";
            return View();
        }
    }

    // GET: Account/Logout
    public ActionResult Logout()
    {
        FormsAuthentication.SignOut();
        return RedirectToAction("Login");
    }
}