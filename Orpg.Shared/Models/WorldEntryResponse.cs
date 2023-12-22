﻿namespace Orpg.Shared.Models;

public readonly struct WorldEntryResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly PlayerSnapshot PlayerSnapshot;

    public WorldEntryResponse(bool success, string message, PlayerSnapshot playerSnapshot)
    {
        Success = success;
        Message = message;
        PlayerSnapshot = playerSnapshot;
    }
}