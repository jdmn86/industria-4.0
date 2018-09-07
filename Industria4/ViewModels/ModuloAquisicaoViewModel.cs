using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Industria4.DataServices.Maquinas;
using Industria4.DataServices.ModulosAquisicao;
using Industria4.DataServices.Sensores;
using Industria4.Models;
using Industria4.Validations;
using Industria4.ViewModels.Base;
using Xamarin.Forms;

namespace Industria4.ViewModels
{
    public class ModuloAquisicaoViewModel : ViewModelBase
    {

        private readonly IModuloAquisicaoService _modulosAquisicaoService;
        private readonly IMaquinaService _maquinaService;
        private readonly ISensoresService _sensoresService;
 //       private readonly IUserDialogs _userDialogs;

        public ModuloAquisicaoViewModel(IModuloAquisicaoService modulosAquisicaoService,
                                        ISensoresService sensoresService,
                                        IMaquinaService maquinaService)
                         //               IUserDialogs userDialogs)
        {
            _modulosAquisicaoService = modulosAquisicaoService;
            _maquinaService = maquinaService;
            _sensoresService = sensoresService;
 //           _userDialogs = userDialogs;

            GetSensorDetailsCommand = new Command<Sensor>(async (item) => await GetSensorDetailsAsync(item));

            ValidateCodigoModuloAquisicaoCommand = new Command(() => ValidateCodigoModuloAquisicao());
            ValidateNomeModuloAquisicaoCommand = new Command(() => ValidateNomeModuloAquisicao());

            AddValidations();
        }

        public override async Task InitializeAsync(object navigationData)
        {

            Maquinas = await _maquinaService.GetMaquinasAsync();

            if (navigationData is ModuloAquisicao)
            {
                 Modulo = navigationData as ModuloAquisicao;

                NewModulo = false;
                EditModulo = true;

                foreach (var item in Maquinas)
                {
                    if (item.Id.Equals(Modulo.IdMaquina))
                    {
                        SelectedItem = item;
                    }
                }

                Sensores = await _sensoresService.GetSensoresFromModuloAsync(Modulo.Id);

            }
            else
            {
                Modulo = new ModuloAquisicao();

                NewModulo = true;
                EditModulo = false;
            }


          //  await base.InitializeAsync(navigationData);
        }

        #region Control Views definition

