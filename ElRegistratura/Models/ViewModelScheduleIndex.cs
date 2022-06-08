using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ElRegistratura.Models
{
    public class ViewModelScheduleIndex
    {
        public IEnumerable<Schedule> Schedule { get; set; }
        public SelectList Doctor { get; set; } = new SelectList(new List<Doctor>(), "Id", "FIOAndClinicName");
    }
}
