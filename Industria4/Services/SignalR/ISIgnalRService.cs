using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Industria4.Models.User;

namespace Industria4.Services.SignalR
{
    public interface ISIgnalRService
    {
        Task StartAsync();
       // void SendMessage(string username, string text);
       // Task SendToGroup(string message, string name, string roomName);
        string getConnectionId();


        void MsgToGroup(string nome, string message, string roomName);

        void MsgToAll(UserChat user, string message);
        void MsgToUser(string nome, string message, string connectionId,string toUser);
        void JoinRoom(string roomName);
        void LeaveRoom(string roomName);

        void LeaveRoomSensor();
        void JoinRoomSensor(string roomName);

        void SendMessage(string message);

        void Registar(string Name,string idUser);
        void SendUserList();
        List<UserChat> getUsers();

    }
}
