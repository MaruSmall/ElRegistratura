using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace ElRegistratura.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required, Display(Name ="Электронная почта")]
            [EmailAddress]
            public string Email { get; set; }

            [Required, Display(Name ="Пароль")]
            [StringLength(100, ErrorMessage = "Длина {0} должна быть не менее {2} и не более {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Повторите пароль")]
            [Compare("Password", ErrorMessage = "Пароль и пароль подтверждения не совпадают.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code)
        {
            if (code == null)
            {
                return BadRequest("Код должен быть предоставлен для сброса пароля.");
            }
            else
            {
                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
                   
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {

            //if (!ModelState.IsValid)
            //    return View(resetPasswordModel);
            //var user = await _userManager.FindByEmailAsync(resetPasswordModel.Email);
            //if (user == null)
            //    RedirectToAction(nameof(ResetPasswordConfirmation));
           // var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            //if (!resetPassResult.Succeeded)
            //{
            //    foreach (var error in resetPassResult.Errors)
            //    {
            //        ModelState.TryAddModelError(error.Code, error.Description);
            //    }
            //    return View();
            //}
            //return RedirectToAction(nameof(ResetPasswordConfirmation));


            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            return RedirectToPage("./ResetPasswordConfirmation");
        }
    }
}
