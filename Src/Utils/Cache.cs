using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MineVote.Utils;

public class Cache
{
    private static Dictionary<string, (Texture2D Texture, DateTime LastUsed)> _textureCache = new();
    private static Dictionary<string, SpriteFont> _fontCache = new();
    private static GraphicsDevice _graphicsDevice;
    private static string _contentDirectory;

    public static void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
    {
        _graphicsDevice = graphicsDevice;
        _contentDirectory = content.RootDirectory;
    }

    public static Texture2D GetTexture(string path)
    {
        if (_textureCache.ContainsKey(path))
        {
            _textureCache[path] = (_textureCache[path].Texture, DateTime.Now);
            return _textureCache[path].Texture;
        }

        string fullPath = Path.Combine(_contentDirectory, path + ".png");
        Texture2D texture = Texture2D.FromFile(_graphicsDevice, fullPath);

        _textureCache[path] = (texture, DateTime.Now);
        return texture;
    }

    public static void LoadFont(ContentManager content, string path)
    {
        _fontCache[path] = content.Load<SpriteFont>(path);
    }

    public static SpriteFont GetFont(string path)
    {
        return _fontCache[path];
    }

    public static void UpdateCache()
    {
        DateTime now = DateTime.Now;
        List<string> textureKeysToRemove = new();

        foreach (var entry in _textureCache)
        {
            if ((now - entry.Value.LastUsed).TotalSeconds > Settings.CacheExpireTime)
            {
                entry.Value.Texture.Dispose();
                textureKeysToRemove.Add(entry.Key);
            }
        }

        foreach (var key in textureKeysToRemove)
        {
            _textureCache.Remove(key);
        }
    }
}