using System;
using System.Threading.Tasks;
using Industria4.Services.Dialog;
using Industria4.Services.Navigation;
using Industria4.Services.SignalR;

namespace Industria4.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
    //    protected readonly IUserDialogs DialogService;
        protected readonly INavigationService NavigationService;
      //  protected readonly IUserDialogs DialogService;
        protected readonly ISIgnalRService SignalRService;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase( )
        {
          
            NavigationService = ViewModelLocator.Instance.Resolve<INavigationService>();
            SignalRService = ViewModelLocator.Instance.Resolve<ISIgnalRService>();
    //        DialogService = UserDialogs.Instance;
         //   DialogService = ViewModelLocator.Instance.Resolve<IUserDialogs>();
            //DialogService= UserDialogs.Instance;//= ViewModelLocator.Instance.Resolve<IUserDialogs>();
        }

        public virtual void OnAppearing(object navigationContext)
        {
        }

        public virtual void OnDisappearing()
        {
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
