using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Cabinet
    {
        public int Id { get; set; }
        [Display(Name = "Кабинет"), Required]
        public string Name { get; set; }
        [Display(Name = "Дополнительная информация")]
        public string Info { get; set; }

        [Display(Name = "Поликлиника")]
        public int? ClinicId { get; set; }
        [Display(Name = "Поликлиника")]
        public Clinic? Clinic { get; set; }

        public string CabinetNameAndClinicName { get; set; }

        public List<Schedule> Schedules { get; set; }
    }
}
