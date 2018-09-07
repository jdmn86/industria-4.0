using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Industria4.Helpers;
using Industria4.Models;
using Industria4.Models.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Industria4.DataServices.Base
{
    public class RequestProvider: IRequestProvider
    {
        private readonly JsonSerializerSettings _serializerSettings;

        public RequestProvider()
        {
            _serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore
            };

            _serializerSettings.Converters.Add(new StringEnumConverter());
        }

        public async Task<TResult> GetAsync<TResult>(string uri)
        {
            HttpClient httpClient =  CreateHttpClient();

            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            Debug.WriteLine("URI : " + uri);
            HttpResponseMessage response = await httpClient.GetAsync(GlobalSettings.DefaultApiAddress+uri);

            await HandleResponse(response);

            string serialized = await response.Content.ReadAsStringAsync();
            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(serialized, _serializerSettings));

            return result;
        }

        public Task<TResult> PostAsync<TResult>(string uri, TResult data)
        {
            return PostAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PostAsync<TRequest, TResult>(string uri, TRequest data)
        {
            HttpClient httpClient = CreateHttpClient();
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            Debug.WriteLine("serialized : " + serialized);

            HttpResponseMessage response = await httpClient.PostAsync(GlobalSettings.DefaultApiAddress+uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        public Task<TResult> PutAsync<TResult>(string uri, TResult data)
        {
            return PutAsync<TResult, TResult>(uri, data);
        }

        public async Task<TResult> PutAsync<TRequest, TResult>(string uri, TRequest data)
        {
            HttpClient httpClient = CreateHttpClient();
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            HttpResponseMessage response = await httpClient.PutAsync(GlobalSettings.DefaultApiAddress+uri, new StringContent(serialized, Encoding.UTF8, "application/json"));

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.AccessToken);

            }

            return httpClient;
        }

        private async Task HandleResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.BadRequest)
                {
              //      await App.Current.MainPage.DisplayAlert("Não autorizado", "Erro", "Ok");

                    throw new ServiceAuthenticationException(content);
                }

           //     await App.Current.MainPage.DisplayAlert(content, "Erro", "Ok");
                throw new HttpRequestException(content);
            }
        }

        public async Task<TResult> PostAsyncToken<TRequest, TResult>(string uri, TRequest data)
        {
    
            HttpClient httpClient = CreateHttpClient(); 
            string serialized = await Task.Run(() => JsonConvert.SerializeObject(data, _serializerSettings));
            Debug.WriteLine("serialized : " + serialized);

            AuthenticationRequest auth = data as AuthenticationRequest;

            var dic = new Dictionary<string, string>();
            dic.Add("username", auth.username);
            dic.Add("password", auth.password);
            dic.Add("grant_type", "password");

            var authorizeValues = new FormUrlEncodedContent(dic);

            HttpResponseMessage response = await httpClient.PostAsync(GlobalSettings.DefaultApiAddress + uri, authorizeValues);

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();
            Debug.WriteLine("responseData :"+ responseData.ToString());
            return await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
        }

      
        public async Task<TResult> DeleteAsync<TResult>(string uri)
        {
            HttpClient httpClient = CreateHttpClient();
            HttpResponseMessage response = await httpClient.DeleteAsync(GlobalSettings.DefaultApiAddress +uri);

            await HandleResponse(response);

            string responseData = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() => JsonConvert.DeserializeObject<TResult>(responseData, _serializerSettings));
            return result;
        }

        public void PostAsync(string uri)
        {
            HttpClient httpClient = CreateHttpClient();

            httpClient.GetAsync(GlobalSettings.DefaultApiAddress + uri);

            return ;
        }
    }
}
