using Raylib_cs;
using System;

namespace ConsoleApp1
{
    public class Timer
    {
        private float lifetime;
        private float timer;
        private bool isPlaying;
        private bool loop;

        public event Action OnTimerDone;

        public float Lifetime => lifetime;
        public float CurrentTime => timer;
        public bool IsPlaying => isPlaying;
        public float Progress => Math.Clamp(timer / lifetime, 0f, 1f);

        public Timer(float durationSeconds, bool shouldLoop = false)
        {
            lifetime = durationSeconds;
            loop = shouldLoop;
            timer = 0;
            isPlaying = false;
        }

        public void Play(bool reset = false)
        {
            if (reset)
            {
                timer = 0;
            }
            isPlaying = true;
        }

        public void Stop()
        {
            isPlaying = false;
            timer = 0;
        }

        public void Pause()
        {
            isPlaying = false;
        }

        public bool Update()
        {
            if (!isPlaying) return false;

            timer += Raylib.GetFrameTime();

            if (timer >= lifetime)
            {
                OnTimerDone?.Invoke();

                if (loop)
                {
                    timer -= lifetime;
                    return true;
                }
                else
                {
                    timer = lifetime;
                    isPlaying = false;
                    return true;
                }
            }

            return false;
        }

        public void SetDuration(float seconds)
        {
            lifetime = seconds;
        }

        public void ModifyTime(float seconds)
        {
            timer += seconds;
            if (timer < 0) timer = 0;
        }
    }
}
