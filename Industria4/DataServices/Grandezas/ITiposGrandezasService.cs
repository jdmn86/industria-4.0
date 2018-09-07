using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Grandezas
{
    public interface ITiposGrandezasService
    {
        Task<ObservableCollection<TiposGrandeza>> GetTiposGrandezasAsync();
    }
}
