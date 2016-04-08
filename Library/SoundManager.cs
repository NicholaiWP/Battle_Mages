using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

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
            volume = 0.25f;
        }

        public void LoadContent(ContentManager content)
        {
            sounds.Add("FireBall", content.Load<SoundEffect>("Sounds/JumpSound"));
        }

        public void LoopedSounds()
        {
        }

        public static void PlaySound(string soundName)
        {
            if (Instance.sounds.ContainsKey(soundName))
            {
                SoundEffect currentSound = Instance.sounds[soundName];
                SoundEffectInstance currentSoundInstance = currentSound.CreateInstance();
                currentSoundInstance.Volume = Instance.volume;
                currentSound.Play();
            }
        }
    }
}
