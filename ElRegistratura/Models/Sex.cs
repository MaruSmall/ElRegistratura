using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Sex
    {
        public int Id { get; set; }

        [Display(Name="Пол")]
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
