using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattleMages
{
    public class IntroductionScene : Scene
    {
        public IntroductionScene()
        {
            var content = GameWorld.Instance.Content;
                
            var continueButton = content.Load<Texture2D>("Images/BMContinueButton1");
            var continueButton2 = content.Load<Texture2D>("Images/BMContinueButton_hover7");
            AddObject(ObjectBuilder.BuildButton(
                new Vector2(GameWorld.Camera.Position.X - continueButton.Width / 2, GameWorld.Camera.Position.Y + continueButton.Height * -2.5f),
                continueButton,
                continueButton2,
                () => { GameWorld.ChangeScene(new LobbyScene()); },
                null,
                false
                ));

            GameObject dialougeObj = new GameObject(Vector2.Zero);
            dialougeObj.AddComponent(new DialougeBox("In the state of Irizal, the freaks, otherwise known as mages, are out casts and put in a colosseum against the scum of society. Forced into battle they seek to preserve their own lives in the name of entertainment for the masses.Common society see it solely as entertainment, and upon entering the arena they will see the banner hanging at top of the entrance 'Battle Mages' You find yourself preparing in the prestigious-looking lobby before a match is about to begin."));
            AddObject(dialougeObj);
        }
    }
 }


