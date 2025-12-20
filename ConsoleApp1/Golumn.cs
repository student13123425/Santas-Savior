using Raylib_cs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Golumn
    {
        public Vec2D pos;
        public int height;
        public Vec2D DownSpawnBarrelPoint;
        public Timer throwTimer;
        public bool IsIdel = false;
        int animation_id = 0;
        private bool has_thrown = false;
        private static Random _rng = new Random();
        private bool is_jumper=false;
        public Golumn(Level level)
        {
            pos = level.DonkeyKongSpawnLocation;
            height = 150;
            throwTimer = new Timer(0.66f, false);
            throwTimer.Play();
            DownSpawnBarrelPoint = new Vec2D(this.pos.X, this.pos.Y + 30);
            IsIdel= !level.is_throwing_barrels;
            is_jumper = level.is_throwing_jumpers;
        }

        public float GetRandomFloat(float min, float max)
        {
            return (float)(_rng.NextDouble() * (max - min) + min);
        }

        public void render(Game game)
        {
            TextureObject textureToDraw;
            int offset_y = 0;
            if (IsIdel)
            {
                textureToDraw = game.GlobalTextures.DonkeyKongTextures.IdelModeAnimation.GetCurrentTexture();
            }
            else
            {
                if (animation_id == 0)
                {
                    textureToDraw = game.GlobalTextures.DonkeyKongTextures.idel;
                }
                else if (animation_id == 1)
                {
                    textureToDraw = game.GlobalTextures.DonkeyKongTextures.ThrowAnimationRight.GetCurrentTexture();
                    if (game.GlobalTextures.DonkeyKongTextures.ThrowAnimationRight.GetFrameIndex()==1)
                        offset_y = 20;
                }
                else
                {
                    textureToDraw = game.GlobalTextures.DonkeyKongTextures.ThrowAnimationDown.GetCurrentTexture();
                    if (game.GlobalTextures.DonkeyKongTextures.ThrowAnimationRight.GetFrameIndex()==1)
                        offset_y = 20;
                }
            }
            
            textureToDraw.DrawBottomCenter(
                this.height,
                false,
                new Vec2D(pos.X,pos.Y+offset_y),
                false
            );
        }

        public void process_timer(Game game)
        {
            if (animation_id != 0)
                return;
            if (throwTimer.Update())
            {
                if (!IsIdel&&!is_jumper)
                    start_throw_animation(game);
                else
                    process_throw_jumper(game);
            }
        }
        void process_throw_jumper(Game game)
        {
            if (!this.is_jumper)
                return;
            game.ThrowJumper();
            throwTimer = new Timer(3f, false); 
            throwTimer.Play(true);
        }
        void start_throw_animation(Game game)
        {
            int choice = _rng.Next(0, 2);

            if (choice == 0)
            {
                this.animation_id = 1;
                game.GlobalTextures.DonkeyKongTextures.ThrowAnimationRight.Play(true);
            }
            else
            {
                this.animation_id = 2;
                game.GlobalTextures.DonkeyKongTextures.ThrowAnimationDown.Play(true);
            }

            this.has_thrown = false;
        }

        void on_throw_animation_end(Game game)
        {
            this.animation_id = 0;
            this.throwTimer = new Timer(GetRandomFloat(1f, 2f), false);
            this.throwTimer.Play(true);
        }

        public void update(Game game)
        {
            if (IsIdel)
            {
                game.GlobalTextures.DonkeyKongTextures.IdelModeAnimation.Update();
                process_timer(game);
            }
            else
            {
                process_timer(game);

                if (animation_id == 1)
                {
                    var anim = game.GlobalTextures.DonkeyKongTextures.ThrowAnimationRight;
                    bool finished = anim.Update();

                    if (!has_thrown && anim.GetFrameIndex() == 2)
                    {
                        game.spawn_barel(true, false);
                        has_thrown = true;
                    }

                    if (finished)
                        this.on_throw_animation_end(game);
                }
                else if (animation_id == 2)
                {
                    var anim = game.GlobalTextures.DonkeyKongTextures.ThrowAnimationDown;
                    bool finished = anim.Update();

                    if (!has_thrown && anim.GetFrameIndex() == 1)
                    {
                        game.spawn_barel(false, true);
                        has_thrown = true;
                    }

                    if (finished)
                        this.on_throw_animation_end(game);
                }
            }
        }
    }
}