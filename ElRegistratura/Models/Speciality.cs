using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElRegistratura.Models
{
    public class Speciality
    {
        [NotMapped]
        public string EncryptedId { get; set; }
        public int Id { get; set; }
        [Display(Name = "Специальность")]
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
