using System;
using System.Collections.Generic;

namespace MineVote.Blocks;

public class BlockFactory
{
    private static Dictionary<int, Type> _blocks = new()
    {
        { 1, typeof(Dirt) },
        { 2, typeof(Grass) },
        { 3, typeof(Stone) },
    };

    public static Block? GetBlock(int id)
    {
        if (_blocks.TryGetValue(id, out var blockType))
        {
            return (Block?)Activator.CreateInstance(blockType);
        }

        return null;
    }
}