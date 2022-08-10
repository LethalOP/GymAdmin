using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace GymAdmin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            load();
        }

        SqlConnection con = new SqlConnection("Data source=LAPTOP-AL4ANPJC;  Initial Catalog=GADB; User Id=SM; Password=123");
        SqlCommand cmd;
        SqlDataReader read;
        SqlDataAdapter drr;
        String id;
        bool Mode = true;
        String sql;

        public void load()
        {
            try
            {
                sql = "Select * from Member";
                cmd = new SqlCommand(sql, con);
                con.Open();
                read = cmd.ExecuteReader();
                drr = new SqlDataAdapter(sql, con); 
                grid.Rows.Clear();
                while(read.Read())
                {
                    grid.Rows.Add(read[0], read[1], read[2], read[3]);
                }
                con.Close();


            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();    
            }
        }

        public void getId(String id)
        {
            sql = "select * from Member where id'" + id + "' ";
            cmd=new SqlCommand(sql, con);
            con.Open();
            read = cmd.ExecuteReader();
            while (read.Read())
            {
                txtName.Text = read[1].ToString();
                textgen.Text = read[2].ToString();
                txtCon.Text=read[3].ToString();
            }
            con.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String name=txtName.Text; ;
            String Gen = textgen.Text;
            String cn = txtCon.Text;

            if (Mode == true)
            {
                sql = "insert into Member(mname,gen,con) values(@mname,@gen,@con)";
                con.Open();
                cmd=new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@mname",name);
                cmd.Parameters.AddWithValue("@gen", Gen);
                cmd.Parameters.AddWithValue("@con",cn);
                MessageBox.Show("Record added");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCon.Clear();
                textgen.Clear();
                txtName.Focus();
            }
            else
            {
                id = grid.CurrentRow.Cells[0].Value.ToString();
                sql = "Update Member set mname=@mname, gen=@gen, con=@con where id=@id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@mname", name);
                cmd.Parameters.AddWithValue("@gen", Gen);
                cmd.Parameters.AddWithValue("@con", cn);
                cmd.Parameters.AddWithValue("@id", id);
                MessageBox.Show("Record Updated");
                cmd.ExecuteNonQuery();

                txtName.Clear();
                txtCon.Clear();
                textgen.Clear();
                txtName.Focus();
                button1.Text = "Edit";
                Mode = true;
            }
            con.Close();
        }

        private void grid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == grid.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = grid.CurrentRow.Cells[0].Value.ToString();
                getId(id);
                button1.Text = "Edit";
            }
            else if (e.ColumnIndex == grid.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                Mode = false;
                id = grid.CurrentRow.Cells[0].Value.ToString();
                sql = "delete from Member where id=@id";
                con.Open();
                cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id",id);
                MessageBox.Show("Record Deleted");
                con.Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtCon.Clear();
            textgen.Clear();
            txtName.Focus();
            button1.Text = "Edit";
            Mode = true;
        }
    }
}