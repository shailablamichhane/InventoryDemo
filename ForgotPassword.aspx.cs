using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net.Mail;
using System.Net;

namespace CellStockManagement
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btn_forgotPassword_Click(object sender, EventArgs e)
        {
            string userName;
            string userEmail;
            string userPassword;

            SqlConnection conn = new SqlConnection(@"Data Source=GUNJAN\GKSERVER;Initial Catalog=cellstock;Integrated Security=True");
            conn.Open();

            SqlCommand cmd = new SqlCommand("getUserForPassword", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@username", forgot_username.Value.ToString());
            cmd.Parameters.AddWithValue("@userEmail", forgot_email.Value.ToString());
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    userName = reader.GetString(1);
                    userEmail = reader.GetString(2);
                    userPassword = reader.GetString(3);
                    sendPassword(userPassword, userEmail);
                    validation_label.Text = "Your Password has been sent to your email!";
                }
            }

            else
            {
                validation_label.Text = "No such username or email found!!";
            }
        }

        public void sendPassword(string password, string email)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new System.Net.NetworkCredential("yeehaw.corp17@gmail.com", "programmersaregod;");
            smtp.EnableSsl = true;
            MailMessage msg = new MailMessage();
            msg.Subject = "Password Reset Request - CellStock";
            msg.Body = "Dear " + forgot_username.Value.ToString() + ", your current password is " + password + " Thank You! ";
            string sentToAddress = forgot_email.Value.ToString();
            msg.To.Add(sentToAddress);
            string hostAddress = "Cell Stock Management <sailablamichhane@gmail.com>";
            msg.From = new MailAddress(hostAddress);
            try
            {
                smtp.Send(msg);
            }

            catch
            {
                throw;
            }
        }
    }
}