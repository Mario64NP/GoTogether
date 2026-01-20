namespace GoTogether.Application.Services.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string email, string displayName, string verificationLink);
}
