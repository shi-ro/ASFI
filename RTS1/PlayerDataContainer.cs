using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class PlayerNodeDataContainer
    {
        public long LastSelectionUpdate = 0;
        public PlayerIndex Index;
        public PlayerNodeDataContainer( long lsu, PlayerIndex idx)
        {
            LastSelectionUpdate = lsu;
            Index = idx;
        }
    }
}
