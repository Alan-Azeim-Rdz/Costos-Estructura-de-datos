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
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);

                        }
                        else
                        {
                            int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value);
                            int newQuantity = previousQuantity + Convert.ToInt32(TxtQuantity.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = newQuantity;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = Convert.ToInt32(TxtQuantity.Text) * Convert.ToInt32(TxtPrice.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 6].Value = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value) + Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 6].Value);


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
                        int totalCost = 0;

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
                                totalCost += firstEntry.Cantidad * firstEntry.Precio;
                                QueueofInventori.Dequeue(); // Eliminamos la entrada ya que se agot�
                            }
                            else
                            {
                                // Usamos una parte de la cantidad de la primera entrada
                                totalCost += remainingQuantity * firstEntry.Precio;
                                firstEntry.Cantidad -= remainingQuantity;
                                remainingQuantity = 0; // Terminamos la salida
                            }
                        }

                        // Actualizamos el DataGrid con la informaci�n de la salida
                        int previousQuantity = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value);
                        int newQuantity = previousQuantity - Convert.ToInt32(TxtQuantity.Text);

                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = newQuantity;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = totalCost / Convert.ToInt32(TxtQuantity.Text); // Precio unitario promedio
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value = totalCost; // Costo total de la salida
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 5].Value = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[typeColumnIndex + 5].Value) - Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 4].Value);

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
                DialogResult Election = MessageBox.Show("Si cambias de formula se borraran los datos de la tabla", "estas deacuerdo con esto?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            
                

                else if (Election == DialogResult.OK)
                {
                    DataGridAlmacen.Rows.Clear();

                    MessageBox.Show("Listo porfavor continua");
                }
                else if (Election == DialogResult.Cancel)
                {
                    MessageBox.Show("Has seleccionado Cancel.");
                }
            

 


        }
    }
}
