using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class GameOverMenu
    {
        Button[] buttons = new Button[2];
        public GameOverMenu()
        {
            buttons[0] = new Button("", "", new Vec2D(270, 500), 750);
            buttons[1] = new Button("", "", new Vec2D(400, 700), 500);
        }
        public void render(Game game)
        {
            this.render_bg(game);
            this.render_buttons(game);
        }
        public void render_buttons(Game game)
        {
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].render(game.GlobalTextures.GameOverMenuButtons[i].Texture, game.GlobalTextures.renderer);
        }
        public void render_bg(Game game)
        {
            Color bg_color = new Color(46, 92, 159, 255);
            Raylib.DrawRectangle(0, 0, 1333, 1100, bg_color);
            Raylib.DrawTexture(game.GlobalTextures.GameOverMenuBackground.Texture, 60, -10, Color.White);
        }
        public void update(Game game)
        {
            bool is_reset = this.buttons[0].update() || Raylib.IsKeyPressed(KeyboardKey.One);
            bool is_exit = this.buttons[1].update() || Raylib.IsKeyPressed(KeyboardKey.Two);
            if (is_reset)
                game.reset_level();
            else if (is_exit)
                Environment.Exit(0);
        }
    }
}