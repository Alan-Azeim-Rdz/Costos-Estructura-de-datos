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

                    if (requierePrecio && !int.TryParse(TxtPrice.Text, out result))
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
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToInt32(TxtQuantity.Text).ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = Convert.ToInt32(TxtQuantity.Text).ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            int totalCost = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalCost.ToString("N0");
                        }
                        else
                        {
                            int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", "").Replace(",", ""));
                            int newQuantity = previousQuantity + Convert.ToInt32(TxtQuantity.Text);

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToInt32(TxtQuantity.Text).ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            int currentCost = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = currentCost.ToString("N0");

                            int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value.ToString().Replace(".", "").Replace(",", ""));
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = (currentCost + previousTotal).ToString("N0");
                        }

                        // Agregar la entrada a la cola
                        QueueofInventori.Enqueue(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToInt32(TxtPrice.Text), Convert.ToInt32(TxtQuantity.Text)));
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
                        List<int> nodeCosts = new List<int>(); // Lista para almacenar los costos de cada nodo
                        List<int> cantidadesUtilizadas = new List<int>(); // Lista para almacenar las cantidades utilizadas de cada nodo
                        int nodeCount = 0; // Contador de nodos utilizados

                        // Verificación previa: Calcula si hay suficiente inventario sin modificar la cola
                        int cantidadDisponible = 0;
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
                                nodeCosts.Add(firstEntry.Precio); // Añadir costo a la lista
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
                        string costsDisplay = string.Join(" - ", nodeCosts.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Actualizamos el DataGrid con la información de la salida
                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", "").Replace(",", ""));
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString("N0");

                        // Calculamos el total de la salida multiplicando la cantidad retirada por el precio de cada nodo
                        int totalSalida = 0;
                        for (int i = 0; i < nodeCosts.Count; i++)
                        {
                            totalSalida += cantidadesUtilizadas[i] * nodeCosts[i]; // Multiplicamos cada cantidad por su respectivo precio
                        }

                        // Asignamos el total calculado en la columna 7 (índice 6) "Debe"
                        DataGridAlmacen.Rows[newRowIndex].Cells[6].Value = totalSalida.ToString("N0"); // Columna "Debe" (índice 6)

                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = (previousTotal - totalSalida).ToString("N0");
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
                    if (requierePrecio && !int.TryParse(TxtPrice.Text, out result))
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
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            int totalPrice = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalPrice.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalPrice.ToString("N0");
                        }
                        else
                        {
                            int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", ""));
                            int newQuantity = previousQuantity + Convert.ToInt32(TxtQuantity.Text);

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;

                            int totalPrice = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalPrice.ToString("N0");

                            // Aquí se convierte el valor de la celda anterior a int, eliminando las comas.
                            int previousTotal = int.Parse(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value.ToString().Replace(".", ""));
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = (previousTotal + totalPrice).ToString("N0");
                        }

                        // Agregar la entrada a la pila
                        StackOfInventario.Push(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToInt32(TxtPrice.Text), Convert.ToInt32(TxtQuantity.Text)));
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
                        int totalCost = 0;
                        List<int> cantidadesUtilizadas = new List<int>(); // Lista para almacenar las cantidades utilizadas
                        List<int> costosPorUnidad = new List<int>(); // Lista para almacenar los costos por cada unidad utilizada

                        // Verificación previa: Calcula si hay suficiente inventario sin modificar la pila
                        int cantidadDisponible = 0;
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
                            MovimientoInventario lastEntry = StackOfInventario.Peek(); // Obtenemos la última entrada sin eliminarla

                            if (lastEntry.Cantidad <= remainingQuantity)
                            {
                                // Usamos toda la cantidad de la última entrada
                                remainingQuantity -= lastEntry.Cantidad;
                                totalCost += lastEntry.Cantidad * lastEntry.Precio;
                                cantidadesUtilizadas.Add(lastEntry.Cantidad); // Almacenamos la cantidad usada
                                costosPorUnidad.Add(lastEntry.Precio); // Almacenamos el precio por unidad
                                StackOfInventario.Pop(); // Eliminamos la entrada ya que se agotó
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la última entrada
                                totalCost += remainingQuantity * lastEntry.Precio;
                                cantidadesUtilizadas.Add(remainingQuantity); // Almacenamos la cantidad usada
                                costosPorUnidad.Add(lastEntry.Precio); // Almacenamos el precio por unidad
                                lastEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                            }
                        }

                        // Mostramos los costos por unidad utilizados separados por guiones en la columna correspondiente (índice 4)
                        string costsDisplay = string.Join(" - ", costosPorUnidad.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Mostramos las cantidades utilizadas separadas por guiones en la columna correspondiente (índice 3)
                        string cantidadesDisplay = string.Join(" - ", cantidadesUtilizadas.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[3].Value = cantidadesDisplay;

                        // Actualizamos el DataGrid con la información de la salida
                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", "").Replace(",", ""));
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString("N0"); // Formato con comas

                        // Calcula el precio unitario promedio
                        int unitPriceAverage = totalCost / Convert.ToInt32(TxtQuantity.Text);
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = unitPriceAverage.ToString("N0"); // Formato con comas

                        // Costo total de la salida
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N0"); // Formato con comas

                        // Actualiza el total restante en la columna 7 (índice +5)
                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        int totalAfterOutput = previousTotal - totalCost;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = totalAfterOutput.ToString("N0"); // Formato con comas
                    }

                    break;




                case "PROMEDIO":

                    // Verificar que el año sea el correcto
                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo año");
                        return;
                    }

                    // Verificar que se haya seleccionado una fórmula
                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de fórmula");
                        return;
                    }

                    // Verificar que se haya seleccionado un tipo de dato
                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    // Validar que la cantidad sea un número válido
                    if (!int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    // Validar el campo de precio si es requerido
                    decimal precio = 0; // Inicializar la variable
                    if (requierePrecio && !decimal.TryParse(TxtPrice.Text, out precio))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vacío");
                        return;
                    }

                    // Agregar una nueva fila vacía y obtener el índice de la nueva fila
                    newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato (1 para entrada, 2 para salida)
                    typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas específicas de la nueva fila
                    DataGridAlmacen.Rows[newRowIndex].Cells[0].Value = DataTimeDay.Value;

                    // Si es una entrada
                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        // Para la primera fila
                        if (newRowIndex == 0)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = precio.ToString("N2");

                            decimal totalPrice = Convert.ToDecimal(TxtQuantity.Text) * precio;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalPrice.ToString("N2");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = totalPrice.ToString("N2");
                        }
                        else // Para las filas subsecuentes
                        {
                            int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(",", ""));
                            int newQuantity = previousQuantity + Convert.ToInt32(TxtQuantity.Text);

                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = precio.ToString("N2");

                            decimal totalPrice = Convert.ToDecimal(TxtQuantity.Text) * precio;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalPrice.ToString("N2");

                            decimal previousTotal = decimal.Parse(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value.ToString().Replace(".", ""));
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = (previousTotal + totalPrice).ToString("N2");
                        }

                        // Agregar la entrada a la pila
                        StackOfInventario.Push(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, (int)Math.Round(precio), Convert.ToInt32(TxtQuantity.Text))); // Convertir a int
                    }
                    // Si es una salida
                    else if (CmBoxDataType.SelectedIndex == 1) // Si es una salida
                    {
                        // Asegurarse de que haya filas anteriores
                        if (newRowIndex == 0)
                        {
                            MessageBox.Show("No puedes hacer una salida si no hay inventario.");
                            return;
                        }

                        // Obtener la cantidad total en el inventario
                        if (newRowIndex > 0) // Asegúrate de que newRowIndex sea válido
                        {
                            int totalQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(",", ""));
                            // Validar que la cantidad a sacar no sea mayor que la disponible
                            int cantidadASacar = Convert.ToInt32(TxtQuantity.Text);
                            if (cantidadASacar > totalQuantity)
                            {
                                MessageBox.Show("No puedes sacar más de lo que hay en inventario");
                                return;
                            }

                            // Calcular el precio promedio
                            decimal previousTotalPrice = decimal.Parse(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(",", ""));
                            decimal averagePrice = previousTotalPrice / totalQuantity;

                            // Actualizar la cantidad en el inventario
                            int nuevaCantidad = totalQuantity - cantidadASacar;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = cantidadASacar.ToString("N0");
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = nuevaCantidad.ToString("N0");

                            // Calcular el total del precio de salida
                            decimal totalPrecioSalida = cantidadASacar * averagePrice;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalPrecioSalida.ToString("N2");

                            // Actualizar el total del inventario
                            decimal nuevoTotal = previousTotalPrice - totalPrecioSalida;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = nuevoTotal.ToString("N2");

                            // Agregar la salida a la pila
                            StackOfInventario.Push(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, (int)Math.Round(averagePrice), cantidadASacar)); // Convertir a int
                        }
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
