using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Raylib_cs.Rectangle;

namespace ConsoleApp1
{
    public class Barel
    {
        public bool is_active = true;
        Vec2D Pos;
        float rotation = 0;
        public Circle circle;
        public Circle ground_check_circle;
        public Rect2D DrawRect;
        public Vec2D velocity = new Vec2D(300, 0);
        public bool mode = false;
        public float error_margin;

        public Barel(Vec2D pos, bool is_active = false, bool mode = false, bool throw_side = true,float error_margin= 15)
        {
            this.error_margin = error_margin;
            this.Pos = new Vec2D(pos.X, pos.Y);
            this.is_active = is_active;
            this.mode = mode;
            this.circle = new Circle(new Vec2D(pos.X + 20, pos.Y + 20), 20f);
            this.ground_check_circle = new Circle(new Vec2D(pos.X + 22, pos.Y + 22), 22f);
            this.DrawRect = new Rect2D(pos, new Vec2D(40, 40));

            if (this.mode == true)
                this.velocity.X = 0;

            if (!throw_side)
                this.velocity.X *= -1;
        }

        void process_gravity(Level level)
        {
            bool is_on_groung = get_if_on_ground(level) && !mode;
            if (is_on_groung)
                velocity.Y = 0;
            else
            {
                float baseSpeed = 150;
                velocity.Y = mode ? baseSpeed * 2.5f : baseSpeed;
            }
        }

        void process_touch_edge_of_screen(Level level)
        {
            if (this.mode) return;

            if (this.velocity.X < 0 && level.Frame[0].CollideWith(this.ground_check_circle))
                this.velocity.X *= -1;
            if (this.velocity.X > 0 && level.Frame[1].CollideWith(this.ground_check_circle))
                this.velocity.X *= -1;
        }

        void proces_end(Level level)
        {
            if (level.BarelDesponLocation.CollideWith(this.ground_check_circle) || this.Pos.Y > 1200)
                this.deactivate();
        }

        void ApplyVelocity(Level level)
        {
            this.Pos.X += velocity.X * Raylib.GetFrameTime();
            update_rect();

            bool is_valid = !get_if_tuch_platform(level, false);
            if (!is_valid)
                this.Pos.X -= velocity.X * Raylib.GetFrameTime();

            this.Pos.Y += velocity.Y * Raylib.GetFrameTime();
            update_rect();

            is_valid = !get_if_tuch_platform(level, true);
            if (!is_valid && !this.mode)
                this.Pos.Y -= velocity.Y * Raylib.GetFrameTime();
        }

        bool get_if_tuch_platform(Level level, bool is_top_only)
        {
            foreach (Platform p in level.platforms)
                if (p.check_colision_circle(this.circle, is_top_only))
                    return true;
            return false;
        }

        void update_rect()
        {
            float circle_size = 40f;
            float margin = (this.error_margin / 100) * circle_size;
            circle = new Circle(new Vec2D(this.Pos.X + 20, this.Pos.Y + 20), circle_size-margin);
            ground_check_circle = new Circle(new Vec2D(this.Pos.X + 20, this.Pos.Y + 21), circle_size-margin+4f);
            DrawRect = new Rect2D(Pos, new Vec2D(40, 40));
        }

        bool get_if_on_ground(Level level)
        {
            foreach (Platform p in level.platforms)
                if (p.check_colision_circle(this.ground_check_circle, true))
                    return true;
            return false;
        }

        void update_rotation()
        {
            rotation += Raylib.GetFrameTime() * 2.75f;
        }

        public void activete()
        {
            this.is_active = true;
        }

        public void deactivate()
        {
            this.is_active = false;
        }

        int get_roation_deg()
        {
            int output = (int)rotation;
            output = output % 4;
            return output * 90;
        }

        public void render(Game game)
        {
            if (!is_active)
                return;
            int r = get_roation_deg();
            
            game.GlobalTextures.Barel.DrawRectCentered(DrawRect, false, (float)r);

#if DEBUG
            render_debug();
#endif
        }

        public void render_debug()
        {
            Rect2D rect = DrawRect;
            Raylib.DrawCircleLines((int)this.circle.Pos.X, (int)this.circle.Pos.Y, this.circle.Radius, Raylib_cs.Color.Green);
            Raylib.DrawCircleLines((int)this.ground_check_circle.Pos.X, (int)this.ground_check_circle.Pos.Y, this.ground_check_circle.Radius, Raylib_cs.Color.Red);
        }

        public void update(Level level)
        {
            if (!is_active)
                return;
            update_rotation();
            update_rect();
            process_gravity(level);
            process_touch_edge_of_screen(level);
            ApplyVelocity(level);
            proces_end(level);
        }
    }
}