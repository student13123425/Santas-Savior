using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class OilBarel
    {
        Rect2D rect;
        Rect2D fireHitbox;
        bool is_empty = false;
        bool is_active = true;
        bool is_to_spawn = false;
        bool is_spawn_on_barrel = false;
        public OilBarel(Vec2D pos, bool e,int spawn_mode, bool isActive = true)
        {
            Vec2D size = new Vec2D(69, 80);
            this.is_empty = e;
            this.is_active = isActive;
            rect = new Rect2D(pos, size);

            float fireWidth = size.X * 0.8f;
            float fireHeight = size.Y * 0.3f;

            Vec2D firePos = new Vec2D(
                pos.X + (size.X - fireWidth) / 2,
                pos.Y - fireHeight
            );
            if (spawn_mode == 1)
            {
                is_to_spawn = true;
                is_spawn_on_barrel = false;
            }
            else if(spawn_mode == 2)
            {
                is_to_spawn = true;
                is_spawn_on_barrel = true;
            }
            fireHitbox = new Rect2D(firePos, new Vec2D(fireWidth, fireHeight));
        }

        public int get_closest_spawn_point_id(Game game)
        {
            Vec2D center = rect.Center;
            int output = -1;
            float dist = 99999;
            int index = 0;
            foreach (GrafNode node  in game.levels[game.current_level_id].graf.Nodes)
            {
                float local_dist = center.DistanceTo(node.Point);
                if (local_dist < dist)
                {
                    output = index;
                    dist = local_dist;
                }
                index += 1;
            }
            return -1;
        }

        public void spawn_enemy(Game game)
        {
            int spawn_point_id = get_closest_spawn_point_id(game);
            game.Robots.Add(new Enemy(game, game.levels[game.current_level_id].graf.Nodes[spawn_point_id].Point));
        }
        public bool IsColide(Circle c)
        {
            if (!is_active) return false;

            return this.rect.CollideWith(c) || this.fireHitbox.CollideWith(c);
        }
        public void render(Game game, bool showDebug = true)
        {
            if (!is_active || is_empty) return;

            game.GlobalTextures.oilbarrel.Play();
            game.GlobalTextures.oilbarrel.Update();
            TextureObject texture = game.GlobalTextures.oilbarrel.GetCurrentTexture();
            float centerX = rect.Pos.X + (rect.Size.X / 2.0f);
            float bottomY = rect.Pos.Y + rect.Size.Y;
            Vec2D bottomCenterPos = new Vec2D(centerX, bottomY);
            
            texture.DrawBottomCenter(rect.Size.Y+20, false, bottomCenterPos);

            if (showDebug)
            {
                Raylib.DrawRectangleLines(
                    (int)rect.Pos.X,
                    (int)rect.Pos.Y,
                    (int)rect.Size.X,
                    (int)rect.Size.Y,
                    Color.Red
                );

                Raylib.DrawRectangleLines(
                    (int)fireHitbox.Pos.X,
                    (int)fireHitbox.Pos.Y,
                    (int)fireHitbox.Size.X,
                    (int)fireHitbox.Size.Y,
                    Color.Orange
                );
            }
        }
    }
}