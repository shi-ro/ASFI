using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS1_Enum
{
    public enum SelectionMethod
    {
        Mean = 0,
        Median = 1
    }

    public enum DisposeStatus
    {
        Failed = -1,
        Succeded = 1,
        None = 0
    }
    public enum AnimationType
    {
        Single = 1,
        Loop = 2,
        LimitedLoop = 3,
        CutIn = 4,
        None = 0
    }
}
