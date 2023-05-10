using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CellStockManagement
{
    public partial class Customers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack) {
                populateGridView();
                populateCustomerList();
            }
        }

        protected void populateGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("getCustomerDetails", conn);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                customerGridView.DataSource = dt;
                customerGridView.DataBind();
                sda.Dispose();
                conn.Close();

            }
        }

        protected void btn_SearchByCustName_Click(object sender, EventArgs e)
        {
            populateSalesGridView();
        }

        protected void populateSalesGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand command = new SqlCommand("getSalesDetailsByCustName", conn);
                command.Parameters.AddWithValue("@custName", customerListForSearch.Value.ToString());
                command.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dTable = new DataTable();
                da.Fill(dTable);
                CustSalesGridView.DataSource = dTable;
                CustSalesGridView.DataBind();
                da.Dispose();
                conn.Close();

            }
        }

        protected void removeRow(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            int id = Convert.ToInt32(customerGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("deleteCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@custID", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            populateGridView();
        }
        protected void editRow(object sender, GridViewEditEventArgs e)
        {
            customerGridView.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void cancelEditingRow(object sender, GridViewCancelEditEventArgs e)
        {
            customerGridView.EditIndex = -1;
            populateGridView();
        }

        protected void updateRow(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            TextBox txtID = customerGridView.Rows[e.RowIndex].FindControl("TextBox1") as TextBox;
            TextBox txtName = customerGridView.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
            TextBox txtEmail = customerGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtAddress = customerGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtPhoneNo = customerGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;
            TextBox txtAddedBy = customerGridView.Rows[e.RowIndex].FindControl("TextBox6") as TextBox;

            int id = Convert.ToInt32(customerGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateCustomerDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@custID", id);
            cmd.Parameters.AddWithValue("@custName", txtName.Text);
            cmd.Parameters.AddWithValue("@custEmail", txtEmail.Text);
            cmd.Parameters.AddWithValue("@custAddress", txtAddress.Text);
            cmd.Parameters.AddWithValue("@custPhoneNo", txtPhoneNo.Text);

            int i = cmd.ExecuteNonQuery();
            conn.Close();
            customerGridView.EditIndex = -1;
            populateGridView();
        }

        public void btn_AddCust_Click(object sender, EventArgs e) { 
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            try
            {
                
                conn.Open();

                SqlCommand cmd = new SqlCommand("insertCustomer", conn);
                Console.WriteLine(cmd);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@custName", cust_name.Value.ToString());
                cmd.Parameters.AddWithValue("@custEmail", cust_email.Value.ToString());
                cmd.Parameters.AddWithValue("@custAddress", cust_address.Value.ToString());
                cmd.Parameters.AddWithValue("@custPhone", cust_phone.Value.ToString());
                cmd.Parameters.AddWithValue("@custAddedBy", Session["userID"].ToString());
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
        //    SqlCommand cmd = new SqlCommand("getSalesDetailsByCustName", conn);
        //    cmd.Parameters.AddWithValue("@custName", customerListForSearch.Value.ToString());
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string cust_name = reader.GetString(1);
        //            string email = reader.GetString(2);
        //            string address = reader.GetString(3);
        //            string phone_no = reader.GetString(4);
        //            data += "<tr><td>" + id + "</td><td>" + cust_name + "</td><td>" + email + "</td><td>" + address + "</td><td>" + phone_no + "</td></tr>";
        //        }
        //    }
        //    return data;
        //}

        public void populateCustomerList()
        {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getCustomerName", conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);

            customerListForSearch.DataSource = ds;
            customerListForSearch.DataTextField = "name";
            customerListForSearch.DataValueField = "name";
            customerListForSearch.DataBind();

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }
    }
}