﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattleMages
{
    internal class HallwayScene : Scene
    {
        private Texture2D lobbyTexture;
        private Vector2 lobbyTexturePosition;
        private KeyboardState keyState;

        private SoundEffectInstance crowdSnd;
        private float crowdVolume;

        private SpriteFont fnt;

        public HallwayScene(string challengeName)
        {
            fnt = GameWorld.Load<SpriteFont>("FontBM");
            var content = GameWorld.Instance.Content;
            lobbyTexturePosition = new Vector2(-32, -360 / 2);
            lobbyTexture = content.Load<Texture2D>("Textures/Backgrounds/Hallway");

            //Side walls
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, 180 + 8), new Vector2(64, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, -180 + 64 - 8), new Vector2(64, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(-32 - 8, 0), new Vector2(16, 360 + 32)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(32 + 8, 0), new Vector2(16, 360 + 32)));

            //teleport trigger
            GameObject doorTriggerGameObject = new GameObject(new Vector2(0, -180 + 64 - 64 / 2));
            doorTriggerGameObject.AddComponent(new Collider(new Vector2(64, 64)));
            doorTriggerGameObject.AddComponent(new Interactable(() =>
           {
               crowdSnd.Stop();
               GameWorld.ChangeScene(new GameScene(challengeName)); GameWorld.SoundManager.PlaySound("teleport");
               GameWorld.SoundManager.SoundVolume = 0.9f;
           }));
            AddObject(doorTriggerGameObject);

            //Player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(new Vector2(0, 180 - 32), false);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;

            crowdSnd = GameWorld.SoundManager.PlaySound("AmbienceSound", true);
            crowdSnd.Volume = 0;
        }

        public override void Update()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape))
            {
                crowdSnd.Pause();
                GameWorld.ChangeScene(new PauseScene(this, () => { crowdSnd.Play(); }));
            }

            GameWorld.Camera.Update(GameWorld.DeltaTime);

            //Turns volume of crowd and music up or down depending on the position of the camera
            float targetVolume = (-GameWorld.Camera.Position.Y * 0.002f) + 0.3f;
            targetVolume = MathHelper.Clamp(targetVolume, 0, 0.5f);

            crowdVolume = MathHelper.SmoothStep(crowdVolume, targetVolume, GameWorld.DeltaTime * 10);
            crowdSnd.Volume = crowdVolume;
            MediaPlayer.Volume = 0.5f - crowdVolume;

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(lobbyTexture, lobbyTexturePosition, Color.White);

            drawer[DrawLayer.UI].DrawString(fnt, GameWorld.Camera.Position.Y + " - " + crowdVolume, GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2), Color.White);

            base.Draw(drawer);
        }
    }
}