using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    public class ChallengeScene : Scene
    {
        private Vector2 backgroundPos;
        private Texture2D background;

        public ChallengeScene()
        {
            var content = GameWorld.Instance.Content;

            var novice = content.Load<Texture2D>("Textures/UI/TestButtons/Novice");
            var noviceHover = content.Load<Texture2D>("Textures/UI/TestButtons/Novice_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - novice.Width / 2, GameWorld.Camera.Position.Y - 80),
                novice,
                noviceHover,
                () =>
                {
                    GameWorld.ChangeScene(new HallwayScene("Novice"));
                    GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                }
                ));

            var skilled = content.Load<Texture2D>("Textures/UI/TestButtons/Skilled");
            var skilledHover = content.Load<Texture2D>("Textures/UI/TestButtons/Skilled_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - skilled.Width / 2, GameWorld.Camera.Position.Y - 30),
                skilled,
                skilledHover,
                 () =>
                 {
                     GameWorld.ChangeScene(new HallwayScene("Skilled"));
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
                     GameWorld.ChangeScene(new HallwayScene("Intermediate"));
                     GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                 }
                 ));
        }
    }
}