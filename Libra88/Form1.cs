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

namespace Libra88
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source = GENTI\SQLEXPRESS01; Initial Catalog = library2; Integrated Security = True;");
        int booksID = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reset();
            FillDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                if (btnSave.Text == "Save")
                {
                    SqlCommand sqlCmd = new SqlCommand("blla6", con);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Add");
                    sqlCmd.Parameters.AddWithValue("@booksID", 0);
                    sqlCmd.Parameters.AddWithValue("@title", textBox1.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@genre", textBox2.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@author", textBox3.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("New Record successfully Added");
                }
                else
                {
                    SqlCommand sqlCmd = new SqlCommand("blla6", con);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                    sqlCmd.Parameters.AddWithValue("@booksID", booksID);
                    sqlCmd.Parameters.AddWithValue("@title", textBox1.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@genre", textBox2.Text.Trim());
                    sqlCmd.Parameters.AddWithValue("@author", textBox3.Text.Trim());
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("This Record successfully has been updated");
                }

                Reset();
                FillDataGridView();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message");
            }
            finally
            {
                con.Close();
            }
        }
        void FillDataGridView()
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlDataAdapter sda = new SqlDataAdapter("sp_search", con);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.AddWithValue("@title", textBox4.Text.Trim());
            DataTable dtbl = new DataTable();
            sda.Fill(dtbl);
            dGV1.DataSource = dtbl;

            con.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }

        private void dGV1_DoubleClick(object sender, EventArgs e)
        {
            if(dGV1.CurrentRow.Index != -1)
            {
                booksID = Convert.ToInt32(dGV1.CurrentRow.Cells[0].Value.ToString());
                textBox1.Text = dGV1.CurrentRow.Cells[1].Value.ToString();
                textBox2.Text = dGV1.CurrentRow.Cells[2].Value.ToString();
                textBox3.Text = dGV1.CurrentRow.Cells[2].Value.ToString();
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        void Reset()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            btnSave.Text = "Save";
            booksID = 0;
            btnDelete.Enabled = false;
                                 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                {
                    SqlCommand sqlCmd = new SqlCommand("sp_delete", con);
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("@booksID", booksID);
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("The Record Deleted successfully");
                    Reset();
                    FillDataGridView();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error Message");
            }
        }
    }
}
