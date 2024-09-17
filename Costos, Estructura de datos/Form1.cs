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
        string Select = "";

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
                    int typeColumnIndex = CmBoxDataType.SelectedIndex == 0 ? 1 : 2; // 1 para "Entrada", 2 para "Salida"

                    // Asignar valores a las celdas específicas de la nueva fila
                    DataGridAlmacen.Rows[newRowIndex].Cells[0].Value = DataTimeDay.Value; // Columna 0: Fecha
                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 2].Value = TxtPrice.Text;
                    }
                    else
                    {
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex].Value = TxtQuantity.Text;
                        DataGridAlmacen.Rows[newRowIndex].Cells[typeColumnIndex + 1].Value = TxtPrice.Text; // Columna 1 o 2: Cantidad según tipo

                    }


                    // Agregar a la cola
                    Queue<MovimientoInventario> QueueofInventori = new Queue<MovimientoInventario>();
                    QueueofInventori.Enqueue(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToInt32(TxtPrice.Text), Convert.ToInt32(TxtQuantity.Text)));

                    while (QueueofInventori.Count > 0)
                    {
                        var item = QueueofInventori.Dequeue();
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
