using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace WindowsFormsApp2
{
    public partial class Form4 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;


        public Form4()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable("Rieltor");
        private void LoadData()

        {                     
            try
            {               
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Operations] FROM Rieltor", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Rieltor");

                dataGridView1.DataSource = dataSet.Tables["Rieltor"];
                dataGridView1.Columns[1].HeaderText = "Имя";
                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[3].HeaderText = "Отчество";
                ReloadData();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[4, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Rieltor"].Clear();

                sqlDataAdapter.Fill(dataSet, "Rieltor");

                dataGridView1.DataSource = dataSet.Tables["Rieltor"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[4, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=KRAZ;Initial Catalog=AgentNedv;Integrated Security=True");

            sqlConnection.Open();

            LoadData();

            }
           

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 4)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();

                    if(task == "Delete")
                    {
                        if(MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Rieltor"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Rieltor");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Rieltor"].NewRow();

                        
                        row["Imya"] = dataGridView1.Rows[rowIndex].Cells["Imya"].Value;
                        row["Familya"] = dataGridView1.Rows[rowIndex].Cells["Familya"].Value;
                        row["Otchestvo"] = dataGridView1.Rows[rowIndex].Cells["Otchestvo"].Value;                       

                        dataSet.Tables["Rieltor"].Rows.Add(row);

                        dataSet.Tables["Rieltor"].Rows.RemoveAt(dataSet.Tables["Rieltor"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Rieltor");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                      
                        dataSet.Tables["Rieltor"].Rows[r]["Imya"] = dataGridView1.Rows[r].Cells["Imya"].Value;
                        dataSet.Tables["Rieltor"].Rows[r]["Familya"] = dataGridView1.Rows[r].Cells["Familya"].Value;
                        dataSet.Tables["Rieltor"].Rows[r]["Otchestvo"] = dataGridView1.Rows[r].Cells["Otchestvo"].Value;
                        

                        sqlDataAdapter.Update(dataSet, "Rieltor");

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";
                    }

                    ReloadData();   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

            private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[4, lastRow] = linkCell;

                    row.Cells["Operations"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[4, rowIndex] = linkCell;

                    editingRow.Cells["Operations"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            this.Close();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
           this.Close();

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Familya LIKE '%{textBox1.Text}%'";                              
            ReloadData();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.ShowDialog();
            Form4 f4 = new Form4();
            f4.ShowDialog();
            aboutBox1.Close();

        }

        private void риелторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void недвижимостьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            this.Close();
        }

        private void застройщикToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 f5 = new Form5();
            f5.ShowDialog();
            this.Close();
        }
    }
}
