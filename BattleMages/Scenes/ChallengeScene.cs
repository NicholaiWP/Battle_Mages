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

            
                var novice = content.Load<Texture2D>("Images/button_novice_brighter");
                var noviceHover = content.Load<Texture2D>("Images/novice_hover");
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

                var skilled = content.Load<Texture2D>("Images/button_skilled_brighter");
                var skilledHover = content.Load<Texture2D>("Images/skilled_hover");
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


                var intermediate = content.Load<Texture2D>("Images/button_intermediate_brighter");
                var intermediateHover = content.Load<Texture2D>("Images/button_intermediate_hover");
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