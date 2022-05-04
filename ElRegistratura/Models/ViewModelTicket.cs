
using System.Collections.Generic;

namespace ElRegistratura.Models
{
    public class ViewModelTicket
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Ticket> Ticket { get; set; }
        public IEnumerable<Doctor> Doctors { get; set; }
        public IEnumerable<Clinic> Clinic { get; set; }
        public IEnumerable<Speciality> Speciality { get; set; }
        public IEnumerable<Cabinet> Cabinets { get; set; }
    }
}
