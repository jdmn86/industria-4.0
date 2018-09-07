using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.Users;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class MaquinasViewModel : ViewModelBase
    {
        private readonly IMaquinaService _maquinaService;
        private readonly IUsersService _usersService;

        public ObservableCollection<Maquina> _maquina { get; private set; }
        public ObservableCollection<Maquina> Maquinas
        {
            get { return _maquina; }
            set
            {
                _maquina = value;
                RaisePropertyChanged(() => Maquinas);
            }
        }

        public ObservableCollection<User> _apiUserList { get; private set; }
        public ObservableCollection<User> ApiUserList
        {
            get { return _apiUserList; }
            set
            {
                _apiUserList = value;
                RaisePropertyChanged(() => ApiUserList);
            }
        }

        public User _selectedItem { get; private set; }
        public User SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
                actualizaListaMaqAsync();
            }
        }

        private async Task actualizaListaMaqAsync()
        {
            Debug.WriteLine("actualizaListaMaqAsync");
            Maquinas.Clear();
            Maquinas = await _maquinaService.GetMaquinasFromUserAsync(SelectedItem.IdUser);
        }

        public MaquinasViewModel(IMaquinaService maquinaService, IUsersService usersService)
        {
            _maquinaService = maquinaService;
            _usersService = usersService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            Maquinas = await _maquinaService.GetMaquinasAsync();

            ApiUserList = await _usersService.GetApiUserAsync();

          
          
        }

        public ICommand GetMaquinaDetailsCommand => new Command<Maquina>(async (item) => await GetMaquinaDetailsAsync(item));

        private async Task GetMaquinaDetailsAsync(Maquina maquina)
        {
            Debug.WriteLine("entrou no metodo do comando Fabviewmodel");
            await NavigationService.NavigateToAsync<MaquinaViewModel>(maquina);
        }
    }
}