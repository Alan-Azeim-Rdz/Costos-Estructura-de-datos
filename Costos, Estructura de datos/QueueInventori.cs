using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costos__Estructura_de_datos
{
    internal class QueueInventori
    {
        private Node Head_tail;

        private Node Route;

        public QueueInventori()
        {
            Head_tail = new Node();
            Head_tail.MovInventory = null;
        }

        public void Transversa()
        {
            Route = Head_tail;

            while (Route.MovInventory != null)
            {
                Route =Route.Next_Pointer;
            
            }
        
        }





    }
}
