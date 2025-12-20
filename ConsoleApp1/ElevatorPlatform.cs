using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace ConsoleApp1
{
    public class ElevatorPlatform
    {
        int width = 80;
        Vec2D pos;
        int[] bounds = new int[2];
        float move_speed = 150f;
        bool direction = false;
        public Rect2D rect;
        public ElevatorPlatform(int center_x, int start_y, int spawn_y, int end_y, bool dir)
        {
            pos = new Vec2D(center_x, start_y);
            bounds[0] = spawn_y;
            bounds[1] = end_y;
            direction = dir;

            rect = new Rect2D();
        }
        
        public void render(Game game)
        {
            TextureObject tile = game.GlobalTextures.platform[0];

            float scale = (float)width / tile.Width;
            float height = tile.Height * scale;

            rect.Pos.X = pos.X - (width / 2.0f);
            rect.Pos.Y = pos.Y - (height / 2.0f);
            rect.Size.X = width;
            rect.Size.Y = 1;

            tile.DrawCenter(width, true, pos);

#if DEBUG
            Raylib.DrawRectangleLines(
                (int)rect.Pos.X,
                (int)rect.Pos.Y,
                (int)rect.Size.X,
                (int)rect.Size.Y,
                Color.Blue
            );
#endif
        }
        public void update(Game game)
        {
            float speed = Raylib.GetFrameTime() * this.move_speed;
            if (direction == false)
                speed *= -1;
            pos.Y += speed;
            if (pos.Y < bounds[0])
                pos.Y = bounds[1];
            if (pos.Y > bounds[1])
                pos.Y = bounds[0];
            bool is_ground = game.player.stair_collision_rect.CollideWith(rect);
            if (is_ground && !game.player.IsJumping)
            {
                game.player.pos.Y = this.rect.Pos.Y - game.player.colision_rect.Size.Y;
                game.player.y_velocity = speed;
            }
        }
    }
}