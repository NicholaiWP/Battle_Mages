using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public class PlayerControls
    {
        private List<string> currentBindings;
        public Dictionary<string, Keys> PlayerKeys = new Dictionary<string, Keys>();

        public PlayerControls()
        {
            PlayerKeys.Add("UpKey", Keys.W);
            PlayerKeys.Add("DownKey", Keys.S);
            PlayerKeys.Add("LeftKey", Keys.A);
            PlayerKeys.Add("RightKey", Keys.D);
            PlayerKeys.Add("Spell1", Keys.D1);
            PlayerKeys.Add("Spell2", Keys.D2);
            PlayerKeys.Add("Spell3", Keys.D3);
            PlayerKeys.Add("Spell4", Keys.D4);
            currentBindings = new List<string>(PlayerKeys.Keys);
        }

        public Keys ChangeBinding(Keys currentBind, Keys newBind)
        {
            foreach (string keyBind in currentBindings)
            {
                if (PlayerKeys[keyBind] == newBind)
                {
                    PlayerKeys[keyBind] = currentBind;
                    break;
                }
            }
            return newBind;
        }
    }
}