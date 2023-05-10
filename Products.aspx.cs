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
    public partial class Products : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            clearSearch();



            //populateTable();
            if (!IsPostBack)
            {
                populateProductList();
                populateGridView();
            }
        }

        protected void populateGridView()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            using (conn)
            {
                SqlCommand cmd = new SqlCommand("getProductDetails", conn);
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
            SqlCommand cmd = new SqlCommand("deleteProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@prodID", id);
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
            TextBox txtMan = productGridView.Rows[e.RowIndex].FindControl("TextBox3") as TextBox;
            TextBox txtRelDate = productGridView.Rows[e.RowIndex].FindControl("TextBox4") as TextBox;
            TextBox txtScreenType = productGridView.Rows[e.RowIndex].FindControl("TextBox5") as TextBox;
            TextBox txtScreenResoultion = productGridView.Rows[e.RowIndex].FindControl("TextBox6") as TextBox;
            TextBox txtCpu = productGridView.Rows[e.RowIndex].FindControl("TextBox7") as TextBox;
            TextBox txtGpu = productGridView.Rows[e.RowIndex].FindControl("TextBox8") as TextBox;
            TextBox txtPrice = productGridView.Rows[e.RowIndex].FindControl("TextBox9") as TextBox;

            int id = Convert.ToInt32(productGridView.DataKeys[e.RowIndex].Values["id"].ToString());
            conn.Open();
            SqlCommand cmd = new SqlCommand("updateProductDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@prodID", id);
            cmd.Parameters.AddWithValue("@prodName", txtName.Text);
            cmd.Parameters.AddWithValue("@prodManufacturer", txtMan.Text);
            cmd.Parameters.AddWithValue("@prodRelDate", txtRelDate.Text);
            cmd.Parameters.AddWithValue("@screenType", txtScreenType.Text);
            cmd.Parameters.AddWithValue("@screenResolution", txtScreenResoultion.Text);
            cmd.Parameters.AddWithValue("@cpu", txtCpu.Text);
            cmd.Parameters.AddWithValue("@gpu", txtGpu.Text);
            cmd.Parameters.AddWithValue("@unit_price", Convert.ToInt32(txtPrice.Text));

            cmd.ExecuteNonQuery();
            conn.Close();
            productGridView.EditIndex = -1;
            populateGridView();
        }

        public void btn_AddProduct_Click(object sender, EventArgs e)
        {
                SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
                try
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("insertProduct", conn);
                    SqlCommand cmd2 = new SqlCommand("updateOrder", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productName", prod_name.Value.ToString());
                    cmd.Parameters.AddWithValue("@productManufacturer", prod_manufacturer.Value.ToString());
                    cmd.Parameters.AddWithValue("@productScreenRes", prod_screen_res.Value.ToString());
                    cmd.Parameters.AddWithValue("@productScreenType", prod_screen_type.Value.ToString());
                    cmd.Parameters.AddWithValue("@productReleaseDate", prod_release_date.Value.ToString());
                    cmd.Parameters.AddWithValue("@productGPU", prod_gpu.Value.ToString());
                    cmd.Parameters.AddWithValue("@productCPU", prod_cpu.Value.ToString());
                    cmd.Parameters.AddWithValue("@productPrice", prod_price.Value.ToString());
                    cmd.Parameters.AddWithValue("@productAddedBy", Session["UserID"].ToString());
                    cmd2.Parameters.AddWithValue("@productSuppliedBy", prod_supplied_by.Value.ToString());
                    cmd.ExecuteNonQuery();
                    cmd2.ExecuteNonQuery();
                    cmd.Dispose();
                    cmd2.Dispose();

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

        public void btn_SearchByName_Click(object sender, EventArgs ea)
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("getProductDetailsByName", conn);
            cmd.Parameters.AddWithValue("@productName", itemListForSearch.Value.ToString());
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    idLabel.Text = reader.GetValue(0).ToString();
                    nameLabel.Text = reader.GetString(1);
                    manufacturerLabel.Text = reader.GetString(2);
                    resolutionLabel.Text = reader.GetString(3);
                    screenTypeLabel.Text = reader.GetString(4);
                    relDateLabel.Text = reader.GetDateTime(5).ToShortDateString();
                    gpuLabel.Text = reader.GetString(6);
                    cpuLabel.Text = reader.GetString(7);
                    unitPriceLabel.Text = reader.GetValue(8).ToString();
                    addedByLabel.Text = reader.GetValue(9).ToString();
                }
            }
        }

        //public string populateTable()
        //{
        //    SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
        //    string data = "";
        //    conn.Open();
        //    SqlCommand cmd = new SqlCommand("getProductDetails", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    SqlDataReader reader = cmd.ExecuteReader();

        //    if (reader.HasRows)
        //    {
        //        while (reader.Read())
        //        {
        //            string id = reader.GetValue(0).ToString();
        //            string name = reader.GetString(1);
        //            string manufacturer = reader.GetString(2);
        //            string screen_resolution = reader.GetString(3);
        //            string screen_type = reader.GetString(4);
        //            string release_date = reader.GetDateTime(5).ToShortDateString();
        //            string gpu = reader.GetString(6);
        //            string cpu = reader.GetString(7);
        //            string unit_price = reader.GetValue(8).ToString();
        //            string added_by = reader.GetString(9);
        //            data += "<tr><td>" + id + "</td><td>" + name + "</td><td>" + manufacturer + "</td><td>" + release_date +  "</td><td>" + screen_type + "</td><td>" + screen_resolution + "</td><td>" + cpu + "</td><td>" + gpu + "</td><td>" + unit_price + "</td><td>" + added_by + "</td></tr>";
        //        }
        //    }
        //    return data;
        //}

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

            itemListForSearch.DataSource = ds;
            itemListForSearch.DataTextField = "name";
            itemListForSearch.DataValueField = "name";
            itemListForSearch.DataBind();

        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }

        protected void clearSearch()
        {
            idLabel.Text = "";
            nameLabel.Text = "";
            manufacturerLabel.Text = "";
            resolutionLabel.Text = "";
            screenTypeLabel.Text = "";
            relDateLabel.Text = "";
            gpuLabel.Text = "";
            cpuLabel.Text = "";
            unitPriceLabel.Text = "";
            addedByLabel.Text = "";
        }
    }
}