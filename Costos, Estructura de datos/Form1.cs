using System.Globalization;
using System.Text.RegularExpressions;

namespace Costos__Estructura_de_datos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string CurrentYear = Convert.ToString(DateTime.Now);
        int Index = 0;
        int Matriz = 0;
        CultureInfo latinoCulture = new CultureInfo("es-MX");
        decimal quantity = 0;
        decimal price = 0;
        private Queue<MovimientoInventario> QueueofInventori = new Queue<MovimientoInventario>();
        private Stack<MovimientoInventario> StackOfInventario = new Stack<MovimientoInventario>();

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            // Obtener el año de la fecha seleccionada por el usuario en el DateTimePicker
            string selectedYear = Convert.ToString(DataTimeDay.Value.Year);

            if (ComBoxFormula.SelectedItem is null)
            {
                MessageBox.Show("Porfavor seleciona un tipo de formula");
                return;
            }
            bool EsPrecioValido(string precio)
            {
                // Verificar que el campo no esté vacío y contenga solo números o decimales
                return !string.IsNullOrWhiteSpace(precio) && Regex.IsMatch(precio, @"^[0-9]*(?:\.[0-9]*)?$");
            }


            string Selection = ComBoxFormula.SelectedItem.ToString();

            // Verificar si el precio debe ser solicitado
            bool requierePrecio = CmBoxDataType.SelectedIndex == 0;


            switch (Selection)
            {
                case "PEPS":


                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo año");
                        return;
                    }

                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    if (!int.TryParse(TxtQuantity.Text, out int result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    if (TxtPrice.Enabled && !EsPrecioValido(TxtPrice.Text))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    // Agregar una nueva fila vacía
                    int newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato
                    int typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas específicas de la nueva fila
                    DataGridAlmacen.Rows[newRowIndex].Cells[0].Value = DataTimeDay.Value;

                    // Si es una entrada
                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        if (newRowIndex == 0)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal totalCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N2", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalCost.ToString("N2", latinoCulture);

                        }
                        else
                        {
                            decimal previousQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture);
                            decimal quentity = Convert.ToDecimal(TxtQuantity.Text, latinoCulture);
                            decimal newQuantity = previousQuantity + quentity;

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal currentCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = currentCost.ToString("N2", latinoCulture);

                            decimal previousTotal = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value, latinoCulture);
                            decimal updatedTotal = previousTotal + currentCost;  // Sumar los valores directamente como decimal

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = updatedTotal.ToString("N2", latinoCulture); // Mostrar el resultado en el formato adecuado
                        }

                        // Agregar la entrada a la cola
                        QueueofInventori.Enqueue(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToDecimal(TxtPrice.Text, latinoCulture), Convert.ToInt32(TxtQuantity.Text)));
                    }
                    // Si es una salida
                    else if (CmBoxDataType.SelectedIndex == 1)
                    {
                        if (QueueofInventori.Count == 0)
                        {
                            MessageBox.Show("No puedes sacar artículos porque el almacén está vacío");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int remainingQuantity = Convert.ToInt32(TxtQuantity.Text);
                        List<decimal> nodeCosts = new List<decimal>(); // Lista para almacenar los costos de cada nodo
                        List<decimal> cantidadesUtilizadas = new List<decimal>(); // Lista para almacenar las cantidades utilizadas de cada nodo
                        decimal nodeCount = 0; // Contador de nodos utilizados

                        // Verificación previa: Calcula si hay suficiente inventario sin modificar la cola
                        decimal cantidadDisponible = 0;
                        foreach (var item in QueueofInventori)
                        {
                            cantidadDisponible += item.Cantidad;
                        }

                        if (remainingQuantity > cantidadDisponible)
                        {
                            MessageBox.Show("No hay suficiente inventario para completar la salida");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        // Modificación real: Ahora que sabemos que hay suficiente inventario, modificamos la cola
                        while (remainingQuantity > 0 && QueueofInventori.Count > 0)
                        {
                            MovimientoInventario firstEntry = QueueofInventori.Peek(); // Obtenemos la primera entrada sin eliminarla

                            if (firstEntry.Cantidad <= remainingQuantity)
                            {
                                // Usamos toda la cantidad de la primera entrada
                                remainingQuantity -= firstEntry.Cantidad;
                                cantidadesUtilizadas.Add(firstEntry.Cantidad); // Guardamos la cantidad utilizada
                                decimal ca = firstEntry.Precio, latinoCulture;
                                nodeCosts.Add(ca); // Añadir costo a la lista
                                QueueofInventori.Dequeue(); // Eliminamos la entrada ya que se agotó
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la primera entrada
                                cantidadesUtilizadas.Add(remainingQuantity); // Guardamos la cantidad utilizada
                                nodeCosts.Add(firstEntry.Precio); // Añadir costo parcial a la lista
                                firstEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                        }

                        // Si usamos 2 o más nodos, mostramos el mensaje en la columna 3
                        if (nodeCount >= 2)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[2].Value = $"Usado de {nodeCount} nodos";
                        }

                        // Mostramos los costos de cada nodo separados por un guion en la columna 5
                        string costsDisplay = string.Join(" - ", nodeCosts.Select(c => c.ToString(latinoCulture)));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Actualizamos el DataGrid con la información de la salida
                        decimal previousQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture);
                        decimal newQuantity = previousQuantity - Convert.ToDecimal(TxtQuantity.Text, latinoCulture);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text; // Cantidad que se está sacando
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString (latinoCulture); // Cantidad actualizada después de la extracción

                        // Calculamos el total de la salida multiplicando la cantidad retirada por el precio de cada nodo
                        decimal totalSalida = 0;
                        for (int i = 0; i < nodeCosts.Count; i++)
                        {
                            totalSalida += cantidadesUtilizadas[i] * nodeCosts[i]; // Multiplicamos cada cantidad por su respectivo precio
                        }

                        DataGridAlmacen.Rows[newRowIndex].Cells[6].Value = totalSalida.ToString( latinoCulture); // Total en la columna "Debe"


                        decimal previousTotal = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value, latinoCulture);
                        decimal newTotal = previousTotal - totalSalida; // Restamos el total de salida del total previo

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = newTotal.ToString(latinoCulture); // Actualizamos el total

                    }
                    break;



                case "UEPS":


                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo año");
                        return;
                    }
                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de fórmula");
                        return;
                    }
                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }
                    if (!int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacío");
                        return;
                    }
                    if (TxtPrice.Enabled && !EsPrecioValido(TxtPrice.Text))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    // Agregar una nueva fila vacía y obtener el índice de la nueva fila
                    newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato
                    typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas específicas de la nueva fila
                    DataGridAlmacen.Rows[newRowIndex].Cells[0].Value = DataTimeDay.Value;

                    // Si es una entrada
                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        if (newRowIndex == 0)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal totalCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N2", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalCost.ToString("N2", latinoCulture);

                        }
                        else
                        {
                            decimal previousQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture);
                            decimal quentity = Convert.ToDecimal(TxtQuantity.Text, latinoCulture);
                            decimal newQuantity = previousQuantity + quentity;

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal currentCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = currentCost.ToString("N2", latinoCulture);

                            decimal previousTotal = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value, latinoCulture);
                            decimal updatedTotal = previousTotal + currentCost;  // Sumar los valores directamente como decimal

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = updatedTotal.ToString("N2", latinoCulture); // Mostrar el resultado en el formato adecuado
                        }

                        // Agregar la entrada a la cola
                        StackOfInventario.Push(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToDecimal(TxtPrice.Text, latinoCulture), Convert.ToInt32(TxtQuantity.Text)));
                    }
                    // Si es una salida
                    else if (CmBoxDataType.SelectedIndex == 1)
                    {
                        if (StackOfInventario.Count == 0)
                        {
                            MessageBox.Show("No puedes sacar artículos porque el almacén está vacío");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int remainingQuantity = Convert.ToInt32(TxtQuantity.Text);
                        List<decimal> nodeCosts = new List<decimal>(); // Lista para almacenar los costos de cada nodo
                        List<decimal> cantidadesUtilizadas = new List<decimal>(); // Lista para almacenar las cantidades utilizadas de cada nodo
                        decimal nodeCount = 0; // Contador de nodos utilizados

                        // Verificación previa: Calcula si hay suficiente inventario sin modificar la pila
                        decimal cantidadDisponible = 0;
                        foreach (var item in StackOfInventario)
                        {
                            cantidadDisponible += item.Cantidad;
                        }

                        if (remainingQuantity > cantidadDisponible)
                        {
                            MessageBox.Show("No hay suficiente inventario para completar la salida");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        // Modificación real: Ahora que sabemos que hay suficiente inventario, modificamos la pila
                        while (remainingQuantity > 0 && StackOfInventario.Count > 0)
                        {
                            MovimientoInventario lastEntry = StackOfInventario.Peek(); // Obtenemos la última entrada sin eliminarla (UEPS)

                            if (lastEntry.Cantidad <= remainingQuantity)
                            {
                                // Usamos toda la cantidad de la última entrada
                                remainingQuantity -= lastEntry.Cantidad;
                                cantidadesUtilizadas.Add(lastEntry.Cantidad); // Guardamos la cantidad utilizada
                                decimal ca = lastEntry.Precio;
                                nodeCosts.Add(ca); // Añadir costo a la lista
                                StackOfInventario.Pop(); // Eliminamos la entrada ya que se agotó
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la última entrada
                                cantidadesUtilizadas.Add(remainingQuantity); // Guardamos la cantidad utilizada
                                nodeCosts.Add(lastEntry.Precio); // Añadir costo parcial a la lista
                                lastEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                        }

                        // Si usamos 2 o más nodos, mostramos el mensaje en la columna 3
                        if (nodeCount >= 2)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[2].Value = $"Usado de {nodeCount} nodos";
                        }

                        // Mostramos los costos de cada nodo separados por un guion en la columna 5
                        string costsDisplay = string.Join(" - ", nodeCosts.Select(c => c.ToString(latinoCulture)));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Actualizamos el DataGrid con la información de la salida
                        decimal previousQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture);
                        decimal newQuantity = previousQuantity - Convert.ToDecimal(TxtQuantity.Text, latinoCulture);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text; // Cantidad que se está sacando
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString(latinoCulture); // Cantidad actualizada después de la extracción

                        // Calculamos el total de la salida multiplicando la cantidad retirada por el precio de cada nodo
                        decimal totalSalida = 0;
                        for (int i = 0; i < nodeCosts.Count; i++)
                        {
                            totalSalida += cantidadesUtilizadas[i] * nodeCosts[i]; // Multiplicamos cada cantidad por su respectivo precio
                        }

                        DataGridAlmacen.Rows[newRowIndex].Cells[6].Value = totalSalida.ToString(latinoCulture); // Total en la columna "Debe"


                        decimal previousTotal = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value, latinoCulture);
                        decimal newTotal = previousTotal - totalSalida; // Restamos el total de salida del total previo

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = newTotal.ToString(latinoCulture); // Actualizamos el total
                    }

                    break;




                case "PROMEDIO":

                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo año");
                        return;
                    }

                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    if (!int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    if (TxtPrice.Enabled && !EsPrecioValido(TxtPrice.Text))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vacío");
                        return;
                    }


                    // Agregar una nueva fila vacía y obtener el índice de la nueva fila
                    newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato
                    typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas específicas de la nueva fila
                    DataGridAlmacen.Rows[newRowIndex].Cells[0].Value = DataTimeDay.Value;

                    // Si es una entrada
                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        if (newRowIndex == 0)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal totalCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N2", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalCost.ToString("N2", latinoCulture);

                        }
                        else
                        {
                            decimal previousQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture);
                            decimal quentity = Convert.ToDecimal(TxtQuantity.Text, latinoCulture);
                            decimal newQuantity = previousQuantity + quentity;

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToDecimal(TxtQuantity.Text).ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0", latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            decimal currentCost = Convert.ToDecimal(TxtQuantity.Text, latinoCulture) * Convert.ToDecimal(TxtPrice.Text, latinoCulture);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = currentCost.ToString("N2", latinoCulture);

                            decimal previousTotal = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value, latinoCulture);
                            decimal updatedTotal = previousTotal + currentCost;  // Sumar los valores directamente como decimal

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = updatedTotal.ToString("N2", latinoCulture); // Mostrar el resultado en el formato adecuado
                        }

                        // Agregar la entrada a la cola
                        StackOfInventario.Push(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToDecimal(TxtPrice.Text, latinoCulture), Convert.ToInt32(TxtQuantity.Text)));
                    }
                    // Si es una salida (Promedio ponderado)
                    else if (CmBoxDataType.SelectedIndex == 1)
                    {
                        if (StackOfInventario.Count == 0)
                        {
                            MessageBox.Show("No puedes sacar artículos porque el almacén está vacío");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int remainingQuantity = Convert.ToInt32(TxtQuantity.Text);
                        decimal totalSaldo = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value, latinoCulture); // Última columna (saldo total)
                        decimal totalQuantity = Convert.ToDecimal(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value, latinoCulture); // Cantidad total en inventario

                        // Verificar que haya suficiente inventario
                        if (remainingQuantity > totalQuantity)
                        {
                            MessageBox.Show("No hay suficiente inventario para completar la salida");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        // Calcular el costo promedio ponderado
                        decimal avgCost = Math.Round(totalSaldo / totalQuantity, 4);

                        // Calcular el total de la salida usando el promedio ponderado
                        decimal totalSalida = remainingQuantity * avgCost;

                        // Actualizar el inventario con la nueva cantidad
                        decimal newQuantity = totalQuantity - remainingQuantity;
                        decimal newSaldo = totalSaldo - totalSalida;

                        // Actualizar el DataGrid
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = remainingQuantity.ToString("N0", latinoCulture); // Cantidad que se está sacando
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString("N0", latinoCulture); // Cantidad restante
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = avgCost;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalSalida.ToString("N2", latinoCulture); // Mostrar el total de la salida
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = newSaldo.ToString("N2", latinoCulture); // Actualizar el saldo total
                    }
                    break;
            }
        }

        private void BtnAccept_Click(object sender, EventArgs e)
        {
            // Mostrar cuántos elementos hay en la cola
            MessageBox.Show("Cantidad de objetos en la cola: " + QueueofInventori.Count);
        }

        private void CmBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar el índice seleccionado en CmBoxDataType
            if (CmBoxDataType.SelectedIndex == 1)
            {
                // Deshabilitar TxtPrice si el índice es 1
                TxtPrice.Enabled = false;
                TxtPrice.Text = ""; // Opcional: Limpia el texto para asegurarse de que no haya valores.
            }
            else
            {
                // Habilitar TxtPrice si el índice no es 1
                TxtPrice.Enabled = true;
            }
        }



        private void ComBoxFormula_Click(object sender, EventArgs e)
        {
            if (ComBoxFormula.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                DialogResult Election = MessageBox.Show("Si cambias de fórmula se borrarán los datos de la tabla. ¿Estás de acuerdo con esto?", "Confirmación", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Election == DialogResult.OK)
                {
                    DataGridAlmacen.Rows.Clear();
                    ComBoxFormula.SelectedIndex = -1;
                    MessageBox.Show("Listo, por favor continúa.");
                }
                else if (Election == DialogResult.Cancel)
                {
                    MessageBox.Show("Has seleccionado Cancel.");
                }
            }




        }
    }
}
