using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Ubora.Web.Services
{   
    public class PandocService
    {
        private readonly IOptions<Pandoc> _appSettings;
        
        public PandocService(IOptions<Pandoc> appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<HttpResponseMessage> ConvertDocumentAsync(string html)
        {
            var isHttps = _appSettings.Value.IsHttps;
            var protocol = isHttps ? "https" : "http";
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{protocol}://{_appSettings.Value.Ip}:{_appSettings.Value.Port}");

                var request = new HttpRequestMessage(HttpMethod.Post, "download/docx");
                request.Headers.Add("privateapikey", _appSettings.Value.Key);
                request.Content = new StringContent(html);

                return await client.SendAsync(request);
            }
        }
    }
}