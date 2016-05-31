﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace BattleMages
{
    public class PlayingSound
    {
        public string Name { get; }
        public SoundEffectInstance Instance { get; }

        public PlayingSound(string name, SoundEffectInstance instance)
        {
            Name = name;
            Instance = instance;
        }
    }

    public class SoundManager
    {
        //Fields
        private Dictionary<string, SoundEffect> sounds = new Dictionary<string, SoundEffect>();

        private List<PlayingSound> playingSounds = new List<PlayingSound>();

        private Dictionary<string, Song> music = new Dictionary<string, Song>();
        private string musicCurrentlyPlaying;

        public float AmbienceVolume { get; set; }
        public float SoundVolume { get; set; }
        public float MusicVolume { get; set; }

        /// <summary>
        /// Constructor for the SoundManager
        /// </summary>
        public SoundManager()
        {
            SoundVolume = 0.20f;
            MusicVolume = 1f;
        }

        /// <summary>
        /// Loading the content for the SoundManager, here sounds are added with a name to the dictionary
        /// </summary>
        /// <param name="content"></param>
        public void LoadContent(ContentManager content)
        {
            sounds.Add("fireball", content.Load<SoundEffect>("sounds/fireball"));
            //sounds.Add("FrostShield", content.Load<SoundEffect>("sounds/FrostShield").CreateInstance());
            //sounds.Add("Earthspikes", content.Load<SoundEffect>("sounds/Earthspikes").CreateInstance());
            sounds.Add("iceshardsbreaking", content.Load<SoundEffect>("sounds/iceshardsbreaking"));
            sounds.Add("lightningStrike", content.Load<SoundEffect>("sounds/lightningStrike"));
            sounds.Add("openHallwayDoor1", content.Load<SoundEffect>("sounds/openHallwayDoor1"));
            sounds.Add("teleport", content.Load<SoundEffect>("sounds/teleport"));
            sounds.Add("AmbienceSound", content.Load<SoundEffect>("Sounds/AmbienceSound"));
            sounds.Add("ElectricitySound", content.Load<SoundEffect>("Sounds/ElectricitySound"));
            sounds.Add("Fireball", content.Load<SoundEffect>("Sounds/Fireball"));
            sounds.Add("WalkSound", content.Load<SoundEffect>("Sounds/WalkSound"));
            sounds.Add("BurnSound", content.Load<SoundEffect>("Sounds/BurnSound"));
            sounds.Add("DialougeSound", content.Load<SoundEffect>("Sounds/DialougeSound"));

            music.Add("HubMusic", content.Load<Song>("Music/Hub"));
            music.Add("CombatMusic", content.Load<Song>("Music/Combat"));

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = MusicVolume;
        }

        /// <summary>
        /// Method for playing music that is looped
        /// </summary>
        public void PlayMusic(string soundName)
        {
            if (MediaPlayer.Volume > 0.5f)
            {
                MediaPlayer.Volume = 0.5f;
            }

            if (musicCurrentlyPlaying != soundName)
            {
                MediaPlayer.Volume = MusicVolume;
                MediaPlayer.Play(music[soundName]);
                musicCurrentlyPlaying = soundName;
            }
        }

        /// <summary>
        /// Method for playing a sound by the soundName
        /// </summary>
        /// <param name="soundName"></param>
        public SoundEffectInstance PlaySound(string soundName, bool loop = false)
        {
            if (sounds.ContainsKey(soundName))
            {
                float volumeToUse = SoundVolume;

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
                    volumeToUse = AmbienceVolume;
                }

                SoundEffectInstance effect = sounds[soundName].CreateInstance();
                effect.Volume = volumeToUse;
                effect.IsLooped = loop;
                effect.Play();
                playingSounds.Add(new PlayingSound(soundName, effect));
                return effect;
            }
            return null;
        }

        public void StopSound(string soundName)
        {
            List<PlayingSound> matchingSounds = playingSounds.Where(a => a.Name == soundName).ToList();
            foreach (PlayingSound sound in matchingSounds)
            {
                sound.Instance.Stop();
                playingSounds.Remove(sound);
            }
        }

        public void Update()
        {
            foreach (PlayingSound sound in playingSounds.ToList())
            {
                if (sound.Instance.State == SoundState.Stopped)
                    playingSounds.Remove(sound);
            }
        }
    }
}