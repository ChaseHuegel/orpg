using Orpg.Shared.Models;

namespace Orpg.Shared.Responses;

public struct CharacterListResponse
{
    public bool Success;
    public string Message;
    public Character[] Characters;

    public CharacterListResponse(bool success, string message, Character[] characters)
    {
        Success = success;
        Message = message;
        Characters = characters;
    }
}