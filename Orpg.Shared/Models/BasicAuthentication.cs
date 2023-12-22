namespace Orpg.Shared.Models;

public readonly struct BasicAuthentication
{
    public readonly string Username;
    public readonly string Password;

    public BasicAuthentication(string username, string password)
    {
        Username = username;
        Password = password;
    }
}