using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineVote.Blocks;
using MineVote.Utils;

namespace MineVote.Core;

public abstract class World
{
    public string id { get; protected set; }
    private Dictionary<int, WorldLayer> layers = new();

    public World(string id)
    {
        this.id = id;
        InitializeLayers();
    }

    public void InitializeLayers()
    {
        for (int i = 0; i < Settings.WorldLayers; i++)
        {
            layers[i] = new WorldLayer();
        }
    }

    public Block? GetBlock(int x, int y, int z)
    {
        var layer = layers[z];
        if (layer == null)
            return null;

        return layer.GetBlock(x, y);
    }

    public void SetBlock(int x, int y, int z, Block block)
    {
        var layer = layers[z];
        if (layer == null)
            return;

        layer.SetBlock(x, y, block);
    }

    private void DrawBase(SpriteBatch spriteBatch, Camera camera)
    {

        // for (int z = 0; z < Settings.WorldLayers; z++)
        for (int z = Settings.WorldLayers - 1; z >= 0; z--)
        {
            var layer = layers[z];
            if (layer == null)
                continue;

            var bounds = camera.GetBoundaries(z);
            var xBounds = ((int)bounds.X - 1, (int)bounds.Z + 1);
            var yBounds = ((int)bounds.Y - 1, (int)bounds.W + 1);

            for (int x = xBounds.Item1; x < xBounds.Item2; x++)
            {
                for (int y = yBounds.Item1; y < yBounds.Item2; y++)
                {
                    var block = layer.GetBlock(x, y);
                    if (block == null)
                        continue;

                    var screenPosition = camera.GetScreenFromWorldPosition(new Vector2(x, y), z);
                    screenPosition.X = (int)Math.Round(screenPosition.X);
                    screenPosition.Y = (int)Math.Round(screenPosition.Y);

                    var size = camera.GetSize(new Vector2(Settings.BlockSize, Settings.BlockSize), z);
                    var drawRectangle = new Rectangle((int)screenPosition.X, (int)screenPosition.Y, (int)(size.X + 0.5), (int)(size.Y + 0.5));

                    block.Draw(spriteBatch, camera, drawRectangle);
                }
            }
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch, Camera camera)
    {
        DrawBase(spriteBatch, camera);
    }

    public virtual byte[] Serialize(Serializer serializer)
    {
        serializer.WriteString(id);
        
        foreach (var (z, layer) in layers)
        {
            serializer.WriteInt(z);
            layer.Serialize(serializer);
        }

        return serializer.GetBytes();
    }

    public static World Deserialize(Serializer serializer)
    {
        var name = serializer.ReadString();
        var world = WorldFactory.GetWorld(name);

        while (serializer.HasData())
        {
            var z = serializer.ReadInt();
            var layer = WorldLayer.Deserialize(serializer);
            world.layers[z] = layer;
        }

        return world;
    }
}

public class Overworld : World
{
    public Overworld() : base("overworld") { }
    
    public Overworld(int x, int y) : base("overworld") {
        for (int z = 0; z < Settings.WorldLayers; z++)
        {
            for (int i = 0; i < x; i++)
            {
                // for (int j = 0; j < y; j++)
                // {
                //     // SetBlock(i, j, z, new Grass());
                // }

                SetBlock(i, 0, z, new Grass());

                for (int j = 1; j < y / 2; j++)
                {
                    SetBlock(i, j, z, new Dirt());
                }

                for (int j = y / 2; j < y; j++)
                {
                    SetBlock(i, j, z, new Stone());
                }
            }
        }
    }
}