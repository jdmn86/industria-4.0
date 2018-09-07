using System;
namespace Industria4.Models
{
    public class ValoresBetweenDates
    {

        public int idSensor { get; set; }

        public DateTime DateInit { get; set; }

        public TimeSpan TimeInit { get; set; }

        public DateTime DateEnd { get; set; }

        public TimeSpan TimeEnd { get; set; }

    }
}
