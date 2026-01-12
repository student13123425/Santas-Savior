using System;
using System.Collections.Generic;
using Raylib_cs;

namespace ConsoleApp1
{
    public class Enemy
    {
        public static Enemy HuntingEnemy = null;
        private static double last_update_time = 0;
        public bool is_alive = true;
        public bool isDying = false;
        public bool ShouldRemove = false;
        int current_node = 0;
        int next_node = 0;
        float interpolation = 0;
        public Rect2D rectangle;
        float movement_speed = 250f;
        int height = 60;
        Vec2D render_base_point;
        private int[] path = new int[0];
        private bool is_hunter = false;

        public Enemy(Game game, Vec2D spawn_position)
        {
            current_node = game.levels[game.current_level_id].graf.get_cloasest_node(spawn_position);
            next_node = current_node;
            update_rect2D(game.levels[game.current_level_id].graf);

            if (HuntingEnemy == null || !HuntingEnemy.is_alive)
            {
                HuntingEnemy = this;
                is_hunter = true;
            }

            path = get_path(game);
        }

        public int[] get_path(Game game)
        {
            if (is_hunter)
            {
                int end_index = game.player.closest_graf_node;
                int start_index = next_node;
                return game.levels[game.current_level_id].graf.GeneratePath(start_index, end_index);
            }
            else
            {
                int start_index = next_node;
                var graf = game.levels[game.current_level_id].graf;
                int count = graf.Nodes.Count;
                if (count == 0) return new int[0];

                int end_index = Utils.GetRandomInt(0, count - 1);
                
                int attempts = 0;
                while ((end_index == start_index || graf.Nodes[start_index].Point.DistanceTo(graf.Nodes[end_index].Point) < 100) && attempts < 15)
                {
                    end_index = Utils.GetRandomInt(0, count - 1);
                    attempts++;
                }

                return graf.GeneratePath(start_index, end_index);
            }
        }

        bool get_movement_side(Graf graf)
        {
            if (graf.Nodes.Count <= next_node || graf.Nodes.Count <= current_node) return false;
            return graf.Nodes[current_node].Point.X > graf.Nodes[next_node].Point.X;
        }

        void update_rect2D(Graf graf)
        {
            if (graf.Nodes.Count <= next_node || graf.Nodes.Count <= current_node) return;
            Line2D line = new Line2D(graf.Nodes[current_node].Point, graf.Nodes[next_node].Point);
            Vec2D point = line.Interpolate(interpolation);
            int width = (int)((float)height * 0.55);
            Vec2D Size = new Vec2D(width, height);
            Vec2D Position = new Vec2D(point.X - Size.X / 2, point.Y - height);
            rectangle = new Rect2D(Position, Size);
            render_base_point = point;
        }

        float distance_in_between_movement_nodes(Graf graf)
        {
            if (graf.Nodes.Count <= next_node || graf.Nodes.Count <= current_node) return 0;
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
            float animation_speed = normalized_speed(game);
            interpolation += animation_speed;
            if (interpolation > 1)
                next_path_step(game);
        }

        public bool IsClimbing(Game game)
        {
            try
            {
                if (game.levels[game.current_level_id].graf.Nodes.Count <= next_node || game.levels[game.current_level_id].graf.Nodes.Count <= current_node) return false;
                int x1 = (int)game.levels[game.current_level_id].graf.Nodes[current_node].Point.X;
                int x2 = (int)game.levels[game.current_level_id].graf.Nodes[next_node].Point.X;
                return x1 == x2;
            }
            catch
            {
                return false;
            }
        }

        public void Kill(Game game)
        {
            if (isDying) return;
            
            is_alive = false;
            isDying = true;
            game.GlobalTextures.EnemyTextures.explode_animation.Play(true);

            if (is_hunter)
            {
                HuntingEnemy = null;
                is_hunter = false;
                foreach (var robot in game.Robots)
                {
                    if (robot.is_alive && !robot.isDying && !robot.is_hunter)
                    {
                        robot.is_hunter = true;
                        Enemy.HuntingEnemy = robot;
                        robot.path = robot.get_path(game); 
                        break;
                    }
                }
            }
        }

        void process_posible_exlosion(Game game)
        {
            if (isDying)
                return;
            if (game.player.is_hit(rectangle, game))
            {
                Kill(game);
            }
        }

        void next_path_step(Game game)
        {
            current_node = next_node;
            
            bool needNewPath = false;

            if (path.Length == 0)
            {
                needNewPath = true;
            }
            else
            {
                int targetNode = path[path.Length - 1];
                if (current_node == targetNode)
                {
                    needNewPath = true;
                }
            }

            if (needNewPath)
            {
                path = get_path(game);
                interpolation = 0;
                
                if (path.Length > 0)
                {
                    next_node = path[0];
                }
                else
                {
                    next_node = current_node; 
                }
                return;
            }

            int currentIndexInPath = -1;
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == current_node)
                {
                    currentIndexInPath = i;
                    break;
                }
            }

            if (currentIndexInPath != -1 && currentIndexInPath + 1 < path.Length)
            {
                next_node = path[currentIndexInPath + 1];
            }
            else
            {
                path = get_path(game);
                if (path.Length > 0)
                    next_node = path[0];
                else
                    next_node = current_node;
            }

            interpolation = 0;
        }

        public void render(Game game)
        {
            bool draw_side = get_movement_side(game.levels[game.current_level_id].graf);
            if (is_alive && !isDying)
            {
                TextureObject texture = game.GlobalTextures.EnemyTextures.walk_animation.GetCurrentTexture();
                if (IsClimbing(game))
                    texture = game.GlobalTextures.EnemyTextures.climb_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point, draw_side);
            }
            else
            {
                TextureObject texture = game.GlobalTextures.EnemyTextures.explode_animation.GetCurrentTexture();
                game.GlobalTextures.renderer.DrawTextureBottomCenter(texture.Texture, height, false, render_base_point, draw_side);
            }

            if (game.is_debug)
            {
                Color debugColor = is_hunter ? Color.Red : Color.Blue;
                Raylib.DrawRectangleLines((int)rectangle.Pos.X, (int)rectangle.Pos.Y, (int)rectangle.Size.X, (int)rectangle.Size.Y, debugColor);
            }
        }

        public void Update(Game game, bool force_path_update)
        {
            if (isDying)
            {
                bool finished = game.GlobalTextures.EnemyTextures.explode_animation.Update();
                if (finished)
                {
                    ShouldRemove = true;
                }
                return;
            }

            if (is_alive && !isDying && (HuntingEnemy == null || !HuntingEnemy.is_alive) && !is_hunter)
            {
                HuntingEnemy = this;
                is_hunter = true;
                force_path_update = true; 
            }

            double currentTime = Raylib.GetTime();
            if (currentTime > last_update_time)
            {
                game.GlobalTextures.EnemyTextures.walk_animation.Play(false);
                game.GlobalTextures.EnemyTextures.walk_animation.Update();
                
                game.GlobalTextures.EnemyTextures.climb_animation.Play(false);
                game.GlobalTextures.EnemyTextures.climb_animation.Update();
                
                last_update_time = currentTime;
            }

            if (force_path_update)
                path = get_path(game);

            process_posible_exlosion(game);
            update_animation(game);
            update_rect2D(game.levels[game.current_level_id].graf);
        }
    }
}