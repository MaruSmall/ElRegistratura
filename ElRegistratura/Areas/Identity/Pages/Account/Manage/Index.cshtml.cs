using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ElRegistratura.Cripto;
using ElRegistratura.Data;
using ElRegistratura.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElRegistratura.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        //Aes myAes = Aes.Create();
        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Фамилия")]
            public string LastName { get; set; }
            [Display(Name = "Имя")]
            public string FirstName { get; set; }
            [Display(Name = "Отчество")]
            public string Patronymic { get; set; }

            [Display(Name = "Имя пользователя")]
            public string UserName { get; set; }

            [Phone]
            [Display(Name = "Номер телефона"), DataType(DataType.PhoneNumber), StringLength(11)]
            public string PhoneNumber { get; set; }

            [Display(Name = "Дата Рождения"), DataType(DataType.Date)]
            public DateTime? Birthday { get; set; }

            [Display(Name = "Номер полиса"), DataType(DataType.Text), StringLength(16, MinimumLength = 16)]
            public string PolisNumber { get; set; }


            [Display(Name = "Серия"), DataType(DataType.Text)]
            public string Series { get; set; }
            [Display(Name = "Номер"), DataType(DataType.Text)]
            public string Number { get; set; }
            [Display(Name = "Кем выдан"), DataType(DataType.Text)]
            public string IssuedBy { get; set; }


            [Display(Name = "Пол")]
            public int? SexId { get; set; }
            [Display(Name = "Пол")]
            public Sex Sex { get; set; }
            [Display(Name = "Улица")]
            public int? StreetId { get; set; }
            [Display(Name = "Улица")]
            public Street Street { get; set; }
            [Display(Name = "Номер дома"), DataType(DataType.Text)]
            public string HouseNumber { get; set; }

            [Display(Name = "Корпус"), DataType(DataType.Text)]
            public string Housing { get; set; }

            [Display(Name = "Номер квартиры"), DataType(DataType.Text)]
            public string Apartment { get; set; }

        }

        private async Task LoadAsync(User user)
        {

            ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name");
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "Name");
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var lastName = user.LastName;
            var firstName = user.FirstName;
            var patronymic = user.Patronymic;
            var bday = user.Birthday;
            var polisNumber = user.PolisNumber;

            var seria = user.Series;
            //CriptoAes cripto= new CriptoAes();
            //try
            //{
            //    byte[] croptoSeria = System.Text.Encoding.UTF8.GetBytes(seria);
            //    // string roundtrip = cripto.DecryptStringFromBytes_Aes(croptoSeria, myAes.Key, myAes.IV);
            //    seria = cripto.DecryptStringFromBytes_Aes(croptoSeria, myAes.Key, myAes.IV);
            //}
            //catch(Exception ex)
            //{
                
            //}
            

            var number = user.Number;
            var issue = user.IssuedBy;

            var sex = user.SexId;
            var street = user.StreetId;

            var houseNumber = user.HouseNumber;
            var housing = user.Housing;
            var apartament = user.Apartment;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                LastName = lastName,
                FirstName = firstName,
                Patronymic = patronymic,
                Birthday = bday,
                PolisNumber = polisNumber,

                Series = seria,
                Number = number,
                IssuedBy = issue,

                SexId = sex,
                StreetId = street,
                HouseNumber = houseNumber,
                Housing = housing,
                Apartment = apartament
            };
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

        public async Task<IActionResult> OnPostAsync()
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

            var lastName = user.LastName;
            var firstName = user.FirstName;
            var patronymic = user.Patronymic;
            var bday = user.Birthday;
            var polisNumber = user.PolisNumber;

            var seria = user.Series;
            var number = user.Number;
            var issued = user.IssuedBy;

            var sex = user.SexId;
            var street = user.StreetId;

            var houseNumber = user.HouseNumber;
            var housing = user.Housing;
            var apartament = user.Apartment;

            if (Input.LastName != lastName)
            {
                user.LastName = lastName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Patronymic != patronymic)
            {
                user.Patronymic = Input.Patronymic;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Birthday != bday)
            {
                user.Birthday = Input.Birthday;
                await _userManager.UpdateAsync(user);
            }
            if (Input.PolisNumber != polisNumber)
            {
                user.PolisNumber = Input.PolisNumber;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Series != seria)
            {
                user.Series = Input.Series;
                //CriptoAes cripto = new CriptoAes();
                //cripto.EncryptStringToBytes_Aes(seria, myAes.Key, myAes.IV);

                await _userManager.UpdateAsync(user);
            }

            if (Input.Number != number)
            {
                user.Series = Input.Series;
                await _userManager.UpdateAsync(user);
            }

            if (Input.IssuedBy != issued)
            {
                user.IssuedBy = Input.IssuedBy;
                await _userManager.UpdateAsync(user);
            }

            if (Input.SexId != sex)
            {
                ViewData["SexId"] = new SelectList(_context.Street, "Id", "Name", user.SexId);
                user.SexId = Input.SexId;
                await _userManager.UpdateAsync(user);

            }

            if (Input.StreetId != street)
            {
                ViewData["StreetId"] = new SelectList(_context.Street, "Id", "Name", user.StreetId);
                user.StreetId = Input.StreetId;
                await _userManager.UpdateAsync(user);
            }

            if (Input.HouseNumber != houseNumber)
            {
                user.HouseNumber = Input.HouseNumber;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Housing != housing)
            {
                user.Housing = Input.Housing;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Apartment != apartament)
            {
                user.Apartment = Input.Apartment;
                await _userManager.UpdateAsync(user);
            }
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Непредвиденная ошибка при попытке установить номер телефона.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Ваш профиль был обновлен";
            return RedirectToPage();
        }

      
    }
}
