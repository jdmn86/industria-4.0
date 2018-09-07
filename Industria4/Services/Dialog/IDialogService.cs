using System;
using System.Threading.Tasks;

namespace Industria4.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        Task<bool> ShowConfirmAsync(string message, string title);
    }
}