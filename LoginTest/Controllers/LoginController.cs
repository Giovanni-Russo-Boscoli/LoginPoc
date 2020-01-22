using LoginTest.Models;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginTest.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {

            if (HttpContext.User.Identity.IsAuthenticated){
                return RedirectToAction("Index", "Home");
            }
            
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(LoginCustom model, string returnUrl)
        {

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            if (ModelState.IsValid)
            {
                //var userExists = loginApp.ValidLogin(model.UserName, model.Password, out int userId);
                var userExists = IsAuthenticated(model.UserName, model.Password);

                if (userExists)
                {
                    //Session["userId"] = userId;

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "The username or password is incorrect");
            return View("Index", model);
        }

        public bool IsAuthenticated(string userName, string pwd)
        {
            //var domainName = "DESKTOP-A7IHSEN";
            //var domainName = "welfare.irlgov.ie";
            var isValid = false;

            //using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainName))
            using (PrincipalContext pc = new PrincipalContext(ContextType.Machine))
            {
                isValid = pc.ValidateCredentials(userName, pwd);
            }

            return isValid;
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Index();
        }
    }
}