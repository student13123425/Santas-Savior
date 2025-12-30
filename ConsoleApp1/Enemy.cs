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
        Vec2D render_base_point;
        private int[] path=new int[0];
        public Enemy(Game game,Vec2D spawn_position)
        {
            current_node = game.levels[game.current_level_id].graf.get_cloasest_node(spawn_position);
            next_node = current_node;
            update_rect2D(game.levels[game.current_level_id].graf);
            path=get_path(game);
        }

        int[] get_path(Game game)
        {
            int end_index=game.player.closest_graf_node;
            int start_index = next_node;
            return game.levels[game.current_level_id].graf.GeneratePath(start_index, end_index);
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

        float normalized_speed(Game game)
        {
            float distance = distance_in_between_movement_nodes(game.levels[game.current_level_id].graf);
            if (distance <= float.Epsilon) return 1f;
            float pixels_to_move = movement_speed * Raylib.GetFrameTime();
            return pixels_to_move / distance;
        }
        public void update_animation(Game game)
        {
            float animation_speed=normalized_speed(game);
            interpolation+=animation_speed;
            if (interpolation > 1)
                next_path_step(game);
        }

        public bool IsClimbing(Game game)
        {
            try
            {
                int x1 = (int)game.levels[game.current_level_id].graf.Nodes[current_node].Point.X;
                int x2 = (int)game.levels[game.current_level_id].graf.Nodes[next_node].Point.X;
                return x1 == x2;
            }
            catch
            {
                return false;
            }
        }
        void process_posible_exlosion(Game game)
        {   if (isDying)
                return;
            if (game.player.is_hit(rectangle,game))
            {
                is_alive = false;
                isDying = true;
                game.GlobalTextures.EnemyTextures.explode_animation.Play(false);
            }
        }
        void next_path_step(Game game)
        {
            current_node=next_node;
            bool is_last_current = false;
            foreach (int v in path)
            {
                if (is_last_current)
                {
                    next_node = v;
                    break;
                }
                is_last_current = v==current_node;
            }
            interpolation = 0;
        }
        public void render(Game game)
        {
            bool draw_side = get_movement_side(game.levels[game.current_level_id].graf);
            if (is_alive && !isDying)
            {
                TextureObject texture = game.GlobalTextures.EnemyTextures.walk_animation.GetCurrentTexture();
                if(IsClimbing(game))
                    texture = game.GlobalTextures.EnemyTextures.climb_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point, draw_side);
            }
            else
            {
                TextureObject texture = game.GlobalTextures.EnemyTextures.explode_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point, draw_side);
            }
#if DEBUG
            Raylib.DrawRectangleLines((int)rectangle.Pos.X, (int)rectangle.Pos.Y, (int)rectangle.Size.X, (int)rectangle.Size.Y, Color.Red);
#endif
        }

        public void Update(Game game,bool force_path_update)
        {
            if (force_path_update)
                path=get_path(game);
            process_posible_exlosion(game);
            update_animation(game);
            update_rect2D(game.levels[game.current_level_id].graf);
        }
    }
}