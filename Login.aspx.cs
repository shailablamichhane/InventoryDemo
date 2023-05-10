using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CellStockManagement
{
    public partial class Login : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
        public string name;
        public string pass;
        public int user_id;
        public string user_type;

        protected void Page_Load(object sender, EventArgs e)
        {
          if (Session["username"] != null) {
                message_label.Text = "You are already logged in as " + Session["username"] + ". You will be redirected in 5 seconds.";
                Response.AddHeader("REFRESH", "5;URL=Dashboard.aspx");
            }
            else { 
                Session.RemoveAll();
                message_label.Text = "";
            }
            
        }

        public void GetUser()
        {
            SqlParameter userName_parameter = new SqlParameter("@user_name", name);
            SqlCommand cmd1 = new SqlCommand("getUserType", conn);
            cmd1.Parameters.Add(userName_parameter);
            cmd1.CommandType = CommandType.StoredProcedure;
            SqlDataReader dataReader = cmd1.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    user_id = (Int32)dataReader.GetValue(0);
                    user_type = dataReader.GetString(1).ToString();
                    Session["userType"] = user_type;
                    Session["userID"] = user_id;
                }
            }
        }

        public void btn_login_Click(object sender, EventArgs e)
        {
            if (Session["username"] != null)
            {
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                name = form_username.Value.ToString().Trim();
                pass = form_password.Value.ToString().Trim();

                SqlParameter name_parameter = new SqlParameter("@userName", name);
                SqlParameter password_parameter = new SqlParameter("@userPassword", pass);
                SqlCommand cmd = new SqlCommand("sp_login", conn);
                cmd.Parameters.Add(name_parameter);
                cmd.Parameters.Add(password_parameter);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Session["username"] = name;
                    dr.Close();
                    GetUser();
                    Response.Redirect("Dashboard.aspx");

                }
                else
                {
                    message_label.Text = "Incorrect User Name or Password!!";
                }
                
                conn.Close();
            }
        }
    }
}