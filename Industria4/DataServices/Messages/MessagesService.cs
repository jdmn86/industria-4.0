using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.DataServices.Base;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.Dialog;

namespace Industria4.DataServices.Messages
{
    public class MessagesService : IMessagesService
    {
        private readonly IRequestProvider _requestProvider;
   //     private readonly IUserDialogs _userDialogs;

        public MessagesService(IRequestProvider requestProvider )
        {
            _requestProvider = requestProvider;
//            _userDialogs = userDialogs;

        }

        public async Task<ObservableCollection<Message>> GetMessagesAsync()
        {
            ObservableCollection<Message> message = null;

            if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                throw new ArgumentException("message", nameof(Settings.AccessToken));
            }

            try
            {
                message = await _requestProvider.GetAsync<ObservableCollection<Message>>("api/messages");
            }  catch (HttpRequestExceptionEx exception) when (exception.HttpCode == System.Net.HttpStatusCode.NotFound || exception.HttpCode == System.Net.HttpStatusCode.BadRequest)
            {

                await App.Current.MainPage.DisplayAlert("Não consegue connectar servidor", "Erro", "Ok");
                return null;

            }
            catch (ServiceAuthenticationException ex)
            {

                await App.Current.MainPage.DisplayAlert(ex.Content, "Erro", "Ok");
                return null;
            }

            return message;
        }

        public async Task<Message> SaveMessagesAsync(Message msg)
        {

            var result = await _requestProvider.PostAsync("api/messages", msg);

            return result;

        }

        public async Task<Message> EditMessagesAsync(Message msg)
        {

            var result = await _requestProvider.PutAsync("api/messages/" + msg.MessageId, msg);

            return result;

        }

        public async Task<ObservableCollection<User>> AddUserListToMessages(int messageId, ObservableCollection<User> userList)
        {

            var result = await _requestProvider.PutAsync("api/messages/" + messageId.ToString() + "/user", userList);

            return result;

        }

        public async Task<User> AddUserToMessages(int messageId, User user)
     {

            var result = await _requestProvider.PutAsync("api/messages/" + messageId.ToString() + "/user", user);

            return result;

     }

     public async Task<User> RemoveUserToMessages(int messageId, User user)
     {
            var result = await _requestProvider.PutAsync("api/messages/" + messageId.ToString() + "/user/remove", user);

            return result;
 
     } 

        public async Task<Message> DeleteMessages(Message message)
     {
            var result = await _requestProvider.DeleteAsync<Message>("api/messages/" + message.MessageId.ToString() );

            return result;
 
     } 

    }
}
