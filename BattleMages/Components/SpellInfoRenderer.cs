using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleMages
{
    internal class SpellInfoRenderer : Component
    {
        private SpellInfo spellInfo;
        private Texture2D background;

        public SpellInfoRenderer(SpellInfo spellInfo)
        {
            this.spellInfo = spellInfo;
            background = GameWorld.Load<Texture2D>("Textures/UI/Ingame/SpellbarSpell");

            Listen<DrawMsg>(Draw);
        }

        private void Draw(DrawMsg msg)
        {
            Vector2 pos = GameObject.Transform.Position;
            msg.Drawer[DrawLayer.UI].Draw(background, position: pos - Utils.HalfTexSize(background));

            if (spellInfo == null) return;

            BaseRune baseRune = spellInfo.GetBaseRune();
            if (baseRune != null)
            {
                Texture2D baseRuneTex = spellInfo.GetBaseRune().Texture;
                msg.Drawer[DrawLayer.UI].Draw(baseRuneTex, position: pos - Utils.HalfTexSize(baseRuneTex));
            }

            Vector2[] runePositions = new Vector2[] { new Vector2(0, -6), new Vector2(6, 0), new Vector2(0, 6), new Vector2(-6, 0) };
            for (int j = 0; j < SpellInfo.AttributeRuneSlotCount; j++)
            {
                AttributeRune attrRune = spellInfo.GetAttributeRune(j);
                if (attrRune != null)
                {
                    msg.Drawer[DrawLayer.UI].Draw(attrRune.Texture, position: pos + runePositions[j] - Utils.HalfTexSize(attrRune.Texture));
                }
            }
        }
    }
}