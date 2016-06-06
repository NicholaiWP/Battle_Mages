using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    public class IntroductionScene : Scene
    {
        public IntroductionScene()
        {
            var content = GameWorld.Instance.Content;

            var continueButton = content.Load<Texture2D>("Textures/UI/Menu/Continue");
            var continueButton2 = content.Load<Texture2D>("Textures/UI/Menu/Continue_Hover");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - continueButton.Width / 2, GameWorld.Camera.Position.Y + continueButton.Height * -2.5f),
                continueButton,
                continueButton2,
                () =>
                {
                    GameWorld.ChangeScene(new LobbyScene());
                    GameObject exclamation = new GameObject(new Vector2(-91, -65));
                    exclamation.AddComponent(new Animator());
                    exclamation.AddComponent(new NPC("Textures/Misc/ExclamationMark", new Vector2(6, 10), 1, 1));
                    GameWorld.Scene.AddObject(exclamation);
                },
                null,
                false
                ));

            GameObject dialougeObj = new GameObject(Vector2.Zero);
            dialougeObj.AddComponent(new DialougeBox(new[] { "In the state of Irizal, the freaks, otherwise known as mages" +
                ", are seen as outcasts. Placed in the colloseum and Forced to battle magical creatures they seek to gain magical power," +
                " and the favor of the masses, who see it solely as entertainment," +
                " You find yourself preparing in the barracks, before a match is about to begin." }, null));
            AddObject(dialougeObj);
        }
    }
}