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
    public partial class Form5 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;


        public Form5()
        {
            InitializeComponent();
        }
        DataTable dt = new DataTable("Zastroyshik");
        private void LoadData()

        {                     
            try
            {               
                sqlDataAdapter = new SqlDataAdapter("SELECT *, 'Delete' AS [Operations] FROM Zastroyshik", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Zastroyshik");

                dataGridView1.DataSource = dataSet.Tables["Zastroyshik"];
                dataGridView1.Columns[1].HeaderText = "Название фирмы";
                dataGridView1.Columns[2].HeaderText = "Стаж на рынке";
                dataGridView1.Columns[3].HeaderText = "ИНН";
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
                dataSet.Tables["Zastroyshik"].Clear();

                sqlDataAdapter.Fill(dataSet, "Zastroyshik");

                dataGridView1.DataSource = dataSet.Tables["Zastroyshik"];

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

                            dataSet.Tables["Zastroyshik"].Rows[rowIndex].Delete();

                            sqlDataAdapter.Update(dataSet, "Zastroyshik");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Zastroyshik"].NewRow();

                        
                        row["Nazvanie_firmi"] = dataGridView1.Rows[rowIndex].Cells["Nazvanie_firmi"].Value;
                        row["Stazh_na_rinke"] = dataGridView1.Rows[rowIndex].Cells["Stazh_na_rinke"].Value;
                        row["INN"] = dataGridView1.Rows[rowIndex].Cells["INN"].Value;                       

                        dataSet.Tables["Zastroyshik"].Rows.Add(row);

                        dataSet.Tables["Zastroyshik"].Rows.RemoveAt(dataSet.Tables["Zastroyshik"].Rows.Count - 1);

                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);

                        dataGridView1.Rows[e.RowIndex].Cells[4].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Zastroyshik");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;
                      
                        dataSet.Tables["Zastroyshik"].Rows[r]["Nazvanie_firmi"] = dataGridView1.Rows[r].Cells["Nazvanie_firmi"].Value;
                        dataSet.Tables["Zastroyshik"].Rows[r]["Stazh_na_rinke"] = dataGridView1.Rows[r].Cells["Stazh_na_rinke"].Value;
                        dataSet.Tables["Zastroyshik"].Rows[r]["INN"] = dataGridView1.Rows[r].Cells["INN"].Value;
                        

                        sqlDataAdapter.Update(dataSet, "Zastroyshik");

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
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Nazvanie_firmi LIKE '%{textBox1.Text}%'";                              
            ReloadData();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.ShowDialog();
            Form5 f5 = new Form5();
            f5.ShowDialog();
            aboutBox1.Close();

        }

        private void риелторToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form4 f4 = new Form4();
            f4.ShowDialog();
            this.Close();
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
            this.Show();          
        }
    }
}
