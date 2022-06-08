using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ElRegistratura.Models
{
    public class CreateScheduleViewModel
    {
        //Понедельник
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartMonday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishMonday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationMonday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartMonday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishMonday { get; set; }

        //Вторник
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartTuesday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishTuesday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationTuesday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartTuesday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishTuesday { get; set; }

        //Среда
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartWednesday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishWednesday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationWednesday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartWednesday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishWednesday { get; set; }

        //Четверг
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartThursday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishThursday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationThursday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartThursday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishThursday { get; set; }

        //Пятница
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartFriday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishFriday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationFriday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartFriday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishFriday { get; set; }

        //Суббота
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartSaturday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishSaturday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationSaturday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartSaturday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishSaturday { get; set; }

        //Воскресение
        [Display(Name = "Время начало работы")]
        public TimeSpan? WorkStartSunday { get; set; }
        [Display(Name = "Время конец работы")]
        public TimeSpan? WorkFinishSunday { get; set; }
        [Display(Name = "Интервал работы")]
        public TimeSpan? DurationSunday { get; set; }
        [Display(Name = "Начала перерыва")]
        public TimeSpan? BreakStartSunday { get; set; }
        [Display(Name = "Конец перерыва")]
        public TimeSpan? BreakFinishSunday { get; set; }

        public Schedule Schedule { get; set; }

    }
}
