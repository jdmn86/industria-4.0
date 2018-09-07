using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.DataServices.Connections;
using Industria4.DataServices.Messages;
using Industria4.DataServices.Rooms;
using Industria4.DataServices.Users;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;
using Industria4.ViewModels;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Plugin.LocalNotifications;
using Plugin.Notifications;
using Xamarin.Forms;

namespace Industria4.Services.SignalR 
{
    public class SignalRService : ISIgnalRService ,INotifyPropertyChanged
    {
        private readonly IRequestProvider _requestProvider;
//        private readonly IUserDialogs _userDialogs;
        private readonly IUsersService _usersService;
        private readonly IRoomsService _roomsService;
        private readonly IMessagesService _messagesService;

        private HubConnection hubConnection;
        private IHubProxy hubProxy;


        public delegate void MessageReceived(string dataTest);
        public event MessageReceived OnMessageReceived;


        public delegate void MessageReceivedToGroup(string user,string message);
        public event MessageReceivedToGroup OnMessageReceivedToGroup;

        public delegate void MessageReceivedToUser(string user, string message,string toUser);
        public event MessageReceivedToUser OnMessageReceivedToUser;

        public delegate void MessageReceivedAlarm(string sensor, string valor);
        public event MessageReceivedAlarm OnMessageReceivedAlarm;

        public delegate void MessageReceivedLista(List<UserChat> lista);
        public event MessageReceivedLista OnMessageReceivedLista;

        public delegate void MessageReceivedIsOn(string id);
        public event MessageReceivedIsOn OnMessageReceivedIsOn;

        public static List<UserChat> UserList = new List<UserChat>();

        public string RoomChart;


        /*

        public delegate void MessageReceivedMessage(Message dataTest);
        public event MessageReceivedMessage OnMessageReceivedMessage;
*/
        public string valor;

        public SignalRService(IRequestProvider requestProvider, 
                             IUsersService usersService,
                             IRoomsService roomsService,
                             IMessagesService messagesService)
        {
            _requestProvider = requestProvider;
//            _userDialogs = userDialogs;
            _usersService = usersService;
            _roomsService = roomsService;
            _messagesService = messagesService;

        //    if (!string.IsNullOrWhiteSpace(Settings.AccessToken))
        //    {
                var query = new Dictionary<string, string>();
                query.Add("access_token", Settings.AccessToken);

                hubConnection = new HubConnection(GlobalSettings.DefaultApiAddress + "signalr", query);

        //    }

            //  hubConnection.ConnectionToken=Settings.AccessToken;

            hubConnection.StateChanged += (StateChange obj) => {
                OnPropertyChanged("ConnectionState");
            };

            hubProxy = hubConnection.CreateHubProxy("Industria4Hub");

            Debug.WriteLine("hubConnection is on");
          
      /*      hubProxy.On<int,string>("MsgToGroup", (userId,message) =>
            {
                OnMessageReceived?.Invoke(userId, message);

               // MessagingCenter.Send<string, string>("SendValor","Notify" ,dataTest);

                Debug.WriteLine("menssagem user : " + userId.ToString());
                Debug.WriteLine("menssagem recebida : " + message.ToString());
            });
*/
            //  Clients.All.sendMessage(message);
            hubProxy.On<string>("sendMessage", (dataTest) =>
            {
                OnMessageReceived?.Invoke(dataTest);
           
            //    string m = JsonConvert.SerializeObject(dataTest);
                    
                MessagingCenter.Send<string, string>("SendMessage", "Notify", dataTest);

                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });

            hubProxy.On<string,string>("msgToGroup", (user,message) =>
            {
                OnMessageReceivedToGroup?.Invoke(user, message);

                //    string m = JsonConvert.SerializeObject(dataTest);
                string[] values = { user, message };
                MessagingCenter.Send<string, string[]>("MsgToGroup", "Notify1", values);
                Debug.WriteLine("recebeu sms" + values[0].ToString());
                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });

            hubProxy.On<string, string, string>("msgToUser", (user, message,toUser) =>
            {
                OnMessageReceivedToUser?.Invoke(user, message, toUser);

                //    string m = JsonConvert.SerializeObject(dataTest);
                string[] values = { user, message,toUser };
                Debug.WriteLine("recebeu sms" + values[0].ToString());

                MessagingCenter.Send<string, string[]>("MsgToUser", "Notify2", values);
                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });
    /*        hubProxy.On<Message>("SendToUser", (dataTest) =>
            {
                OnMessageReceivedMessage?.Invoke(dataTest);
                string m = JsonConvert.SerializeObject(dataTest);

                MessagingCenter.Send<string, string>("SendMessage", "Notify", m);

                Debug.WriteLine("menssagem recebida : " + dataTest.ToString());
                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });
          
            hubProxy.On<Message>("SendToGroup", (dataTest) =>
            {
                OnMessageReceivedMessage?.Invoke(dataTest);
                string m = JsonConvert.SerializeObject(dataTest);

                MessagingCenter.Send<string, string>("SendMessage", "Notify", m);

                Debug.WriteLine("menssagem recebida : " + dataTest.ToString());
                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });
          */

            hubProxy.On<string,string>("sendAlarm", ( sensorString,valorString) =>
            {
                OnMessageReceivedAlarm?.Invoke(sensorString,valorString);

                Sensor sensor = JsonConvert.DeserializeObject<Sensor>(sensorString);
                ValoresSensores valor = JsonConvert.DeserializeObject<ValoresSensores>(valorString);

                // MessagingCenter.Send<string, string>("alarm", "Notify2", alarm);
                string tipo=string.Empty;
                Debug.WriteLine("recebeu sensor  " + sensor);
                if(sensor.ValorMax<valor.Valor ){
                    tipo = "ultrapassou o valor maximo";
                }else{
                    tipo = "ultrapassou o valor minimo";
                }

                CrossLocalNotifications.Current.Show("SENSOR ALARM", "O sensor "+sensor.NomeSensor+" " +tipo+" com o valor de "+valor.Valor.ToString());

              //  CrossLocalNotifications.Current.Show("ALARM", "sensor"+sensor+" Valor"+valor,1, DateTime.Now);
                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });

            hubProxy.On<List<UserChat>>("updateUserList", (lista) =>
            {
                OnMessageReceivedLista?.Invoke(lista );
               // List<UserChat> listaUsers = JsonConvert.DeserializeObject<List<UserChat>>(lista);
                //UserList.Clear();
                //UserList= lista;
                if(UserList!=null){
                  //  Debug.WriteLine("Lista de utilizadores 2:" +UserList.Count);
                    MessagingCenter.Send<string, List<UserChat>>("UserList", "Notify4", lista);
                }

                //_dialogService.ShowAlertAsync(data.ToString(),"username","OK");
            });

            hubProxy.On<string>("on", (id) =>
            {
                OnMessageReceivedIsOn?.Invoke(id);
                JoinRoom(Settings.Role);
                Debug.WriteLine("AKIII");
                Registar(Settings.Username, Settings.idUser);
                Debug.WriteLine("IN SIGNAL Connection Id = " + hubConnection.ConnectionId);
                MessagingCenter.Send<string>("On", "Notify4");
            });

            hubProxy.On<string>("onSecond", (id) =>
            {
                OnMessageReceivedIsOn?.Invoke(id);
                JoinRoom(Settings.Role);
                Debug.WriteLine("AKIII SECOND");
                SecondRegistar(Settings.Username, Settings.idUser);
                MessagingCenter.Send<string>("On", "Notify5");
            });


            hubConnection.Start();


        }


