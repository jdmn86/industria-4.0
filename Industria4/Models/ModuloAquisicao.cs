using System;
namespace Industria4.Models
{
    public class ModuloAquisicao
    {
        public int Id { get; set; }
        public string CodigoModuloAquisicao { get; set; }
        public string NomeModuloAquisicao { get; set; }
        public int IdMaquina { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
    }
}
