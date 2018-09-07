using System;
namespace Industria4.Models
{
    public class Maquina
    {
       
        public int Id { get; set; }
        public string CodigoMaquina { get; set; }
        public string NomeMaquina { get; set; }
        public int IdFabrica { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
