namespace Orpg.Shared.Models;

public readonly struct TokenResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly string Token;

    public TokenResponse(bool success, string message, string token)
    {
        Success = success;
        Message = message;
        Token = token;
    }
}