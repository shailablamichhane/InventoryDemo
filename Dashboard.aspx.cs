using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CellStockManagement
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClientScript.GetPostBackEventReference(this, string.Empty);

            if (Session["username"] == null) {
                Response.Redirect("Login.aspx");
            }
            Session["lowOnStock"] = false;
            getItemsLowOnStock();
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }

        protected string populateOutOfStockTable()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            string data = "";
            conn.Open();
            SqlCommand cmd = new SqlCommand("getItemsOutOfStock", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string stock_id = reader.GetValue(0).ToString();
                    string product_id = reader.GetValue(1).ToString();
                    string product_name = reader.GetString(2);
                    string remaining_quantity = reader.GetValue(3).ToString();
                    data += "<tr><td>" + stock_id + "</td><td>" + product_id + "</td><td>" + product_name + "</td><td class='text-right'>" + remaining_quantity + "</td></tr>";
                }
                return data;
            }
            else {
                return "<center><p class='text-muted text-center'>No items currently out of stock.</p></center>";
            }
        }

        protected string getItemsLowOnStock()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            string data = "";
            
            conn.Open();
            SqlCommand cmd = new SqlCommand("getItemsLowOnStock", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                Session["lowOnStock"] = true;
                while (reader.Read())
                {
                    string product_name = reader.GetString(0);
                    data += product_name + "<br/>";
                }
                
                return data;
            }
            else
            {
                return "Nothing low on stock!";
            }
        }

        //protected void btnChangePassword_Click(object sender, EventArgs e) {
        //    if (Session["userType"].ToString().Equals("Admin")) {
        //        Response.Redirect("Users.aspx");
        //    }
        //    else
        //    {
        //        Response.Redirect("ChangePassword.aspx");
        //    }
        //}
        protected string populateInactiveCustomerTable()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            string data = "";
            conn.Open();
            SqlCommand cmd = new SqlCommand("getInactiveCustomers", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string cust_id = reader.GetValue(0).ToString();
                    string customer_name = reader.GetString(1);
                    string phone_no = reader.GetString(2);
                    string cust_email = reader.GetValue(3).ToString();
                    data += "<tr><td>" + cust_id + "</td><td>" + customer_name + "</td><td>" + phone_no + "</td><td class='text-right'>" + cust_email + "</td></tr>";
                }
                return data;
            }
            else
            {
                return "<h6 class='text-muted text-center'>No items to list.</h6>";
            }
        }

        protected string populateUnpopularItemsTable()
        {
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            string data = "";
            conn.Open();
            SqlCommand cmd = new SqlCommand("getUnpopularItems", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string product_id = reader.GetValue(0).ToString();
                    string product_name = reader.GetString(1);
                    string lastSales_date = reader.GetDateTime(2).ToShortDateString();
                    data += "<tr><td>" + product_id + "</td><td>" + product_name + "</td><td class='text-right'>" + lastSales_date + "</td></tr>";
                }
                return data;
            }
            else
            {
                return "<h6 class='text-muted text-center'>No items to list.</h6>";
            }
        }
    }
}