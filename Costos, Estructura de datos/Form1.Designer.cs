namespace Costos__Estructura_de_datos
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridAlmacen = new DataGridView();
            dia = new DataGridViewTextBoxColumn();
            Entrada = new DataGridViewTextBoxColumn();
            Salida = new DataGridViewTextBoxColumn();
            Existencia = new DataGridViewTextBoxColumn();
            Costo = new DataGridViewTextBoxColumn();
            Debe = new DataGridViewTextBoxColumn();
            Haber = new DataGridViewTextBoxColumn();
            Sldo = new DataGridViewTextBoxColumn();
            BtnAdd = new Button();
            BtnAccept = new Button();
            LblDay = new Label();
            LblType = new Label();
            LblDatOutput = new Label();
            LblCost = new Label();
            CmBoxDataType = new ComboBox();
            DataTimeDay = new DateTimePicker();
            TxtQuantity = new TextBox();
            LblContain = new Label();
            LblPrice = new Label();
            TxtPrice = new TextBox();
            BtnEdit = new Button();
            ComBoxFormula = new ComboBox();
            F = new Label();
            ((System.ComponentModel.ISupportInitialize)DataGridAlmacen).BeginInit();
            SuspendLayout();
            // 
            // DataGridAlmacen
            // 
            DataGridAlmacen.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridAlmacen.Columns.AddRange(new DataGridViewColumn[] { dia, Entrada, Salida, Existencia, Costo, Debe, Haber, Sldo });
            DataGridAlmacen.Location = new Point(426, 12);
            DataGridAlmacen.Name = "DataGridAlmacen";
            DataGridAlmacen.Size = new Size(843, 553);
            DataGridAlmacen.TabIndex = 0;
            // 
            // dia
            // 
            dia.HeaderText = "Dia";
            dia.Name = "dia";
            // 
            // Entrada
            // 
            Entrada.HeaderText = "Entrada";
            Entrada.Name = "Entrada";
            // 
            // Salida
            // 
            Salida.HeaderText = "Salida";
            Salida.Name = "Salida";
            // 
            // Existencia
            // 
            Existencia.HeaderText = "Existencia";
            Existencia.Name = "Existencia";
            // 
            // Costo
            // 
            Costo.HeaderText = "Costo";
            Costo.Name = "Costo";
            // 
            // Debe
            // 
            Debe.HeaderText = "Debe";
            Debe.Name = "Debe";
            // 
            // Haber
            // 
            Haber.HeaderText = "Haber";
            Haber.Name = "Haber";
            // 
            // Sldo
            // 
            Sldo.HeaderText = "Saldo";
            Sldo.Name = "Sldo";
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(214, 330);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(154, 54);
            BtnAdd.TabIndex = 1;
            BtnAdd.Text = "Agregar";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // BtnAccept
            // 
            BtnAccept.Location = new Point(108, 461);
            BtnAccept.Name = "BtnAccept";
            BtnAccept.Size = new Size(154, 54);
            BtnAccept.TabIndex = 2;
            BtnAccept.Text = "Aceptar";
            BtnAccept.UseVisualStyleBackColor = true;
            // 
            // LblDay
            // 
            LblDay.AutoSize = true;
            LblDay.Location = new Point(12, 28);
            LblDay.Name = "LblDay";
            LblDay.Size = new Size(27, 15);
            LblDay.TabIndex = 3;
            LblDay.Text = "Day";
            // 
            // LblType
            // 
            LblType.AutoSize = true;
            LblType.Location = new Point(12, 124);
            LblType.Name = "LblType";
            LblType.Size = new Size(142, 15);
            LblType.TabIndex = 4;
            LblType.Text = "Selecciona el tipo de dato";
            // 
            // LblDatOutput
            // 
            LblDatOutput.AutoSize = true;
            LblDatOutput.Location = new Point(12, 225);
            LblDatOutput.Name = "LblDatOutput";
            LblDatOutput.Size = new Size(0, 15);
            LblDatOutput.TabIndex = 5;
            // 
            // LblCost
            // 
            LblCost.AutoSize = true;
            LblCost.Location = new Point(12, 168);
            LblCost.Name = "LblCost";
            LblCost.Size = new Size(0, 15);
            LblCost.TabIndex = 6;
            // 
            // CmBoxDataType
            // 
            CmBoxDataType.DropDownStyle = ComboBoxStyle.DropDownList;
            CmBoxDataType.FormattingEnabled = true;
            CmBoxDataType.Items.AddRange(new object[] { "Entrada", "Salida" });
            CmBoxDataType.Location = new Point(12, 142);
            CmBoxDataType.Name = "CmBoxDataType";
            CmBoxDataType.Size = new Size(121, 23);
            CmBoxDataType.TabIndex = 7;
            // 
            // DataTimeDay
            // 
            DataTimeDay.Format = DateTimePickerFormat.Short;
            DataTimeDay.ImeMode = ImeMode.NoControl;
            DataTimeDay.Location = new Point(12, 46);
            DataTimeDay.Name = "DataTimeDay";
            DataTimeDay.Size = new Size(121, 23);
            DataTimeDay.TabIndex = 8;
            DataTimeDay.Value = new DateTime(2024, 8, 27, 0, 0, 0, 0);
            // 
            // TxtQuantity
            // 
            TxtQuantity.Location = new Point(193, 142);
            TxtQuantity.Name = "TxtQuantity";
            TxtQuantity.Size = new Size(161, 23);
            TxtQuantity.TabIndex = 9;
            // 
            // LblContain
            // 
            LblContain.AutoSize = true;
            LblContain.Location = new Point(193, 124);
            LblContain.Name = "LblContain";
            LblContain.Size = new Size(55, 15);
            LblContain.TabIndex = 10;
            LblContain.Text = "Cantidad";
            // 
            // LblPrice
            // 
            LblPrice.AutoSize = true;
            LblPrice.Location = new Point(12, 207);
            LblPrice.Name = "LblPrice";
            LblPrice.Size = new Size(82, 15);
            LblPrice.TabIndex = 12;
            LblPrice.Text = "Precio / Costo";
            // 
            // TxtPrice
            // 
            TxtPrice.Location = new Point(12, 225);
            TxtPrice.Name = "TxtPrice";
            TxtPrice.Size = new Size(161, 23);
            TxtPrice.TabIndex = 11;
            // 
            // BtnEdit
            // 
            BtnEdit.Location = new Point(12, 330);
            BtnEdit.Name = "BtnEdit";
            BtnEdit.Size = new Size(154, 54);
            BtnEdit.TabIndex = 13;
            BtnEdit.Text = "Editar";
            BtnEdit.UseVisualStyleBackColor = true;
            // 
            // ComBoxFormula
            // 
            ComBoxFormula.DropDownStyle = ComboBoxStyle.DropDownList;
            ComBoxFormula.FormattingEnabled = true;
            ComBoxFormula.Items.AddRange(new object[] { "PEPS", "UEPS", "PROMEDIO" });
            ComBoxFormula.Location = new Point(193, 46);
            ComBoxFormula.Name = "ComBoxFormula";
            ComBoxFormula.Size = new Size(161, 23);
            ComBoxFormula.TabIndex = 14;
            // 
            // F
            // 
            F.AutoSize = true;
            F.Location = new Point(193, 28);
            F.Name = "F";
            F.Size = new Size(51, 15);
            F.TabIndex = 15;
            F.Text = "Formula";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1281, 589);
            Controls.Add(F);
            Controls.Add(ComBoxFormula);
            Controls.Add(BtnEdit);
            Controls.Add(LblPrice);
            Controls.Add(TxtPrice);
            Controls.Add(LblContain);
            Controls.Add(TxtQuantity);
            Controls.Add(DataTimeDay);
            Controls.Add(CmBoxDataType);
            Controls.Add(LblCost);
            Controls.Add(LblDatOutput);
            Controls.Add(LblType);
            Controls.Add(LblDay);
            Controls.Add(BtnAccept);
            Controls.Add(BtnAdd);
            Controls.Add(DataGridAlmacen);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)DataGridAlmacen).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView DataGridAlmacen;
        private DataGridViewTextBoxColumn dia;
        private DataGridViewTextBoxColumn Entrada;
        private DataGridViewTextBoxColumn Salida;
        private DataGridViewTextBoxColumn Existencia;
        private DataGridViewTextBoxColumn Costo;
        private DataGridViewTextBoxColumn Debe;
        private DataGridViewTextBoxColumn Haber;
        private DataGridViewTextBoxColumn Sldo;
        private Button BtnAdd;
        private Button BtnAccept;
        private Label LblDay;
        private Label LblType;
        private Label LblDatOutput;
        private Label LblCost;
        private ComboBox CmBoxDataType;
        private DateTimePicker DataTimeDay;
        private TextBox TxtQuantity;
        private Label LblContain;
        private Label LblPrice;
        private TextBox TxtPrice;
        private Button BtnEdit;
        private ComboBox ComBoxFormula;
        private Label F;
    }
}
