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

                    if (newRowIndex == 0)
                    {
                        if(ComBoxFormula.SelectedIndex == 0)
                        {
                            // Sumar la cantidad del movimiento anterior
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                        }
                        else if (ComBoxFormula.SelectedIndex == 1)
                        {
                            // Restar la cantidad del movimiento anterior
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = TxtPrice.Text;
                        }
                       
                    }
                    else if (newRowIndex > 0)
                    {
                        if (CmBoxDataType.SelectedIndex == 0 && DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value != null)
                        {
                            int Result = Convert.ToInt32(TxtQuantity.Text) + Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = Result;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 3].Value = TxtPrice.Text;
                        }
                        else if (CmBoxDataType.SelectedIndex == 1 && DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value != null)
                        {
                            int Result2 = Convert.ToInt32(DataGridAlmacen.Rows[newRowIndex - 1].Cells[3].Value) - Convert.ToInt32(TxtQuantity.Text);
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = Result2;
                            DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtPrice.Text; // Columna 1 o 2: Cantidad según tipo
                        }
                    }

                       






                    // Agregar a la cola
                    Queue<MovimientoInventario> QueueofInventori = new Queue<MovimientoInventario>();

                    // Agregar un elemento a la cola
                    QueueofInventori.Enqueue(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToInt32(TxtPrice.Text), Convert.ToInt32(TxtQuantity.Text)));

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
    }
}
