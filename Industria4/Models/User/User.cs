using System;
using System.Windows.Input;
using Xamarin.Forms;
//using Xamarin.Forms.BehaviorsPack;

namespace Industria4.Models.User
{
    public class User
    {
        public int Id { get; set; }

        public string IdUser { get; set; }

        public int NumeroFuncionario { get; set; }

        public string Nome { get; set; }

        public AspNetUser AspNetUser { get; set; }

        public string SignalrConn { get; set; }

        public bool IsConnSignalR { get; set; }
    }


}
