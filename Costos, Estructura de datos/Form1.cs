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
                        MessageBox.Show("Debes de selecionar un tipo de dato");
                        return;
                    }
                    if (CmBoxDataType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Debes de selecionar un tipo de dato");
                        return;

                    }
                    if (!int.TryParse(TxtQuantity.Text, out int result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacio");
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
                        int totalCost = 0;

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
                                totalCost += firstEntry.Cantidad * firstEntry.Precio;
                                QueueofInventori.Dequeue(); // Eliminamos la entrada ya que se agotó
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la primera entrada
                                totalCost += remainingQuantity * firstEntry.Precio;
                                firstEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                            }
                        }

                        // Actualizamos el DataGrid con la información de la salida
                        // Asegúrate de limpiar los valores de entrada
                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value.ToString().Replace(".", "").Replace(",", ""));
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text.Replace(".", "").Replace(",", ""));

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = Convert.ToInt32(TxtQuantity.Text.Replace(".", "").Replace(",", "")).ToString("N0");
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity.ToString("N0");
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = (totalCost / Convert.ToInt32(TxtQuantity.Text.Replace(".", "").Replace(",", ""))).ToString("N0"); // Precio unitario promedio
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost.ToString("N0"); // Costo total de la salida

                        // Asegúrate de limpiar el valor de la celda anterior al convertir
                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = (previousTotal - Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value.ToString().Replace(".", "").Replace(",", ""))).ToString("N0");

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
                                StackOfInventario.Pop(); // Eliminamos la entrada ya que se agotó
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la última entrada
                                totalCost += remainingQuantity * lastEntry.Precio;
                                lastEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                            }
                        }

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

                        // Actualiza el total restante
                        int previousTotal = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value.ToString().Replace(".", "").Replace(",", ""));
                        int totalAfterOutput = previousTotal - Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value.ToString().Replace(".", "").Replace(",", ""));
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = totalAfterOutput.ToString("N0"); // Formato con comas
                    }

                    break;




                case "PROMEDIO":


                   

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
