using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Категория"), Required]
        public string Name { get; set; }
        public List<Doctor> Doctors { get; set; }
    }
}
