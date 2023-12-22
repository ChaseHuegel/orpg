using Orpg.Shared.Models;

namespace Orpg.Shared.Responses;

public readonly struct CharacterCreationResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly Character Character;

    public CharacterCreationResponse(bool success, string message, Character character)
    {
        Success = success;
        Message = message;
        Character = character;
    }
}