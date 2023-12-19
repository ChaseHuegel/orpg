namespace Orpg.Shared.Models;

public struct RegistrationResponse
{
    public bool Success;
    public string Message;

    public RegistrationResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}