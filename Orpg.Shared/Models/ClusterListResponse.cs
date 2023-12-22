﻿namespace Orpg.Shared.Models;

public readonly struct ClusterListResponse
{
    public readonly bool Success;
    public readonly string Message;
    public readonly ClusterServer[] Servers;

    public ClusterListResponse(bool success, string message, ClusterServer[] servers)
    {
        Success = success;
        Message = message;
        Servers = servers;
    }
}