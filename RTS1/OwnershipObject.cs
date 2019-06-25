using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1
{
    public class OwnershipObject
    {
        public readonly GameObject Object;
        public readonly String Owner = "";
        public OwnershipObject(GameObject obj, String owner)
        {
            Owner = owner;
            Object = obj;
        }
    }
}
