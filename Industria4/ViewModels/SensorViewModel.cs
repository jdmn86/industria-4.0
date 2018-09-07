using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Grandezas;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.DataServices.Sensores;
using Industria4.DataServices.TipoPorta;
using Industria4.Models;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class SensorViewModel : ViewModelBase
    {
        private readonly ISensoresService _sensoresService;
        private readonly IModuloAquisicaoService _modulosAquisicaoService;
        private readonly ITiposPortaService _tiposPortaService;
        private readonly ITiposGrandezasService _tiposGrandezasService;
//        private readonly IUserDialogs _userDialogs;

        public SensorViewModel(ISensoresService sensoresService,
                               IModuloAquisicaoService modulosAquisicaoService,
                               ITiposPortaService tiposPortaService,
                               ITiposGrandezasService tiposGrandezasService)
              //                 IUserDialogs userDialogs)
        {
            _sensoresService = sensoresService;
            _modulosAquisicaoService = modulosAquisicaoService;
            _tiposPortaService = tiposPortaService;
            _tiposGrandezasService = tiposGrandezasService;
      //      _userDialogs = userDialogs;

          //  GetSensorDetailsCommand = new Command<Sensor>(async (item) => await GetSensorDetailsAsync(item));

            ValidateCodigoSensorCommand = new Command(() => ValidateCodigoSensor());
            ValidateNomeSensorCommand = new Command(() => ValidateNomeSensor());
            ValidateValorMaxCommand = new Command(() => ValidateValorMax());
            ValidateValorMinCommand = new Command(() => ValidateValorMin());
            ValidateObservacoesCommand = new Command(() => ValidateObservacoes());
            ValidateTempoCommand = new Command(() => ValidateTempo());

            ValidateFatorCommand = new Command(() => ValidateFator());
            ValidateCodeCommand = new Command(() => ValidateCode());


            AddValidations();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            ModulosAquisicao = await _modulosAquisicaoService.GetModulosAquisicaoAsync();

            TiposPorta = await _tiposPortaService.GetTiposPortaAsync();

            TiposGrandezas = await _tiposGrandezasService.GetTiposGrandezasAsync();


            if (navigationData is Sensor)
            {
                sensor = navigationData as Sensor;

                NewSensor = false;
                EditSensor = true;

                foreach (var item in ModulosAquisicao)
                {
                    if (item.Id.Equals(sensor.IdModuloAquisicao))
                    {
                        SelectedItemModuloAquisicao = item;
                    }
                }

                foreach (var item in TiposPorta)
                {
                    if (item.Id.Equals(sensor.IdTipoPorta))
                    {
                        SelectedItemTipoPorta = item;
                    }
                }

                foreach (var item in TiposGrandezas)
                {
                    if (item.Id.Equals(sensor.IdGrandeza))
                    {
                        SelectedItemTipoGrandeza = item;
                    }
                }

           
            }
            else
            {
                sensor = new Sensor();

                NewSensor = true;
                EditSensor = false;

            }

            //await GetMaquinasAsync();  
            //  await base.InitializeAsync(navigationData);
        }

        #region Control Views definition

        private bool _IsValid = false;
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; RaisePropertyChanged(() => IsValid); }
        }

        public bool _NewSensor = true;
        public bool NewSensor
        {
            get { return _NewSensor; }
            set
            {
                _NewSensor = value;
                RaisePropertyChanged(() => NewSensor);
            }
        }

        public bool _EditSensor = false;
        public bool EditSensor
        {
            get { return _EditSensor; }
            set
            {
                _EditSensor = value;
                RaisePropertyChanged(() => EditSensor);
            }
        }

        public bool _CodeVisible = false;
        public bool CodeVisible
        {
            get { return _CodeVisible; }
            set
            {
                _CodeVisible = value;
                RaisePropertyChanged(() => CodeVisible);
            }
        }

        public bool _FatorVisible = false;
        public bool FatorVisible
        {
            get { return _FatorVisible; }
            set
            {
                _FatorVisible = value;
                RaisePropertyChanged(() => FatorVisible);
            }
        }

        #endregion

        #region Model definition

        public Sensor _sensor { get; private set; }
        public Sensor sensor
        {
            get { return _sensor; }
            set
            {
                _sensor = (Sensor)value;

                // Id = value.Id;
                CodigoSensor.Value = value.CodigoSensor;
                IdModuloAquisicao = value.IdModuloAquisicao;
                NomeSensor.Value = value.NomeSensor;
                ValorMax.Value = value.ValorMax;
                ValorMin.Value = value.ValorMin;
                Observacoes.Value = value.Observacoes;
                CreatedAt = value.CreatedAt;
                Active = value.Active;
                Tempo.Value = value.Tempo;
                Nporta.Value = value.Nporta;
                IdTipoPorta = value.IdTipoPorta;
                IdGrandeza = value.IdGrandeza;

                Code.Value = value.Code;
                Fator.Value = value.Fator;

                RaisePropertyChanged(() => sensor);
            }
        }

        private ValidatableObject<string> _CodigoSensor = new ValidatableObject<string>();
        public ValidatableObject<string> CodigoSensor
        {
            get
            {
                return _CodigoSensor;
            }
            set
            {
                _CodigoSensor = value;
                RaisePropertyChanged(() => CodigoSensor);
            }
        }

        private ValidatableObject<string> _NomeSensor = new ValidatableObject<string>();
        public ValidatableObject<string> NomeSensor
        {
            get
            {
                return _NomeSensor;
            }
            set
            {
                _NomeSensor = value;
                RaisePropertyChanged(() => NomeSensor);
            }
        }

        private ValidatableObject<double> _ValorMax = new ValidatableObject<double>();
        public ValidatableObject<double> ValorMax
        {
            get
            {
                return _ValorMax;
            }
            set
            {
                _ValorMax = value;
                RaisePropertyChanged(() => ValorMax);
            }
        }

        private ValidatableObject<double> _ValorMin = new ValidatableObject<double>();
        public ValidatableObject<double> ValorMin
        {
            get
            {
                return _ValorMin;
            }
            set
            {
                _ValorMin = value;
                RaisePropertyChanged(() => ValorMin);
            }
        }

        private ValidatableObject<string> _Observacoes = new ValidatableObject<string>();
        public ValidatableObject<string> Observacoes
        {
            get
            {
                return _Observacoes;
            }
            set
            {
                _Observacoes = value;
                RaisePropertyChanged(() => Observacoes);
            }
        }

        private ValidatableObject<int> _Tempo = new ValidatableObject<int>();
        public ValidatableObject<int> Tempo
        {
            get
            {
                return _Tempo;
            }
            set
            {
                _Tempo = value;
                RaisePropertyChanged(() => Tempo);
            }
        }

        private ValidatableObject<double> _Fator = new ValidatableObject<double>();
        public ValidatableObject<double> Fator
        {
            get
            {
                return _Fator;
            }
            set
            {
                _Fator = value;
                RaisePropertyChanged(() => Fator);
            }
        }

        private ValidatableObject<long> _Nporta = new ValidatableObject<long>();
        public ValidatableObject<long> Nporta
        {
            get
            {
                return _Nporta;
            }
            set
            {
                _Nporta = value;
                RaisePropertyChanged(() => Nporta);
            }
        }

        private ValidatableObject<int> _Code = new ValidatableObject<int>();
        public ValidatableObject<int> Code
        {
            get
            {
                return _Code;
            }
            set
            {
                _Code = value;
                RaisePropertyChanged(() => Code);
            }
        }

        private int _IdModuloAquisicao;
        public int IdModuloAquisicao
        {
            get
            {
                return _IdModuloAquisicao;
            }
            set
            {
                _IdModuloAquisicao = value;
                RaisePropertyChanged(() => IdModuloAquisicao);
            }
        }

        private int _IdTipoPorta;
        public int IdTipoPorta
        {
            get
            {
                return _IdTipoPorta;
            }
            set
            {
                _IdTipoPorta = value;
                RaisePropertyChanged(() => IdTipoPorta);
            }
        }
        private int _IdGrandeza;
        public int IdGrandeza
        {
            get
            {
                return _IdGrandeza;
            }
            set
            {
                _IdGrandeza = value;
                RaisePropertyChanged(() => IdGrandeza);
            }
        }
                
        private DateTime _CreatedAt { get; set; }
        public DateTime CreatedAt
        {
            get
            {
                return _CreatedAt;
            }
            set
            {
                _CreatedAt = value;
                RaisePropertyChanged(() => CreatedAt);
            }
        }

        private bool _Active { get; set; }
        public bool Active
        {
            get
            {
                return _Active;
            }
            set
            {
                _Active = value;
                RaisePropertyChanged(() => Active);
            }
        }
        #endregion


        #region Data definition


        public ModuloAquisicao _selectedItemModuloAquisicao { get; private set; }
        public ModuloAquisicao SelectedItemModuloAquisicao
        {
            get { return _selectedItemModuloAquisicao; }
            set
            {
                _selectedItemModuloAquisicao = value;
                RaisePropertyChanged(() => SelectedItemModuloAquisicao);
            }
        }

        private ObservableCollection<ModuloAquisicao> _modulosAquisicao = new ObservableCollection<ModuloAquisicao>();
        public ObservableCollection<ModuloAquisicao> ModulosAquisicao
        {
            get { return _modulosAquisicao; }
            set
            {
                _modulosAquisicao = value;
                RaisePropertyChanged(() => ModulosAquisicao);
            }
        }

        public TiposPorta _selectedItemTipoPorta { get; private set; }
        public TiposPorta SelectedItemTipoPorta
        {
            get { return _selectedItemTipoPorta; }
            set
            {
                _selectedItemTipoPorta = value;
                populateNumPorta(value.NrMinPorta,value.NrMaxPorta);
                if(SelectedItemTipoPorta.Tporta.Equals("i2c")){
                    CodeVisible = true;
                    FatorVisible = false;
                }else if(SelectedItemTipoPorta.Tporta.Equals("analogica")){
                    FatorVisible = true;
                    CodeVisible = false;
                }
                RaisePropertyChanged(() => SelectedItemTipoPorta);

            }
        }

        public void populateNumPorta(long min,long max){

            ObservableCollection<long> aux = new ObservableCollection<long>();

            for (long i = min; i < max; i++)
            {
                aux.Add(i);
            }
            NumPorta = aux;
        }

        private ObservableCollection<TiposPorta> _tiposPorta = new ObservableCollection<TiposPorta>();
        public ObservableCollection<TiposPorta> TiposPorta
        {
            get { return _tiposPorta; }
            set
            {
                _tiposPorta = value;
                RaisePropertyChanged(() => TiposPorta);
            }
        }

        private ObservableCollection<long> _NumPorta { get; set; }
        public ObservableCollection<long> NumPorta
        {
            get { return _NumPorta; }
            set
            {
                _NumPorta = value;
                RaisePropertyChanged(() => NumPorta);
            }
        }

        public long _selectedNumPorta { get; private set; }
        public long SelectedNumPorta
        {
            get { return _selectedNumPorta; }
            set
            {
                _selectedNumPorta = value;
                RaisePropertyChanged(() => SelectedNumPorta);

            }
        }


        public TiposGrandeza _selectedItemTipoGrandeza { get; private set; }
        public TiposGrandeza SelectedItemTipoGrandeza
        {
            get { return _selectedItemTipoGrandeza; }
            set
            {
                _selectedItemTipoGrandeza = value;
                RaisePropertyChanged(() => SelectedItemTipoGrandeza);
            }
        }

        private ObservableCollection<TiposGrandeza> _tiposGrandezas;
        public ObservableCollection<TiposGrandeza> TiposGrandezas
        {
            get { return _tiposGrandezas; }
            set
            {
                _tiposGrandezas = value;
                RaisePropertyChanged(() => TiposGrandezas);
            }
        }

        #endregion

        #region Commands definition

        public ICommand GuardarSensor
        {
            get
            {
                return new Command(async () =>
                {
                    IsValid = Validate();

                    if (IsValid)
                    {
                        Sensor sensorSave;

                        bool result = await App.Current.MainPage.DisplayAlert("Tem a certeza que pretende Guardar?", "Guardar Maquina", "Sim", "Cancel");
                        if (result == false)
                        {
                            Debug.WriteLine("result : " + result);
                            return;
                        }

                        if (NewSensor == true)
                        {
                            if (CodeVisible == true){
                              
                                sensor = new Sensor
                                {
                                    //Id = 0,
                                    CodigoSensor = CodigoSensor.Value,
                                    IdModuloAquisicao = SelectedItemModuloAquisicao.Id,
                                    NomeSensor = NomeSensor.Value,
                                    ValorMax = ValorMax.Value,
                                    ValorMin = ValorMin.Value,
                                    Observacoes = Observacoes.Value,
                                    CreatedAt = DateTime.Now,
                                    Active = Active,
                                    Tempo = Tempo.Value,
                                  //  Fator = null, 
                                    Nporta = Nporta.Value,
                                    IdTipoPorta = SelectedItemTipoPorta.Id,
                                    Code = Code.Value,
                                    IdGrandeza = SelectedItemTipoGrandeza.Id
                                                                         
                                };
                            }else{
                                sensor = new Sensor
                                {
                                    //Id = 0,
                                    CodigoSensor = CodigoSensor.Value,
                                    IdModuloAquisicao = SelectedItemModuloAquisicao.Id,
                                    NomeSensor = NomeSensor.Value,
                                    ValorMax = ValorMax.Value,
                                    ValorMin = ValorMin.Value,
                                    Observacoes = Observacoes.Value,
                                    CreatedAt = DateTime.Now,
                                    Active = Active,
                                    Tempo = Tempo.Value,
                                    Fator = Fator.Value,
                                    Nporta = Nporta.Value,
                                    IdTipoPorta = SelectedItemTipoPorta.Id,
                                  //  Code = null,
                                    IdGrandeza = SelectedItemTipoGrandeza.Id

                                };
                            }

                            var response = await _sensoresService.SaveSensorAsync(sensor);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<SensoresViewModel>();

                            }
                        }
                        else
                        {
                            if (CodeVisible == true)
                            {

                                sensor = new Sensor
                                {
                                    //Id = 0,
                                    CodigoSensor = CodigoSensor.Value,
                                    IdModuloAquisicao = SelectedItemModuloAquisicao.Id,
                                    NomeSensor = NomeSensor.Value,
                                    ValorMax = ValorMax.Value,
                                    ValorMin = ValorMin.Value,
                                    Observacoes = Observacoes.Value,
                                    CreatedAt = DateTime.Now,
                                    Active = Active,
                                    Tempo = Tempo.Value,
                                   // Fator = null,
                                    Nporta = Nporta.Value,
                                    IdTipoPorta = SelectedItemTipoPorta.Id,
                                    Code = Code.Value,
                                    IdGrandeza = SelectedItemTipoGrandeza.Id

                                };
                            }
                            else
                            {
                                sensor = new Sensor
                                {
                                    //Id = 0,
                                    CodigoSensor = CodigoSensor.Value,
                                    IdModuloAquisicao = SelectedItemModuloAquisicao.Id,
                                    NomeSensor = NomeSensor.Value,
                                    ValorMax = ValorMax.Value,
                                    ValorMin = ValorMin.Value,
                                    Observacoes = Observacoes.Value,
                                    CreatedAt = DateTime.Now,
                                    Active = Active,
                                    Tempo = Tempo.Value,
                                    Fator = Fator.Value,
                                    Nporta = Nporta.Value,
                                    IdTipoPorta = SelectedItemTipoPorta.Id,
                                 //   Code = null,
                                    IdGrandeza = SelectedItemTipoGrandeza.Id

                                };
                            }

                            var response = await _sensoresService.EditSensorAsync(sensor);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<SensoresViewModel>();

                            }
                        }
                    }else{
                        await App.Current.MainPage.DisplayAlert("Existem campos não preenchidos", "Erro de Preenchimento", "Ok");
                    }
                });
            }
        }

  /*      public ICommand GetSensorDetailsCommand { get; set; }
        private async Task GetSensorDetailsAsync(Sensor sensor)
        {
            await NavigationService.NavigateToAsync<SensorViewModel>(sensor);
        }

*/
        private bool Validate()
        {
            bool isValidCodigoSensor = ValidateCodigoSensor();
            bool isValidNomeSensor = ValidateNomeSensor();
            bool isValidValorMaxSensor = ValidateValorMax();
            bool isValidValorMinSensor = ValidateValorMin();
            bool isValidObservacoesSensor = ValidateObservacoes();
            bool isValidTempoSensor = ValidateTempo();
            bool isValidFatorSensor = ValidateFator();
            bool isValidCodeSensor = ValidateCode();

            if(Tempo==null ){
                return false;
            }

            if(CodeVisible==true){
                if(Code==null){
                    return false;
                }
                return isValidCodigoSensor && isValidNomeSensor && isValidValorMaxSensor
                && isValidValorMinSensor && isValidObservacoesSensor && isValidTempoSensor
                && isValidCodeSensor;
            }

            if (FatorVisible == true) { 
                if(Fator==null){
                    return false; 
                }
                return isValidCodigoSensor && isValidNomeSensor && isValidValorMaxSensor
                && isValidValorMinSensor && isValidObservacoesSensor && isValidTempoSensor
                && isValidFatorSensor;
            }
            return isValidCodigoSensor && isValidNomeSensor && isValidValorMaxSensor 
                && isValidValorMinSensor &&  isValidObservacoesSensor && isValidTempoSensor;
        }

        private void AddValidations()
        {
            _CodigoSensor.Validations.Add(new IsBetweenValueOfByteRule<string> { ValidationMessage = "Codigo Sensor should by between 0-255" });
            _CodigoSensor.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Codigo Sensor should not by empty" });
            _NomeSensor.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Sensor should by between 0-255" });
            _NomeSensor.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Nome Sensor should not by empty" });
            _Observacoes.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Sensor should by between 0-255" });
            _ValorMax.Validations.Add(new IsFormatValueOfSensorMaxMinRule<double> { ValidationMessage = "Valor Maximo format invalid, should be like 1.00" });
            _ValorMax.Validations.Add(new IsNotNullOrEmptyDoubleRule<double> { ValidationMessage = "Valor Maximo Sensor should not by empty" });
            _ValorMin.Validations.Add(new IsFormatValueOfSensorMaxMinRule<double> { ValidationMessage = "Valor Maximo format invalid, should be like 1.00" });
            _ValorMin.Validations.Add(new IsNotNullOrEmptyDoubleRule<double> { ValidationMessage = "Valor Maximo Sensor should not by empty" });
            _Tempo.Validations.Add(new IsNotNullOrEmptyPositiveIntRule<int> { ValidationMessage = "Valor Tempo Sensor should not by empty and positivo" });



            _Fator.Validations.Add(new IsNotNullOrEmptyDoubleRule<double> { ValidationMessage = "Valor Minimo Sensor should not by empty" });
            _Code.Validations.Add(new IsNotNullOrEmptyPositiveIntRule<int> { ValidationMessage = "Valor Minimo Sensor should not by empty" });



        }

    
        public ICommand ValidateCodigoSensorCommand { get; private set; }
        private bool ValidateCodigoSensor()
        {
            return _CodigoSensor.Validate();
        }

        public ICommand ValidateNomeSensorCommand { get; private set; }
        private bool ValidateNomeSensor()
        {
            return _NomeSensor.Validate();
        }
       
        public ICommand ValidateValorMaxCommand { get; private set; }
        private bool ValidateValorMax()
        {
            return _ValorMax.Validate();
        }

        public ICommand ValidateValorMinCommand { get; private set; }
        private bool ValidateValorMin()
        {
            return _ValorMin.Validate();
        }

        public ICommand ValidateObservacoesCommand { get; private set; }
        private bool ValidateObservacoes()
        {
            return _Observacoes.Validate();
        }

        public ICommand ValidateTempoCommand { get; private set; }
        private bool ValidateTempo()
        {
            return _Tempo.Validate();
        }

        public ICommand ValidateFatorCommand { get; private set; }
        private bool ValidateFator()
        {
            return _Fator.Validate();
        }

        public ICommand ValidateCodeCommand { get; private set; }
        private bool ValidateCode()
        {
            return _Code.Validate();
        }


    }
}
#endregion