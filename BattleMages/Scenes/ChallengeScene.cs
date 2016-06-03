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

        public ChallengeScene(Scene oldScene)
        {
            var content = GameWorld.Instance.Content;
            backGround = content.Load<Texture2D>("Textures/Backgrounds/ChallengeGuybg");

            var novice = content.Load<Texture2D>("Textures/UI/SpellBook/LongButton");
            var noviceHover = content.Load<Texture2D>("Textures/UI/SpellBook/LongButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - 123, GameWorld.Camera.Position.Y - 64),
                novice,
                noviceHover,
                () =>
                {
                    GameWorld.ChangeScene(oldScene);
                    if (oldScene is LobbyScene)
                    {
                        foreach (GameObject go in oldScene.ActiveObjects)
                        {
                            if (go.GetComponent<NPC>() != null)
                            {
                                go.GetComponent<NPC>().ChangeAnimation("EarthBattle");
                                GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                            }
                        }
                    }
                }
                ));

            var skilled = content.Load<Texture2D>("Textures/UI/SpellBook/LongButton");
            var skilledHover = content.Load<Texture2D>("Textures/UI/SpellBook/LongButton_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - 123, GameWorld.Camera.Position.Y - 17),
                skilled,
                skilledHover,
                 () =>
                 {
                     GameWorld.ChangeScene(new HallwayScene("FrostBattle"));
                     GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                 }
                 ));

            var intermediate = content.Load<Texture2D>("Textures/UI/TestButtons/Intermediate");
            var intermediateHover = content.Load<Texture2D>("Textures/UI/TestButtons/Intermediate_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - intermediate.Width / 2, GameWorld.Camera.Position.Y + 20),
                intermediate,
                intermediateHover,
                 () =>
                 {
                     GameWorld.ChangeScene(new HallwayScene("ArcaneBattle"));
                     GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                 }
                 ));
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(backGround, new Vector2(GameWorld.Camera.Position.X - GameWorld.GameWidth / 2,
                GameWorld.Camera.Position.Y - GameWorld.GameHeight / 2), Color.White);
            base.Draw(drawer);
        }
    }
}