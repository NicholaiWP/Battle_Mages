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
            SoundVolume = 0.20f;
            MediaPlayer.Volume = 0.5f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("fireball", content.Load<SoundEffect>("sounds/fireball").CreateInstance());
            sounds.Add("FrostShield", content.Load<SoundEffect>("sounds/FrostShield").CreateInstance());
            sounds.Add("Earthspikes", content.Load<SoundEffect>("sounds/Earthspikes").CreateInstance());
            sounds.Add("iceshardsbreaking", content.Load<SoundEffect>("sounds/iceshardsbreaking").CreateInstance());
            sounds.Add("lightningStrike", content.Load<SoundEffect>("sounds/lightningStrike").CreateInstance());
            sounds.Add("openHallwayDoor1", content.Load<SoundEffect>("sounds/openHallwayDoor1").CreateInstance());
            sounds.Add("teleport", content.Load<SoundEffect>("sounds/teleport").CreateInstance());
            sounds.Add("AmbienceSound", content.Load<SoundEffect>("Sounds/AmbienceSound").CreateInstance());
            sounds.Add("ElectricitySound", content.Load<SoundEffect>("Sounds/ElectricitySound").CreateInstance());
            sounds.Add("Fireball", content.Load<SoundEffect>("Sounds/Fireball").CreateInstance());
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
            if (MediaPlayer.Volume > 0.8f)
            {
                MediaPlayer.Volume = 0.8f;
            }
            if (soundName == "CombatBGM" && isPLaying1 == false)
            {
                MediaPlayer.Play(combatBGM);
                isPlaying = false;
                isPLaying1 = true;
            }
            if (soundName == "HubBGM" && isPlaying == false)
            {
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
                if (soundName == "AmbienceSound")
                {
                    if (AmbienceVolume > 0.26f)
                    {
                        AmbienceVolume = 0.26f;
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

        public void StopSound(string soundName)
        {
        }

        public void Update(string soundName)
        {
        }
    }
}