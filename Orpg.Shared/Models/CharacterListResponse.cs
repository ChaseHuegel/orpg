namespace Orpg.Shared.Models;

public struct CharacterListResponse
{
    public bool Success;
    public string Message;
    public CharacterSnippet[] Characters;

    public CharacterListResponse(bool success, string message, CharacterSnippet[] characters)
    {
        Success = success;
        Message = message;
        Characters = characters;
    }
}