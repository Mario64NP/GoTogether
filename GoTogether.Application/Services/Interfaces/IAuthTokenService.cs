namespace GoTogether.Application.Services.Interfaces;

public interface IAuthTokenService
{
    public string GenerateToken(Guid userId, string username, string role);
}
