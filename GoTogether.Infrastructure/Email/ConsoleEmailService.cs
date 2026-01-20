using GoTogether.Application.Services.Interfaces;
using System.Diagnostics;

namespace GoTogether.Infrastructure.Email;

public class ConsoleEmailService : IEmailService
{
    public Task SendVerificationEmailAsync(string email, string displayName, string verificationLink)
    {
        var message = $"""
            **********************************************************
            EMAIL SENT TO: {email}
            NAME: {displayName}
            SUBJECT: Verify your GoTogether account
            
            Click the link below to verify:
            {verificationLink}
            **********************************************************
            """;

        Debug.WriteLine(message);

        return Task.CompletedTask;
    }
}
