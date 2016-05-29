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
        private Vector2 backgroundPos;
        private Texture2D background;

        public ChallengeScene()
        {
            var content = GameWorld.Instance.Content;

            var normal = content.Load<Texture2D>("Images/ResDown");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - normal.Width / 2, GameWorld.Camera.Position.Y - 50),
                normal,
                normal,
                () =>
                {
                    GameWorld.ChangeScene(new HallwayScene("Normal"));
                    GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                }
                ));

            var hard = content.Load<Texture2D>("Images/ResUp");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - hard.Width / 2, GameWorld.Camera.Position.Y),
                hard,
                hard,
                 () =>
                 {
                     GameWorld.ChangeScene(new HallwayScene("Hard"));
                     GameWorld.SoundManager.PlaySound("openHallwayDoor1");
                 }
                 ));
        }
    }
}