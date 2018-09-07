using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Fabricas;
using Industria4.Models;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class ListaFabricasViewModel : ViewModelBase
    {
        private readonly IFabricaService _fabricaService;

        private ObservableCollection<Fabrica> _fabricasItems;
        public ObservableCollection<Fabrica> FabricasItems
        {
            get { return _fabricasItems; }
            set { _fabricasItems = value; RaisePropertyChanged(() => FabricasItems); }
        }

        public ListaFabricasViewModel(IFabricaService fabricaService)
        {
            _fabricaService = fabricaService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (FabricasItems == null)
                FabricasItems = new ObservableCollection<Fabrica>();

            FabricasItems = await _fabricaService.GetFabricasAsync();
        }

        public ICommand GetFabricaDetailsCommand => new Command<Fabrica>(async (item) => await GetFabricaDetailsAsync(item));

        private async Task GetFabricaDetailsAsync(Fabrica fab)
        {

            await NavigationService.NavigateToAsync<FabricaViewModel>(fab);
         
        }

    }
}
