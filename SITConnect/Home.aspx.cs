using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    DBServiceReference1.Service1Client client = new DBServiceReference1.Service1Client();
                    var user = client.SelectByEmail(Session["LoggedIn"].ToString());
                    fname_lb.Text = user.First_Name;
                    lname_lb.Text = user.Last_Name;
                    dob_lb.Text = user.Dob.Date.ToString("dd/MM/yyyy");
                    email_lb.Text = user.Email;
                    cardname_lb.Text = user.Card_Name;
                    cardnum_lb.Text = user.Card_Num;
                    cvv_lb.Text = user.Card_CVV;
                    expiry_lb.Text = user.Card_Expiry;
                } else
                {
                    Response.Redirect("Login.aspx");
                }
            } else
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void logout_btn_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();

            Response.Redirect("Login.aspx");

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
        }

        protected void pass_btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
        }
    }
}