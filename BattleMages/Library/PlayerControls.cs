using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public enum PlayerBind
    {
        Up,
        Down,
        Left,
        Right,
        Spell1,
        Spell2,
        Spell3,
        Spell4,
    }

    public class PlayerControls
    {
        private List<Keys> usableKeys = new List<Keys>();
        private Dictionary<PlayerBind, Keys> playerKeys = new Dictionary<PlayerBind, Keys>();

        public PlayerControls()
        {
            playerKeys.Add(PlayerBind.Up, Keys.W);
            playerKeys.Add(PlayerBind.Down, Keys.S);
            playerKeys.Add(PlayerBind.Left, Keys.A);
            playerKeys.Add(PlayerBind.Right, Keys.D);
            playerKeys.Add(PlayerBind.Spell1, Keys.D1);
            playerKeys.Add(PlayerBind.Spell2, Keys.D2);
            playerKeys.Add(PlayerBind.Spell3, Keys.D3);
            playerKeys.Add(PlayerBind.Spell4, Keys.D4);

            //Itterating through all the values of the Enum called Keys
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                //Checking if the key is enter, space or tab
                //Also checking for the hashcode of the value, if the hashcode is between 37 and 40 it is the arrow keys
                //if it is between 48 and 57 it is the number 0 to 9, not numpad, and 65 to 90 is A to Z.
                //These keys are the keys we want the user to be able to bind.
                if (key == Keys.Enter || key == Keys.Space || key == Keys.Tab ||
                    key.GetHashCode() >= 37 && key.GetHashCode() <= 40 ||
                    key.GetHashCode() >= 48 && key.GetHashCode() <= 57 ||
                    key.GetHashCode() >= 65 && key.GetHashCode() <= 90)
                {
                    usableKeys.Add(key);
                }
            }
        }

        public Keys GetBinding(PlayerBind targetBind)
        {
            return playerKeys[targetBind];
        }

        public void ChangeBinding(PlayerBind targetBind, Keys newKey)
        {
            if (usableKeys.Contains(newKey))
            {
                foreach (PlayerBind keyBind in playerKeys.Keys)
                {
                    if (playerKeys[keyBind] == newKey)
                    {
                        playerKeys[keyBind] = playerKeys[targetBind];
                        break;
                    }
                }
                playerKeys[targetBind] = newKey;
            }
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