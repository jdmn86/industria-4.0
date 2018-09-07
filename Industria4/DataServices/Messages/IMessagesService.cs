using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;
using Industria4.Models.User;

namespace Industria4.DataServices.Messages
{
    public interface IMessagesService
    {
        Task<ObservableCollection<Message>> GetMessagesAsync();
        Task<Message> SaveMessagesAsync(Message msg);
        Task<Message> EditMessagesAsync(Message msg);
        Task<ObservableCollection<User>> AddUserListToMessages(int messageId, ObservableCollection<User> userList);
        Task<User> AddUserToMessages(int messageId, User user);
        Task<User> RemoveUserToMessages(int messageId, User user);
        Task<Message> DeleteMessages(Message message);
    }
}
