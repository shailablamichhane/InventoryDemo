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
    public partial class Stock : System.Web.UI.Page
    {
        int stockID;
        int prodID;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack) {
                populateGridView();
                populateProductList();
                populateProductListForSearch();
            }           

        }

        protected void populateGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("getStockDetails", conn);
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                stockGridView.DataSource = dt;
                stockGridView.DataBind();
                sda.Dispose();
                conn.Close();

            }
        }

        protected void removeRow(object sender, GridViewDeleteEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            int id = Convert.ToInt32(stockGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("deleteStock", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@stockID", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            populateGridView();
        }
        protected void editRow(object sender, GridViewEditEventArgs e)
        {
            stockGridView.EditIndex = e.NewEditIndex;
            populateGridView();
        }

        protected void cancelEditingRow(object sender, GridViewCancelEditEventArgs e)
        {
            stockGridView.EditIndex = -1;
            populateGridView();
        }

        protected void updateRow(object sender, GridViewUpdateEventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            TextBox txtInitialQuantity = stockGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtRemainingQuantity = stockGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtStockDate = stockGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;

            int id = Convert.ToInt32(stockGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateStockDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@stockID", id);
            cmd.Parameters.AddWithValue("@stockInitialQuantity", txtInitialQuantity.Text);
            cmd.Parameters.AddWithValue("@stockRemainingQuantity", txtRemainingQuantity.Text);
            cmd.Parameters.AddWithValue("@stockDate", txtStockDate.Text);

            cmd.ExecuteNonQuery();
            conn.Close();
            stockGridView.EditIndex = -1;
            populateGridView();
        }

        public int getID() {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getProductID", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productName", product_select.Value.ToString());        


            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read()) {
                prodID = (Int32)dr.GetValue(0);
            }
            dr.Close();
            return prodID;
            

        }

        public void populateProductList() {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getProductName", conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);

            product_select.DataSource = ds;
            product_select.DataTextField = "name";
            product_select.DataValueField = "name";
            product_select.DataBind();

        }

        public void updateStockManagement(SqlConnection conn) {

            SqlCommand cmd2 = new SqlCommand("insertStockMan", conn);

            stockID = getStockId(conn);

            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@userID", (Int32)Session["userID"]);
            cmd2.Parameters.AddWithValue("@stockID", stockID);

            cmd2.ExecuteNonQuery();
            cmd2.Dispose();

        }


        /**
         * 
         * Method which returns the Stock ID to be updated autoatically in the database.
         * Calls Stored Procedure "getStockId"
         * 
         **/
        public int getStockId(SqlConnection conn) {
            SqlCommand cmd3 = new SqlCommand("getStockId", conn);

            cmd3.CommandType = CommandType.StoredProcedure;

            int productID = getID();

            cmd3.Parameters.AddWithValue("@productID", productID);

            SqlDataReader dataReader = cmd3.ExecuteReader();
            if (dataReader.HasRows) {
                while (dataReader.Read())
                {
                    stockID = (Int32)dataReader.GetValue(0);
                } 
            }
            dataReader.Close();
            return stockID;
        }


        public void btn_AddStock_Click(object sender, EventArgs e) { 
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            int productID = getID();

            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("insertStock", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                DateTime todaysDate = DateTime.Today;
                //productID = product_ID.Value.ToString();
                cmd.Parameters.AddWithValue("@prodID", productID);
                cmd.Parameters.AddWithValue("@initialQuantity", product_quantity.Value.ToString());
                cmd.Parameters.AddWithValue("@remainingQuantity", product_quantity.Value.ToString());
                cmd.Parameters.AddWithValue("@stockDate", todaysDate.ToShortDateString());

                cmd.ExecuteNonQuery();
                cmd.Dispose();

                updateStockManagement(conn);
            }
            catch (SqlException ex) {
                Console.WriteLine(ex.Message);
            }
            finally {
                conn.Close();
            }
        }

        public void populateProductListForSearch()
        {

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getProductName", conn);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.StoredProcedure;

            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            sda.Fill(ds);

            itemListForSearch.DataSource = ds;
            itemListForSearch.DataTextField = "name";
            itemListForSearch.DataValueField = "name";
            itemListForSearch.DataBind();

        }

        //public string populateTable()
        //{
        //    SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
        //    string data = "";
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("getStockDetails", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string product_name = reader.GetString(1);
        //            string initial_quantity = reader.GetValue(2).ToString();
        //            string remaining_quantity = reader.GetValue(3).ToString();
        //            data += "<tr><td>" + id + "</td><td>" + product_name + "</td><td>" + initial_quantity + "</td><td>" + remaining_quantity + "</td></tr>";
        //        }
        //    }
        //    return data;
        //}

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }

        public void btn_SearchByName_Click(object sender, EventArgs ea)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("getStockDetailsByName", conn);
            cmd.Parameters.AddWithValue("@productName", itemListForSearch.Value.ToString());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    prodIDLabel.Text = reader.GetValue(0).ToString();
                    prodNameLabel.Text = reader.GetString(1);
                    stockIDLabel.Text = reader.GetValue(2).ToString();
                    stockDateLabel.Text = reader.GetDateTime(3).ToShortDateString();
                    stockInitialQuantityLabel.Text = reader.GetValue(4).ToString();
                    stockRemainingQuantityLabel.Text = reader.GetValue(5).ToString();
                }
            }
        }
       
    }
}