using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.Models;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class ModulosAquisicaoViewModel : ViewModelBase
    {
        private readonly IModuloAquisicaoService _modulosAquisicaoService;


        public ObservableCollection<ModuloAquisicao> _modulosAquisicao { get; private set; }
        public ObservableCollection<ModuloAquisicao> ModulosAquisicao
        {
            get { return _modulosAquisicao; }
            set
            {
                _modulosAquisicao = value;
                RaisePropertyChanged(() => ModulosAquisicao);
            }
        }

        public ModulosAquisicaoViewModel(IModuloAquisicaoService modulosAquisicaoService)
        {
            _modulosAquisicaoService = modulosAquisicaoService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (ModulosAquisicao == null)
                ModulosAquisicao = new ObservableCollection<ModuloAquisicao>();

            ModulosAquisicao = await _modulosAquisicaoService.GetModulosAquisicaoAsync();


            await base.InitializeAsync(navigationData);
        }

        public ICommand GetModuloDetailsCommand => new Command<ModuloAquisicao>(async (item) => await GetModuloDetailsAsync(item));

        private async Task GetModuloDetailsAsync(ModuloAquisicao modulo)
        {
            await NavigationService.NavigateToAsync<ModuloAquisicaoViewModel>(modulo);
        }

    }
}
