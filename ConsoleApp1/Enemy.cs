using System;
using Raylib_cs;

namespace ConsoleApp1
{
    public class Enemy
    {
        bool is_alive = true;
        bool isDying = false;
        int current_node = 0;
        int next_node = 0;
        float interpolation = 0;
        Rect2D rectangle;
        float movement_speed = 300f;
        int height = 60;
        private Vec2D render_base_point;
        public Enemy(Graf graf, Vec2D spawn_position)
        {
            current_node = get_cloasest_node(graf, spawn_position);
            next_node = current_node;
            update_rect2D(graf);
        }

        bool get_movement_side(Graf graf)
        {
            return graf.Nodes[current_node].Point.X < graf.Nodes[next_node].Point.X;
        }
        void update_rect2D(Graf graf)
        {
            Line2D line = new Line2D(graf.Nodes[current_node].Point, graf.Nodes[next_node].Point);
            Vec2D point = line.Interpolate(interpolation);
            int width = (int)((float)height * 0.55);
            Vec2D Size = new Vec2D(width, height);
            Vec2D Position = new Vec2D(point.X - Size.X / 2, point.Y);
            rectangle = new Rect2D(Position, Size);
            render_base_point = point;
        }

        float distance_in_between_movement_nodes(Graf graf)
        {
            return graf.Nodes[current_node].Point.DistanceTo(graf.Nodes[next_node].Point);
        }

        int get_cloasest_node(Graf graf, Vec2D spawn_position)
        {
            float min_value = 9999;
            int index = 0;
            int min_node_index = 0;
            foreach (GrafNode node in graf.Nodes)
            {
                float value = node.Point.DistanceTo(spawn_position);
                if (value < min_value)
                {
                    min_value = value;
                    min_node_index = index;
                }
                index++;
            }
            return min_node_index;
        }

        public void render(Game game)
        {   
            bool draw_side = get_movement_side(game.levels[game.current_level_id].graf);
            if (is_alive && !isDying)
            {
                TextureObject texture=game.GlobalTextures.EnemyTextures.walk_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point,
                    draw_side);
            }
            else
            {
                TextureObject texture=game.GlobalTextures.EnemyTextures.explode_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point,
                    draw_side);
            }
#if DEBUG
            Raylib.DrawRectangleLines((int)rectangle.Pos.X, (int)rectangle.Pos.Y, (int)rectangle.Size.X, (int)rectangle.Size.Y, Color.Red);
#endif
            
            
        }
        public void Update(Game game)
        {
            update_rect2D(game.levels[game.current_level_id].graf);
        }
    }
}