using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;

namespace CellStockManagement
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else if (Session["userType"].ToString().Equals("Staff"))
            {
                Response.Redirect("Dashboard.aspx");
            }

            if (!IsPostBack) 
            {
                populateGridView();
            }
        }

        protected void populateGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("getUserDetails", conn);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                productGridView.DataSource = dt;
                productGridView.DataBind();
                sda.Dispose();
                conn.Close();

            }
        }

        protected void removeRow(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            int id = Convert.ToInt32(productGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("deleteUser", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            populateGridView();
        }
        protected void editRow(object sender, GridViewEditEventArgs e)
        {
            productGridView.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void cancelEditingRow(object sender, GridViewCancelEditEventArgs e)
        {
            productGridView.EditIndex = -1;
            populateGridView();
        }

        protected void updateRow(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            TextBox txtName = productGridView.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
            TextBox txtEmail = productGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtPassword = productGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtUserType = productGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;
            int id = Convert.ToInt32(productGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateUserDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userID", id);
            cmd.Parameters.AddWithValue("@userName", txtName.Text);
            cmd.Parameters.AddWithValue("@userEmail", txtEmail.Text);
            cmd.Parameters.AddWithValue("@userPassword", txtPassword.Text);
            cmd.Parameters.AddWithValue("@userType", txtUserType.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            productGridView.EditIndex = -1;
            populateGridView();
        }

        public void btn_addUser_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            try
            {
                
                conn.Open();

                SqlCommand cmd = new SqlCommand("insertUser", conn);
                //Console.WriteLine(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userName", user_name.Value.ToString());
                cmd.Parameters.AddWithValue("@userEmail", user_email.Value.ToString());
                cmd.Parameters.AddWithValue("@userPassword", user_password.Value.ToString());
                cmd.Parameters.AddWithValue("@userType", user_type.Value.ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally {
                conn.Close();
            }
        }
        //public string populateTable()
        //{
        //    SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
        //    string data = "";
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("getUserDetails", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string user = reader.GetString(1);
        //            string email = reader.GetString(2);
        //            string password = reader.GetString(3);
        //            string user_type = reader.GetString(4);
        //            data += "<tr><td>" + id + "</td><td>" + user + "</td><td>" + email + "</td><td>" + password + "</td><td>" + user_type + "</td></tr>";
        //        }
        //    }
        //    return data;
        //}

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }
    }
}