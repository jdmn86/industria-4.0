using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.TipoPorta
{
    public interface ITiposPortaService
    {
        Task<ObservableCollection<TiposPorta>> GetTiposPortaAsync();
       
    }
}
