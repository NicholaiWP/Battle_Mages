using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class SoundManager
    {
        //Fields
        private Dictionary<string, SoundEffectInstance> sounds = new Dictionary<string, SoundEffectInstance>();
        private Song backgroundMusic;

        public float SoundVolume { get; set; }
        public float MusicVolume { get; set; }

        /// <summary>
        /// Constructor for the SoundManager
        /// </summary>
        public SoundManager()
        {
            SoundVolume = 0.25f;
            MusicVolume = 0.10f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("ambience", content.Load<SoundEffect>("Sounds/ambience").CreateInstance());
            sounds.Add("FireBall", content.Load<SoundEffect>("Sounds/JumpSound").CreateInstance());
            sounds.Add("walk", content.Load<SoundEffect>("Sounds/walk").CreateInstance());
            backgroundMusic = content.Load<Song>("Sounds/backgroundMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MusicVolume;
        }

        /// <summary>
        /// Method for playing music that have to be looped
        /// </summary>
        public void Music(string soundName)
        {
            MediaPlayer.Play(backgroundMusic);
        }

        /// <summary>
        /// Method for playing a sound by the soundName
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            if (sounds.ContainsKey(soundName))
            {
                if (sounds[soundName].State == SoundState.Stopped)
                {
                    sounds[soundName].Volume = SoundVolume;
                    sounds[soundName].Play();
                }
            }
        }

        public void UpdateMusicVolume()
        {
        }
    }
}