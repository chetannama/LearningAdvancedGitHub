using EmailSpamDetectionService.DTOs;
using EmailSpamDetectionService.Helpers;
using EmailSpamDetectionService.Model;
using EmailSpamDetectionService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace EmailSpamDetectionService.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSpamDetectorController : ControllerBase
    {
        private readonly IEmailSpamService _spamService;
        
        public EmailSpamDetectorController(IEmailSpamService spamService)
        {
            _spamService = spamService;
        }

        [HttpPost("/check-spam")]
        public async Task<IActionResult> CheckSpam(EmailRequest request)
        {
            var result = await _spamService.SendEmailAsync(request);
            return Ok(result);
        }

        
    }
}
