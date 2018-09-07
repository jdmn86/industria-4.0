using System;
namespace Industria4.Models
{
    public class TiposGrandeza
    {
        public int Id { get; set; }
        public string Grandeza { get; set; }
        public string Unidade { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Descricao { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
