using SecondTry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondTry.Model
{
    public class SessionGroupByMusician
    {
        public string FullName { get; set; }
        public List<RecordingSession> Sessions { get; set; }
    }
}
