using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CroquetAustralia.Library.Settings;
using Newtonsoft.Json;

namespace CroquetAustralia.Website.App.tournaments
{
    public class WebApi
    {
        private readonly WebApiSettings _webApiSettings;

        public WebApi(WebApiSettings webApiSettings)
        {
            _webApiSettings = webApiSettings;
        }

        public async Task PostAsync(string requestUri, object data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_webApiSettings.BaseUri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsJsonAsync(requestUri, data);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                throw new Exception($"POST {requestUri} failed '{response.StatusCode}'.")
                {
                    Data =
                    {
                        {"Data", JsonConvert.SerializeObject(data)},
                        {"Response", JsonConvert.SerializeObject(new { response.ReasonPhrase, response.StatusCode})}
                    }
                };
            }
        }
    }
}