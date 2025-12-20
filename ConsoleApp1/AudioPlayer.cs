using Raylib_cs;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class AudioPlayer
    {
        private List<Music> tracks;
        private int current_track_index;
        private bool is_active;
        private float volume = 1.0f;

        public AudioPlayer()
        {
            tracks = new List<Music>();
            current_track_index = -1;
            is_active = false;
        }

        public AudioPlayer(string file_path) : this()
        {
            AddTrack(file_path);
            current_track_index = 0;
        }

        public void AddTrack(string file_path)
        {
            Music new_track = Raylib.LoadMusicStream(file_path);
            new_track.Looping = false;
            tracks.Add(new_track);

            if (current_track_index == -1)
                current_track_index = 0;
        }

        public void Play(int track_index, bool loop = true)
        {
            if (track_index < 0 || track_index >= tracks.Count) return;

            if (is_active && current_track_index != -1)
            {
                Raylib.StopMusicStream(tracks[current_track_index]);
            }

            current_track_index = track_index;

            Music track = tracks[current_track_index];
            track.Looping = loop;
            tracks[current_track_index] = track;

            Raylib.SetMusicVolume(track, volume);
            Raylib.PlayMusicStream(track);

            is_active = true;
        }

        public void Play(bool loop = true)
        {
            if (current_track_index != -1)
            {
                Play(current_track_index, loop);
            }
        }

        public void Stop()
        {
            if (current_track_index != -1 && is_active)
            {
                Raylib.StopMusicStream(tracks[current_track_index]);
                is_active = false;
            }
        }

        public void Update()
        {
            if (is_active && current_track_index != -1)
            {
                Music track = tracks[current_track_index];
                Raylib.UpdateMusicStream(track);

                if (!track.Looping && !Raylib.IsMusicStreamPlaying(track))
                    is_active = false;
            }
        }

        public void SetVolume(float new_volume)
        {
            this.volume = new_volume;
            if (current_track_index != -1)
            {
                Raylib.SetMusicVolume(tracks[current_track_index], new_volume);
            }
        }

        public bool IsFinished()
        {
            return !is_active;
        }

        public void Unload()
        {
            foreach (var track in tracks)
            {
                Raylib.UnloadMusicStream(track);
            }
            tracks.Clear();
        }
    }
}