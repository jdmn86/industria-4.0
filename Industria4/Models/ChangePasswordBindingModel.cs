using System;
namespace Industria4.Models
{
    public class ChangePasswordBindingModel
    {
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }

        public string ConfirmPassword { get; set; }


    }
}
