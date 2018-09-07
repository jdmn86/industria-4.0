using System;
namespace Industria4.Models
{
    public class AspNetUser
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string UserName { get; set; }

        public string RoleUser { get; set; }

        public bool IsEnable { get; set; }
       
    }
}
