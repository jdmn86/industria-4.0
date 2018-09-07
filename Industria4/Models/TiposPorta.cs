using System;
namespace Industria4.Models
{
    public class TiposPorta
    {
        public int Id { get; set; }
        public string Tporta { get; set; }
        public long NrMinPorta { get; set; }
        public long NrMaxPorta { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Descricao { get; set; }
    }
}
