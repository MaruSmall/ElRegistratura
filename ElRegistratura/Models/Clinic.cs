using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Clinic
    {
        public int Id { get; set; }
        [Display(Name = "Поликлиника"), Required(ErrorMessage = "Обязательное поле")]
        public string Name { get; set; }
        [Display(Name = "Улица"), Required]
        public int StreetId { get; set; }
        [Display(Name = "Улица")]
        public Street Street { get; set; }
        [Display(Name = "Номер дома"), Required]
        public string HouseNumb { get; set; }
        [Display(Name = "Корпус")]
        public string? Housing { get; set; }
        [Display(Name = "Номер телефона"), Required]
        public string PhoneNumber { get; set; }

        public List<Plot> Plots { get; set; }
        public List<Cabinet> Cabinets { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
