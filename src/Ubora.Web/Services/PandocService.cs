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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_appSettings.Value.Address);

                var request = new HttpRequestMessage(HttpMethod.Post, "download/docx");
                request.Content = new StringContent(html);

                return await client.SendAsync(request);
            }
        }
    }
}