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
        private Texture2D background;
        private float scroll;
        private float scrollSpeed;

        public IntroductionScene()
        {
            GameObject dialougeObj = new GameObject(Vector2.Zero);
            dialougeObj.AddComponent(new DialougeBox(new[] { "In the state of Irizal, the freaks, otherwise known as mages, are seen as outcasts.",
                "Placed in the colosseum and forced to battle magical creatures they seek to gain magical power, and the favor of the masses, who see it solely as entertainment.",
                "This is how it has been done for centuries, since the previous ruler was overthrown.",
                "There is no salvation, no rebellion, only survival, but with enough fame, perhaps a mage can hope his future will be brighter somehow.",
                "In the barracks, the newest mage is preparing before a match is about to begin... " }, () =>
                {
                    GameWorld.ChangeScene(new LobbyScene());

                    GameObject exclamation = new GameObject(new Vector2(-91, -65));
                    exclamation.AddComponent(new Animator());
                    exclamation.AddComponent(new NPC("Textures/Misc/ExclamationMark", new Vector2(6, 10), 1, 1));
                    GameWorld.Scene.AddObject(exclamation);
                }));
            AddObject(dialougeObj);

            background = GameWorld.Load<Texture2D>("Textures/Backgrounds/Intro");
        }

        public override void Update()
        {
            scrollSpeed = Math.Min(scrollSpeed + GameWorld.DeltaTime * 0.5f, 1);
            scroll = Math.Min(scroll + GameWorld.DeltaTime * scrollSpeed * 16, background.Height - GameWorld.GameHeight);

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, position: GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2) - new Vector2(0, scroll));

            base.Draw(drawer);
        }
    }
}