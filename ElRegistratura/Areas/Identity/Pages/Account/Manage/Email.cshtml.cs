using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Linq;
using System.Threading.Tasks;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ElRegistratura.Email;

namespace ElRegistratura.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly Email.IEmailSender _emailSender;

        public EmailModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
             Email.IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Новая электронная почта")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;

            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmailChange",
                    pageHandler: null,
                    values: new { userId, email = Input.NewEmail, code },
                    protocol: Request.Scheme);
                var message = new Message(new string[] { user.Email }, "Подтвердите ваш адрес электронной почты",
                  $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.", null);
                await _emailSender.SendEmailAsync(message);
                //await _emailSender.SendEmailAsync(
                //    Input.NewEmail,
                //    "Confirm your email",
                //    $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

                StatusMessage = "Ссылка для подтверждения изменения электронной почты отправлена. Пожалуйста, проверьте свою электронную почту.";
                return RedirectToPage();
            }

            StatusMessage = "Ваш адрес электронной почты не изменился.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Не удалось загрузить пользователя с идентификатором '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);
            var message = new Message(new string[] { user.Email }, "Confirm your email",
                   $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.", null);
            await _emailSender.SendEmailAsync(message);
            //await _emailSender.SendEmailAsync(
            //    email,
            //    "Confirm your email",
            //    $"Пожалуйста, подтвердите свой аккаунт через <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>нажмите здесь</a>.");

            StatusMessage = "Письмо с подтверждением отправлено. Пожалуйста, проверьте свою электронную почту.";
            return RedirectToPage();
        }
    }
}
