
using GemBox.Document;
using System.Collections.Generic;


namespace ElRegistratura.Models
{
    public class ViewModelTicket
    {
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Ticket> Ticket { get; set; }
        //public IEnumerable<Doctor> Doctors { get; set; }
        //public IEnumerable<Clinic> Clinic { get; set; }
        //public IEnumerable<Speciality> Speciality { get; set; }
        //public IEnumerable<Cabinet> Cabinets { get; set; }
        public string FIODoctor { get; set; } //
        public string FIOUser { get; set; }//
        public string Address { get; set; }//
        public string Spec { get; set; }//
        public string OnClinic { get; set; }//
        public string Cabinet { get; set; }//
        public string Number { get; set; }
        public string Format { get; set; } = "PDF";
        public SaveOptions Options => this.FormatMappingDictionary[this.Format];
        public IDictionary<string, SaveOptions> FormatMappingDictionary => new Dictionary<string, SaveOptions>()
        {
           
            ["PDF"] = new PdfSaveOptions(),
            
        };
    }
}
