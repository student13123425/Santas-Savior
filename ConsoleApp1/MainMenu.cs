using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class MainMenu
    {
        Button[] buttons = new Button[3];
        public MainMenu()
        {
            string[] paths = new string[3];
            float screenWidth = 1333;

            buttons[0] = new Button(paths[0], "", new Vec2D((screenWidth - 450) / 2, 540), 450, false);
            buttons[1] = new Button(paths[1], "", new Vec2D((screenWidth - 400) / 2, 640 + 50), 400, false);
            buttons[2] = new Button(paths[2], "", new Vec2D((screenWidth - 200) / 2, 740 + 100), 200, false);
        }

        public void render(Game game)
        {
            TextureObject bgTextureObj = game.GlobalTextures.MainMenuBackground;
            Texture2D bgTexture = bgTextureObj.Texture;

            float screenWidth = 1333;
            float screenHeight = 1100;

            float scale = screenHeight / bgTexture.Height;

            float destWidth = bgTexture.Width * scale;

            float destX = (screenWidth - destWidth) / 2.0f;

            Rectangle src = new Rectangle(0, 0, bgTexture.Width, bgTexture.Height);
            Rectangle dest = new Rectangle(destX, 0, destWidth, screenHeight);
            Vector2 origin = new Vector2(0, 0);

            Raylib.DrawTexturePro(bgTexture, src, dest, origin, 0f, Color.White);

            for (int i = 0; i < buttons.Length; i++)
                buttons[i].render(game.GlobalTextures.MainMenuButtons[i].Texture, game.GlobalTextures.renderer);
        }

        public int proces_select(Game game, int index)
        {
            game.GlobalAudio.MenuSelect.Play(false);
            return index;
        }

  
        public int update(Game game)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.One))
                return proces_select(game,0);
            if (Raylib.IsKeyPressed(KeyboardKey.Two))
                return proces_select(game,1);
            if (Raylib.IsKeyPressed(KeyboardKey.Three))
                return proces_select(game,2);
            for (int i = 0; i < buttons.Length; i++)
                if (buttons[i].update())
                    return proces_select(game,i);
            return -1;
        }

    }
}