using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Battle_Mages
{
    public interface ICanBuild
    {
        GameObject GetResult();
        void Buildgameobject(Vector2 position);
    }
}
