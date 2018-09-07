using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using Industria4.DataServices.DataSensores;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.Sensores;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Industria4.Services.SignalR;
using Industria4.ViewModels.Base;
using Newtonsoft.Json;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class DataAnalyseViewModel : ViewModelBase
    {
        private readonly IMaquinaService _maquinaService;
        private readonly ISensoresService _sensoresService;
        private readonly IValoresSensorService _valoresSensor;
        private ObservableCollection<ValoresSensores> auxMaxMin;
        private ObservableCollection<ValoresSensoresContagem> auxContagem;


        public DataAnalyseViewModel(IMaquinaService maquinaService, ISensoresService sensoresService, IValoresSensorService valoresSensor,ISIgnalRService sIgnalRService)
        {
            _maquinaService = maquinaService;
            _sensoresService = sensoresService;
            _valoresSensor = valoresSensor;
        

        }

        public override async Task InitializeAsync(object navigationData)
        {
            auxMaxMin = new ObservableCollection<ValoresSensores>();
           
            if (Settings.Role.Equals("Func") )
            {
                ObservableCollection<Maquina> maquinas = new ObservableCollection<Maquina>(await _maquinaService.GetMaquinasFromUserAsync(Settings.idUser));
                ObservableCollection<Sensor> sensores = new ObservableCollection<Sensor>();

                Debug.WriteLine("after maquinas service");

                foreach (var item in maquinas)
                {
                    ListaMaquinasSensores.Add(new GroupedMaquinasSensores() { maquina = item });
                }

                foreach (var item in ListaMaquinasSensores)
                {
                    sensores = await _sensoresService.GetSensoresFromMaquinaAsync(item.maquina.Id);
                    Debug.WriteLine("num de sensores " + sensores.Count);
                    foreach (var item2 in sensores)
                    {
                        item.Add(item2);
                    }

                }
            }else if(Settings.Role.Equals("AdminLocal")){
                //ir buscar as maquinas das fabricas dele
                ObservableCollection<Maquina> maquinas = new ObservableCollection<Maquina>(await _maquinaService.GetMaquinasFromUserAsync(Settings.idUser));
                ObservableCollection<Sensor> sensores = new ObservableCollection<Sensor>();

                Debug.WriteLine("after maquinas service");

                foreach (var item in maquinas)
                {
                    ListaMaquinasSensores.Add(new GroupedMaquinasSensores() { maquina = item });
                }

                foreach (var item in ListaMaquinasSensores)
                {
                    sensores = await _sensoresService.GetSensoresFromMaquinaAsync(item.maquina.Id);
                    Debug.WriteLine("num de sensores " + sensores.Count);
                    foreach (var item2 in sensores)
                    {
                        item.Add(item2);
                    }

                }
            }else{
                //ir buscar as maquinas das fabricas dele
                ObservableCollection<Maquina> maquinas = new ObservableCollection<Maquina>(await _maquinaService.GetMaquinasAsync());
                ObservableCollection<Sensor> sensores = new ObservableCollection<Sensor>();

                Debug.WriteLine("after maquinas service");

                foreach (var item in maquinas)
                {
                    ListaMaquinasSensores.Add(new GroupedMaquinasSensores() { maquina = item });
                }

                foreach (var item in ListaMaquinasSensores)
                {
                    sensores = await _sensoresService.GetSensoresFromMaquinaAsync(item.maquina.Id);

                    Debug.WriteLine("num de sensores " + sensores.Count);
                    foreach (var item2 in sensores)
                    {
                        item.Add(item2);
                    }

                }
            }
     
               
        
        }

        private ObservableCollection<GroupedMaquinasSensores> _listaMaquinasSensores = new ObservableCollection<GroupedMaquinasSensores>();
        public ObservableCollection<GroupedMaquinasSensores> ListaMaquinasSensores
        {
            get
            {
                return _listaMaquinasSensores;
            }
            set
            {
                _listaMaquinasSensores = value;
                RaisePropertyChanged(() => ListaMaquinasSensores);
            }
        }

        private Sensor _sensor;
        public Sensor sensor
        {
            get
            {
                return _sensor;
            }
            set
            {
                _sensor = value;
                RaisePropertyChanged(() => sensor);
            }
        }


        private string _nomeGrafico;
        public string nomeGrafico
        {
            get
            {
                return _nomeGrafico;
            }
            set
            {
                _nomeGrafico = value;
                RaisePropertyChanged(() => nomeGrafico);
            }
        }

        private double _Media;
        public double Media
        {
            get
            {
                return _Media;
            }
            set
            {
                _Media = value;
                RaisePropertyChanged(() => Media);
            }
        }

        private decimal _Total= 0;
        public decimal Total
        {
            get
            {
                return _Total;
            }
            set
            {
                _Total = value;
                RaisePropertyChanged(() => Total);
            }
        }

        private bool _IsMaxMin = false;
        public bool IsMaxMin
        {
            get
            {
                return _IsMaxMin;
            }
            set
            {
                _IsMaxMin = value;
                RaisePropertyChanged(() => IsMaxMin);
            }
        }

        private bool _IsCorrente = false;
        public bool IsCorrente
        {
            get
            {
                return _IsCorrente;
            }
            set
            {
                _IsCorrente = value;
                RaisePropertyChanged(() => IsCorrente);
            }
        }

        private bool _IsContagem = false;
        public bool IsContagem
        {
            get
            {
                return _IsContagem;
            }
            set
            {
                _IsContagem = value;
                RaisePropertyChanged(() => IsContagem);
            }
        }

        private string _labelEixoX;
        public string labelEixoX
        {
            get
            {
                return _labelEixoX;
            }
            set
            {
                _labelEixoX = value;
                RaisePropertyChanged(() => labelEixoX);
            }
        }

        private string _labelEixoY;
        public string labelEixoY
        {
            get
            {
                return _labelEixoY;
            }
            set
            {
                _labelEixoY = value;
                RaisePropertyChanged(() => labelEixoY);
            }
        }

        public class GroupedMaquinasSensores : ObservableCollection<Sensor>
        {
            public Maquina maquina { get; set; }

        }

        public ObservableCollection<ChartDataPoint> _listaSF { get; set; }
        public ObservableCollection<ChartDataPoint> ListaSF
        {
            get
            {
                return _listaSF;
            }
            set
            {
                _listaSF = value;
                RaisePropertyChanged(() => ListaSF);
            }
        }


        public ObservableCollection<ChartDataPoint> _ListaSFContagem { get; set; }
        public ObservableCollection<ChartDataPoint> ListaSFContagem
        {
            get
            {
                return _ListaSFContagem;
            }
            set
            {
                _ListaSFContagem = value;
                RaisePropertyChanged(() => ListaSFContagem);
            }
        }

   /*     public bool _chartIsVisible { get; set; }
        public bool ChartIsVisible
        {
            get
            {
                return _chartIsVisible;
            }
            set
            {
                _chartIsVisible = value;
                RaisePropertyChanged(() => ChartIsVisible);
            }
        }*/

        public double _maximo { get; set; }
        public double Maximo
        {
            get
            {
                return _maximo;
            }
            set
            {
                _maximo = value;
                RaisePropertyChanged(() => Maximo);
            }
        }

        public double _minimo { get; set; }
        public double Minimo
        {
            get
            {
                return _minimo;
            }
            set
            {
                _minimo = value;
                RaisePropertyChanged(() => Minimo);
            }
        }

      


        public SfChart _ChartMaxMin = new SfChart();
        public SfChart ChartMaxMin
        {
            get
            {
                return _ChartMaxMin;
            }
            set
            {
                _ChartMaxMin = value;
                RaisePropertyChanged(() => ChartMaxMin);
            }
        }

        public SfChart _ChartCorrente = new SfChart();
        public SfChart ChartCorrente
        {
            get
            {
                return _ChartCorrente;
            }
            set
            {
                _ChartCorrente = value;
                RaisePropertyChanged(() => ChartCorrente);
            }
        }

        public SfChart _ChartContagem = new SfChart();
        public SfChart ChartContagem
        {
            get
            {
                return _ChartContagem;
            }
            set
            {
                _ChartContagem = value;
                RaisePropertyChanged(() => ChartContagem);
            }
        }

        public DateTime _selectedDateInit = DateTime.Now;
        public DateTime SelectedDateInit
        {
            get
            {
                return _selectedDateInit;
            }
            set
            {
                _selectedDateInit = value;
                RaisePropertyChanged(() => SelectedDateInit);
            }
        }

        public TimeSpan _selectedTimeInit = DateTime.Now.TimeOfDay.Subtract(new TimeSpan(1,0,0));
        public TimeSpan SelectedTimeInit
        {
            get
            {
                return _selectedTimeInit;
            }
            set
            { 
                _selectedTimeInit = value;
                RaisePropertyChanged(() => SelectedTimeInit);
            }
        }
     
        public DateTime _selectedDateEnd = DateTime.Now;
        public DateTime SelectedDateEnd
        {
            get
            {
                return _selectedDateEnd;
            }
            set
            {
                _selectedDateEnd = value;
                RaisePropertyChanged(() => SelectedDateEnd);
            }
        }

        public TimeSpan _selectedTimeEnd = DateTime.Now.TimeOfDay;
        public TimeSpan SelectedTimeEnd
        {
            get
            {
                return _selectedTimeEnd;
            }
            set
            {
                _selectedTimeEnd = value;
                RaisePropertyChanged(() => SelectedTimeEnd);
            }
        }


  /*      public FastLineSeries _series { get; set; }
        public FastLineSeries series
        {
            get
            {
                return _series;
            }
            set
            {
                _series = value;
                RaisePropertyChanged(() => series);
            }
        }
*/
        public ICommand GetItemViewCommand => new Command<object>(async (item) => await GetItemView(item));

        private async Task GetItemView(object teste)
        {
            if (teste is Sensor)
            {
                
                if(((Sensor)teste).TiposGrandeza.Grandeza.ToUpper().Equals("CORRENTE")){ 

                    IsCorrente = true;

                    sensor = ((Sensor)teste);
                    nomeGrafico = ((Sensor)teste).NomeSensor;
                    labelEixoX = "Data";
                    labelEixoY = "Valor";

              
                    await GetItemDetailsRealDataMaxMinAsync();
                   

                }else if(((Sensor)teste).TiposGrandeza.Grandeza.ToUpper().Equals("CONTAGEM")){

                    IsContagem = true;

                    sensor = ((Sensor)teste);
                    nomeGrafico = ((Sensor)teste).NomeSensor;
                    labelEixoX = "Data";
                    labelEixoY = "Valor";


                    await GetItemDetailsRealDataContagemAsync();

                }else{  //tipo maxMin 

                    IsMaxMin = true;

                    sensor = ((Sensor)teste);
                    nomeGrafico = ((Sensor)teste).NomeSensor;
                    labelEixoX = "Data";
                    labelEixoY = "Valor";

                    Maximo = ((Sensor)teste).ValorMax;
                    Minimo = ((Sensor)teste).ValorMin;

                    await GetItemDetailsRealDataMaxMinAsync();
                }

                
               
            }
            if (teste is Maquina)
            {
                Debug.WriteLine("maquina");
            }
         
        }

        public ICommand GetItemDetailsMaxMinCommand => new Command(async () => await GetItemDetailsMaxMinAsync());

        private async Task GetItemDetailsMaxMinAsync()
        {
      
            auxMaxMin =await _valoresSensor.GetValoresSensorFromSensorAsync(sensor.Id, SelectedDateInit.ToString("yyyy-MM-dd"), SelectedTimeInit.ToString(@"hh\-mm"),SelectedDateEnd.ToString("yyyy-MM-dd"),SelectedTimeEnd.ToString(@"hh\-mm"));

            Debug.WriteLine("table have rows" + auxMaxMin.Count());

            decimal total = 0;
            foreach (var item in auxContagem)
            {
                total += item.Unidades;
            }

            if(total!=0){
                Media = (double)(total / auxContagem.Count);
            }
           
            CriarGraficoAsync();

            auxMaxMin.Clear();

        }

        public ICommand GetItemDetailsContagemCommand => new Command(async () => await GetItemDetailsContagemAsync());

        private async Task GetItemDetailsContagemAsync()
        {

            auxContagem = await _valoresSensor.GetValoresSensorFromSensorContagemAsync(sensor.Id, SelectedDateInit.ToString("yyyy-MM-dd"), SelectedDateEnd.ToString("yyyy-MM-dd"));

            Debug.WriteLine("table have rows" + auxContagem.Count());

            decimal total = 0;
            foreach (var item in auxContagem)
            {
                total += item.Unidades;
            }

            if (total != 0){
                Media = (double)(total / auxContagem.Count);
            }
        

            CriarGraficoContagemAsync();

            auxContagem.Clear();

        }

        public ICommand GetItemDetailsRealDataContagemCommand => new Command(async () => await GetItemDetailsRealDataContagemAsync());

        private async Task GetItemDetailsRealDataContagemAsync()
        {

            auxContagem = await _valoresSensor.GetValoresSensorFromSensorContagemAsync(sensor.Id, SelectedDateInit.ToString("yyyy-MM-dd"), SelectedDateEnd.ToString("yyyy-MM-dd"));

            Debug.WriteLine("table have rows" + auxContagem.Count());

            CriarGraficoContagemAsync();

            auxContagem.Clear();

        }

        public void CriarGraficoAsync()
        {
            ChartMaxMin = new SfChart();

            ListaSF = new ObservableCollection<ChartDataPoint>();

            double total = 0;
            ChartMaxMin.SuspendSeriesNotification();
            foreach (var item in auxMaxMin)
            {
               
                ListaSF.Add(new ChartDataPoint(item.CreatedAtSource, item.Valor));
                total += item.Valor;
            }
            ChartMaxMin.ResumeSeriesNotification();  
            if(total!=0){
                Media = (double)(total / auxContagem.Count);

            }
        }

        public async Task CriarGraficoContagemAsync()
        {
            ChartContagem = new SfChart();

            ListaSFContagem = new ObservableCollection<ChartDataPoint>();

            decimal total = 0;
            ChartContagem.SuspendSeriesNotification();
            foreach (var item in auxContagem)
            {

                ListaSFContagem.Add(new ChartDataPoint(item.Data, (double)item.Unidades));
                total += item.Unidades;
            }
            ChartContagem.ResumeSeriesNotification();
            if(total!=0){
                Media = (double)(total / auxContagem.Count);     
            }
           
            Total = await _valoresSensor.GetTotalFromSensorContagemAsync(sensor.Id);
        }
    

        public ICommand GetItemDetailRealDataMaxMinCommand => new Command(async () => await GetItemDetailsRealDataMaxMinAsync());

        private async Task GetItemDetailsRealDataMaxMinAsync()
        {

            TimeSpan timeForRealChart = new TimeSpan(TimeSpan.TicksPerHour -1); 
            SelectedTimeInit = timeForRealChart;
            DateTime dataForRealChart = SelectedDateInit.Add(timeForRealChart);
            auxMaxMin = await _valoresSensor.GetValoresSensorFromSensorAsync(sensor.Id, SelectedDateInit.ToString("yyyy-MM-dd"), SelectedTimeInit.ToString(@"hh\-mm"), SelectedDateEnd.ToString("yyyy-MM-dd"), SelectedTimeEnd.ToString(@"hh\-mm"));

            Debug.WriteLine("  _sIgnalRService.JoinRoom(sensor + sensor.Id);" );

            SignalRService.LeaveRoomSensor();

            //assinar grupo dos svalores dos sensores deste sensor
            SignalRService.JoinRoomSensor("sensorChart" + sensor.Id);

            CriarGraficoAsync();

            MessagingCenter.Subscribe<string, string>("SendValor", "Notify", (sender, arg) =>
            {
                Debug.WriteLine("no subscribe recebeu : " + arg.ToString());
                SmallValoresSensores valorSensor = JsonConvert.DeserializeObject<SmallValoresSensores>(arg);

                   actualizarGraficoMaxMin(valorSensor);

            });

            auxMaxMin.Clear();

        }
       

        public ICommand ResetSensorContagemCommand => new Command(async () => await ResetSensorContagem());

        private async Task ResetSensorContagem()
        {
            await _valoresSensor.ResetSensorContagemAsync(sensor.Id,sensor);
        
        }

        public void actualizarGraficoMaxMin(SmallValoresSensores valorSensor)
        {
            ChartMaxMin = new SfChart();

            ListaSF = new ObservableCollection<ChartDataPoint>();

            ChartMaxMin.SuspendSeriesNotification();

            ListaSF.Add(new ChartDataPoint(valorSensor.CreatedAtSource, valorSensor.Valor));

            ChartMaxMin.ResumeSeriesNotification();  
        }




    }

}
