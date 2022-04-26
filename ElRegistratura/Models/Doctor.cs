using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }
        [Display(Name = "Фотография")]
        public byte[] DoctorPicture { get; set; }
        [Display(Name = "Поликлиника")]
        public int ClinicId { get; set; }
        [Display(Name = "Поликлиника")]
        public Clinic Clinic { get; set; }
        [Display(Name = "Категория")]
        public int CategoryId { get; set; }
        [Display(Name = "Категория")]
        public Category Category { get; set; }

        [Display(Name = "Должность")]
        public int PositionId { get; set; }
        [Display(Name = "Должность")]
        public Position Position { get; set; }
        [Display(Name = "Специальность")]
        public int SpecialityId { get; set; }
        [Display(Name = "Специальность")]
        public Speciality Speciality { get; set; }
        [Display(Name = "Участок")]
        public int? PlotId { get; set; }
        [Display(Name = "Участок")]
        public Plot Plot { get; set; }
        public List<Schedule> Schedules { get; set; }
    }
}
