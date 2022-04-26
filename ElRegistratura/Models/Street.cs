using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Street
    {
        public int Id { get; set; }
        [Display(Name = "Улица")]
        public string Name { get; set; }

        public List<Clinic> Clinics { get; set; }
        public List<User> Users { get; set; }
        public List<AddressForPlot> Address { get; set; }
    }
}
