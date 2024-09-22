using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace Costos__Estructura_de_datos
{
    internal class Node
    {
        public MovimientoInventario MovInventory {  get; set; }
        public Node Next_Pointer { get; set; }

        public Node()
        {
            MovInventory = null;
            Next_Pointer = null;
        }
        public Node(MovimientoInventario motion)
        {
            MovInventory = motion;
            Next_Pointer = null;
        }
    }
}
