using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected string sourcekey { 
            get {
                StreamReader sr = File.OpenText(Server.MapPath("sitekey.txt"));
                return @"https://www.google.com/recaptcha/api.js?render=" + sr.ReadToEnd();
            } 
        }

        protected string sitekey
        {
            get
            {
                StreamReader sr = File.OpenText(Server.MapPath("sitekey.txt"));
                return sr.ReadToEnd();
            }
        }

        protected string secretkey
        {
            get
            {
                StreamReader sr = File.OpenText(Server.MapPath("secretkey.txt"));
                return @"https://www.google.com/recaptcha/api/siteverify?secret=" + sr.ReadToEnd();
            }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captcha = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(secretkey + "&response=" + captcha);
            try
            {
                using (WebResponse wr = req.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(wr.GetResponseStream()))
                    {
                        string jsonResponse = sr.ReadToEnd();

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }            
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
                        error_lb.Text = "Your account has been locked. Please wait " + span + " minutes before trying again.";
                        pass = false;
                    } else
                    {
                        string salt = user.Password_Salt;

                        // initializing hashing thingy
                        SHA512Managed hashing = new SHA512Managed();

                        // salting plaintext and hashing after
                        string saltedpw = pwd_tb.Text.Trim() + salt;
                        string hashedpw = Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(saltedpw)));

                        if (hashedpw == user.Password)
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

                if (pass && !mt && ValidateCaptcha())
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