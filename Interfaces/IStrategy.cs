﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public interface IStrategy
    {
        void Execute(MovingDirection mDirection);

        void Execute(FacingDirection fDirection);
    }
}