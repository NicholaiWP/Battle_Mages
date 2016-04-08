using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class SoundManager
    {
        //Fields
        private SoundEffectInstance soundEngine;

        private SoundEffect loopedsound;
        private SoundEffectInstance currentSoundInstance;
        public float volume;

        //Lists
        private List<string> soundsDurationKeys;

        //Dictionaries
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private Dictionary<string, float> soundsDuration = new Dictionary<string, float>();

        //For the singleton
        private static SoundManager instance;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// Constructor for the SoundManager
        /// </summary>
        private SoundManager()
        {
            volume = 0.25f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("FireBall", content.Load<SoundEffect>("Sounds/JumpSound"));
            soundsDuration.Add("FireBall", 0);
            soundsDurationKeys = new List<string>(soundsDuration.Keys);
        }

        /// <summary>
        /// Method for playing music that have to be looped
        /// </summary>
        public void LoopedSounds()
        {
        }

        /// <summary>
        /// Method for playing a sound by the soundName with a specific duration
        /// </summary>
        /// <param name="soundName"></param>
        public void PlaySound(string soundName)
        {
            if (sounds.ContainsKey(soundName))
            {
                if (soundsDuration[soundName] <= 0)
                {
                    SoundEffect currentSound = sounds[soundName];
                    currentSoundInstance = currentSound.CreateInstance();
                    currentSoundInstance.Volume = volume;
                    soundsDuration[soundName] = (float)currentSound.Duration.TotalSeconds;
                    currentSound.Play();
                }
            }
        }

        /// <summary>
        /// Method for updating the SoundManager, here all sound-durations are subtracted by
        /// the deltatime of the GameWorld unless the duration is less than 0
        /// </summary>
        public void Update()
        {
            foreach (string nameOfSound in soundsDurationKeys)
            {
                if (soundsDuration[nameOfSound] >= 0)
                {
                    soundsDuration[nameOfSound] -= GameWorld.Instance.DeltaTime;
                }
            }
        }
    }
}