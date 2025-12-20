using Raylib_cs;
using System;

namespace ConsoleApp1
{
    public class AnimationPlayer
    {
        private TextureObject[] frames;
        private float frameDuration;
        private float timer;
        private int currentFrame;
        private bool loop;
        private bool isPlaying;

        public float[] CustomFrameDurations { get; set; }

        public float FrameDuration => frameDuration;
        public int FrameCount => frames != null ? frames.Length : 0;

        public AnimationPlayer(TextureObject[] animationFrames, float speedSeconds, bool shouldLoop = true)
        {
            frames = animationFrames;
            frameDuration = speedSeconds;
            loop = shouldLoop;
            currentFrame = 0;
            timer = 0;
            isPlaying = false;
        }

        public void Play(bool reset = false)
        {
            if (reset)
            {
                currentFrame = 0;
                timer = 0;
            }

            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
            currentFrame = 0;
            timer = 0;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public bool Update()
        {
            if (!isPlaying || frames == null || frames.Length == 0) return false;

            timer += Raylib.GetFrameTime();

            float currentDuration = frameDuration;
            if (CustomFrameDurations != null && currentFrame < CustomFrameDurations.Length)
            {
                currentDuration = CustomFrameDurations[currentFrame];
            }

            if (timer >= currentDuration)
            {
                timer -= currentDuration;
                currentFrame++;

                if (currentFrame >= frames.Length)
                {
                    if (loop)
                    {
                        currentFrame = 0;
                    }
                    else
                    {
                        currentFrame = frames.Length - 1;
                        isPlaying = false;
                        return true;
                    }
                }
            }
            return false;
        }

        public TextureObject GetCurrentTexture()
        {
            if (frames != null && frames.Length > 0)
            {
                return frames[currentFrame];
            }
            return default;
        }

        public TextureObject GetFrame(int index)
        {
            if (frames != null && index >= 0 && index < frames.Length)
            {
                return frames[index];
            }
            return default;
        }

        public int GetFrameIndex()
        {
            return currentFrame;
        }

        public void SetFrames(TextureObject[] newFrames)
        {
            frames = newFrames;
            currentFrame = 0;
        }
    }
}