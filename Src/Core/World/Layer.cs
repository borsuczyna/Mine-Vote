using System;
using System.Collections.Generic;
using MineVote.Blocks;
using MineVote.Utils;

namespace MineVote.Core;

public class WorldLayer
{
    private Dictionary<(int, int), Block> blocks = new();

    public Block? GetBlock(int x, int y)
    {
        if (blocks.TryGetValue((x, y), out var block))
        {
            return block;
        }

        return null;
    }

    public void SetBlock(int x, int y, Block block)
    {
        blocks[(x, y)] = block;
    }

    public void Serialize(Serializer serializer)
    {
        serializer.WriteInt(blocks.Count);
        foreach (var (position, block) in blocks)
        {
            serializer.WriteVector2(new(position.Item1, position.Item2));
            block.Serialize(serializer);
        }
    }

    public static WorldLayer Deserialize(Serializer serializer)
    {
        var layer = new WorldLayer();
        var blockCount = serializer.ReadInt();
        for (int i = 0; i < blockCount; i++)
        {
            var position = serializer.ReadVector2();
            var block = Block.Deserialize(serializer);
            layer.SetBlock((int)position.X, (int)position.Y, block);
        }

        return layer;
    }
}