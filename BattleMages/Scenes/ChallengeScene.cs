using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class ChallengeScene : Scene
    {
        private Texture2D backGround;
        private SpriteFont font;
        private Scene oldScene;

        public ChallengeScene(Scene oldScene)
        {
            this.oldScene = oldScene;

            var content = GameWorld.Instance.Content;
            backGround = content.Load<Texture2D>("Textures/Backgrounds/ChallengeGuybg");
            font = content.Load<SpriteFont>("FontBM");
            var earthBattle = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButton");
            var earthBattleHL = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButtonHL");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - 133, GameWorld.Camera.Position.Y - 73),
                earthBattle,
                earthBattleHL,
                () =>
                {
                    if (oldScene is LobbyScene)
                    {
                        foreach (GameObject go in oldScene.ActiveObjects)
                        {
                            if (go.GetComponent<Player>() != null)
                            {
                                GameWorld.Camera.Position = new Vector2(0, -90.83358f);
                                GameWorld.Camera.AllowMovement = false;
                                oldScene.RemoveObject(go);
                                GameObject nrPlayer = new GameObject(new Vector2(0, -90.83358f));
                                nrPlayer.AddComponent(new NPC("Textures/Player/PlayerSheet", new Vector2(32, 32), 1, 1, false, 96));
                                nrPlayer.AddComponent(new Animator());
                                oldScene.AddObject(nrPlayer);
                                GameWorld.ChangeScene(oldScene);
                            }

                            if (go.GetComponent<NPC>() != null)
                            {
                                go.GetComponent<NPC>().ChangeAnimation("EarthBattle");
                                GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                            }
                        }
                    }
                }
                ));

            var frostBattle = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButton");
            var frostBattleHL = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButtonHL");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - 133, GameWorld.Camera.Position.Y - 25),
                frostBattle,
                frostBattleHL,
                 () =>
                 {
                     if (oldScene is LobbyScene)
                     {
                         foreach (GameObject go in oldScene.ActiveObjects)
                         {
                             if (go.GetComponent<Player>() != null)
                             {
                                 GameWorld.Camera.Position = new Vector2(0, -90.83358f);
                                 GameWorld.Camera.AllowMovement = false;
                                 oldScene.RemoveObject(go);
                                 GameObject nrPlayer = new GameObject(new Vector2(0, -90.83358f));
                                 nrPlayer.AddComponent(new NPC("Textures/Player/PlayerSheet", new Vector2(32, 32), 1, 1, false, 96));
                                 nrPlayer.AddComponent(new Animator());
                                 oldScene.AddObject(nrPlayer);
                                 GameWorld.ChangeScene(oldScene);
                             }

                             if (go.GetComponent<NPC>() != null)
                             {
                                 go.GetComponent<NPC>().ChangeAnimation("FrostBattle");
                                 GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                             }
                         }
                     }
                 }
                 ));

            var arcaneBattle = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButton");
            var arcaneBattleHL = content.Load<Texture2D>("Textures/UI/SpellBook/challengeButtonHL");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - 133, GameWorld.Camera.Position.Y + 23),
                arcaneBattle,
                arcaneBattleHL,
                 () =>
                 {
                     if (oldScene is LobbyScene)
                     {
                         foreach (GameObject go in oldScene.ActiveObjects)
                         {
                             if (go.GetComponent<Player>() != null)
                             {
                                 GameWorld.Camera.Position = new Vector2(0, -90.83358f);
                                 GameWorld.Camera.AllowMovement = false;
                                 oldScene.RemoveObject(go);
                                 GameObject nrPlayer = new GameObject(new Vector2(0, -90.83358f));
                                 nrPlayer.AddComponent(new NPC("Textures/Player/PlayerSheet", new Vector2(32, 32), 1, 1, false, 96));
                                 nrPlayer.AddComponent(new Animator());
                                 oldScene.AddObject(nrPlayer);
                                 GameWorld.ChangeScene(oldScene);
                             }

                             if (go.GetComponent<NPC>() != null)
                             {
                                 go.GetComponent<NPC>().ChangeAnimation("ArcaneBattle");
                                 GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                             }
                         }
                     }
                 }
                 ));
        }

        public override void Update()
        {
            if (GameWorld.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                GameWorld.ChangeScene(oldScene);
                if (oldScene is LobbyScene)
                    (oldScene as LobbyScene).CanPause = true;
            }
            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            Color textColor = new Color(120, 100, 80);
            drawer[DrawLayer.Background].Draw(backGround, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), Color.White);
            base.Draw(drawer);
            drawer[DrawLayer.Background].DrawString(font, "Earth Battle", new Vector2(GameWorld.Camera.Position.X - 83, GameWorld.Camera.Position.Y - 60),
                textColor);
            drawer[DrawLayer.Background].DrawString(font, "Frost Battle", new Vector2(GameWorld.Camera.Position.X - 83, GameWorld.Camera.Position.Y - 13),
                textColor);
            drawer[DrawLayer.Background].DrawString(font, "Arcane Battle", new Vector2(GameWorld.Camera.Position.X - 83, GameWorld.Camera.Position.Y + 35),
                textColor);
        }
    }
}