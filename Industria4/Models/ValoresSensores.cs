using System;
namespace Industria4.Models
{
    public class ValoresSensores
    {
        public int Id { get; set; }
        public int IdSensor { get; set; }
        public double Valor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedAtSource { get; set; }
    }
}
