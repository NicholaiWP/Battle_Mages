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

        public IntroductionScene()
        {
            GameObject dialougeObj = new GameObject(Vector2.Zero);
            dialougeObj.AddComponent(new DialougeBox(new[] { "In the state of Irizal, the freaks, otherwise known as mages, are seen as outcasts.",
                "Placed in the colloseum and Forced to battle magical creatures they seek to gain magical power, and the favor of the masses, who see it solely as entertainment.",
                "You find yourself preparing in the barracks, before a match is about to begin." }, () => { GameWorld.ChangeScene(new LobbyScene()); }));
            AddObject(dialougeObj);

            background = GameWorld.Load<Texture2D>("Textures/Backgrounds/Intro");
        }

        public override void Update()
        {
            scroll = Math.Min(scroll + GameWorld.DeltaTime * 16, background.Height - GameWorld.GameHeight);

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(background, position: GameWorld.Camera.Position - new Vector2(GameWorld.GameWidth / 2, GameWorld.GameHeight / 2) - new Vector2(0, scroll));

            base.Draw(drawer);
        }
    }
}