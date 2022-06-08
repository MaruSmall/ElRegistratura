using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ElRegistratura.Models
{
    public class ViewModelIndexTickets
    {
        public IEnumerable<Ticket> Ticket { get; set; }
        public SelectList Status { get; set; } = new SelectList(new List<Status>(), "Id", "Name");
    }
}
