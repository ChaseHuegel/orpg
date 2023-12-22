﻿namespace Orpg.Shared.Models;

public readonly struct CharacterDeletionResponse
{
    public readonly bool Success;
    public readonly string Message;

    public CharacterDeletionResponse(bool success, string message)
    {
        Success = success;
        Message = message;
    }
}