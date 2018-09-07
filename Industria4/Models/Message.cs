using System;
namespace Industria4.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string Msg { get; set; }
        public string IdUser { get; set; }
        public string toUser { get; set; }
        public DateTime Date { get; set; }
        public string Nome { get; set; }
    }
}
