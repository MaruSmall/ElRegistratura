using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ElRegistratura.Models
{
    public class UserIndexViewModel
    {
        public IEnumerable<User> User { get; set; }
        public string? Name { get; set; }
        //public SelectList Doctor { get; set; } = new SelectList(new List<Doctor>(), "Id", "FIOAndClinicName");
    }
}
