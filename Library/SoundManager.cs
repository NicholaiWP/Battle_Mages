using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class SoundManager
    {
        //Fields
        private Dictionary<string, SoundEffectInstance> sounds = new Dictionary<string, SoundEffectInstance>();

        public float Volume { get; set; }

        /// <summary>
        /// Constructor for the SoundManager
        /// </summary>
        public SoundManager()
        {
            Volume = 1f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("Music", content.Load<SoundEffect>("Sounds/JumpSound").CreateInstance());
            sounds.Add("FireBall", content.Load<SoundEffect>("Sounds/JumpSound").CreateInstance());
        }

        /// <summary>
        /// Method for playing music that have to be looped
        /// </summary>
        public void Music(string soundName)
        {
            if (sounds.ContainsKey(soundName))
            {
                sounds[soundName].IsLooped = true;
                sounds[soundName].Volume = Volume;
                sounds[soundName].Play();
            }
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
                    sounds[soundName].Volume = Volume;
                    sounds[soundName].Play();
                }
            }
        }

        public void UpdateMusicVolume()
        {
            if (Volume < 0)
            {
                Volume = 0;
            }
            else if (Volume > 1)
            {
                Volume = 1;
            }
            sounds["Music"].Volume = Volume;
        }
    }
}