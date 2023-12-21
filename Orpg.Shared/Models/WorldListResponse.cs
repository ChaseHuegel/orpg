using Orpg.Shared.Models;

namespace Orpg.Client.Tests;

public readonly struct WorldListResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly WorldServer[] Worlds;

    public WorldListResponse(bool success, string message, WorldServer[] worlds)
    {
        Success = success;
        Message = message;
        Worlds = worlds;
    }
}