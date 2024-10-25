using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MineVote.Utils;

public class FpsCounter
{
    private static int _frames;
    private static double _elapsedTime;
    private static double _fps;

    public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 screenSize)
    {
        _frames++;
        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

        if (_elapsedTime >= 1)
        {
            _fps = _frames / _elapsedTime;
            _frames = 0;
            _elapsedTime = 0;
        }

        var text = $"FPS: {_fps:0.00}";
        var font = Cache.GetFont("Arial");
        var textSize = font.MeasureString(text);
        var position = new Vector2(screenSize.X - textSize.X - 10, 10);

        spriteBatch.DrawString(font, text, position, Color.White);
    }
}