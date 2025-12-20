using Raylib_cs;
using System.Numerics;

namespace ConsoleApp1;

public class Jumper
{
    public bool is_active = false;
    
    public Vec2D defalut_pos;
    public Vec2D pos;
    
    public Vec2D velocity;

    public static int terminal_velocity = 800; 
    public static float gravity = 4000.0f;
    public static Vec2D defalut_velocity = new Vec2D(500, terminal_velocity);
    public static int fall_dawn_speed = 500;
    public Rect2D rect = new Rect2D();
    public Line2D jump_line;

    public Jumper(bool isActive, Line2D jump_line, Vec2D pos)
    {
        this.pos = new Vec2D(pos.X, pos.Y);
        this.defalut_pos = new Vec2D(pos.X, pos.Y);
        
        this.jump_line = jump_line;
        this.is_active = isActive;
        
        this.velocity = new Vec2D(defalut_velocity.X, defalut_velocity.Y);
        
        rect = get_main_colision_rect(this.pos, velocity.Y > 0);
    }

    public static Rect2D get_main_colision_rect(Vec2D pos, bool mode)
    {
        Vec2D size = new Vec2D(30, 45);
        if (mode)
            size.Y = 60;
        return new Rect2D(new Vec2D(pos.X - size.X / 2, pos.Y - size.Y), size);
    }

    private void reset_jumper()
    {
        pos.X = defalut_pos.X;
        pos.Y = defalut_pos.Y;
        
        velocity.X = defalut_velocity.X;
        velocity.Y = defalut_velocity.Y;
        
        rect = get_main_colision_rect(this.pos, velocity.Y > 0);
    }

    public bool activate()
    {
        if (is_active) 
            return false;

        is_active = true;
        reset_jumper();
        return true;
    }

    public void ProcessDownDrop(Game game)
    {
        pos.Y += Raylib.GetFrameTime() * fall_dawn_speed;
    }

    public void processDeactivate()
    {
        if (pos.Y > 1500)
            is_active = false;
    }

    public void update(Game game)
    {
        if (!is_active)
            return;
        
        game.player.is_hit(rect, game);
        Vec2D oldPos = new Vec2D(pos.X, pos.Y);

        rect = get_main_colision_rect(this.pos, velocity.Y > 0);
        if (pos.X > game.levels[game.current_level_id].jumper_X)
        {
            ProcessDownDrop(game);
            processDeactivate();
            return;
        }
        
        float dt = Raylib.GetFrameTime();
        float offset_y = dt * velocity.Y;
        
        pos.X += dt * velocity.X;
        pos.Y += offset_y;
        
        ProcessGravity(game);
        
        rect = get_main_colision_rect(this.pos, velocity.Y > 0);
        
        bool collision = jump_line.CollideWith(this.rect);

        if (!collision && velocity.Y > 0)
        {
            float lineY = jump_line.Start.Y;
            if (oldPos.Y <= lineY && pos.Y >= lineY)
            {
                if (pos.X >= jump_line.Start.X && pos.X <= jump_line.End.X)
                {
                    collision = true;
                    pos.Y = lineY;
                }
            }
        }

        if (collision)
        {
            if (jump_line.CollideWith(this.rect))
                pos.Y -= offset_y;
                
            Jump(game);
        }
    }

    public void Jump(Game game)
    {
        velocity.Y = -1200;
    }

    public void ProcessGravity(Game game)
    {
        velocity.Y += gravity * Raylib.GetFrameTime();
        if (velocity.Y > terminal_velocity)
            velocity.Y = terminal_velocity;
    }

    public TextureObject getTexture(Game game)
    {
        if (velocity.Y > 0)
            return game.GlobalTextures.Jumpers[1];
        return game.GlobalTextures.Jumpers[0];
    }

    public void render(Game game)
    {
        if (!is_active)
            return;
        TextureObject texture = getTexture(game);
        texture.DrawBottomCenter(this.rect.Size.Y, false, this.pos);
#if DEBUG
        Raylib.DrawRectangleLines(
            (int)rect.Pos.X, 
            (int)rect.Pos.Y, 
            (int)rect.Size.X, 
            (int)rect.Size.Y, 
            Color.Red
        );

        Raylib.DrawLineV(
            new Vector2(jump_line.Start.X, jump_line.Start.Y), 
            new Vector2(jump_line.End.X, jump_line.End.Y), 
            Color.Green
        );
#endif
    }
}