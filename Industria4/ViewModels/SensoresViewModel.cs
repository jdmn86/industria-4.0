using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Sensores;
using Industria4.Models;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class SensoresViewModel : ViewModelBase
    {
        private readonly ISensoresService _sensoresService;

        public ObservableCollection<Sensor> _sensores { get; private set; }
        public ObservableCollection<Sensor> Sensores
        {
            get { return _sensores; }
            set
            {
                _sensores = value;
                RaisePropertyChanged(() => Sensores);
            }
        }

        public SensoresViewModel(ISensoresService sensoresService)
        {
            _sensoresService = sensoresService;
        }

        public override async Task InitializeAsync(object navigationData)
        {

            if (Sensores == null)
                Sensores = new ObservableCollection<Sensor>();

            Sensores = await _sensoresService.GetSensoresAsync();


            await base.InitializeAsync(navigationData);
        }


        public ICommand GetSensorDetailsCommand => new Command<Sensor>(async (item) => await GetSensorDetailsAsync(item));
        private async Task GetSensorDetailsAsync(Sensor sensor)
        {
            await NavigationService.NavigateToAsync<SensorViewModel>(sensor);
        }


    }
}
