using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class ManageUserRolesViewModel
    {
        public string RoleId { get; set; }
        [Display(Name = "Название роли")]
        public string RoleName { get; set; }
        [Display(Name = "Выбор")]
        public bool Selected { get; set; }
    }
}
