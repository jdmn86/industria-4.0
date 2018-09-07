using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Sensores
{
    public interface ISensoresService
    {
        Task<ObservableCollection<Sensor>> GetSensoresAsync();
        Task<Sensor> SaveSensorAsync(Sensor sensor);
        Task<Sensor> EditSensorAsync(Sensor sensor);
        Task<ObservableCollection<Sensor>> GetSensoresFromMaquinaAsync(int idMaquina);
        Task<ObservableCollection<Sensor>> GetSensoresFromModuloAsync(int idModulo);
    }
}