        private bool _IsValid = false;
        public bool IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; RaisePropertyChanged(() => IsValid); }
        }

        public bool _NewModulo = true;
        public bool NewModulo
        {
            get { return _NewModulo; }
            set
            {
                _NewModulo = value;
                RaisePropertyChanged(() => NewModulo);
            }
        }

        public bool _EditModulo = false;
        public bool EditModulo
        {
            get { return _EditModulo; }
            set
            {
                _EditModulo = value;
                RaisePropertyChanged(() => EditModulo);
            }
        }

        #endregion

        #region Model definition

        public ModuloAquisicao _Modulo { get; set; }
        public ModuloAquisicao Modulo
        {
            get { return _Modulo; }
            set
            {
                _Modulo = (ModuloAquisicao)value;

                CodigoModuloAquisicao.Value = value.CodigoModuloAquisicao;
                NomeModuloAquisicao.Value = value.NomeModuloAquisicao;
                IdMaquina = value.IdMaquina;
                CreatedAt = value.CreatedAt;
                Active = value.Active;

                RaisePropertyChanged(() => Modulo);
            }
        }

        private ValidatableObject<string> _CodigoModuloAquisicao = new ValidatableObject<string>();
        public ValidatableObject<string> CodigoModuloAquisicao
        {
            get
            {
                return _CodigoModuloAquisicao;
            }
            set
            {
                _CodigoModuloAquisicao = value;
                RaisePropertyChanged(() => CodigoModuloAquisicao);
            }
        }

        private ValidatableObject<string> _NomeModuloAquisicao = new ValidatableObject<string>();
        public ValidatableObject<string> NomeModuloAquisicao
        {
            get
            {
                return _NomeModuloAquisicao;
            }
            set
            {
                _NomeModuloAquisicao = value;
                RaisePropertyChanged(() => NomeModuloAquisicao);
            }
        }

        private int _IdMaquina;
        public int IdMaquina
        {
            get
            {
                return _IdMaquina;
            }
            set
            {
                _IdMaquina = value;
                RaisePropertyChanged(() => IdMaquina);
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

        private bool _Active { get; set;}
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

        public Maquina _selectedItem { get; private set; }
        public Maquina SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        private ObservableCollection<Maquina> _maquinas = new ObservableCollection<Maquina>();
        public ObservableCollection<Maquina> Maquinas
        {
            get { return _maquinas; }
            set
            {
                _maquinas = value;
                RaisePropertyChanged(() => Maquinas);
            }
        }

        private ObservableCollection<Sensor> _sensores = new ObservableCollection<Sensor>();
        public ObservableCollection<Sensor> Sensores
        {
            get { return _sensores; }
            set
            {
                _sensores = value;
                RaisePropertyChanged(() => Sensores);
            }
        }
        #endregion

        #region Commands definition

        public ICommand GuardarModulo
        {
            get
            {
                return new Command(async () =>
                {
                    IsValid = Validate();

                    if (IsValid)
                    {
                        ModuloAquisicao mod;

                        bool result = await App.Current.MainPage.DisplayAlert("Tem a certeza que pretende Guardar?", "Guardar Modulo", "Sim", "Cancel");
                        if (result == false)
                        {
                            Debug.WriteLine("result : " + result);
                            return;
                        }


                        if (NewModulo == true)
                        {
                            mod = new ModuloAquisicao
                            {
                       
                                CodigoModuloAquisicao = CodigoModuloAquisicao.Value,
                                NomeModuloAquisicao = NomeModuloAquisicao.Value,
                                IdMaquina = SelectedItem.Id,
                                CreatedAt = DateTime.Now,
                                Active = Active
                            };

                            var response = await _modulosAquisicaoService.SaveModulosAquisicaoAsync(mod);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<ModulosAquisicaoViewModel>();

                            }

                        }
                        else
                        {

                            mod = new ModuloAquisicao
                            {
                                Id = Modulo.Id,
                                CodigoModuloAquisicao = CodigoModuloAquisicao.Value,
                                NomeModuloAquisicao = NomeModuloAquisicao.Value,
                                IdMaquina = SelectedItem.Id,
                              //  CreatedAt = DateTime.Now,
                                Active = Active
                            };

                            var response = await _modulosAquisicaoService.EditModulosAquisicaoAsync(mod);

                            if (response != null)
                            {
                                await NavigationService.NavigateToAsync<ModulosAquisicaoViewModel>();

                            }
                        }
                    }
                });
            }
        }

        public ICommand GetSensorDetailsCommand { get; set; }
        private async Task GetSensorDetailsAsync(Sensor sensor)
        {
            await NavigationService.NavigateToAsync<SensorViewModel>(sensor);
        }


        private bool Validate()
        {
            bool isValidCodigoModuloAquisicao = ValidateCodigoModuloAquisicao();
            bool isValidNomeModuloAquisicao = ValidateNomeModuloAquisicao();

            return isValidCodigoModuloAquisicao && isValidNomeModuloAquisicao;
        }

        private void AddValidations()
        {
            _CodigoModuloAquisicao.Validations.Add(new IsBetweenValueOfByteRule<string> { ValidationMessage = "Codigo Modulo should by between 0-255" });
            _CodigoModuloAquisicao.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Codigo Modulo should not by empty" });
            _NomeModuloAquisicao.Validations.Add(new IsBetweenValueOfVarChar250Rule<string> { ValidationMessage = "Nome Modulo should by between 0-250 caracteres" });
            _NomeModuloAquisicao.Validations.Add(new IsNotNullOrEmptyStringRule<string> { ValidationMessage = "Nome Modulo should not by empty" });

        }


        public ICommand ValidateCodigoModuloAquisicaoCommand { get; private set; }
        private bool ValidateCodigoModuloAquisicao()
        {
            return _CodigoModuloAquisicao.Validate();
        }

        public ICommand ValidateNomeModuloAquisicaoCommand { get; private set; }
        private bool ValidateNomeModuloAquisicao()
        {
            return _NomeModuloAquisicao.Validate();
        }

    }
}
#endregion
