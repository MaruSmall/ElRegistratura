using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using ElRegistratura.Data;
using System.Net.Mail;
using System.Security.Cryptography;

namespace ElRegistratura.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Укажите адрес электронной почты.")]
            [EmailAddress(ErrorMessage = "Неверный адрес электронной почты")]
            [Display(Name = "Электронная почта")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Поле Пароль обязательно для заполнения.")]
            [StringLength(15, ErrorMessage = "Длина пароля должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Пароль")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Поле Повторите пароль обязательно для заполнения.")]
            [DataType(DataType.Password)]
            [Display(Name = "Повторите пароль")]
            [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "Поле Фамилия обязательно для заполнения.")]
            [Display(Name = "Фамилия"), DataType(DataType.Text)]
            public string LastName { get; set; }
            [Required(ErrorMessage = "Поле Имя обязательно для заполнения.")]
            [Display(Name = "Имя"), DataType(DataType.Text)]
            public string FirstName { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

       


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                // var user = new User { UserName = Input.Email, Email = Input.Email };

                MailAddress address = new MailAddress(Input.Email);
                string userName = address.User;
               

            
                var user = new User
                {
                    UserName = userName,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Пользователь создал новую учетную запись с паролем.");
                    await _userManager.AddToRoleAsync(user, EnumRole.Basic.ToString());//
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
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

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