        //  Clients.All.sendMessage(message);
        public void SendMessage(string message)
        {
            //nome do metodo no asp.net
            hubProxy.Invoke("Send", message);
        }

        public void MsgToGroup(string nome, string message, string roomName)
        {
            hubProxy.Invoke("MsgToGroup", nome, message, roomName);
        }

        public async void JoinRoom(string roomName)
        {
            //erro aqui
             await hubProxy.Invoke("JoinRoom", roomName);
        }

        public void JoinRoomSensor(string roomName)
        {
            RoomChart = roomName;
            hubProxy.Invoke("JoinRoom", roomName);
        }

        public void LeaveRoomSensor()
        {
            Debug.WriteLine("LeaveRoomSensor");
            if(RoomChart!=null){
                hubProxy.Invoke("LeaveRoom", RoomChart);
                RoomChart = null;
            }
           
        }

        public void LeaveRoom(string roomName)
        {
            hubProxy.Invoke("LeaveRoom", roomName);
        }



        public  void MsgToUser(string name, string message, string connectionId, string toUser)
        {
            hubProxy.Invoke("MsgToUser", name, message, connectionId, toUser);
        }

        //MsgToUser(int user, string message, string connectionId)





        public void MsgToAll(UserChat user, string message)
        {
            
            hubProxy.Invoke("MsgToAll", user, message);
        }

       
      



        public Task StartAsync()
        {
            
            return hubConnection.Start();
        }

        public bool IsConnectedOrConnecting
        {
            get
            {
                return hubConnection.State != ConnectionState.Disconnected;
            }
        }

        public List<UserChat> getUsers()
        {
           
         return UserList;
        }

        public ConnectionState ConnectionState 
        { 
            get { return hubConnection.State; } 
        }

        public string getConnectionId(){
            return hubConnection.ConnectionId;
        }

        public void Registar(string name,string idUser){
             hubProxy.Invoke("Registar",name ,idUser);
        }

        public void SecondRegistar(string name, string idUser)
        {
            hubProxy.Invoke("SecondRegistar", name, idUser);
        }

        public void SendUserList()
        {
            hubProxy.Invoke("SendUserList");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
