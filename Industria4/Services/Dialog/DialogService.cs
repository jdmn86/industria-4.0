using System;
using System.Threading.Tasks;

namespace Industria4.Services.Dialog
{
    public class DialogService : IDialogService
    {
        /* public Task ShowAlertAsync(string message, string title, string buttonLabel)
		 {
			 return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);

		 }

		 public Task<bool> ShowConfirmAsync(string message, string title)
		 {
			 return UserDialogs.Instance.ConfirmAsync(new ConfirmConfig
			 {
				 Title=title,
				 Message = message,
				 OkText = "YES",
				 CancelText = "NO"
			 });

		 }*/
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ShowConfirmAsync(string message, string title)
        {
            throw new NotImplementedException();
        }
    }
}