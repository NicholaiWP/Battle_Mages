using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battle_Mages
{
    public interface ICanBuild
    {
        GameObject GetResult();

        void BuildGameObject(Vector2 position);
    }
}