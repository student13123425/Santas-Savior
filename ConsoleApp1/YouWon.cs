using Raylib_cs;
using System.Numerics;

namespace ConsoleApp1
{
    public class YouWon
    {
        private bool isFirstUpdate = true;

        public void Reset()
        {
            isFirstUpdate = true;
        }

        public void update(Game game)
        {
            if (isFirstUpdate)
            {
                isFirstUpdate = false;
            }
        }

        public void render(Game game)
        {
            float screenWidth = Raylib.GetScreenWidth();
            float screenHeight = Raylib.GetScreenHeight();
            Vec2D center = new Vec2D(screenWidth / 2.0f, screenHeight / 2.0f);

            game.GlobalTextures.EndSprite.DrawCenter(screenHeight, false, center);
        }
    }
}