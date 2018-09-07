using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Rooms
{
    public interface IRoomsService
    {
        Task<ObservableCollection<Room>> GetRoomsAsync();
    }
}
