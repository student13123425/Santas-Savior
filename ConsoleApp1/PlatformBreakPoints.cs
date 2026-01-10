using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class PlatformBreakPoint
    {
        public bool broke = false;
        public Vec2D pos;
        public float size;
        public Rect2D triggerRect;
        public bool is_active;

        public PlatformBreakPoint(Vec2D pos, float size, bool is_active)
        {
            this.pos = pos;
            this.size = size;
            this.is_active = is_active;
            this.triggerRect = new Rect2D(pos.X - size / 2, pos.Y - 3, size, 3);
        }

        public void update(Game game)
        {
            if (!is_active) return;
            if (broke) return;

            if (game.player.is_on_ground_colision_rect.CollideWith(this.triggerRect))
                broke = true;
        }

        public void render(Game game)
        {
            if (!is_active) return;

            int x = (int)(pos.X - size / 2);
            int y = (int)pos.Y;
            int s = (int)size;

            if (!broke)
            {
                Raylib.DrawRectangle(x, y, s, s, new Color(20, 20, 20, 255));

                int borderThickness = 3;
                Raylib.DrawRectangleLinesEx(new Rectangle(x, y, s, s), borderThickness, Color.Gray);

                int barCount = 4;
                float spacing = s / (float)barCount;

                for (int i = 1; i < barCount; i++)
                {
                    int barY = (int)(y + (i * spacing));
                    Raylib.DrawRectangle(x + borderThickness, barY, s - (borderThickness * 2), 2, Color.LightGray);
                    Raylib.DrawRectangle(x + borderThickness, barY + 2, s - (borderThickness * 2), 1, new Color(0, 0, 0, 100));
                }

                Raylib.DrawLine(x, y, x + s, y + s, new Color(200, 200, 200, 50));
                Raylib.DrawLine(x + s, y, x, y + s, new Color(200, 200, 200, 50));

                int rivetOffset = 5;
                float rivetRadius = 2.0f;
                Color rivetColor = Color.RayWhite;

                Raylib.DrawCircle(x + rivetOffset, y + rivetOffset, rivetRadius, rivetColor);
                Raylib.DrawCircle(x + s - rivetOffset, y + rivetOffset, rivetRadius, rivetColor);
                Raylib.DrawCircle(x + rivetOffset, y + s - rivetOffset, rivetRadius, rivetColor);
                Raylib.DrawCircle(x + s - rivetOffset, y + s - rivetOffset, rivetRadius, rivetColor);
            }
            else
            {
                Raylib.DrawRectangle(x, y, s, s, Color.Black);
                Raylib.DrawLine(x, y, x + (s / 4), y + 5, Color.Gray);
                Raylib.DrawLine(x + s, y, x + s - (s / 4), y + 5, Color.Gray);
            }

            if (game.is_debug)
            {
                Raylib.DrawRectangleLines(
                    (int)triggerRect.Pos.X,
                    (int)triggerRect.Pos.Y,
                    (int)triggerRect.Size.X,
                    (int)triggerRect.Size.Y,
                    Color.Purple
                );
            }
        }
    }
}