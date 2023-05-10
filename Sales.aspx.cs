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
    public partial class Sales : System.Web.UI.Page
    {
        int prodID;
        int stokID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack) {
                populateGridView();
                populateProductList();
            }
        }

        protected void populateGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("getSalesDetails", conn);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                salesGridView.DataSource = dt;
                salesGridView.DataBind();
                sda.Dispose();
                conn.Close();

            }
        }

        protected void removeRow(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            int id = Convert.ToInt32(salesGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("deleteSales", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@salesID", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            populateGridView();
        }
        protected void editRow(object sender, GridViewEditEventArgs e)
        {
            salesGridView.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void cancelEditingRow(object sender, GridViewCancelEditEventArgs e)
        {
            salesGridView.EditIndex = -1;
            populateGridView();
        }

        protected void updateRow(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            TextBox txtIssuer = salesGridView.Rows[e.RowIndex].FindControl("TextBox2") as TextBox;
            TextBox txtQuantity = salesGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtxTotal_price = salesGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtDate = salesGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;
            TextBox txtOrderedBy = salesGridView.Rows[e.RowIndex].FindControl("TextBox6") as TextBox;
            TextBox txtProductID = salesGridView.Rows[e.RowIndex].FindControl("TextBox7") as TextBox;
            TextBox txtStockID = salesGridView.Rows[e.RowIndex].FindControl("TextBox8") as TextBox;

            int id = Convert.ToInt32(salesGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateSalesDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@salesID", id);
            cmd.Parameters.AddWithValue("@salesDate", txtDate.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            salesGridView.EditIndex = -1;
            populateGridView();
        }

        public void populateProductList()
        {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getProductName", conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);

            salesProductName.DataSource = ds;
            salesProductName.DataTextField = "name";
            salesProductName.DataValueField = "name";
            salesProductName.DataBind();

        }

        public int getID()
        {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getProductID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productName", salesProductName.Value.ToString());


            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                prodID = (Int32)dr.GetValue(0);
            }
            dr.Close();

            return prodID;
        }

        public int getStockID(int productID)
        {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getStockId", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productID", productID);


            SqlDataReader dataReader = cmd.ExecuteReader();
            if (dataReader.Read())
            {
                stokID = (Int32)dataReader.GetValue(0);
            }

            dataReader.Close();

            return stokID;
        }

        private void updateStock(SqlConnection conn, int quantity, int stockID) {
            SqlCommand cmd = new SqlCommand("updateStock", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@stockID", stockID);
            cmd.Parameters.AddWithValue("@productQuantity", quantity);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public double getProductPrice(int productID, SqlConnection conn)
        {
            double unitPrice = 0;
            SqlCommand cmd = new SqlCommand("getProductPrice", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@productID", productID);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    unitPrice = Convert.ToDouble(reader.GetValue(0));
                }
            }
            reader.Close();
            return unitPrice;
        }

        public void btn_AddSales_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            try
            {

                int productID = getID();
                int stockID = getStockID(productID);
                int quantity = Convert.ToInt32(salesQuantity.Value);

                conn.Open();

                double unit_price = getProductPrice(productID, conn);
                double total_price = unit_price * quantity;

                SqlCommand cmd = new SqlCommand("insertSales", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@totalPrice", total_price);
                cmd.Parameters.AddWithValue("@salesDate", salesDate.Value);
                cmd.Parameters.AddWithValue("@orderedBy", salesOrderedBy.Value.ToString());
                cmd.Parameters.AddWithValue("@productID", productID);
                cmd.Parameters.AddWithValue("@stockID", stockID);
                cmd.Parameters.AddWithValue("@issuedBy", Session["userID"]);
                cmd.ExecuteNonQuery();
                cmd.Dispose();

                updateStock(conn, quantity, stockID);

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
        //    SqlCommand cmd = new SqlCommand("getSalesDetails", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string product_name = reader.GetString(1);
        //            string quantity = reader.GetValue(2).ToString();
        //            string total_price = reader.GetValue(3).ToString();
        //            string sales_date = reader.GetDateTime(4).ToShortDateString();
        //            string cust_name = reader.GetString(5);
        //            string issued_by = reader.GetString(6);
        //            string stock_id = reader.GetValue(7).ToString();
        //            data += "<tr><td>" + id + "</td><td>" + product_name + "</td><td>" + quantity + "</td><td>" + total_price + "</td><td>" + sales_date + "</td><td>" + cust_name + "</td><td>" + issued_by + "</td><td>" + stock_id + "</td></tr>";
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