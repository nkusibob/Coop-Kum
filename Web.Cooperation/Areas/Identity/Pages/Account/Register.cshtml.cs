using Business.Cooperative;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model.Cooperative;
using Model.Cooperative.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Web.Cooperation.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly CooperativeContext _context;
       


        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            CooperativeContext context,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
      
            [Display(Name = "City")]
            public string City { get; set; }
            [Required]
        
            [Display(Name = "Country")]
            public string   Country { get; set; }
            [Display(Name = "LastName")]
            public string LastName { get; set; }
            [Required]

            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Display(Name = "Fees")]
            public int Fees { get; set; }

            [Required]

            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            

            [Display(Name = "Profile Picture")]

            public virtual ApplicationUserImage ProfilePicture { get; set; }
           

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;

            ViewData["Coop"] = await _context.Coop.ToListAsync();
            ViewData["SelectedCoopId"] = 0;  // Set your default selected value here

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(
    int selectedCoopId,
    List<IFormFile> imageFiles,
    string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // 1) Validate coop exists (created by admin)
            var coop = await _context.Coop.FindAsync(selectedCoopId);
            if (coop == null)
            {
                ModelState.AddModelError("", "Invalid cooperative selected.");
                await OnGetAsync();
                return Page();
            }

            // 2) Read first image (optional)
            byte[]? imageData = null;
            var firstImage = imageFiles?.FirstOrDefault(f => f != null && f.Length > 0);
            if (firstImage != null)
            {
                using var ms = new MemoryStream();
                await firstImage.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            // 3) Create Identity user FIRST (so user.Id is generated)
            var user = new ApplicationUser
            {
                UserName = Input.Email,
                Email = Input.Email,
                LastName = Input.LastName,
                FirstName = Input.FirstName,
                PhoneNumber = Input.PhoneNumber,
                City = Input.City,
                Country = Input.Country,
                Fees = Input.Fees
            };

            var result = await _userManager.CreateAsync(user, Input.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                await OnGetAsync();
                return Page();
            }

            try
            {
                // 4) Save profile picture (Identity context via navigation)
                if (imageData != null)
                {
                    user.UserImage = new ApplicationUserImage
                    {
                        Data = imageData,
                        AspUserId = user.Id   // ✅ FK, NOT Id = user.Id
                    };

                    await _userManager.UpdateAsync(user);
                }

                // 5) Create Person linked to this user
                var newPerson = new ConnectedMember
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    Country = user.Country,

                    CoopUser = user // ✅ link Person -> ApplicationUser
                };

                _context.Person.Add(newPerson);
                await _context.SaveChangesAsync(); // get PersonId

                // 6) Create Membre linked to existing Coop + Person
                var membre = new Membre
                {
                    // Recommended if you add FK properties:
                    // PersonId = newPerson.PersonId,
                    // MyCoopId = coop.IdCoop,

                    Person = newPerson,
                    MyCoop = coop,

                    FeesPerYear = user.Fees
                };

                _context.Membre.Add(membre);
                await _context.SaveChangesAsync();

                // 7) Email confirmation
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = user.Id, code, returnUrl },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });

                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl);
            }
            catch
            {
                // If domain creation fails, delete the user to avoid half-created accounts
                await _userManager.DeleteAsync(user);
                throw;
            }
        }

    }
}
