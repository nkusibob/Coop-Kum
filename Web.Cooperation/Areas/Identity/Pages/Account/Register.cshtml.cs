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

        public async Task<IActionResult> OnPostAsync(int selectedCoopId, List<IFormFile> imageFiles,string returnUrl = null )
        {
            returnUrl = returnUrl ?? Url.Content("~");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                 
                List<byte[]> imageDatas = new List<byte[]>();

                foreach (var imageFile in imageFiles)
                {
                    byte[] imageData = null;
                    if (imageFile != null)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imageFile.CopyToAsync(memoryStream);
                            imageData = memoryStream.ToArray();
                        }
                    }
                    imageDatas.Add(imageData);
                }
                // Create a list of LivestockImage objects from the byte arrays
                List<ApplicationUserImage> UserImages = new List<ApplicationUserImage>();
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    LastName = Input.LastName,
                    FirstName = Input.FirstName,
                    PhoneNumber = Input.PhoneNumber,
                    City =Input.City,
                    Country =Input.Country,
                    Fees =Input.Fees
                };
                foreach (var imageData in imageDatas)
                {
                    ApplicationUserImage userPicture = new ApplicationUserImage
                    {
                        Data = imageData,
                        Id = user.Id
                        
                    };
                    UserImages.Add(userPicture);
                }
                user.UserImage =UserImages.FirstOrDefault();
                // Assign the image list to the goat
                ConnectedMember newPerson = new ConnectedMember
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    Country = user.Country
                };
                var coop = _context.Coop.Find(selectedCoopId);
                if (coop!=null)
                {
                    Membre membre = new Membre
                    {
                        Person = newPerson,
                        FeesPerYear = user.Fees,
                        MyCoop = coop
                    };
                    _context.Membre.Add(membre);

                }
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        _context.SaveChanges();
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            if (!ModelState.IsValid)
            {
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        _logger.LogError(error.ErrorMessage); // Log the validation error
                    }
                }
                await OnGetAsync();
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
