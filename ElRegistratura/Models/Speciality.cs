using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Speciality
    {
        public int Id { get; set; }
        [Display(Name = "Специальность")]
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
