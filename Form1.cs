using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        private const string FilePath = "datos.csv";

        public Form1()
        {
            InitializeComponent();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            if (File.Exists(FilePath))
            {
                LoadData();
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "")
            {
                MessageBox.Show("Por favor, ingrese datos en todos los campos.");
                return;
            }

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridView1);
            row.SetValues(textBox1.Text, textBox2.Text, textBox3.Text);

            dataGridView1.Rows.Add(row);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();

            SaveData();
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione una fila para eliminar.");
                return;
            }

            int selectedIndex = dataGridView1.SelectedRows[0].Index;

            dataGridView1.Rows.RemoveAt(selectedIndex);

            SaveData();
        }

        private void LoadData()
        {
            DataTable dataTable = new DataTable();
            List<string[]> rows = File.ReadAllLines(FilePath).Select(line => line.Split(',')).ToList();

            if (dataTable.Columns.Count == 0)
            {
                for (int i = 0; i < rows[0].Length; i++)
                {
                    dataTable.Columns.Add($"Columna {i + 1}");
                }
            }

            foreach (var row in rows)
            {
                dataTable.Rows.Add(row);
            }

            dataGridView1.DataSource = dataTable;
        }

        private void SaveData()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    List<string> rowData = new List<string>();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        rowData.Add(cell.Value.ToString());
                    }
                    writer.WriteLine(string.Join(",", rowData));
                }
            }
        }
    }
}