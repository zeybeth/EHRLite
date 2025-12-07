using Microsoft.AspNetCore.Identity.UI.Services;

namespace EHRLite.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
           
            // Sistem mail attığını sanacak ama aslında hiçbir şey yapmayacak.
            // Böylece hata almıyoruz
            return Task.CompletedTask;
        }
    }
}