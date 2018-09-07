using System;
namespace Industria4.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public string CodigoSensor { get; set; }
        public int IdModuloAquisicao { get; set; }
        public string NomeSensor { get; set; }
        public double ValorMax { get; set; }
        public double ValorMin { get; set; }
        public string Observacoes { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
        public int Tempo { get; set; }
        public double Fator { get; set; }
        public long Nporta { get; set; }
        public int IdTipoPorta { get; set; }
        public int Code { get; set; }
        public int IdGrandeza { get; set; }

        public TiposGrandeza TiposGrandeza  { get; set; }
    }
}
