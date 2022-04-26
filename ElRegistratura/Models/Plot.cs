using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Plot
    {
        public int Id { get; set; }
        [Display(Name = "Участок")]
        public string Name { get; set; }
        [Display(Name = "Поликлиника")]
        public int ClinicId { get; set; }
        [Display(Name = "Поликлиника")]
        public Clinic Clinic { get; set; }
        public List<AddressForPlot> Address { get; set; }
    }
}
