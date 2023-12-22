namespace Orpg.Shared.Responses;

public readonly struct LoginResponse
{
    public readonly bool Success;
    public readonly string Message;

    public LoginResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}