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
            // Obtener el a�o de la fecha seleccionada por el usuario en el DateTimePicker
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
                        MessageBox.Show("La fecha debe de ser de este mismo a�o");
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
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vac�o");
                        return;
                    }

                    if (requierePrecio && !int.TryParse(TxtPrice.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vac�o");
                        return;
                    }

                    // Agregar una nueva fila vac�a
                    int newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato
                    int typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas espec�ficas de la nueva fila
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
                            MessageBox.Show("No puedes sacar art�culos porque el almac�n est� vac�o");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int remainingQuantity = Convert.ToInt32(TxtQuantity.Text);
                        List<int> nodeCosts = new List<int>(); // Lista para almacenar los costos de cada nodo
                        List<int> cantidadesUtilizadas = new List<int>(); // Lista para almacenar las cantidades utilizadas de cada nodo
                        int nodeCount = 0; // Contador de nodos utilizados

                        // Verificaci�n previa: Calcula si hay suficiente inventario sin modificar la cola
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

                        // Modificaci�n real: Ahora que sabemos que hay suficiente inventario, modificamos la cola
                        while (remainingQuantity > 0 && QueueofInventori.Count > 0)
                        {
                            MovimientoInventario firstEntry = QueueofInventori.Peek(); // Obtenemos la primera entrada sin eliminarla

                            if (firstEntry.Cantidad <= remainingQuantity)
                            {
                                // Usamos toda la cantidad de la primera entrada
                                remainingQuantity -= firstEntry.Cantidad;
                                cantidadesUtilizadas.Add(firstEntry.Cantidad); // Guardamos la cantidad utilizada
                                nodeCosts.Add(firstEntry.Precio); // A�adir costo a la lista
                                QueueofInventori.Dequeue(); // Eliminamos la entrada ya que se agot�
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la primera entrada
                                cantidadesUtilizadas.Add(remainingQuantity); // Guardamos la cantidad utilizada
                                nodeCosts.Add(firstEntry.Precio); // A�adir costo parcial a la lista
                                firstEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                                nodeCount++; // Incrementamos el contador de nodos utilizados
                            }
                        }

                        // Si usamos 2 o m�s nodos, mostramos el mensaje en la columna 3
                        if (nodeCount >= 2)
                        {
                            DataGridAlmacen.Rows[newRowIndex].Cells[2].Value = $"Usado de {nodeCount} nodos";
                        }

                        // Mostramos los costos de cada nodo separados por un guion en la columna 5
                        string costsDisplay = string.Join(" - ", nodeCosts.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Actualizamos el DataGrid con la informaci�n de la salida
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

                        // Asignamos el total calculado en la columna 7 (�ndice 6) "Debe"
                        DataGridAlmacen.Rows[newRowIndex].Cells[6].Value = totalSalida.ToString("N0"); // Columna "Debe" (�ndice 6)

                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = (previousTotal - totalSalida).ToString("N0");
                    }
                    break;



                case "UEPS":


                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo a�o");
                        return;
                    }
                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de f�rmula");
                        return;
                    }
                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }
                    if (!int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vac�o");
                        return;
                    }
                    if (requierePrecio && !int.TryParse(TxtPrice.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vac�o");
                        return;
                    }

                    // Agregar una nueva fila vac�a y obtener el �ndice de la nueva fila
                    newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato
                    typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas espec�ficas de la nueva fila
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

                            // Aqu� se convierte el valor de la celda anterior a int, eliminando las comas.
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
                            MessageBox.Show("No puedes sacar art�culos porque el almac�n est� vac�o");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int remainingQuantity = Convert.ToInt32(TxtQuantity.Text);
                        int totalCost = 0;
                        List<int> cantidadesUtilizadas = new List<int>(); // Lista para almacenar las cantidades utilizadas
                        List<int> costosPorUnidad = new List<int>(); // Lista para almacenar los costos por cada unidad utilizada

                        // Verificaci�n previa: Calcula si hay suficiente inventario sin modificar la pila
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

                        // Modificaci�n real: Ahora que sabemos que hay suficiente inventario, modificamos la pila
                        while (remainingQuantity > 0 && StackOfInventario.Count > 0)
                        {
                            MovimientoInventario lastEntry = StackOfInventario.Peek(); // Obtenemos la �ltima entrada sin eliminarla

                            if (lastEntry.Cantidad <= remainingQuantity)
                            {
                                // Usamos toda la cantidad de la �ltima entrada
                                remainingQuantity -= lastEntry.Cantidad;
                                totalCost += lastEntry.Cantidad * lastEntry.Precio;
                                cantidadesUtilizadas.Add(lastEntry.Cantidad); // Almacenamos la cantidad usada
                                costosPorUnidad.Add(lastEntry.Precio); // Almacenamos el precio por unidad
                                StackOfInventario.Pop(); // Eliminamos la entrada ya que se agot�
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la �ltima entrada
                                totalCost += remainingQuantity * lastEntry.Precio;
                                cantidadesUtilizadas.Add(remainingQuantity); // Almacenamos la cantidad usada
                                costosPorUnidad.Add(lastEntry.Precio); // Almacenamos el precio por unidad
                                lastEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                            }
                        }

                        // Mostramos los costos por unidad utilizados separados por guiones en la columna correspondiente (�ndice 4)
                        string costsDisplay = string.Join(" - ", costosPorUnidad.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[4].Value = costsDisplay;

                        // Mostramos las cantidades utilizadas separadas por guiones en la columna correspondiente (�ndice 3)
                        string cantidadesDisplay = string.Join(" - ", cantidadesUtilizadas.Select(c => c.ToString("N0")));
                        DataGridAlmacen.Rows[newRowIndex].Cells[3].Value = cantidadesDisplay;

                        // Actualizamos el DataGrid con la informaci�n de la salida
                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", "").Replace(",", ""));
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString("N0"); // Formato con comas

                        // Calcula el precio unitario promedio
                        int unitPriceAverage = totalCost / Convert.ToInt32(TxtQuantity.Text);
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = unitPriceAverage.ToString("N0"); // Formato con comas

                        // Costo total de la salida
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N0"); // Formato con comas

                        // Actualiza el total restante en la columna 7 (�ndice +5)
                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        int totalAfterOutput = previousTotal - totalCost;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = totalAfterOutput.ToString("N0"); // Formato con comas
                    }

                    break;




                case "PROMEDIO":

                    // Verificar que el a�o sea el correcto
                    if (selectedYear == CurrentYear)
                    {
                        MessageBox.Show("La fecha debe de ser de este mismo a�o");
                        return;
                    }

                    // Verificar que se haya seleccionado una f�rmula
                    if (ComBoxFormula.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de f�rmula");
                        return;
                    }

                    // Verificar que se haya seleccionado un tipo de dato
                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de seleccionar un tipo de dato");
                        return;
                    }

                    // Validar que la cantidad sea un n�mero v�lido
                    if (!int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vac�o");
                        return;
                    }

                    // Validar el campo de precio si es requerido
                    decimal precio = 0; // Inicializar la variable
                    if (requierePrecio && !decimal.TryParse(TxtPrice.Text, out precio))
                    {
                        MessageBox.Show("No puedes ingresar letras en el campo de precio y tampoco puedes dejar este campo vac�o");
                        return;
                    }

                    // Agregar una nueva fila vac�a y obtener el �ndice de la nueva fila
                    newRowIndex = DataGridAlmacen.Rows.Add();

                    // Determinar la columna para el tipo de dato (1 para entrada, 2 para salida)
                    typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2;

                    // Asignar valores a las celdas espec�ficas de la nueva fila
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
                        if (newRowIndex > 0) // Aseg�rate de que newRowIndex sea v�lido
                        {
                            int totalQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(",", ""));
                            // Validar que la cantidad a sacar no sea mayor que la disponible
                            int cantidadASacar = Convert.ToInt32(TxtQuantity.Text);
                            if (cantidadASacar > totalQuantity)
                            {
                                MessageBox.Show("No puedes sacar m�s de lo que hay en inventario");
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
            // Mostrar cu�ntos elementos hay en la cola
            MessageBox.Show("Cantidad de objetos en la cola: " + QueueofInventori.Count);
        }

        private void CmBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verificar el �ndice seleccionado en CmBoxDataType
            if (CmBoxDataType.SelectedIndex == 1)
            {
                // Deshabilitar TxtPrice si el �ndice es 1
                TxtPrice.Enabled = false;
                TxtPrice.Text = ""; // Opcional: Limpia el texto para asegurarse de que no haya valores.
            }
            else
            {
                // Habilitar TxtPrice si el �ndice no es 1
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
                DialogResult Election = MessageBox.Show("Si cambias de f�rmula se borrar�n los datos de la tabla. �Est�s de acuerdo con esto?", "Confirmaci�n", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                if (Election == DialogResult.OK)
                {
                    DataGridAlmacen.Rows.Clear();
                    ComBoxFormula.SelectedIndex = -1;
                    MessageBox.Show("Listo, por favor contin�a.");
                }
                else if (Election == DialogResult.Cancel)
                {
                    MessageBox.Show("Has seleccionado Cancel.");
                }
            }




        }
    }
}
