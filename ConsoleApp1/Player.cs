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
    public class Player
    {
        public int score = 0;
        public Vec2D pos;
        bool IsClimbing = false;
        public Rect2D colision_rect;
        public Rect2D is_on_ground_colision_rect; 
        bool is_on_ground = true;
        int health = 5;
        int max_health = 5;
        public float y_velocity = 0;
        bool side = false;
        bool IsAtacking = false;
        public bool IsDying = false; 
        Rect2D player_hit_rect;
        Rect2D player_hit_rect_top;
        public Rect2D stair_collision_rect;
        int climbing_star_index = -1;
        Timer step_delay_timer;
        public bool IsJumping = false;

        public Player(Vec2D pos)
        {
            this.pos = new Vec2D(pos.X, pos.Y - 4);
            this.step_delay_timer = new Timer(0.15f);
            this.update_rects();
        }

        public int GetHealth() { return health; }
        public void SetHealth(int h) { health = h; }

        void update_rects()
        {
            Vec2D player_size_colision = new Vec2D(48, 78);
            Vec2D player_offset_colision = new Vec2D(4, 2);
            colision_rect = new Rect2D(pos.X + player_offset_colision.X, pos.Y + player_offset_colision.Y, player_size_colision.X, player_size_colision.Y);
            is_on_ground_colision_rect = new Rect2D(pos.X + 26, 78 + pos.Y, 2, 4);
            Vec2D hit_box_size = new Vec2D(55, 90);
            Vec2D hit_offset = new Vec2D(-70, -10);

            if (!this.side)
            {
                hit_offset.X *= -1f;
                hit_offset.X += 3f;
            }

            this.player_hit_rect = new Rect2D(new Vec2D(this.pos.X + hit_offset.X, this.pos.Y + hit_offset.Y), hit_box_size);

            Vec2D top_hit_size = new Vec2D(48, 60);
            Vec2D top_hit_pos = new Vec2D(this.pos.X + 4, this.pos.Y - 40);
            this.player_hit_rect_top = new Rect2D(top_hit_pos, top_hit_size);

            Vec2D size = new Vec2D(this.colision_rect.Size.X, 10);
            this.stair_collision_rect = new Rect2D(new Vec2D(this.colision_rect.Pos.X, this.colision_rect.Pos.Y + this.colision_rect.Size.Y - (size.Y / 2)), size);
        }

        int find_current_stairs(Level level)
        {
            int output = 0;
            foreach (Stairs stair in level.stairs)
            {
                if (stair.getPlayerColision(this.colision_rect) != 0)
                    return output;
                output += 1;
            }
            return -1;
        }

        void movement_input(Level level, Game instance)
        {
            bool is_right = Raylib.IsKeyDown(KeyboardKey.D) || Raylib.IsKeyDown(KeyboardKey.Right);
            bool is_left = Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.Left);

            if ((is_right && is_left) || (!is_right && !is_left))
                return;

            if (is_left)
                process_movement(level, false, instance);
            else if (is_right)
                process_movement(level, true, instance);
        }

        bool check_in_frame(Level level)
        {
            return !(level.Frame[0].CollideWith(colision_rect) || level.Frame[1].CollideWith(colision_rect));
        }

        void process_atack(Level level, Game instance)
        {
            int frameIndex = instance.GlobalTextures.PlayerTextures.atack_animation.GetFrameIndex();

            Rect2D active_rect = null;

            if (frameIndex == 1)
            {
                active_rect = this.player_hit_rect;
            }
            else if (frameIndex == 0 || frameIndex == 2)
            {
                active_rect = this.player_hit_rect_top;
            }

            if (active_rect != null)
            {
                foreach (Barel b in instance.barels)
                {
                    if (b.circle.CollideWith(active_rect))
                    {
                        this.score += 300;
                        b.deactivate();
                        instance.GlobalAudio.EnemyHit.Play(false);
                    }
                }
            }
        }

        void atack(Level level, Game instance)
        {
            this.IsAtacking = true;
            var anim = instance.GlobalTextures.PlayerTextures.atack_animation;
            anim.Play(true);
        }

        void process_movement(Level level, bool side, Game instance)
        {
            this.side = !side;
            bool is_on_ground = get_if_on_ground(level);
            float move_speed = 300 * Raylib.GetFrameTime();

            if (is_on_ground)
            {
                instance.GlobalTextures.PlayerTextures.run_animation.Play(false);
                instance.GlobalTextures.PlayerTextures.run_animation.Update();

                float slope = get_ground_slope(level);
                if (!side)
                    slope -= 180;
                Vec2D old = new Vec2D(pos.X, pos.Y);
                pos.MoveAtAngle(slope, move_speed);
                update_rects();

                if (check_colide_platform(level) || !check_in_frame(level))
                {
                    pos = old;
                }
                else
                {
                    step_delay_timer.Update();

                    if (instance.GlobalAudio.Walk.IsFinished())
                    {
                        if (!step_delay_timer.IsPlaying && step_delay_timer.CurrentTime >= step_delay_timer.Lifetime)
                        {
                            instance.GlobalAudio.Walk.Play(false);
                            step_delay_timer.Stop();
                        }
                        else if (!step_delay_timer.IsPlaying)
                        {
                            step_delay_timer.Play(true);
                        }
                    }
                    else
                    {
                        step_delay_timer.Stop();
                    }
                }
                update_rects();
            }
            else
            {
                if (side)
                {
                    pos.X += move_speed;
                    update_rects();
                    if (check_colide_platform(level) || !check_in_frame(level))
                        pos.X -= move_speed;
                    update_rects();
                }
                else
                {
                    pos.X -= move_speed;
                    update_rects();
                    if (check_colide_platform(level) || !check_in_frame(level))
                        pos.X += move_speed;
                    update_rects();
                }
            }
        }

        public void slide(float speed, Level level)
        {
            this.pos.X += speed;
            update_rects();

            if (check_colide_platform(level) || !check_in_frame(level))
            {
                this.pos.X -= speed;
                update_rects();
            }
        }

        public bool check_colide_platform(Level level)
        {
            foreach (Platform p in level.platforms)
                if (p.check_colision_rect(colision_rect, false))
                    return true;
            foreach (Elevator e in level.elevators)
            {
                if (e.is_active)
                {
                    if (e.TopRect.CollideWith(colision_rect)) return true;
                    if (e.BottomRect.CollideWith(colision_rect)) return true;
                }
            }

            foreach (ConveyerBelt b in level.conveyerBelts)
            {
                if (b.is_active && b.rect.CollideWith(colision_rect))
                    return true;
            }

            return false;
        }

        public void ProcessStartClimbing(Level level)
        {
            int index = find_current_stairs(level);
            if (index == -1 || !(Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Up) || Raylib.IsKeyDown(KeyboardKey.Down)))
                return;
            pos.X = level.stairs[index].get_center_x() - 28;
            bool is_top = level.stairs[index].getPlayerColision(colision_rect) == 1;
            if ((Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up)) && is_top)
            {
                IsClimbing = true;
                climbing_star_index = index;
                y_velocity = 0;
            }
            else if ((Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down)) && !is_top)
            {
                IsClimbing = true;
                climbing_star_index = index;
                y_velocity = 0;
            }
            update_rects();

        }

        void ClimbControl(Level level, Game instance)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                jump(level, instance);
            float climb_speed = Raylib.GetFrameTime() * 300;
            bool is_down = Raylib.IsKeyDown(KeyboardKey.S) || Raylib.IsKeyDown(KeyboardKey.Down);
            bool is_up = Raylib.IsKeyDown(KeyboardKey.W) || Raylib.IsKeyDown(KeyboardKey.Up);
            Vec2D[] end_points = level.stairs[this.climbing_star_index].StartAndEnd;
            bool moving = false;

            if (is_down && is_up)
                return;
            if (is_down)
            {
                moving = true;
                pos.Y += climb_speed;
            }
            if (is_up)
            {
                moving = true;
                pos.Y -= climb_speed;
            }

            if (moving)
            {
                instance.GlobalTextures.PlayerTextures.climb_animation.Play(false);
                instance.GlobalTextures.PlayerTextures.climb_animation.Update();
            }

            update_rects();
            if (is_down && this.stair_collision_rect.Contains(end_points[0]))
                stopClimbing(level);
            if (is_up && this.stair_collision_rect.Contains(end_points[1]))
                stopClimbing(level);
        }

        void stopClimbing(Level level)
        {
            IsClimbing = false;
            int index = 0;
            if (this.stair_collision_rect.Contains(level.stairs[this.climbing_star_index].StartAndEnd[1]))
                index = 1;
            this.pos.Y = level.stairs[this.climbing_star_index].StartAndEnd[index].Y - this.colision_rect.Size.Y - 3;
            this.climbing_star_index = -1;

            this.y_velocity = 0;
            update_rects();
        }

        bool can_atack(Level level, Game instance)
        {
            return this.get_if_on_ground(level) && !IsClimbing && !IsAtacking;
        }

        public void update(Level level, Game instance)
        {
            if (IsDying)
            {
                bool animFinished = instance.GlobalTextures.PlayerTextures.death_animation.Update();
                bool audioFinished = instance.GlobalAudio.Death.IsFinished();
                
                if (animFinished && audioFinished)
                {
                    instance.onLive();
                }
                return; 
            }

            if (IsClimbing)
            {
                ClimbControl(level, instance);
                return;
            }

            if (IsAtacking)
            {
                process_atack(level, instance);
                bool finished = instance.GlobalTextures.PlayerTextures.atack_animation.Update();
                if (finished)
                {
                    IsAtacking = false;
                }
                return;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Space))
                jump(level, instance);
            else if (can_atack(level, instance) && Raylib.IsKeyPressed(KeyboardKey.E))
            {
                atack(level, instance);
                return;
            }

            process_fall(level);

            apply_velocity(level);
            movement_input(level, instance);
            ProcessStartClimbing(level);

            is_on_ground = get_if_on_ground(level);

            if (is_on_ground && y_velocity >= 0)
            {
                IsJumping = false;
            }
        }

        public void apply_velocity(Level level)
        {
            float move_amount = this.y_velocity;
            pos.Y += move_amount;
            update_rects();
            if (check_colide_platform(level))
            {
                y_velocity = 0;
                pos.Y -= move_amount;
            }
            update_rects();
        }

        public void process_fall(Level level)
        {
            float fall_speed = 5 * Raylib.GetFrameTime();

            if (y_velocity < 0)
                fall_speed = 3 * Raylib.GetFrameTime();

            bool is_on_ground = this.get_if_on_ground(level);
            const int max_velocity = 12;
            if (!is_on_ground)
                y_velocity += fall_speed;
            if (y_velocity > max_velocity)
                y_velocity = max_velocity;
            if (is_on_ground && y_velocity > 0)
                y_velocity = 0;
        }

        public void jump(Level level, Game instance)
        {
            float jump_power = 500 * Raylib.GetFrameTime();
            bool is_on_ground = this.get_if_on_ground(level);
            if ((is_on_ground || this.IsClimbing) && !check_colide_platform(level))
            {
                y_velocity = -jump_power;
                this.IsClimbing = false;
                this.IsJumping = true;
                instance.GlobalAudio.Jump.Play(false);
            }
        }

        bool get_if_on_ground(Level level)
        {
            foreach (Platform p in level.platforms)
                if (p.check_colision_rect(is_on_ground_colision_rect, true))
                    return true;
            foreach (Elevator e in level.elevators)
            {
                if (e.is_active)
                {
                    foreach (ElevatorPlatform platform in e.platforms)
                        if (platform.rect.CollideWith(is_on_ground_colision_rect))
                            return true;
                    if (e.TopRect.CollideWith(is_on_ground_colision_rect)) return true;
                    if (e.BottomRect.CollideWith(is_on_ground_colision_rect)) return true;
                }
            }

            foreach (ConveyerBelt b in level.conveyerBelts)
            {
                if (b.is_active && b.rect.CollideWith(is_on_ground_colision_rect))
                    return true;
            }

            return false;
        }

        public float get_ground_slope(Level level)
        {
            foreach (Platform p in level.platforms)
                if (p.check_colision_rect(is_on_ground_colision_rect, true))
                    return p.collison_lines[0].get_angle();
            return 0;
        }

        void render_health(Game game, bool isRight)
        {
            int screenWidth = Raylib.GetScreenWidth();
            for (int i = 0; i < 5; i++)
            {
                bool is_full = i < this.health;
                TextureObject tex;
                if (is_full)
                    tex = game.GlobalTextures.PlayerTextures.health_texture[0];
                else
                    tex = game.GlobalTextures.PlayerTextures.health_texture[1];

                float x;
                if (isRight)
                {
                    x = screenWidth - 64 - (60 * i);
                }
                else
                {
                    x = 10 + (60 * i);
                }

                Rect2D dest = new Rect2D(x, 10, 54, 50);
                tex.DrawRect(dest, false);
            }

            string scoreText = "SCORE: " + this.score;
            int fontSize = 35;
            float spacing = 1.0f;

            Vector2 textSize = Raylib.MeasureTextEx(game.GlobalTextures.GameFont, scoreText, fontSize, spacing);

            float textX = isRight ? screenWidth - textSize.X - 10 : 10;
            float textY = 70;

            Raylib.DrawTextEx(game.GlobalTextures.GameFont, scoreText, new Vector2(textX, textY), fontSize, spacing, Raylib_cs.Color.White);
        }

        public bool is_hit(Circle circle, Game game)
        {
            bool output = circle.CollideWith(this.colision_rect);
            if (output == false)
                return output;

            if (IsDying) return false;

            health -= 1;
            
            IsDying = true;
            score = 0;
            game.GlobalTextures.PlayerTextures.death_animation.Play(true);
            game.GlobalAudio.Death.Play(false);
            
            return output;
        }

        public bool is_hit(Rect2D rect, Game game)
        {
            bool output = rect.CollideWith(this.colision_rect);
            if (output == false)
                return output;

            if (IsDying) return false;

            health -= 1;
            
            IsDying = true;
            game.GlobalTextures.PlayerTextures.death_animation.Play(true);
            game.GlobalAudio.Death.Play(false);
            
            return output;
        }

        public void render(Game game, bool healthOnRight = true)
        {
            render_health(game, healthOnRight);
            TextureObject textureToDraw = default;
            Rect2D drawRect = new Rect2D(colision_rect.Pos.X, colision_rect.Pos.Y, colision_rect.Size.X, colision_rect.Size.Y);
            bool flip = !side;

            bool usedCustomRender = false;

            if (IsDying)
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.death_animation.GetCurrentTexture();
                int offset = 0;
                if (game.GlobalTextures.PlayerTextures.death_animation.GetFrameIndex() == 3)
                    offset = 22;
                int height = 80 + offset;
                if (!side)
                    offset *= -1;
                Vec2D pos = new Vec2D(colision_rect.Pos.X + colision_rect.Size.X / 2.0f+offset, colision_rect.Pos.Y + colision_rect.Size.Y);
                textureToDraw.DrawBottomCenter(height, false, pos, flip);
                return;
            }
            if (IsClimbing)
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.climb_animation.GetCurrentTexture();
            }
            else if (IsAtacking)
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.atack_animation.GetCurrentTexture();
                int index = game.GlobalTextures.PlayerTextures.atack_animation.GetFrameIndex();

                float aspect = (float)textureToDraw.Width / textureToDraw.Height;
                float dest_width;
                float dest_height;
                float original_height = colision_rect.Size.Y;
                float original_width = colision_rect.Size.X;

                if (index == 0 || index == 2)
                {
                    dest_height = original_height * 1.5f;
                    dest_width = dest_height * aspect;
                }
                else
                {
                    dest_width = original_width * 2.8f;
                    dest_height = dest_width / aspect;
                }

                float dest_x;
                float dest_y = colision_rect.Pos.Y + original_height - dest_height;

                if (!side)
                {
                    dest_x = colision_rect.Pos.X;
                }
                else
                {
                    dest_x = colision_rect.Pos.X + original_width - dest_width;
                }

                drawRect = new Rect2D(dest_x, dest_y, dest_width, dest_height);
            }
            else if (!is_on_ground && !IsClimbing)
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.jump_texture;
                flip = side;
            }
            else if ((Raylib.IsKeyDown(KeyboardKey.A) || Raylib.IsKeyDown(KeyboardKey.D) ||
                      Raylib.IsKeyDown(KeyboardKey.Left) || Raylib.IsKeyDown(KeyboardKey.Right)) && is_on_ground)
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.run_animation.GetCurrentTexture();

                Vec2D pos = new Vec2D(colision_rect.Pos.X + colision_rect.Size.X / 2.0f, colision_rect.Pos.Y + colision_rect.Size.Y);

                textureToDraw.DrawBottomCenter(colision_rect.Size.Y, false, pos, flip);
                usedCustomRender = true;
            }
            else
            {
                textureToDraw = game.GlobalTextures.PlayerTextures.idel;
            }

            if (!usedCustomRender)
            {
                textureToDraw.DrawRect(drawRect, flip);
            }

