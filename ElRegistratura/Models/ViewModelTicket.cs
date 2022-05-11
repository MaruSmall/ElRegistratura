
using GemBox.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public  class ViewModelTicket
    {
        public static IEnumerable<User> Users { get; set; }
        public static IEnumerable<Ticket> Ticket { get; set; }

        public static string fioDoctor { get; set; }
        public static string fioUser { get; set; }
        public static string address { get; set; }//
        public static string spec { get; set; }//
        public static string onClinic { get; set; }//
        public static  string cabinet { get; set; }//
        public static string number { get; set; }
        [DataType(DataType.Date)]
        public static string date {get;set;}
        [DataType(DataType.Time)]
        public static string time {get;set;}

        public string FIODoctor = fioDoctor;
        public string FIOUser = fioUser;
        public string Address = address;
        public string Spec=spec;
        public string OnClinic = onClinic;
        public string Cabinet=cabinet;
        public string Number=number;
        [DataType(DataType.Date)]
        public string Date=date;
        [DataType(DataType.Time)]
        public string Time=time;
        public  string Format { get; set; } = "PDF";
        public  SaveOptions Options => this.FormatMappingDictionary[this.Format];
        public  IDictionary<string, SaveOptions> FormatMappingDictionary => new Dictionary<string, SaveOptions>()
        {
            ["PDF"] = new PdfSaveOptions(),
        };
    }
}
