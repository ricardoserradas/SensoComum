using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SensoComum.Mobile.Forms
{
    public class ApiServiceManager
    {
        HttpClient _client;

        public ApiServiceManager()
        {
            this._client = new HttpClient();
        }

        public async Task<string> RefreshDataAsync()
        {
            var uri = new Uri(Constants.GetService);
            string content = string.Empty;

            try
            {
                var response = await this._client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    content = await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
            }

            return content;
        }

        public async void SumToService()

        {
            var uri = new Uri(Constants.PostService);

            StringContent content = new StringContent("1", Encoding.UTF8, "application/json");

            var response = await this._client.PostAsync(uri, content);
        }
    }
}
