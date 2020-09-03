using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleApp.ApiService.Models;
using SampleApp.ApiService.Services;
using System;
using System.Threading.Tasks;

namespace SampleApp.ApiService.Controllers
{
    /// <summary>
    /// API Controller to send a email 
    /// https://localhost:44380/api/email
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IEmailService _emailService;

        public EmailController(
            IEmailService emailService,
            ILogger<EmailController> logger)
        {
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost()]
        public async Task<ActionResult<Exception>> Post_Email(EmailMessage emailMessage)
        {
            try
            {
                await _emailService.SendEmailAsync(emailMessage.To, emailMessage.Subject, emailMessage.Body);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Post_Email", emailMessage);
                return ex;
            }
        }
    }
}
