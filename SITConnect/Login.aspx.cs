using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void login_btn_Click(object sender, EventArgs e)
        {
            bool mt = false;
            bool pass;

            if (String.IsNullOrWhiteSpace(email_tb.Text) || String.IsNullOrWhiteSpace(pwd_tb.Text))
            {
                error_lb.Text = "Please fill all fields";
                mt = true;
            }

            if (!mt)
            {
                DBServiceReference1.Service1Client client = new DBServiceReference1.Service1Client();
                var user = client.SelectByEmail(email_tb.Text.Trim());

                if (user == null)
                {
                    error_lb.Text = "Invalid credentials pp";
                    pass = false;
                } 
                else
                {
                    var suspended = client.CheckSuspended(user.Email);
                    if (suspended)
                    {
                        int span = 30 - Convert.ToInt16(DateTime.Now.Subtract(Convert.ToDateTime(user.Suspended_Since)).TotalMinutes);
                        error_lb.Text = "Your account has been locked. Please wait " + span +" minutes before trying again.";
                        pass = false;
                    } else
                    {
                        string salt = user.Password_Salt;

                        // initializing hashing thingy
                        SHA512Managed hashing = new SHA512Managed();

                        // salting plaintext and hashing after
                        string saltedpw = pwd_tb.Text.Trim() + salt;
                        string hashedpw = Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(saltedpw)));

                        if (pwd_tb.Text.Trim() == user.Password)
                        {
                            client.CheckAttempts(user.Email, true);
                            pass = true;
                        } else
                        {
                            client.CheckAttempts(user.Email, false);
                            error_lb.Text = "Invalid credentials";
                            pass = false;
                        }
                    }
                }

                if (pass && !mt)
                {
                    // log in
                    Session["LoggedIn"] = user.Email;

                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;

                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                    Response.Redirect("Home.aspx");
                }
                
            }
        }
    }
}