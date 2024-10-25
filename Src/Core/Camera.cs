using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MineVote.Core;

public class Camera
{
    public Vector2 position;
    public Vector2 screenSize;

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        var speed = (state.IsKeyDown(Keys.LeftShift) ? 500f : 200f) * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (state.IsKeyDown(Keys.W)) position.Y -= speed;
        if (state.IsKeyDown(Keys.S)) position.Y += speed;
        if (state.IsKeyDown(Keys.A)) position.X -= speed;
        if (state.IsKeyDown(Keys.D)) position.X += speed;
    }

    public void Resize(int width, int height)
    {
        screenSize = new Vector2(width, height);
    }

    public float GetZDepthMultiplier(float z)
    {
        return 1 - z / 12;
    }

    public Vector2 GetSize(Vector2 size, float z = 0)
    {
        return size * GetZDepthMultiplier(z);
    }

    public Vector2 GetScreenFromWorldPosition(Vector2 worldPosition, float z = 0)
    {
        worldPosition *= Settings.BlockSize;

        var screenOffset = new Vector2(screenSize.X / 2, screenSize.Y / 2);
        var screenPosition = new Vector2(worldPosition.X - position.X, worldPosition.Y - position.Y);
        var zMultiplier = GetZDepthMultiplier(z);

        return screenOffset + screenPosition * zMultiplier;
    }

    public Vector2 GetWorldFromScreenPosition(Vector2 screenPosition, float z = 0)
    {
        var screenOffset = new Vector2(screenSize.X / 2, screenSize.Y / 2);
        var worldPosition = (screenPosition - screenOffset) / GetZDepthMultiplier(z);

        return new Vector2(worldPosition.X + position.X, worldPosition.Y + position.Y) / Settings.BlockSize;
    }

    public Vector4 GetBoundaries()
    {
        var topLeft = GetWorldFromScreenPosition(Vector2.Zero);
        var bottomRight = GetWorldFromScreenPosition(screenSize);

        return new Vector4(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
    }

    public Vector4 GetBoundaries(float z)
    {
        var topLeft = GetWorldFromScreenPosition(Vector2.Zero, z);
        var bottomRight = GetWorldFromScreenPosition(screenSize, z);

        return new Vector4(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
    }
}