using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sand
{
    static class brushes
    {
        public static int[,] CircleBrush =
            {{0,0,0,1,0,1,0,0,0},
            {0,1,0,1,1,1,0,1,0},
            {0,0,1,0,1,0,1,0,0},
            {1,1,0,1,1,1,0,1,1},
            {0,1,1,1,0,1,1,1,0},
            {1,1,0,1,1,1,0,1,1},
            {0,0,1,0,1,0,1,0,0},
            {0,1,0,1,1,1,0,1,0},
            {0,0,0,1,0,1,0,0,0}};

        public static int[,] SolidBrush =
            {{1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1},
             {1,1,1,1,1,1,1,1,1}};
    }
}
