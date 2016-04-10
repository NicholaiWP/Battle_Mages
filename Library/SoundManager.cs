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
            //Adding sounds by name to the dictionary of sounds
            sounds.Add("FireBall", content.Load<SoundEffect>("Sounds/JumpSound"));
            //Adding soundNames to the dictionary with a float to evaluate the duration of the sound
            soundsDuration.Add("FireBall", 0);
            //Adding all soundNames from the duration dictionary so the dictionary´s values can be changed
            //when the list is iterated with a foreach loop
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
            //Checks if the dictionary called sounds contains the key soundName
            if (sounds.ContainsKey(soundName))
            {
                //Checks if the float value at the string key is less than or equal to zero in the dictionary soundsDuration
                if (soundsDuration[soundName] <= 0)
                {
                    //Making a SoundEffect called currentSound and sets it to be the SoundEffect at the string key
                    //in the dictionary called sounds
                    SoundEffect currentSound = sounds[soundName];
                    //Making an instance of the currentSound
                    SoundEffectInstance currentSoundInstance = currentSound.CreateInstance();
                    //Setting the volume
                    currentSoundInstance.Volume = volume;
                    //Changing the float value at the key string to the currentSound duration
                    soundsDuration[soundName] = (float)currentSound.Duration.TotalSeconds;
                    //Playing the sound
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
            //Iterating through the list soundsDurationKeys so we can check and change the values in the dictionary
            foreach (string nameOfSound in soundsDurationKeys)
            {
                //Checking if the float value at the string key is more or equal to zero in the dictionary soundsDuration
                if (soundsDuration[nameOfSound] >= 0)
                {
                    //Subtracting the float value at the string key by the DeltaTime in GameWorld
                    soundsDuration[nameOfSound] -= GameWorld.Instance.DeltaTime;
                }
            }
        }
    }
}