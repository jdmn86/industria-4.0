using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Users;
using Industria4.Models.User;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly IUsersService _usersService;

        public ObservableCollection<User> _users { get; private set; }
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }


        public UsersViewModel(IUsersService usersService)
        {
            _usersService = usersService;


        }

        public override async Task InitializeAsync(object navigationData)
        {

            Users = await _usersService.GetApiUserAsync();

           
        }

        public ICommand GetUserDetailsCommand => new Command<User>(async (item) => await GetUserDetailsAsync(item));

        private async Task GetUserDetailsAsync(User user)
        {
            await NavigationService.NavigateToAsync<UserViewModel>(user);
        }
    }
}
