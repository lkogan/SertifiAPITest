using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SertifiAPITest.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SertifiAPITest.Helpers
{ 
        public class HttpHelper
        {
            private HttpClient httpClient;

            private readonly string baseUrl;

            private const string jsonMediaType = "application/json";

            public HttpHelper(string baseUrl)
            {
                this.baseUrl = FormatBaseUrl(baseUrl);
            }

            public async Task<List<StudentDTO>> GetAsync(string url)
            {
                try
                {
                    EnsureHttpClientCreated();
                 
                    using (var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)))
                    {
                        response.EnsureSuccessStatusCode();

                        string content = await response.Content.ReadAsStringAsync();

                        return await Task.Run(() => JsonConvert.DeserializeObject<List<StudentDTO>>(content));
                    }

                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    return null;
                }
            }

            public async Task<string> PutAsync(string url, object input)
            {
                try
                {
                    var payload = new StringContent(JsonConvert.SerializeObject(input, Formatting.Indented), Encoding.UTF8, jsonMediaType);

                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri(url),
                        Content = payload
                    };

                    EnsureHttpClientCreated();

                    using (var response = await httpClient.SendAsync(request))
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                    return null;
                }
            }
          
            public void CreateHttpClient()
            {
                var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
                var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
                httpClient = httpClientFactory.CreateClient();
                  
                if (!string.IsNullOrWhiteSpace(baseUrl))
                {
                    httpClient.BaseAddress = new Uri(baseUrl);
                }

                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(jsonMediaType));
            }

            public void EnsureHttpClientCreated()
            {
                if (httpClient == null)
                {
                    CreateHttpClient();
                }
            }

            public static string FormatBaseUrl(string url)
            {
                return url.EndsWith("/") ? url : url + "/";
            }
        }
}
