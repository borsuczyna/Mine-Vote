using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MineVote.Core;
using MineVote.Utils;

namespace MineVote;

public class Game : Microsoft.Xna.Framework.Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Vector2 _screenSize;
    private bool _fullscreen;

    private World _world;
    private Camera _camera;

    public Game()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _camera = new Camera();
        _world = new Overworld(10, 20);
        Resize(1280, 720, false);

        Cache.Initialize(Content, GraphicsDevice);
        Cache.LoadFont(Content, "Arial");

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _camera.Update(gameTime);
        Cache.UpdateCache();

        // on key K save world
        if (Keyboard.GetState().IsKeyDown(Keys.K))
        {
            var serializer = new Serializer();
            _world.Serialize(serializer);
            serializer.Save("world.dat");
        }

        // on key L load world
        if (Keyboard.GetState().IsKeyDown(Keys.L))
        {
            var serializer = new Serializer();
            serializer.Load("world.dat");
            _world = World.Deserialize(serializer);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _world.Draw(_spriteBatch, _camera);
        FpsCounter.Draw(_spriteBatch, gameTime, _screenSize);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void Resize(int width, int height, bool? fullscreen = null)
    {
        _fullscreen = fullscreen ?? _fullscreen;
        _graphics.PreferredBackBufferWidth = width;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.IsFullScreen = _fullscreen;

        // unlock fps
        _graphics.SynchronizeWithVerticalRetrace = false;
        IsFixedTimeStep = false;

        _graphics.ApplyChanges();

        _screenSize = new Vector2(width, height);
        _camera.Resize(width, height);
    }
}
