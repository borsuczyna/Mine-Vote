using System;
using System.Collections.Generic;

namespace MineVote.Core;

public class WorldFactory
{
    private static Dictionary<string, Type> _worlds = new()
    {
        { "overworld", typeof(Overworld) },
    };

    public static World? GetWorld(string name)
    {
        if (_worlds.TryGetValue(name, out var worldType))
        {
            return (World?)Activator.CreateInstance(worldType);
        }

        return null;
    }
}