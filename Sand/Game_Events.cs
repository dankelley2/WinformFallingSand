using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

/* BIT MAP
 * 
 * 128 - "Just Moved" indicator
 * 64  - 
 * 32  -
 * 16  -
 *     -
 * 8   - Material
 * 4   - Material
 * 2   - Material
 * 1   - Material
 */

namespace Sand
{
    public partial class Game
    {
        public int penIndex = 0;
        public byte[] penTypes = { (int)TYPES.VOID  ,
                                   (int)TYPES.FLAME ,
                                   (int)TYPES.EMBER ,
                                   (int)TYPES.SAND  ,
                                   (int)TYPES.WATER ,
                                   (int)TYPES.BOUNDS};


        public void ImgSandbox_LMB(object sender, EventArgs e)
        {
            var p = ((MouseEventArgs)e).Location;
            if (!canvas.Bounds.Contains(p))
            {
                return;
            }

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    Point scaledPoint = new Point((p.X + i) / scale, (p.Y + j) / scale);
                    AddPixelsOfType(scaledPoint, penTypes[penIndex]);
                }
            }

        }

        public void ImgSandbox_RMB(object sender, EventArgs e)
        {
            var p = ((MouseEventArgs)e).Location;
            if (!canvas.Bounds.Contains(p))
            {
                return;
            }
            Point scaledPoint = new Point(p.X / scale, p.Y / scale);
            AddPixelsOfType(scaledPoint, 2);
        }

        public void cycleMaterials(object sender, EventArgs e)
        {


            if (penIndex + 1 < penTypes.Length) { penIndex += 1; } else { penIndex = 0; }
        }


    }
}
