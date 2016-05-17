﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleMages
{
    public interface IStrategy
    {
        void Execute(bool movingLeft, bool movingRight, bool movingUp, bool movingDown, FacingDirection fDirection);
    }
}