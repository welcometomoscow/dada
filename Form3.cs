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
    public partial class Form3 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form3()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable("Nedvizhimost");
        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Operations] FROM Nedvizhimost", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Nedvizhimost");

                dataGridView1.DataSource = dataSet.Tables["Nedvizhimost"];

                dataGridView1.Columns[1].HeaderText = "Наименование";
                dataGridView1.Columns[2].HeaderText = "Площадь";
                dataGridView1.Columns[3].HeaderText = "Стоимость";
                dataGridView1.Columns[4].HeaderText = "Этаж";
                dataGridView1.Columns[5].HeaderText = "Кол-во комнат";
                dataGridView1.Columns[6].HeaderText = "Город";
                dataGridView1.Columns[7].HeaderText = "Улица";
                dataGridView1.Columns[8].HeaderText = "Дом";
                ReloadData();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;
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
                dataSet.Tables["Nedvizhimost"].Clear();

                sqlDataAdapter.Fill(dataSet, "Nedvizhimost");

                dataGridView1.DataSource = dataSet.Tables["Nedvizhimost"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[9, i] = linkCell;
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
                if (e.ColumnIndex == 9)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();

                    if(task == "Delete")
                    {
                        if(MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;

                            dataGridView1.Rows.RemoveAt(rowIndex);

                            dataSet.Tables["Nedvizhimost"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Nedvizhimost");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Nedvizhimost"].NewRow();

                        
                        row["Naimenovanie"] = dataGridView1.Rows[rowIndex].Cells["Naimenovanie"].Value;
                        row["Ploshad"] = dataGridView1.Rows[rowIndex].Cells["Ploshad"].Value;
                        row["Stoimost"] = dataGridView1.Rows[rowIndex].Cells["Stoimost"].Value;
                        row["Etazh"] = dataGridView1.Rows[rowIndex].Cells["Etazh"].Value;
                        row["Kolvo_komnat"] = dataGridView1.Rows[rowIndex].Cells["Kolvo_komnat"].Value;
                        row["Gorod"] = dataGridView1.Rows[rowIndex].Cells["Gorod"].Value;
                        row["Ulica"] = dataGridView1.Rows[rowIndex].Cells["Ulica"].Value;
                        row["Dom"] = dataGridView1.Rows[rowIndex].Cells["Dom"].Value;

                        dataSet.Tables["Nedvizhimost"].Rows.Add(row);

                        dataSet.Tables["Nedvizhimost"].Rows.RemoveAt(dataSet.Tables["Nedvizhimost"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Nedvizhimost");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                      
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Naimenovanie"] = dataGridView1.Rows[r].Cells["Naimenovanie"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Ploshad"] = dataGridView1.Rows[r].Cells["Ploshad"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Stoimost"] = dataGridView1.Rows[r].Cells["Stoimost"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Etazh"] = dataGridView1.Rows[r].Cells["Etazh"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Kolvo_komnat"] = dataGridView1.Rows[r].Cells["Kolvo_komnat"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Gorod"] = dataGridView1.Rows[r].Cells["Gorod"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Ulica"] = dataGridView1.Rows[r].Cells["Ulica"].Value;
                        dataSet.Tables["Nedvizhimost"].Rows[r]["Dom"] = dataGridView1.Rows[r].Cells["Dom"].Value;

                        sqlDataAdapter.Update(dataSet, "Nedvizhimost");

                        dataGridView1.Rows[e.RowIndex].Cells[9].Value = "Delete";
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

                    dataGridView1[9, lastRow] = linkCell;

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

                    dataGridView1[9, rowIndex] = linkCell;

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void клиентыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 f2 = new Form2();
            f2.ShowDialog();
            this.Close();
        }

        private void недвижимостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
           this.Close();

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Naimenovanie LIKE '%{textBox1.Text}%'";                              
            ReloadData();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.ShowDialog();
            Form3 f3 = new Form3();
            f3.ShowDialog();
            aboutBox1.Close();
        }

        private void риелторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 f4 = new Form4();
            f4.ShowDialog();
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
