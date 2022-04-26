using System;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        [Display(Name = "Расписание")]
        public int ScheduleId { get; set; }
        [Display(Name = "Расписание")]
        public Schedule Schedule { get; set; }
        [Display(Name = "Пользватель")]
        public string UserId { get; set; }
        [Display(Name = "Пользователь")]
        public User User { get; set; }
        [Display(Name = "Время")]
        public TimeSpan Time { get; set; }
        [Display(Name = "Статус")]
        public int StatusId { get; set; }
        [Display(Name = "Статус")]
        public Status Status { get; set; }
    }
}
