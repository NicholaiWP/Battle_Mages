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
        private List<Keys> usableKeys = new List<Keys>();
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

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (key == Keys.Enter || key == Keys.Space || key == Keys.Tab ||
                    key.GetHashCode() >= 37 && key.GetHashCode() <= 40 ||
                    key.GetHashCode() >= 48 && key.GetHashCode() <= 57 ||
                    key.GetHashCode() >= 65 && key.GetHashCode() <= 90)
                {
                    usableKeys.Add(key);
                }
            }
        }

        public Keys ChangeBinding(Keys currentBind, Keys newBind)
        {
            if (usableKeys.Contains(newBind))
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
            return currentBind;
        }

        public string KeyToString(Keys key)
        {
            string keyString;
            switch (key)
            {
                case Keys.D0:
                    keyString = "0";
                    break;

                case Keys.D1:
                    keyString = "1";
                    break;

                case Keys.D2:
                    keyString = "2";
                    break;

                case Keys.D3:
                    keyString = "3";
                    break;

                case Keys.D4:
                    keyString = "4";
                    break;

                case Keys.D5:
                    keyString = "5";
                    break;

                case Keys.D6:
                    keyString = "6";
                    break;

                case Keys.D7:
                    keyString = "7";
                    break;

                case Keys.D8:
                    keyString = "8";
                    break;

                case Keys.D9:
                    keyString = "9";
                    break;

                default:
                    keyString = key.ToString();
                    break;
            }
            return keyString;
        }
    }
}