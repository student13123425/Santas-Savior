using Raylib_cs;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class SantaClausTextureMap
    {
        public AnimationPlayer anim;
        public TextureObject[] drops;
        public Dictionary<int, int> DropScores;

        public void Load(TextureRenderer renderer)
        {
             anim = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/Clause1.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Clause2.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Clause3.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Clause4.png"), renderer),
            ], 0.7f, true);
            anim.Play(true);
            drops = [
                new TextureObject(Raylib.LoadTexture("./sprites/Lollipop.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Purse.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Cloud.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Umbrela.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Mirror.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Letter.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Parfume.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Rose.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/MusicBox.png"), renderer)
            ];

            DropScores = new Dictionary<int, int>()
            {
                { 0, 200 },
                { 1, 800 },
                { 2, 100 },
                { 3, 500 },
                { 4, 500 },
                { 5, 1500 },
                { 6, 800 },
                { 7, 1000 },
                { 8, 1200 }
            };
        }
    }

    public class PlayerTextureMap
    {
        public AnimationPlayer climb_animation;
        public AnimationPlayer run_animation;
        public AnimationPlayer atack_animation;
        public TextureObject idel;
        public TextureObject jump_texture;
        public TextureObject[] health_texture = new TextureObject[2];
        public AnimationPlayer death_animation;

        public void Load(TextureRenderer renderer)
        {
            climb_animation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/Climb1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Climb2.png"), renderer)
            ], 0.200f, true);

            run_animation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/Run1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Run2.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Run3.png"), renderer)
            ], 0.2f, true);
            
            atack_animation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/Attack1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Attack2.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Attack3.png"), renderer)
            ], 0.1f, false);
            
            death_animation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/Death1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Death2.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/Death3.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Death4.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/Death5.png"), renderer)
            ], 0.3f, false);
            
            if (death_animation.FrameCount > 1)
            {
                float totalDuration = death_animation.FrameDuration * death_animation.FrameCount;
                float lastFrameTime = totalDuration * 0.40f; 
                float otherFramesTime = (totalDuration * 0.60f) / (death_animation.FrameCount - 1);

                float[] deathDurations = new float[death_animation.FrameCount];
                for (int i = 0; i < death_animation.FrameCount; i++)
                {
                    if (i == death_animation.FrameCount - 1)
                    {
                        deathDurations[i] = lastFrameTime;
                    }
                    else
                    {
                        deathDurations[i] = otherFramesTime;
                    }
                }
                death_animation.CustomFrameDurations = deathDurations;
            }

            if (atack_animation.FrameCount > 1)
            {
                float totalDuration = atack_animation.FrameDuration * atack_animation.FrameCount;
                float longFrameTime = totalDuration * 0.5f;
                float otherFramesTime = (totalDuration * 0.5f) / (atack_animation.FrameCount - 1);

                float[] durations = new float[atack_animation.FrameCount];
                for (int i = 0; i < atack_animation.FrameCount; i++)
                {
                    if (i == 1)
                    {
                        durations[i] = longFrameTime;
                    }
                    else
                    {
                        durations[i] = otherFramesTime;
                    }
                }
                atack_animation.CustomFrameDurations = durations;
            }

            idel = new TextureObject(Raylib.LoadTexture("./sprites/Idle.png"), renderer);
            jump_texture = new TextureObject(Raylib.LoadTexture("./sprites/Jump.png"), renderer);

            health_texture[0] = new TextureObject(Raylib.LoadTexture("./sprites/HeartFull.png"), renderer);
            health_texture[1] = new TextureObject(Raylib.LoadTexture("./sprites/HeartBroken.png"), renderer);
        }
    }

    public class GolumnTextureMap
    {
        public TextureObject idel;
        public AnimationPlayer ThrowAnimationRight;
        public AnimationPlayer ThrowAnimationDown;
        public AnimationPlayer IdelModeAnimation;
        
        public void Load(TextureRenderer renderer)
        {
            idel = new TextureObject(Raylib.LoadTexture("./sprites/GolumnIdle.png"), renderer);
            
            ThrowAnimationRight = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnBarrel_1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnBarrel_2.png"), renderer,new Vec2D(0,20)), 
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnBarrel_3.png"), renderer)
            ], 0.4f, false);
            
            ThrowAnimationDown = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnBarrel_1.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnBarrel_2.png"), renderer,new Vec2D(0,20))
            ], 0.2f, false);
            
            IdelModeAnimation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/BeatLeft.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/GolumnIdle.png"), renderer), 
                new TextureObject(Raylib.LoadTexture("./sprites/BeatRight.png"), renderer)
            ], 0.4f, true);
            
            IdelModeAnimation.Play();
            ThrowAnimationDown.CustomFrameDurations = new float[] { 0.134f, 0.266f };
        }
    }

    public class ElevatorTextureMap
    {
        public TextureObject Rope;
        public TextureObject ElevatorTop;
        public TextureObject ElevatorBottom;

        public void Load(TextureRenderer renderer)
        {
            Rope = new TextureObject(Raylib.LoadTexture("./sprites/Rope.png"), renderer);
            ElevatorTop = new TextureObject(Raylib.LoadTexture("./sprites/ElevatorTop.png"), renderer);
            ElevatorBottom = new TextureObject(Raylib.LoadTexture("./sprites/ElevatorBottom.png"), renderer);
        }
    }

    public class ConveyorTextureMap
    {
        public TextureObject ConveyerSideTile;
        public AnimationPlayer ConvayerAnimation;

        public void Load(TextureRenderer renderer)
        {
            ConvayerAnimation = new AnimationPlayer([
                new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_0_deg.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_45_deg.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_90_deg.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_135_deg.png"), renderer),
                new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_0_deg.png"), renderer)
            ], 0.5f, true);

            ConveyerSideTile = new TextureObject(Raylib.LoadTexture("./sprites/ConveyerSide_Tile.png"), renderer);
            ConvayerAnimation.Play();
        }
    }

    public class LoadingStatus
    {
        public float Percentage { get; set; }
        public string Title { get; set; }
        public bool IsFinished { get; set; }
    }

    public class TextureMap
    {
        public TextureObject[] platform = new TextureObject[2];
        public TextureObject[] stairs = new TextureObject[2];
        public AnimationPlayer oilbarrel;
        public TextureObject MainMenuBackground;
        public TextureObject[] MainMenuButtons = new TextureObject[3];
        public TextureObject Barel;
        public TextureObject EndSprite;

        public PlayerTextureMap PlayerTextures;
        public GolumnTextureMap GolumnTextures;
        public SantaClausTextureMap SantaClaus;
        public ElevatorTextureMap ElevatorTextures;
        public ConveyorTextureMap ConveyorTextures;

        public TextureObject GameOverMenuBackground;
        public TextureObject[] GameOverMenuButtons = new TextureObject[2];
        public TextureRenderer renderer = new TextureRenderer();
        public Font GameFont;
        public TextureObject[] SlideItem = new TextureObject[2];
        public TextureObject[] Jumpers = new TextureObject[2];

        private int _loadingStep = 0;
        private const int TotalSteps = 6;

        public TextureMap()
        {
            PlayerTextures = new PlayerTextureMap();
            GolumnTextures = new GolumnTextureMap();
            SantaClaus = new SantaClausTextureMap();
            ElevatorTextures = new ElevatorTextureMap();
            ConveyorTextures = new ConveyorTextureMap();
        }

        public LoadingStatus UpdateLoad()
        {
            LoadingStatus status = new LoadingStatus();
            status.IsFinished = false;

            switch (_loadingStep)
            {
                case 0: // Level Data
                    status.Title = "Loading Level Assets...";
                    platform[0] = new TextureObject(Raylib.LoadTexture("./sprites/Platform.png"), renderer);
                    platform[1] = new TextureObject(Raylib.LoadTexture("./sprites/Platform2.png"), renderer);
                    stairs[0] = new TextureObject(Raylib.LoadTexture("./sprites/Stairs.png"), renderer);
                    stairs[1] = new TextureObject(Raylib.LoadTexture("./sprites/Stairs2.png"), renderer);
                    ElevatorTextures.Load(renderer);
                    ConveyorTextures.Load(renderer);
                    break;

                case 1: // Player
                    status.Title = "Loading Player...";
                    PlayerTextures.Load(renderer);
                    break;

                case 2: // Enemy
                    status.Title = "Loading Enemies...";
                    GolumnTextures.Load(renderer);
                    oilbarrel = new AnimationPlayer([
                        new TextureObject(Raylib.LoadTexture("./sprites/Burning barel 1.png"), renderer), 
                        new TextureObject(Raylib.LoadTexture("./sprites/Burning barel 2.png"), renderer)
                    ], 0.333f, true);
                    Barel = new TextureObject(Raylib.LoadTexture("./sprites/Gift.png"), renderer);
                    Jumpers[0] = new TextureObject(Raylib.LoadTexture("./sprites/Jumper1.png"), renderer);
                    Jumpers[1] = new TextureObject(Raylib.LoadTexture("./sprites/Jumper2.png"), renderer);
                    break;

                case 3: // Menu Data
                    status.Title = "Loading Menus...";
                    MainMenuBackground = new TextureObject(Raylib.LoadTexture("./sprites/MainMenu Screen.png"), renderer);
                    MainMenuButtons[0] = new TextureObject(Raylib.LoadTexture("./sprites/CONTINUE BTN.png"), renderer);
                    MainMenuButtons[1] = new TextureObject(Raylib.LoadTexture("./sprites/NEW GAME BTN.png"), renderer);
                    MainMenuButtons[2] = new TextureObject(Raylib.LoadTexture("./sprites/EXIT BTN.png"), renderer);
                    GameOverMenuBackground = new TextureObject(Raylib.LoadTexture("./sprites/game over screen.png"), renderer);
                    GameOverMenuButtons[0] = new TextureObject(Raylib.LoadTexture("./sprites/tray again button.png"), renderer);
                    GameOverMenuButtons[1] = new TextureObject(Raylib.LoadTexture("./sprites/EXIT TO TITLE.png"), renderer);
                    EndSprite = new TextureObject(Raylib.LoadTexture("./sprites/YouWon.png"), renderer);
                    break;

                case 4: // Fonts
                    status.Title = "Loading Fonts...";
                    GameFont = Raylib.LoadFontEx("./fonts/comic.ttf", 35, null, 0);
                    break;

                case 5: // Misc
                    status.Title = "Finalizing...";
                    SlideItem[0] = new TextureObject(Raylib.LoadTexture("./sprites/FiledPlate.png"), renderer);
                    SlideItem[1] = new TextureObject(Raylib.LoadTexture("./sprites/CupCake.png"), renderer);
                    SantaClaus.Load(renderer);
                    break;

                default:
                    status.Title = "Done";
                    status.IsFinished = true;
                    status.Percentage = 1.0f;
                    return status;
            }

            _loadingStep++;
            status.Percentage = (float)_loadingStep / TotalSteps;

            if (_loadingStep >= TotalSteps)
            {
                status.IsFinished = true;
            }
            else
            {
                status.IsFinished = false;
            }

            return status;
        }
    }
}