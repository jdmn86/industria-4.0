using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.TipoPorta;
using Industria4.Models;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class TiposPortaViewModel : ViewModelBase
    {
        private readonly ITiposPortaService _tiposPortaService;

        public ObservableCollection<TiposPorta> _tiposPorta { get; private set; }
        public ObservableCollection<TiposPorta> TiposPorta
        {
            get { return _tiposPorta; }
            set
            {
                _tiposPorta = value;
                RaisePropertyChanged(() => TiposPorta);
            }
        }


        public TiposPortaViewModel(ITiposPortaService tiposPortaService)
        {
            _tiposPortaService = tiposPortaService;
        }

        public override async Task InitializeAsync(object navigationData)
        {

            if (TiposPorta == null)
                TiposPorta = new ObservableCollection<TiposPorta>();

            TiposPorta = await _tiposPortaService.GetTiposPortaAsync();

            await base.InitializeAsync(navigationData);
        }


        public ICommand GetTipoPortaDetailsCommand => new Command<TiposPorta>(async (item) => await GetTipoPortaDetailsAsync(item));

        private async Task GetTipoPortaDetailsAsync(TiposPorta tipoPorta)
        {
            await NavigationService.NavigateToAsync<TiposPortaViewModel>(tipoPorta);
        }
    }
}
