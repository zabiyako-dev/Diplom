using System;
using System.ComponentModel.DataAnnotations;

namespace SecondTry.Models
{
    public class RecordingSession
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public string StudioName { get; set; } // Название студии
        public decimal CostPerHour { get; set; } // Стоимость часа аренды
        public virtual Musician Musician { get; set; } // Исполнитель
        public int MusicianId { get; set; }
    }
}