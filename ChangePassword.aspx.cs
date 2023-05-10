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
    public partial class ChangePassword : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            error_label.Text = "";
        }

        public void btn_changePassword_Click(object sender, EventArgs e)
        {
            int userIdentity = Convert.ToInt16(Session["userID"]);
            string password = null;
            string oldPassword = old_password.Value.ToString();
            string newPassword = new_password.Value.ToString();
            string confirmPassword = confirm_password.Value.ToString();
            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");

            conn.Open();

            SqlCommand cmd = new SqlCommand("getUserPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", Session["userID"].ToString());
            SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    password = reader.GetString(0);
                }
                reader.Close();

                if (password.Equals(oldPassword))
                {
                    if (newPassword.Equals(confirmPassword))
                    {
                        SqlCommand command = new SqlCommand("updatePassword", conn);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@newPassword", confirmPassword);
                        command.Parameters.AddWithValue("@user_id", userIdentity);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        error_label.Text = "Please enter matching password in new password and confirm password field!";
                    }
                }

                else
                {
                    error_label.Text = "Please enter your current password correctly!";
                }
                reader.Close();
            }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect("Login.aspx");
        }
    }
}