using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    public class AudioMap
    {
        public AudioPlayer Music;
        public AudioPlayer Pickup;
        public AudioPlayer EnemyHit;
        public AudioPlayer Walk;
        public AudioPlayer PlayerHit;
        public AudioPlayer Jump;
        public AudioPlayer Death;
        public AudioPlayer MenuSelect;
        public AudioPlayer LevelCompleted;
        public AudioMap()
        {
            Music = new AudioPlayer("./audio/Music.wav");
            Pickup = new AudioPlayer("./audio/Pickup.wav");
            EnemyHit = new AudioPlayer("./audio/EnemyHit.wav");
            Walk = new AudioPlayer("./audio/Walk.wav");
            PlayerHit = new AudioPlayer("./audio/PlayerHit.wav");
            Jump = new AudioPlayer("./audio/Jump.wav");
            Death=new AudioPlayer("./audio/Death.wav");
            MenuSelect = new AudioPlayer("./audio/MenuSelect.wav");
            LevelCompleted=new AudioPlayer("./audio/LevelCompleted.wav");
            Music.Play(true);
            Music.SetVolume(0.1f);
            Walk.SetVolume(0.1f);
        }

        public void Update()
        {
            Music.Update();
            Pickup.Update();
            EnemyHit.Update();
            Walk.Update();
            PlayerHit.Update();
            Jump.Update();
            Death.Update();
            MenuSelect.Update();
            LevelCompleted.Update();
        }
    }
}