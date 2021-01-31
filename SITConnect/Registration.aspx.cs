using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.IO;

namespace SITConnect
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            pw1_tb.Attributes.Add("onkeyup", "pwdChecker();");
            pw2_tb.Attributes.Add("onkeyup", "pwdMatcher();");
            expiry_cc.Attributes.Add("onkeyup", "expiryFormatter();");
            cardno_cc.Attributes.Add("onkeyup", "cardFormatter();");
        }

        protected void register_btn_Click(object sender, EventArgs e)
        {
            error2_lb.Text = "";
            error_lb.Text = "";
            bool pass = true; // overall validation
            bool pmt = false; // personal info empty check
            bool cmt = false; // cc info empty check

            // checking if any fields are empty
            if (String.IsNullOrWhiteSpace(fname_tb.Text) || String.IsNullOrWhiteSpace(lname_tb.Text) || String.IsNullOrWhiteSpace(email_tb.Text) || String.IsNullOrWhiteSpace(dob_tb.Text) || String.IsNullOrWhiteSpace(pw1_tb.Text) || String.IsNullOrWhiteSpace(pw2_tb.Text))
            {
                error_lb.Text = "Please fill all fields. <br>";
                pmt = true;
            }

            if (String.IsNullOrWhiteSpace(name_cc.Text) || String.IsNullOrWhiteSpace(cardno_cc.Text) || String.IsNullOrWhiteSpace(cvv_cc.Text) || String.IsNullOrWhiteSpace(expiry_cc.Text))
            {
                error2_lb.Text = "Please fill all fields. <br>";
                cmt = true;
            }

            if (!pmt)
            {
                // checks if user exists
                DBServiceReference1.Service1Client client = new DBServiceReference1.Service1Client();
                var user = client.SelectByEmail(email_tb.Text.Trim());
                if (user != null)
                {
                    error_lb.Text = error_lb.Text + "User already exists.";
                    pass = false;
                }

                Regex nameRegex = new Regex("[A-Za-z]");
                if (!nameRegex.IsMatch(fname_tb.Text.Trim()) || !nameRegex.IsMatch(lname_tb.Text.Trim()))
                {
                    error_lb.Text = error_lb.Text + "Please input a valid name <br>";
                    pass = false;
                }

                // as long as dob is not today or in the future
                if (Convert.ToDateTime(dob_tb.Text.Trim()) >= DateTime.Now.Date)
                {
                    error_lb.Text = error_lb.Text + "Please input a valid date of birth <br>";
                    pass = false;
                }

                Regex pwRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}");
                if (!pwRegex.IsMatch(pw1_tb.Text.Trim()))
                {
                    error_lb.Text = error_lb.Text + "Please input a password that fulfills all criteria <br>";
                    pass = false;
                }

                if (pw1_tb.Text.Trim() != pw2_tb.Text.Trim())
                {
                    error_lb.Text = error_lb.Text + "Passwords must match <br>";
                    pass = false;
                }
            }

            if (!cmt)
            {
                // validating credit card name is 2 words, number is 16 digits, cvv is 3 digits and date is valid
                Regex nameRegex = new Regex(@"^[A-Za-z]+\s+[A-Za-z]+$");
                if (!nameRegex.IsMatch(name_cc.Text.Trim()))
                {
                    error2_lb.Text = error2_lb.Text + "Please input a valid name <br>";
                    pass = false;
                }

                Regex numRegex = new Regex(@"^([0-9]{4}\s*){4}$");
                if (!numRegex.IsMatch(cardno_cc.Text.Trim()))
                {
                    error2_lb.Text = error2_lb.Text + "Please input a valid card number <br>";
                    pass = false;
                }

                Regex cvvRegex = new Regex("^[0-9]{3}$");
                if (!cvvRegex.IsMatch(cvv_cc.Text.Trim()))
                {
                    error2_lb.Text = error2_lb.Text + "Please input a valid CVV <br>";
                    pass = false;
                }

                Regex expiryRegex = new Regex("^[0-9]{2}[/]{1}[0-9]{2}$");
                if (!expiryRegex.IsMatch(expiry_cc.Text.Trim()))
                {
                    error2_lb.Text = error2_lb.Text + "Please input a valid expiry date <br>";
                    pass = false;
                } else
                {
                    string date = expiry_cc.Text.Trim();
                    string[] split = date.Split('/');
                    DateTime expiry = Convert.ToDateTime("01/" + split[0] + "/20" + split[1]);

                    if (expiry <= DateTime.Now.Date)
                    {
                        error2_lb.Text = error2_lb.Text + "Please check your card's expiry <br>";
                        pass = false;
                    }
                }
            }

            if (pass && !pmt && !cmt)
            {
                DBServiceReference1.Service1Client client = new DBServiceReference1.Service1Client();

                // retrieving data to hash
                string pw = pw1_tb.Text;

                // initializing bytes for salts
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] pwsaltbyte = new byte[8];

                // getting random salt bytes and converting into string
                rng.GetBytes(pwsaltbyte);
                string pwsalt = Convert.ToBase64String(pwsaltbyte);

                // initializing hashing thingy
                SHA512Managed hashing = new SHA512Managed();

                // salting plaintext and hashing after
                string saltedpw = pw.ToString() + pwsalt;
                string hashedpw = Convert.ToBase64String(hashing.ComputeHash(Encoding.UTF8.GetBytes(saltedpw)));

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();

                client.CreateAccount(email_tb.Text.Trim(), hashedpw, pwsalt, fname_tb.Text.Trim(), lname_tb.Text.Trim(), Convert.ToDateTime(dob_tb.Text.Trim()), name_cc.Text.Trim(), cardno_cc.Text.Trim(), cvv_cc.Text.Trim(), expiry_cc.Text.Trim(), cipher.IV, cipher.Key);
                Response.Redirect("Login.aspx");
            }
        }
    }
}