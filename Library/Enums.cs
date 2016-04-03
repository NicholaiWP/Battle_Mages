using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public enum MovingDirection
    {
        UpLeft, UpRight, Up, Left, Right, DownLeft, DownRight, Down, Idle
    }

    public enum FacingDirection
    {
        Left, Right, Back, Front
    }

    public enum GameState
    {
        MainMenu, InGame, Settings, Shop, quit
    }

    public class Enums
    {
    }
}