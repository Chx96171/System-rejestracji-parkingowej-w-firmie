using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SystemRejestracjiParkingowej.Models;
using System.ComponentModel.DataAnnotations;

namespace SystemRejestracjiParkingowej.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumberForParking = model.PhoneNumber
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email, 
                    model.Password, 
                    model.RememberMe, 
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Konto zostało zablokowane z powodu zbyt wielu nieudanych prób logowania.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email")]
        [Display(Name = "Adres e-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [StringLength(100, ErrorMessage = "Imię nie może być dłuższe niż 100 znaków")]
        [Display(Name = "Imię")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [StringLength(100, ErrorMessage = "Nazwisko nie może być dłuższe niż 100 znaków")]
        [Display(Name = "Nazwisko")]
        public required string LastName { get; set; }

        [Phone(ErrorMessage = "Podaj prawidłowy numer telefonu")]
        [Display(Name = "Numer telefonu")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Hasło musi mieć od 6 do 100 znaków")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne")]
        public required string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Podaj prawidłowy adres email")]
        [Display(Name = "Adres e-mail")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Hasło jest wymagane")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public required string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }
}