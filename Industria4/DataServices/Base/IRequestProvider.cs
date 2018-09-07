using System;
using System.Threading.Tasks;
using Industria4.Models;

namespace Industria4.DataServices.Base
{
    public interface IRequestProvider
    {
        Task<TResult> GetAsync<TResult>(string uri);

        Task<TResult> PostAsync<TResult>(string uri, TResult data);

        Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data);

        Task<TResult> PutAsync<TResult>(string uri, TResult data);

        Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data);

        Task<TResult> PostAsyncToken<TRequest, TResult>(string uri, TRequest data);

        Task<TResult> DeleteAsync<TResult>(string uri);

		void PostAsync(string v);
		
	}
}
