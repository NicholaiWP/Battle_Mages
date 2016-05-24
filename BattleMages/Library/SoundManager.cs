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
        private Song hubBGM;
        private Song combatBGM;
        private bool isPlaying, isPLaying1;

        public float AmbienceVolume { get; set; }
        public float SoundVolume { get; set; }
        public float MusicVolume { get; set; }

        /// <summary>
        /// Constructor for the SoundManager
        /// </summary>
        public SoundManager()
        {
            SoundVolume = 0.25f;
            MusicVolume = 0.20f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("AmbienceSound", content.Load<SoundEffect>("Sounds/AmbienceSound").CreateInstance());
            //sounds.Add("Lightning", content.Load<SoundEffect>("Sounds/ElectricitySound").CreateInstance());
            sounds.Add("WalkSound", content.Load<SoundEffect>("Sounds/WalkSound").CreateInstance());
            hubBGM = content.Load<Song>("Sounds/HubMusic");
            combatBGM = content.Load<Song>("Sounds/CombatMusic");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MusicVolume;
        }

        /// <summary>
        /// Method for playing music that is looped
        /// </summary>
        public void Music(string soundName)
        {
            if (soundName == "CombatBGM" && isPLaying1 == false)
            {
                MusicVolume = 0.20f;
                    MediaPlayer.Play(combatBGM);
                isPlaying = false;
                isPLaying1 = true;
            }
            if (soundName == "HubBGM" && isPlaying== false)
            {
                MusicVolume = 0.20f;
                    MediaPlayer.Play(hubBGM);
                   isPlaying = true;
                isPLaying1 = false;
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
                if(soundName == "AmbienceSound")
                {
                    if (AmbienceVolume > 0.12f)
                    {                       
                        AmbienceVolume = 0.12f; 
                    }
                    if (AmbienceVolume < 0.02f)
                    {
                        AmbienceVolume = 0.02f;
                    }
                    sounds[soundName].Volume = AmbienceVolume;
                    sounds[soundName].Play();
                }
                
                if (sounds[soundName].State == SoundState.Stopped)
                {
                    sounds[soundName].Volume = SoundVolume;
                    sounds[soundName].Play();
                }
            }
        }

        public void Update(string soundName)
        {
           
        }
    }
}