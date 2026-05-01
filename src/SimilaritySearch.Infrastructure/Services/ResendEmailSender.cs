using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Resend;
using SimilaritySearch.Infrastructure.Identity;

namespace SimilaritySearch.Infrastructure.Services;

public class ResendEmailSender : IEmailSender<ApplicationUser>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _fromEmail;
    private readonly string _frontendUrl;
    private readonly ILogger<ResendEmailSender> _logger;

    public ResendEmailSender(IServiceScopeFactory scopeFactory, ILogger<ResendEmailSender> logger, IConfiguration configuration)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _fromEmail = configuration["Resend:FromEmail"] ?? "onboarding@resend.dev";
        _frontendUrl = configuration["FrontendUrl"] ?? "http://localhost:5173/";
    }
    
    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        var frontendLink = confirmationLink.Replace(GetApiBaseUrl(confirmationLink), _frontendUrl + "/confirm-email");
        
        await SendEmailAsync(
            email,
            "Confirm your email",
            $"Please confirm your account by <a href='{frontendLink}'>clicking here</a>.");
    }

    public async Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
    {
        var frontendLink = resetLink.Replace(GetApiBaseUrl(resetLink), _frontendUrl + "/reset-password");
        
        await SendEmailAsync(
            email,
            "Reset your password",
            $"Please reset your password by <a href='{frontendLink}'>clicking here</a>.");
    }

    public async Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
    {
        var resetLink = $"{_frontendUrl}/reset-password?code={Uri.EscapeDataString(resetCode)}&email={Uri.EscapeDataString(email)}";

        await SendEmailAsync(
            email,
            "Reset your password",
            $"Please reset your password by <a href='{resetLink}'>clicking here</a> or use this code: <strong>{resetCode}</strong>");
    }

    private async Task SendEmailAsync(string email, string subject, string htmlBody)
    {
        using var scope = _scopeFactory.CreateScope();
        var resend = scope.ServiceProvider.GetRequiredService<IResend>();
        
        var message = new EmailMessage();
        message.From = _fromEmail;
        message.To.Add( email );
        message.Subject = subject;
        message.HtmlBody = htmlBody;
        
        try
        {
            await resend.EmailSendAsync(message);
            _logger.LogInformation("E-mail has been sent to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending an email to {Email}", email);
        }
    }
    
    private string GetApiBaseUrl(string link)
    {
        var uri = new Uri(link);
        return $"{uri.Scheme}://{uri.Authority}/confirmEmail";
    }
}