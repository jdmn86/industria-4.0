using System;
using System.Collections.Generic;
using System.Text;

namespace Industria4.Models
{
    public class Fabrica
    {
        public int Id { get; set; }
        public string CodigoFabrica { get; set; }
        public string NomeFabrica { get; set; }
        public string Localizacao { get; set; }
        public string Telefone { get; set; }
        public string IpGateway { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
        public string CodigoGateway { get; set; }
    }
}
