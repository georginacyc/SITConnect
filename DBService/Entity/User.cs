using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SITConnect.Entity;

namespace DBService1.Entity
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Password_Salt { get; set; }
        public string Password_Last1 { get; set; }
        public string Password_Last2 { get; set; }
        public DateTime Password_Age { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public DateTime Dob { get; set; }
        public string Card_Name { get; set; }
        public string Card_Num { get; set; }
        public string Card_CVV { get; set; }
        public string Card_Expiry { get; set; }
        public byte[] IV { get; set; }
        public byte[] Key { get; set; }
        public int Attempts_Left { get; set; }
        public DateTime? Suspended_Since { get; set; }

        public User()
        {

        }

        public User(string email, string pw, string pwsalt, string fname, string lname, DateTime dob, CreditCard cc, byte[] iv, byte[] key)
        {
            Email = email;
            Password = pw;
            Password_Salt = pwsalt;
            Password_Age = DateTime.Now;
            First_Name = fname;
            Last_Name = lname;
            Dob = dob;
            Card_Name = cc.Name;
            Card_Num = cc.Number;
            Card_CVV = cc.Cvv;
            Card_Expiry = cc.Expiry;
            IV = iv;
            Key = key;
            Attempts_Left = 3;
        }

        public User(string email, string pw, string pwsalt, string pw1, string pw2, DateTime pwage, string fname, string lname, DateTime dob, CreditCard cc, byte[] iv, byte[] key, int attempts_left, DateTime? suspended_since)
        {
            Email = email;
            Password = pw;
            Password_Salt = pwsalt;
            Password_Last1 = pw1;
            Password_Last2 = pw2;
            Password_Age = pwage;
            First_Name = fname;
            Last_Name = lname;
            Dob = dob;
            Card_Name = cc.Name;
            Card_Num = cc.Number;
            Card_CVV = cc.Cvv;
            Card_Expiry = cc.Expiry;
            IV = iv;
            Key = key;
            Attempts_Left = attempts_left;
            Suspended_Since = suspended_since;
        }

        //public User(string email, string pw, string pwsalt, string fname, string lname, DateTime dob, CreditCard cc, byte[] iv, byte[] key)
        //{
        //    Email = email;
        //    Password = pw;
        //    Password_Salt = pwsalt;
        //    First_Name = fname;
        //    Last_Name = lname;
        //    Dob = dob;
        //    IV = iv;
        //    Key = key;
        //    Card = encryptData(cc);
        //}

        public int Insert()
        {
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);

            string query = "INSERT INTO Account (email, password, password_salt, password_age, first_name, last_name, dob, card_name, card_num, card_cvv, card_expiry, iv, ekey, attempts_left) " + "VALUES (@email, @password, @password_salt, @password_age, @first_name, @last_name, @dob, @card_name, @card_num, @card_cvv, @card_expiry, @iv, @key, @attempts_left)";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@email", Email);
            cmd.Parameters.AddWithValue("@password", Password);
            cmd.Parameters.AddWithValue("@password_salt", Password_Salt);
            cmd.Parameters.AddWithValue("@password_age", Password_Age);
            cmd.Parameters.AddWithValue("@first_name", First_Name);
            cmd.Parameters.AddWithValue("@last_name", Last_Name);
            cmd.Parameters.AddWithValue("@dob", Dob);
            cmd.Parameters.AddWithValue("@card_name", Convert.ToBase64String(encryptData(Card_Name)));
            cmd.Parameters.AddWithValue("@card_num", Convert.ToBase64String(encryptData(Card_Num)));
            cmd.Parameters.AddWithValue("@card_cvv", Convert.ToBase64String(encryptData(Card_CVV)));
            cmd.Parameters.AddWithValue("@card_expiry", Convert.ToBase64String(encryptData(Card_Expiry)));
            cmd.Parameters.AddWithValue("@iv", Convert.ToBase64String(IV));
            cmd.Parameters.AddWithValue("@key", Convert.ToBase64String(Key));
            cmd.Parameters.AddWithValue("@attempts_left", Attempts_Left);

            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();

            return result;
        }

        public User SelectByEmail(string email)
        {
            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
            SqlConnection conn = new SqlConnection(connStr);

            string query = "SELECT * FROM Account WHERE email = @email";
            SqlDataAdapter da = new SqlDataAdapter(query, conn);
            da.SelectCommand.Parameters.AddWithValue("@email", email);

            DataSet ds = new DataSet();

            da.Fill(ds);

            User user = null;
            int count = ds.Tables[0].Rows.Count;
            if (count == 1)
            {
                DataRow row = ds.Tables[0].Rows[0];
                string password = row["password"].ToString();
                string password_salt = row["password_salt"].ToString();
                string password_last1 = row["password_last1"].ToString();
                string password_last2 = row["password_last2"].ToString();
                DateTime password_age = Convert.ToDateTime(row["password_age"]);
                string first_name = row["first_name"].ToString();
                string last_name = row["last_name"].ToString();
                DateTime dob = Convert.ToDateTime(row["dob"]).Date;
                byte[] iv = Convert.FromBase64String(row["iv"].ToString());
                byte[] key = Convert.FromBase64String(row["ekey"].ToString());
                string card_name = decryptData(Convert.FromBase64String(row["card_name"].ToString()), iv, key);
                string card_num = decryptData(Convert.FromBase64String(row["card_num"].ToString()), iv, key);
                string card_cvv = decryptData(Convert.FromBase64String(row["card_cvv"].ToString()), iv, key);
                string card_expiry = decryptData(Convert.FromBase64String(row["card_expiry"].ToString()), iv, key);
                int attempts = Convert.ToInt16(row["attempts_left"]);
                DateTime? suspended_since = null;
                if (!DBNull.Value.Equals(row["suspended_since"]))
                {
                    suspended_since = Convert.ToDateTime(row["suspended_since"]);
                }

                user = new User(email, password, password_salt, password_last1, password_last2, password_age, first_name, last_name, dob, new CreditCard(card_name, card_num, card_cvv, card_expiry), iv, key, attempts, suspended_since);
            }
            return user;
        }

        public int ChangePassword(string email, string newpass)
        {
            User user = new User().SelectByEmail(email);
            string currentpass = user.Password;
            string lastpass = user.Password_Last1;

            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);

            string query = "UPDATE Account SET password = @new, password_last1 = @current, password_last2 = @old, password_age = @age WHERE email = @email";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@new", newpass);
            cmd.Parameters.AddWithValue("@current", currentpass);
            cmd.Parameters.AddWithValue("@old", lastpass);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@age", DateTime.Now);

            conn.Open();
            int result = cmd.ExecuteNonQuery();
            conn.Close();

            return result;
        }

        public bool CheckSuspended(string email) // true == suspended, false == not suspended
        {
            User user = new User().SelectByEmail(email);
            
            if (user.Suspended_Since != null)
            {
                int span = Convert.ToInt16(DateTime.Now.Subtract(Convert.ToDateTime(user.Suspended_Since)).TotalMinutes);
                if (span < 30)
                {
                    return true;
                } else
                {
                    if (user.Attempts_Left == 0)
                    {

                        string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

                        SqlConnection conn = new SqlConnection(connStr);

                        string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
                        SqlCommand cmd = new SqlCommand(query, conn);

                        cmd.Parameters.AddWithValue("@attempts_left", 3);
                        cmd.Parameters.AddWithValue("@email", user.Email);

                        //System.Diagnostics.Debug.WriteLine("attempts reset to 3 a ");


                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        return false;
                    }
                    return false;
                }
            } else
            {
                if (user.Attempts_Left == 0)
                {
                    string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

                    SqlConnection conn = new SqlConnection(connStr);

                    string query = "UPDATE Account SET attempts_left = @attempts_left, suspended_since = @since WHERE email = @email";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@attempts_left", 0);
                    cmd.Parameters.AddWithValue("@since", DateTime.Now);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //System.Diagnostics.Debug.WriteLine("suspended");
                    return true;
                }
                return false;
            }
            
        }

        public bool CheckAttempts(string email, bool pass) // true == attempt passed, false == attempt failed
        {

            string connStr = ConfigurationManager.ConnectionStrings["db"].ConnectionString;

            SqlConnection conn = new SqlConnection(connStr);

            User user = new User().SelectByEmail(email);
            int attempts = user.Attempts_Left;
            //System.Diagnostics.Debug.WriteLine(attempts);


            // CHECK IF SUSPENDED
            // if attempts <= 0, check suspended since
            // if suspended since <= 30 min, currently suspended, dont do anything but wait

            // if suspended since > 30 min, reset attempts

            // if attempts > 0, check if pass
            // if pass, return true
            // if fail, minus attempt check if suspended again, and return false

            //if (attempts <= 0)
            //{
            //    if (CheckSuspended(email))
            //    {
            //        return false;
            //    }
            //}

            //user = new User().SelectByEmail(email);
            //attempts = user.Attempts_Left;

            if (pass)
            {
                string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@attempts_left", 3);
                cmd.Parameters.AddWithValue("@email", user.Email);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                //System.Diagnostics.Debug.WriteLine("attempts reset to 3 b ");

                return true;
            } else
            {
                attempts -= 1;

                if (attempts > 0)
                {
                    string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@attempts_left", attempts);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //System.Diagnostics.Debug.WriteLine("attempts minused 1 to become " + attempts.ToString());
                    return false;
                } else
                {
                    string query = "UPDATE Account SET attempts_left = @attempts_left, suspended_since = @since WHERE email = @email";
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@attempts_left", 0);
                    cmd.Parameters.AddWithValue("@since", DateTime.Now);
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    //System.Diagnostics.Debug.WriteLine("suspended");
                    return false;
                }
            }


            

            //if (attempts > 0)
            //{
            //    if (!pass)
            //    {
            //        string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
            //        SqlCommand cmd = new SqlCommand(query, conn);

            //        cmd.Parameters.AddWithValue("@attempts_left", attempts);
            //        cmd.Parameters.AddWithValue("@email", user.Email);

            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //        conn.Close();

            //        //return "not suspended, no login";
            //        return true;
            //    }
            //} else if (attempts <= 0)
            //{
            //    string query = "UPDATE Account SET attempts_left = @attempts_left, suspended_since = @suspended_since WHERE email = @email";
            //    SqlCommand cmd = new SqlCommand(query, conn);

            //    cmd.Parameters.AddWithValue("@attempts_left", attempts);
            //    cmd.Parameters.AddWithValue("@suspended_since", DateTime.Now);
            //    cmd.Parameters.AddWithValue("@email", user.Email);


            //    conn.Open();
            //    cmd.ExecuteNonQuery();
            //    conn.Close();

            //    //return "just suspended, no login";
            //    return false;
            //}

            //if (pass)
            //{
            //    if (user.Suspended_Since == null)
            //    {
            //        //return "not suspended, login";
            //        return true;
            //    }
            //    else
            //    {
            //        int span = Convert.ToInt16(DateTime.Now.Subtract(DateTime.Now).TotalMinutes);
            //        if (span > 30)
            //        {
            //            //return "not suspended, login";
            //            string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
            //            SqlCommand cmd = new SqlCommand(query, conn);

            //            cmd.Parameters.AddWithValue("@attempts_left", 5);
            //            cmd.Parameters.AddWithValue("@email", user.Email);

            //            conn.Open();
            //            cmd.ExecuteNonQuery();
            //            conn.Close();
            //            return true;
            //        }
            //        else
            //        {
            //            //return "suspended, no login";
            //            return false;
            //        }
            //    }
            //}
            //else
            //{
            //    if (attempts > 0)
            //    {
            //        string query = "UPDATE Account SET attempts_left = @attempts_left WHERE email = @email";
            //        SqlCommand cmd = new SqlCommand(query, conn);

            //        cmd.Parameters.AddWithValue("@attempts_left", attempts);
            //        cmd.Parameters.AddWithValue("@email", user.Email);

            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //        conn.Close();

            //        //return "not suspended, no login";
            //        return true;
            //    }
            //    else if (attempts <= 0)
            //    {
            //        string query = "UPDATE Account SET attempts_left = @attempts_left, suspended_since = @suspended_since WHERE email = @email";
            //        SqlCommand cmd = new SqlCommand(query, conn);

            //        cmd.Parameters.AddWithValue("@attempts_left", attempts);
            //        cmd.Parameters.AddWithValue("@suspended_since", DateTime.Now);
            //        cmd.Parameters.AddWithValue("@email", user.Email);


            //        conn.Open();
            //        cmd.ExecuteNonQuery();
            //        conn.Close();

            //        //return "just suspended, no login";
            //        return false;
            //    }
            //}

            //return false;            
        }

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {

            }
            return cipherText;
        }

        //protected byte[] encryptData(DateTime data)
        //{
        //    byte[] cipherText = null;
        //    try
        //    {

        //        RijndaelManaged cipher = new RijndaelManaged();
        //        cipher.IV = IV;
        //        cipher.Key = Key;
        //        ICryptoTransform encryptTransform = cipher.CreateEncryptor();
        //        byte[] plainText = BitConverter.GetBytes(data.Ticks); // new DateTime(ticks) to get back date
        //        cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    finally
        //    {

        //    }
        //    return cipherText;
        //}

        protected string decryptData(byte[] ciphertext, byte[] iv, byte[] key)
        {
            string plaintext = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = iv;
                cipher.Key = key;

                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plaintext;
        }
        //protected DateTime decryptDate(byte[] ciphertext, byte[] iv, byte[] key)
        //{
        //    DateTime plaintext = DateTime.Now.Date;

        //    try
        //    {
        //        RijndaelManaged cipher = new RijndaelManaged();
        //        cipher.IV = iv;
        //        cipher.Key = key;

        //        ICryptoTransform decryptTransform = cipher.CreateDecryptor();
        //        using (MemoryStream msDecrypt = new MemoryStream(ciphertext))
        //        {
        //            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
        //            {
        //                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        //                {
        //                    string tickstr = srDecrypt.ReadToEnd();
        //                    long ticks = long.Parse(tickstr);
        //                    long test = 8765435634654;
        //                    plaintext = new DateTime(1000000);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }
        //    finally { }
        //    return plaintext;
        //}
    }
}
