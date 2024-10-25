using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MineVote.Core;
using MineVote.Utils;

namespace MineVote.Blocks;

public abstract class Block
{
    public int id { get; protected set; }
    private string _texturePath;

    protected Block(int id, string texturePath)
    {
        this.id = id;
        _texturePath = texturePath;
    }

    public Texture2D GetTexture()
    {
        return Cache.GetTexture(_texturePath);
    }

    public void DrawBase(SpriteBatch spriteBatch, Rectangle drawRectangle)
    {
        var texture = GetTexture();
        spriteBatch.Draw(texture, drawRectangle, Color.White);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Camera camera, Rectangle drawRectangle)
    {
        DrawBase(spriteBatch, drawRectangle);
    }

    public virtual void Serialize(Serializer serializer)
    {
        serializer.WriteInt(id);
    }

    public static Block Deserialize(Serializer serializer)
    {
        var id = serializer.ReadInt();
        return BlockFactory.GetBlock(id);
    }
}