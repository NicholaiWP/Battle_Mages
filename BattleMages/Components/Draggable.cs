using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class Draggable : Component
    {
        private Vector2 startPosition;
        private Texture2D normalTex;
        private Texture2D hoverTex;
        private Action onDragStart;
        private SpriteRenderer spriteRenderer;
        private bool hovering;
        private bool dragging;
        private string tag;

        private Texture2D ActiveTex { get { return hovering ? hoverTex : normalTex; } }

        public Draggable(string tag, Texture2D normalTex, Texture2D hoverTex, Action onDragStart)
        {
            this.tag = tag;
            this.normalTex = normalTex;
            this.hoverTex = hoverTex;
            this.onDragStart = onDragStart;

            Listen<InitializeMsg>(Initialize);
            Listen<UpdateMsg>(Update);
            Listen<DrawMsg>(Draw);
        }

        private void Initialize(InitializeMsg msg)
        {
            spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
            startPosition = GameObject.Transform.Position;
        }

        private void Update(UpdateMsg msg)
        {
            Rectangle rectangle = new Rectangle((GameObject.Transform.Position - Utils.HalfTexSize(ActiveTex)).ToPoint(), ActiveTex.Bounds.Size);
            if (rectangle.Contains(GameWorld.Cursor.Position))
            {
                hovering = true;

                GameWorld.Cursor.SetCursor(CursorStyle.Interactable);

                if (GameWorld.Cursor.LeftButtonPressed)
                {
                    dragging = true;
                    onDragStart?.Invoke();

                    foreach (DragDropPoint dropPoint in GetDropPoints())
                    {
                        dropPoint.Highlighted = true;
                    }
                }
            }
            else
            {
                hovering = false;
            }

            if (dragging)
            {
                GameObject.Transform.Position = GameWorld.Cursor.Position;

                if (!GameWorld.Cursor.LeftButtonHeld)
                {
                    dragging = false;
                    GameObject.Transform.Position = startPosition;
                    foreach (DragDropPoint dropPoint in GetDropPoints())
                    {
                        dropPoint.Highlighted = false;
                        if (dropPoint.Hovering)
                            dropPoint.Activate();
                    }
                }
            }
        }

        private IEnumerable<DragDropPoint> GetDropPoints()
        {
            return GameWorld.Scene.ActiveObjects.Select(a => a.GetComponent<DragDropPoint>()).Where(a => a != null && a.Tag == tag);
        }

        private void Draw(DrawMsg msg)
        {
            SpriteBatch spriteBatch = msg.Drawer[DrawLayer.UI];
            spriteBatch.Draw(ActiveTex, position: GameObject.Transform.Position - new Vector2(ActiveTex.Width, ActiveTex.Height) / 2);
        }
    }
}