namespace Orpg.Shared.Responses;

public readonly struct AuthenticationResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly string AuthenticationToken;

    public AuthenticationResponse(bool success, string message, string token)
    {
        Success = success;
        Message = message;
        AuthenticationToken = token;
    }
}