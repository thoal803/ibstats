using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using IbStats.Web.Models;
using Vintrex.Platform.Identity;

namespace IbStats.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (returnUrl == null)
                returnUrl = Url.Action("Index", "Home");

            /* 
            string[] usernames = new string[] { "michael.colliander@dll.se", "per.aker@dll.se", "niklas.tengner@dll.se", "robert.lindh@dll.se", "marcus.eriksson@dll.se", "mikael.ekstrand@dll.se", "johan.ringman@dll.se", "jonas.olsson@innebandy.se", "henrik.alund@dll.se" };
            string[] firstnames = new string[] { "Michael", "Per", "Niklas", "Robert", "Marcus", "Mikael", "Johan", "Jonas", "Henrik" };
            string[] lastnames = new string[] { "Colliander", "Åker", "Tengner", "Lindh", "Ericsson", "Ekstrand", "Ringman", "Olsson", "Ålund" };
            string[] passwords = new string[] { "michael", "per", "niklas", "robert", "marcus", "mikael", "johan", "jonas", "henrik"};
            
            for(int i = 0; i < usernames.Length; i++)
            {
                ApplicationUser user2 = new ApplicationUser() { UserName = usernames[i], FirstName = firstnames[i], LastName = lastnames[i], Email = usernames[i], CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
                var result2 = await UserManager.CreateAsync(user2, passwords[i]);
            }

            
            ApplicationUser user2 = new ApplicationUser() { UserName="thobias@alund.net", FirstName = "Thobias", LastName = "Ålund", Email = "thobias@alund.net", CreatedDate = DateTime.Now, ModifiedDate = DateTime.Now };
            var result2 = await UserManager.CreateAsync(user2, model.Password);

            if (result2.Succeeded)
            {
                ModelState.AddModelError("", "User created successfully.");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "User creation failed: " + result2.Errors.ToArray()[0]);
                return View(model);
            }
            */

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Username, model.Password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("LoginError", "Inloggningen misslyckades. Kontrollera användarnamn och lösenord");
                    return View(model);
            }

        }



        //
        // POST: /Account/LogOff
        [HttpGet]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}