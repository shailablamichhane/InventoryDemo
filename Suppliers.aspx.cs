using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CellStockManagement
{
    public partial class Suppliers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
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
                SqlCommand cmd = new SqlCommand("getSupplierDetails", conn);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                supplierGridView.DataSource = dt;
                supplierGridView.DataBind();
                sda.Dispose();
                conn.Close();

            }
        }

        protected void removeRow(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            int id = Convert.ToInt32(supplierGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("deleteSupplier", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@suppID", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            populateGridView();
        }
        protected void editRow(object sender, GridViewEditEventArgs e)
        {
            supplierGridView.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void cancelEditingRow(object sender, GridViewCancelEditEventArgs e)
        {
            supplierGridView.EditIndex = -1;
            populateGridView();
        }

        protected void updateRow(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            TextBox txtID = supplierGridView.Rows[e.RowIndex].FindControl("TextBox1") as TextBox;
            TextBox txtName = supplierGridView.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
            TextBox txtEmail = supplierGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtAddress = supplierGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtPhoneNo = supplierGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;

            int id = Convert.ToInt32(supplierGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateSupplierDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@suppId", id);
            cmd.Parameters.AddWithValue("@suppName", txtName.Text);
            cmd.Parameters.AddWithValue("@suppEmail", txtEmail.Text);
            cmd.Parameters.AddWithValue("@suppAddress", txtAddress.Text);
            cmd.Parameters.AddWithValue("@suppPhoneNo", txtPhoneNo.Text);

            int i = cmd.ExecuteNonQuery();
            conn.Close();
            supplierGridView.EditIndex = -1;
            populateGridView();
        }

        public void btn_addSupp_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("insertSupp", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@supplierName", suppName.Value.ToString());
                cmd.Parameters.AddWithValue("@supplierEmail", suppMail.Value.ToString());
                cmd.Parameters.AddWithValue("@supplierPhone", suppPhone.Value.ToString());
                cmd.Parameters.AddWithValue("@supplierAddress", suppAddress.Value.ToString());
                cmd.Parameters.AddWithValue("@supplierAddedBy", Session["userID"].ToString());
                cmd.ExecuteNonQuery();
                cmd.Dispose();

            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        //public string populateTable()
        //{
        //    SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
        //    string data = "";
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("getSupplierDetails", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string supp_name = reader.GetString(1);
        //            string email = reader.GetString(2);
        //            string address = reader.GetString(3);
        //            string phone_no = reader.GetString(4);
        //            data += "<tr><td>" + id + "</td><td>" + supp_name + "</td><td>" + email + "</td><td>" + address + "</td><td>" + phone_no + "</td></tr>";
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