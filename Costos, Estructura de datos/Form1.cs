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

                    if (Matriz == 7)
                    {
                        Matriz = 0;
                    }

                    if (CmBoxDataType.SelectedIndex == 0)
                    {
                        DataGridAlmacen.Rows.Add(CurrentYear);

                    }

                    DataGridAlmacen.Rows.Add()


                    Queue<MovimientoInventario> QueueofInventori = new Queue<MovimientoInventario>();

                    QueueofInventori.Enqueue(new MovimientoInventario(DataTimeDay.Value, CmBoxDataType.Text, Convert.ToInt32(TxtPrice.Text), Convert.ToInt32(TxtQuantity.Text)));
                   





                    break;



                case "UEPS":


                    break;




                case "PROMEDIO":


                    break;

            }




        }
    }
}
