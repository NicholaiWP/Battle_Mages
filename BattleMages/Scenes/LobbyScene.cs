using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public class LobbyScene : Scene
    {
        private Texture2D lobbyTexture;
        private Texture2D lobbyTextureForeground;
        private Vector2 lobbyTexturePosition;
        public bool CanPause { get; set; } = true;

        public LobbyScene()
        {
            var content = GameWorld.Instance.Content;
            lobbyTexturePosition = new Vector2(-160, -270);
            lobbyTexture = content.Load<Texture2D>("Textures/Backgrounds/Lobby");
            lobbyTextureForeground = content.Load<Texture2D>("Textures/Backgrounds/LobbyLighting");

            //Side walls
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, 90 + 8), new Vector2(320, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(0, -90 - 8), new Vector2(320, 16)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(-160 - 8, 0), new Vector2(16, 180 + 32)));
            AddObject(ObjectBuilder.BuildInvisibleWall(new Vector2(160 + 8, 0), new Vector2(16, 180 + 32)));

            //Door guard
            GameObject doorGuardObj = new GameObject(new Vector2(-40, -90));
            doorGuardObj.AddComponent(new NPC("Textures/Npc's/ChallengeGuy-Sheet", new Vector2(32, 32), 8, 4));
            doorGuardObj.AddComponent(new Animator());
            doorGuardObj.AddComponent(new Collider(new Vector2(16, 32), true));
            doorGuardObj.AddComponent(new Interactable(() =>
            {
                GameObject dialougeObj = new GameObject(Vector2.Zero);
                dialougeObj.AddComponent(new DialougeBox(new[]
                {
                    "Hello, I'm a guard. I'll arrange a challenge for you. \nPick one and enter the arena."
                },
                 () => { GameWorld.ChangeScene(new ChallengeScene(this)); CanPause = false; }));

                AddObject(dialougeObj);
            }));
            AddObject(doorGuardObj);

            //Door
            GameObject door = new GameObject(new Vector2(0, -90 - 98 / 2));
            door.AddComponent(new NPC("Textures/Npc's/DoorSheet", new Vector2(64, 128), 1, 15, true));
            door.AddComponent(new Animator());
            AddObject(door);

            //ShopKeeper
            GameObject shopkeeperObj = new GameObject(new Vector2(138, -6));
            shopkeeperObj.AddComponent(new NPC("Textures/Npc's/shopKeeper-Sheet", new Vector2(48, 48), 12, 6));
            shopkeeperObj.AddComponent(new Animator());
            shopkeeperObj.AddComponent(new Collider(new Vector2(40, 24), true) { Offset = new Vector2(0, 12) });
            shopkeeperObj.AddComponent(new Interactable(() =>
            {
                GameWorld.ChangeScene(new ShopScene(GameWorld.Scene));
            }));
            AddObject(shopkeeperObj);
            GameWorld.SoundManager.PlayMusic("HubMusic");

            //Tutorial guy
            GameObject tutorialGuy = new GameObject(new Vector2(-90, -45));
            tutorialGuy.AddComponent(new NPC("Textures/NPC's/TutorialGuy-Sheet", new Vector2(32, 32), 7, 7));
            tutorialGuy.AddComponent(new Animator());
            tutorialGuy.AddComponent(new Collider(new Vector2(15, 20), true) { Offset = new Vector2(0, 5) });
            tutorialGuy.AddComponent(new Interactable(() =>
            {
                GameObject dialougeObj = new GameObject(Vector2.Zero);
                dialougeObj.AddComponent(new DialougeBox(new[]
                {
                    "Are you new? Yes? No? Listen up...\nTo go into battles, talk to the guard. It'll earn" +
                    " you money, to buy runes from the fat mage over there.",
                    "Runes are used to enhance your spells. Once you have some, press TAB to open up your " +
                    "Spell Book. The book has instructions inside, it's a very smart book, hoh-hoh!"
                }, null));
                AddObject(dialougeObj);
            }));
            AddObject(tutorialGuy);
            //Player
            GameObject playerGameObject = ObjectBuilder.BuildPlayer(Vector2.Zero, false);
            AddObject(playerGameObject);
            GameWorld.Camera.Target = playerGameObject.Transform;
        }

        public override void Update()
        {
            KeyboardState keyState = Keyboard.GetState();

            int dialougeCount = 0;
            foreach (var go in ActiveObjects)
            {
                if (go.GetComponent<DialougeBox>() != null)
                {
                    dialougeCount++;
                }
            }

            if (GameWorld.KeyPressed(Keys.Escape) && dialougeCount == 0 && CanPause)
            {
                GameWorld.ChangeScene(new PauseScene(this));
            }

            //Spellbook opening
            if (GameWorld.KeyPressed(Keys.Tab) && dialougeCount == 0 && CanPause)
            {
                GameWorld.ChangeScene(new SpellbookScene(GameWorld.Scene));
            }

            GameWorld.Camera.Update(GameWorld.DeltaTime);

            base.Update();
        }

        public override void Draw(Drawer drawer)
        {
            drawer[DrawLayer.Background].Draw(lobbyTexture, lobbyTexturePosition, Color.White);
            drawer[DrawLayer.Foreground].Draw(lobbyTextureForeground, lobbyTexturePosition, Color.White);

            base.Draw(drawer);
        }
    }
}