using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecondTry.Models
{
    public class Musician
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Instrument { get; set; } // Возможные значения: гитара, пианино, укулеле

        // Коллекция связанных сессий
        public ICollection<RecordingSession> Sessions { get; set; }
    }
}