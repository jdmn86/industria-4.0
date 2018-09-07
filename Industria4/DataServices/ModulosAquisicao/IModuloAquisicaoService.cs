using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.ModulosAquisicao
{
    public interface IModuloAquisicaoService
    {
        Task<ObservableCollection<ModuloAquisicao>> GetModulosAquisicaoAsync();
        Task<ModuloAquisicao> SaveModulosAquisicaoAsync(ModuloAquisicao moduloAquisicao);
        Task<ModuloAquisicao> EditModulosAquisicaoAsync(ModuloAquisicao moduloAquisicao);
        Task<ObservableCollection<ModuloAquisicao>> GetModulosAquisicaoFromMaquinaAsync(int idMaquina);
    }
}
