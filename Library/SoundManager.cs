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
        private SoundEffectInstance soundEngine;
        private SoundEffect loopedsound;

        //String name of the effect
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        public float volume;
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

        private SoundManager()
        {
        }

        public void LoadContent(ContentManager content)
        {
            sounds.Add("FireBall", content.Load<SoundEffect>(""));
        }

        public void LoopedSounds()
        {
        }

        public void PlaySound(string soundName)
        {
            if (sounds.ContainsKey(soundName))
            {
                SoundEffect currentSound = sounds[soundName];
                SoundEffectInstance currentSoundInstance = currentSound.CreateInstance();
                currentSoundInstance.Volume = volume;
                currentSound.Play();
            }
        }
    }
}