#if DEBUG
            Raylib.DrawRectangleLines((int)colision_rect.Pos.X, (int)colision_rect.Pos.Y, (int)colision_rect.Size.X, (int)colision_rect.Size.Y, Raylib_cs.Color.Green);
            Raylib.DrawRectangleLines((int)is_on_ground_colision_rect.Pos.X, (int)is_on_ground_colision_rect.Pos.Y, (int)is_on_ground_colision_rect.Size.X, (int)is_on_ground_colision_rect.Size.Y, Raylib_cs.Color.Red);
            Raylib.DrawRectangleLines((int)player_hit_rect.Pos.X, (int)player_hit_rect.Pos.Y, (int)player_hit_rect.Size.X, (int)player_hit_rect.Size.Y, Raylib_cs.Color.Yellow);
            Raylib.DrawRectangleLines((int)player_hit_rect_top.Pos.X, (int)player_hit_rect_top.Pos.Y, (int)player_hit_rect_top.Size.X, (int)player_hit_rect_top.Size.Y, Raylib_cs.Color.Orange);
            Raylib.DrawRectangleLines((int)stair_collision_rect.Pos.X, (int)stair_collision_rect.Pos.Y, (int)stair_collision_rect.Size.X, (int)stair_collision_rect.Size.Y, Raylib_cs.Color.Blue);
#endif
        }
    }
}