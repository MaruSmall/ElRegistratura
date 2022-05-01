using System.Linq;

namespace ElRegistratura.Models
{
    public class DoctorsSearchViewModel
    {
        public IQueryable<Doctor> Doctors { get; set; }
        public string Text { get; set; }
    }
}
