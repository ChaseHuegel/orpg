namespace Orpg.Shared.Responses;

public readonly struct LoginResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly string SessionToken;

    public LoginResponse(bool success, string message, string token)
    {
        Success = success;
        Message = message;
        SessionToken = token;
    }
}