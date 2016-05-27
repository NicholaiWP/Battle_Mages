using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleMages
{
    /// <summary>
    /// Sent to components before InitializeMsg. Use for adding components or other things that need to be done before any Initialize messages are sent.<para />
    /// <c>Do not try to access other components from this message!</c> Other newly added components are not visible until InitializeMsg is sent.
    /// </summary>
    public class PreInitializeMsg : Msg
    {
    }
}
