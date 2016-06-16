using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BattleMages
{
   public class BossDialogueScene: Scene
    {
        public BossDialogueScene()
        {
            GameObject dialougeObj = new GameObject(Vector2.Zero);
            dialougeObj.AddComponent(new DialougeBox(new[]
            {
                    "Rawr, I'm a Boss!",
                }, null));
        }
    }
}
