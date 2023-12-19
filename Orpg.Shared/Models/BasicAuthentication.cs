namespace Orpg.Client.Tests;

public readonly struct BasicAuthentication
{
    private readonly string _username;
    private readonly string _password;

    public BasicAuthentication(string username, string password)
    {
        _username = username;
        _password = password;
    }
}