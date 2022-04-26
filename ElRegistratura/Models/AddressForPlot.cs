using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class AddressForPlot
    {
        public int Id { get; set; }
        [Display(Name = "Номер дома")]
        public string HouseNumber { get; set; }
        [Display(Name = "Улица")]
        public int StreetId { get; set; }
        [Display(Name = "Улица")]
        public Street Street { get; set; }
        [Display(Name = "Участок")]
        public int PlotId { get; set; }
        [Display(Name = "Участок")]
        public Plot Plot { get; set; }
    }
}
