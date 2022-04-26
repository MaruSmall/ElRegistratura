using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Status
    {
        public int Id { get; set; }
        [Display(Name = "Статус")]
        public string Name { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
