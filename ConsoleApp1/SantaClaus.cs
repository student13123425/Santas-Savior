using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class SantaClaus
    {
        public Vec2D pos;
        public SantaClaus(Vec2D pos)
        {
            this.pos = pos;
        }
        public void update(Game game)
        {
            game.GlobalTextures.SantaClaus.anim.Update();
        }
        public void render(Game game)
        {
            TextureObject t = game.GlobalTextures.SantaClaus.anim.GetCurrentTexture();
            t.DrawBottomCenter(80, false, this.pos);
        }
    }
}