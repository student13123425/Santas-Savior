using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Elevator
    {
        public int center_x;
        public int[] bounds = new int[2];
        public int width = 80;
        public bool is_active;
        public ElevatorPlatform[] platforms = new ElevatorPlatform[3];
        public int[] LiftEndpoint = new int[2];

        public Rect2D TopRect;
        public Rect2D BottomRect;

        public Elevator(int center_x, int[] bounds, bool is_active, bool side)
        {
            this.center_x = center_x;
            this.bounds[0] = bounds[0];
            this.bounds[1] = bounds[1];
            this.is_active = is_active;
            int offset = 45;
            LiftEndpoint[0] = bounds[0] + offset;
            LiftEndpoint[1] = bounds[1] - offset;

            int space = LiftEndpoint[1] - LiftEndpoint[0];
            int spacing = space / platforms.Length;

            for (int i = 0; i < platforms.Length; i++)
            {
                int startY;

                if (side)
                {
                    startY = LiftEndpoint[0] + (spacing * i);
                }
                else
                {
                    startY = LiftEndpoint[1] - (spacing * i);
                }

                platforms[i] = new ElevatorPlatform(center_x, startY, LiftEndpoint[0], LiftEndpoint[1], side);
            }

            TopRect = new Rect2D();
            BottomRect = new Rect2D();
        }

        public void render(Game game)
        {
            if (!is_active)
                return;
            int rope_width = 15;
            float current_y = bounds[0];

            TextureObject rope = game.GlobalTextures.ElevatorTextures.Rope;
            TextureObject bottom = game.GlobalTextures.ElevatorTextures.ElevatorBottom;
            TextureObject top = game.GlobalTextures.ElevatorTextures.ElevatorTop;

            float scale = (float)rope_width / rope.Width;
            float segment_height = rope.Height * scale;
            while (current_y < bounds[1])
            {
                rope.DrawTopCenter(rope_width, true, new Vec2D(this.center_x, current_y));
                current_y += segment_height;
            }
            foreach (ElevatorPlatform p in platforms)
                p.render(game);

            float topScale = (float)width / top.Width;
            float topHeight = top.Height * topScale;
            TopRect = new Rect2D(this.center_x - (width / 2.0f), this.bounds[0] + 5, width, topHeight - 10);

            float bottomScale = (float)width / bottom.Width;
            float bottomHeight = bottom.Height * bottomScale;
            BottomRect = new Rect2D(this.center_x - (width / 2.0f), (this.bounds[1] - bottomHeight) + 5, width, bottomHeight - 10);

            top.DrawTopCenter(width, true, new Vec2D(this.center_x, this.bounds[0]));
            bottom.DrawBottomCenter(width, true, new Vec2D(this.center_x, this.bounds[1]));

#if DEBUG
            Raylib.DrawLine(this.center_x, this.bounds[0], this.center_x, this.bounds[1], Color.Red);
            Raylib.DrawCircle(this.center_x, this.LiftEndpoint[0], 10, Color.Red);
            Raylib.DrawCircle(this.center_x, this.LiftEndpoint[1], 10, Color.Red);

            Raylib.DrawRectangleLines((int)TopRect.Pos.X, (int)TopRect.Pos.Y, (int)TopRect.Size.X, (int)TopRect.Size.Y, Color.Magenta);
            Raylib.DrawRectangleLines((int)BottomRect.Pos.X, (int)BottomRect.Pos.Y, (int)BottomRect.Size.X, (int)BottomRect.Size.Y, Color.Magenta);
#endif
        }

        public void update(Game game)
        {
            if (!is_active)
                return;
            foreach (ElevatorPlatform p in platforms)
                p.update(game);
        }
    }
}