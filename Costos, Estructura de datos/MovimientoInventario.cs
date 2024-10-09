using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Costos__Estructura_de_datos
{
    internal class MovimientoInventario
    {
        public DateTime Fecha { get; set; }
        public string TipoDeDato { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }



        public MovimientoInventario()
        {
            Fecha = DateTime.Now;
            TipoDeDato = "";
            Precio = 0;
            Cantidad = 0;
        }

        public MovimientoInventario(DateTime fecha, string tipoDeDato, decimal precio, int cantidad)
        {
            Fecha = fecha;
            TipoDeDato = tipoDeDato;
            Precio = precio;
            Cantidad = cantidad;
        }

        // Método para obtener una representación en cadena de la instancia
        public override string ToString()
        {
            return Fecha.ToShortDateString() + " " + TipoDeDato.ToString() + " " + Precio.ToString() + " " + Cantidad.ToString();
        }



    }
}
