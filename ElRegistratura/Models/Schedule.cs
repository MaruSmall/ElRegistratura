using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElRegistratura.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Data { get; set; }

        [Display(Name = "Врач")]
        public int DoctorId { get; set; }
        [Display(Name = "Врач")]
        public Doctor Doctor { get; set; }
        [Display(Name = "Начало работы")]
        public TimeSpan WorkStart { get; set; }
        [Display(Name = "Конец работы")]
        public TimeSpan WorkFinish { get; set; }
        [Display(Name = "Продолжительность работы")]
        public TimeSpan Duration { get; set; }
        [Display(Name = "Кабинет")]
        public int CabinetId { get; set; }
        [Display(Name = "Кабинет")]
        public Cabinet Cabinet { get; set; }
        [Display(Name = "Показ расписания врача")]
        public bool IsShow { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateFinish { get; set; }
        public List<Ticket> Tickets { get; set; }

    }
}
