using EmailSpamDetectionService.Model;
using EmailSpamDetectionService.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace EmailSpamDetectionService.Services.Services
{
    public class EmailSpamService : IEmailSpamService
    {
        private readonly HttpClient _httpClient;
        public EmailSpamService(IHttpClientFactory httpClientFactory) 
        { 
            _httpClient = httpClientFactory.CreateClient(nameof(EmailSpamService));
        }

        public async Task<bool> SendEmailAsync(EmailRequest email)
        {
            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("http://127.0.0.1:8002/check-spam", content);

            var result = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }
    }
}
