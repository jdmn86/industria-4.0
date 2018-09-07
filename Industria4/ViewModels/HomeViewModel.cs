using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices;
using Industria4.DataServices.Connections;
using Industria4.DataServices.Fabricas;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.Messages;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.DataServices.Rooms;
using Industria4.DataServices.Sensores;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;
using Industria4.Services.SignalR;
using Industria4.ViewModels.Base;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Xamarin.Forms;


namespace Industria4.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISIgnalRService _sIgnalRService;
       // private readonly IUserDialogs _dialogService;
        private readonly IUsersService _usersService;
        private readonly IRoomsService _roomsService;
        private readonly IMessagesService _messagesService;
        private readonly IMaquinaService _maquinaService;
        private readonly ISensoresService _sensoresService;

        public delegate void MessageReceivedTest(string dataTest);
        public event MessageReceivedTest OnMessageReceivedTest;


        public HomeViewModel(IAuthenticationService authenticationService, 
                             IUsersService usersService,
                             IRoomsService roomsService,
                             IMaquinaService maquinaService,
                             ISensoresService sensoresService,
                             IMessagesService messagesService)
                          //   IUserDialogs dialogService)
        {
            _authenticationService = authenticationService;
         //   _dialogService = UserDialogs.Instance;
            _usersService = usersService;
            _roomsService = roomsService;
            _messagesService = messagesService;
            _maquinaService = maquinaService;
            _sensoresService = sensoresService;

     //       Messages = new ObservableCollection<Message>();
            MessagingCenter.Subscribe<string, string>("SendMessage", "Notify", (sender, arg) =>
            {
             //   MessagesAux = new ObservableCollection<Message>();
                // Message msg = JsonConvert.DeserializeObject<Message>(arg);
                Message msg = new Message();
                msg.Msg = arg.ToString();
                //this.addMessage(msg);
                ObservableCollection<Message> messagesAux = new ObservableCollection<Message>();
                messagesAux.Add(msg);
                foreach (var item in Messages)
                {
                    messagesAux.Add(item);
                }
                Messages = messagesAux;
             //   Messages.Add(msg); 

            });

            MessagingCenter.Subscribe<string, string[]>("MsgToGroup", "Notify1", (sender, arg) =>
            {
           //     MessagesAux = new ObservableCollection<Message>();
                Debug.WriteLine("recebeu sms grupo : " + arg.ToString());
                // Message msg = JsonConvert.DeserializeObject<Message>(arg);
                Message msg = new Message();
                msg.Msg = arg[1].ToString();
                msg.Nome=arg[0].ToString();

                ObservableCollection<Message> messagesAux = new ObservableCollection<Message>();
                messagesAux.Add(msg);
                foreach (var item in Messages)
                {
                    messagesAux.Add(item);
                }
                Messages = messagesAux;
                //this.addMessage(msg);
              //  Messages.Add(msg);
            });

            MessagingCenter.Subscribe<string, string[]>("MsgToUser", "Notify2", (sender, arg) =>
            {
          //      MessagesAux= new ObservableCollection<Message>();
                Debug.WriteLine("recebeu sms user : " + arg.ToString());
                // Message msg = JsonConvert.DeserializeObject<Message>(arg);
                Message msg = new Message();
                msg.Msg = arg[1].ToString();
                msg.Nome = arg[0].ToString();
                msg.toUser = arg[2].ToString();
                 ObservableCollection<Message> messagesAux = new ObservableCollection<Message>();
                messagesAux.Add(msg);
                foreach (var item in Messages)
                {
                    messagesAux.Add(item);
                }
                Messages = messagesAux;
            //    Messages.Add(msg);

            });
            MessagingCenter.Subscribe<string, List<UserChat>>("UserList", "Notify4", (sender, arg) =>
            {
                Friends = new ObservableCollection<UserChat>();
                Debug.WriteLine("recebeu USERLISTA");
                foreach (var item in arg)
                {
                    UserChat friend = new UserChat();
                    friend.Id = item.Id;
                    friend.idConnection = item.idConnection;
                    friend.Nome = item.Nome;
                    friend.Isconnected = item.Isconnected;
                    if(!friend.Nome.Equals(Settings.Username)){
                        Friends.Add(friend);
                    }
                }

            });

            MessagingCenter.Subscribe<string>("On", "Notify4", async (sender) =>
            {
                Debug.WriteLine("NO OOONNNN");
                maquinas = await _maquinaService.GetMaquinasFromUserAsync(Settings.idUser);

                foreach (var itemMaquina in maquinas)
                {
                    sensores = await _sensoresService.GetSensoresFromMaquinaAsync(itemMaquina.Id);
                    foreach (var itemSensor in sensores)
                    {
                        //add group
                        SignalRService.JoinRoom("sensor" + itemSensor.Id);
                    }
                }

                Debug.WriteLine("count users :" + Friends.Count);
            });


        }

        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;

            IsFriendsVisible = true;
            IsGroupVisible = false;

            User user = await _usersService.GetCurrentProfileAsync();

            //update connection true
            //    user.IsConnSignalR = true;
            //   user.SignalrConn = _sIgnalRService.getConnectionId() ;
            Debug.WriteLine("Connection Id = " + user.SignalrConn);
            //  User useraux = await _usersService.editUserAsync(user);

            //    Friends = new ObservableCollection<UserChat>(await _usersService.GetUsersChatAsync());
             Groups = new ObservableCollection<Room>(await _roomsService.GetRoomsAsync());
   //          Debug.WriteLine("count groups :"+Groups.Count );
            //   await SignalRService.initSignal();

       //     await  UserDialogs.Instance.AlertAsync("HomeViewModel", "test", "OK");

       //     await  App.Current.MainPage.DisplayAlert("HomeViewModel", "test", "OK");

           
  //          await UserDialogs.Instance.AlertAsync("HomeViewModel", "test", "OK");
            /*   foreach (var item in _sIgnalRService.getUsers())
               {
                   UserChat friend = new UserChat();
                   friend.IdConnection = item.Key;
                   friend.Nome = item.Value;
                   Friends.Add( friend);
               }
   */
          //  await SignalRService.initSignal();
         
        
            IsBusy = false;
            await base.InitializeAsync(navigationData);
        }


        private string _broadcastResults;
        public string BroadcastResults
        {
            get
            {
                return _broadcastResults;
            }

            set
            {
                _broadcastResults = value;
                RaisePropertyChanged(() => BroadcastResults);
            }
        }

        private ObservableCollection<object> _startDate;
        public ObservableCollection<object> StartDate
        {
            get
            {
                return _startDate;
            }

            set
            {
                _startDate = value;
                RaisePropertyChanged(() => StartDate);
            }
        }

        private ObservableCollection<Maquina> _maquinas;
        public ObservableCollection<Maquina> maquinas
        {
            get
            {
                return _maquinas;
            }

            set
            {
                _maquinas = value;
                RaisePropertyChanged(() => maquinas);
            }
        }
        private ObservableCollection<Sensor> _sensores;
        public ObservableCollection<Sensor> sensores
        {
            get
            {
                return _sensores;
            }

            set
            {
                _sensores = value;
                RaisePropertyChanged(() => sensores);
            }
        }


        private string _newText;
        public string NewText
        {
            get
            {
                return _newText;
            }

            set
            {
                _newText = value;
                RaisePropertyChanged(() => NewText);
            }
        }

       // private ObservableCollection<Message> messagesAux;
        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get
            {
                return _messages;
            }

            set
            {
                _messages = value;
                RaisePropertyChanged(() => Messages);
            //    preventButton = true;
            }
        }

        private ObservableCollection<UserChat> _friends = new ObservableCollection<UserChat>();
        public ObservableCollection<UserChat> Friends
        {
            get
            {
                return _friends;
            }

            set
            {
                _friends = value;
                RaisePropertyChanged(() => Friends);
            }
        }


        private ObservableCollection<Room> _groups = new ObservableCollection<Room>();
        public ObservableCollection<Room> Groups
        {
            get
            {
                return _groups;
            }

            set
            {
                _groups = value;
                RaisePropertyChanged(() => Groups);
            }
        }

        private UserChat _selectedFriend { get; set; }
        public UserChat SelectedFriend
        {
            get
            {
                return _selectedFriend;
            }

            set
            {
                _selectedFriend = value;
                RaisePropertyChanged(() => SelectedFriend);
            }
        }

        private Room _selectedGroup { get; set; }
        public Room SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }

            set
            {
                _selectedGroup = value;
                RaisePropertyChanged(() => SelectedGroup);
            }
        }

        private bool _isFriendsVisible;
        public bool IsFriendsVisible
        {
            get
            {
                return _isFriendsVisible;
            }

            set
            {
                _isFriendsVisible = value;
                RaisePropertyChanged(() => IsFriendsVisible);
            }
        }

        private bool _isGroupVisible;
        public bool IsGroupVisible
        {
            get
            {
                return _isGroupVisible;
            }

            set
            {
                _isGroupVisible = value;
                RaisePropertyChanged(() => IsGroupVisible);
            }
        }

        private bool _preventButton;
        public bool preventButton
        {
            get
            {
                return _preventButton;
            }

            set
            {
                _preventButton = value;
                RaisePropertyChanged(() => preventButton);
            }
        }

        public ICommand LogoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await _authenticationService.LogoutAsync();

                    await NavigationService.NavigateToAsync<LoginViewModel>();
                });
            }
        }


        public ICommand SendMessageCommandToUser => new Command<string>(async (item) => await SendMessageToUserCommandAsync(item));

        private async Task SendMessageToUserCommandAsync(string message)
        {
            Debug.WriteLine("select friend command");
            if (SelectedFriend != null)
            {
//                preventButton = false;
                Message m = new Message();
                m.Msg = message;
                //m.Nome = Settings.Name;
                Messages.Add(m);

                SignalRService.MsgToUser(Settings.Name, message, SelectedFriend.idConnection,SelectedFriend.Nome); // SendMessage( message);
               
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Please select a User", "User Not Found", "OK");
            }
           
        }

        public ICommand SendMessageCommandToGroups => new Command<string>(async (item) => await SendMessageToGroupsAsync( item));

        private async Task SendMessageToGroupsAsync(string message)
        {
            Debug.WriteLine("hera command : "+ message);

            //   _sIgnalRService.MsgToGroup(Settings.id , message, SelectedGroup.RoomName);
            //     _sIgnalRService.SendValor(message);
            if(SelectedGroup!=null){
 //               preventButton = false;

                Message m = new Message();
                m.Msg = message;
               // m.Nome = Settings.Name;
                Messages.Add(m);
                SignalRService.MsgToGroup(Settings.Name, message, SelectedGroup.RoomName); // SendMessage( message);    

            }else{
                await App.Current.MainPage.DisplayAlert("Please select a Group", "Group Not Found", "OK");
            }

        }



        public ICommand TabUsers => new Command(async () => await TabUsersAsync());

        private async Task TabUsersAsync()
        {
            Debug.WriteLine("change tab to users");

            if(IsFriendsVisible==false){
                IsFriendsVisible = true;
                IsGroupVisible = false;
            }

        }

        public ICommand TabGroups => new Command(async () => await TabGroupsAsync());

        private async Task TabGroupsAsync()
        {
            Debug.WriteLine("change tab to groups");

            if (IsGroupVisible == false)
            {
                IsGroupVisible = true;
                IsFriendsVisible = false;
            }

        }


    }
}
