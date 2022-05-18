using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElRegistratura.Models
{
    public class User : IdentityUser
    {

        [Display(Name = "Фамилия"), DataType(DataType.Text)]
        public string LastName { get; set; }

        [Display(Name = "Имя"), DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Display(Name = "Отчество"), DataType(DataType.Text)]
        public string Patronymic { get; set; }

        [Display(Name = "Дата Рождения"), DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Номер полиса"), DataType(DataType.Text), StringLength(16, MinimumLength = 16)]
        public string PolisNumber { get; set; }

        [Display(Name = "Место работы"), DataType(DataType.Text)]
        public string PlaceOfWork { get; set; }


        [Display(Name = "Серия"), DataType(DataType.Text)]
        public string Series { get; set; }
        [Display(Name = "Номер"), DataType(DataType.Text)]
        public string Number { get; set; }
        [Display(Name = "Кем выдан"), DataType(DataType.Text)]
        public string IssuedBy { get; set; }

        public int ChatId { get; set; }

        [Display(Name = "Пол")]
        public int? SexId { get; set; }
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
        public List<Ticket> Tickets { get; set; }



    }
}
