using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;
using Industria4.Models.User;
using Syncfusion.SfChart.XForms;

namespace Industria4.DataServices.DataSensores
{
    public interface IValoresSensorService
    {
        Task<ObservableCollection<ValoresSensores>> GetValoresSensorFromSensorAsync(int idSensor,string DateInit, string TimeInit, string DateEnd,string TimeEnd);
        Task<ObservableCollection<ValoresSensoresContagem>> GetValoresSensorFromSensorContagemAsync(int idSensor, string DateInit, string DateEnd);
        Task<decimal> GetTotalFromSensorContagemAsync(int idSensor);
        Task<Sensor> ResetSensorContagemAsync(int idSensor, Sensor sensor);
    }
}
