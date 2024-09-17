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
                    if (!int.TryParse(TxtPrice.Text, out int result) || !int.TryParse(TxtQuantity.Text, out result))
                    {
                        MessageBox.Show("No puedes ingresar letras y tampoco puedes dejar este campo vacio");
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
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                        }
                        else
                        {
                            int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value);
                            int newQuantity = previousQuantity + Convert.ToInt32(TxtQuantity.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
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

                        if (remainingQuantity > 0)
                        {
                            MessageBox.Show("No hay suficiente inventario para completar la salida");
                            DataGridAlmacen.Rows.RemoveAt(newRowIndex);
                            return;
                        }

                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value);
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = totalCost / Convert.ToInt32(TxtQuantity.Text); // Precio unitario promedio
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost; // Costo total de la salida
                    }

                    // Mostrar cuántos elementos hay en la cola
                    MessageBox.Show("Cantidad de objetos en la cola: " + QueueofInventori.Count);

                    // Recorrer la cola sin borrar los elementos
                    foreach (var item in QueueofInventori)
                    {
                        MessageBox.Show(item.ToString());
                    }





                    break;



                case "UEPS":


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
    }
}
