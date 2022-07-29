using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DataBase_Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly parola parola;

        string data = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=AppUsers;Integrated Security=True";
        public static int USERID = 0;
        public static string user_name = "";

        public MainWindow()
        {
            InitializeComponent();
            parola = new parola();
            this.DataContext = parola;
        }

        private void button_check_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_username.Text != "")
            {
                SqlConnection sql = new SqlConnection(data);

                SqlCommand com = new SqlCommand("existance", sql);
                com.CommandType = CommandType.StoredProcedure;
                SqlParameter user = new SqlParameter("user", textbox_username.Text);
                com.Parameters.Add(user);
                sql.Open();
                SqlDataReader rd = com.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    MessageBox.Show("Username Already Exists!");
                }
                else
                {
                    rd.Read();
                    MessageBox.Show("Username available!");
                }
            }
            else
            {
                MessageBox.Show("Username cannot be blank!");
            }
        }

        private void checkbox_showpass_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(passwordbox_password.Password);
        }

        private void button_register_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_username.Text != "" && passwordbox_password.Password != "")
            {
                try
                {
                    SqlConnection sql = new SqlConnection(data);

                    //AddUser
                    SqlCommand com = new SqlCommand("addUser", sql);
                    com.CommandType = CommandType.StoredProcedure;
                    SqlParameter user = new SqlParameter("user", textbox_username.Text);
                    SqlParameter pass = new SqlParameter("password", hash.getHashSha256(passwordbox_password.Password));
                    com.Parameters.Add(user);
                    com.Parameters.Add(pass);

                    //UserExist
                    SqlCommand exist = new SqlCommand("existance", sql);
                    exist.CommandType = CommandType.StoredProcedure;
                    SqlParameter user_exist = new SqlParameter("user", textbox_username.Text);
                    exist.Parameters.Add(user_exist);

                    sql.Open();
                    SqlDataReader rd = exist.ExecuteReader();
                    if (rd.HasRows)
                    {
                        rd.Read();
                        MessageBox.Show("Username Already Exists!");
                        sql.Close();
                    }
                    else
                    {
                        sql.Close();
                        sql.Open();
                        com.ExecuteNonQuery();
                        MessageBox.Show("Registered Succesfully!");
                        sql.Close();

                        SqlCommand cmd = new SqlCommand("CheckUser", sql);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter p1 = new SqlParameter("username", textbox_username.Text);
                        SqlParameter p2 = new SqlParameter("password", hash.getHashSha256(passwordbox_password.Password));
                        cmd.Parameters.Add(p1);
                        cmd.Parameters.Add(p2);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataSet d = new DataSet();
                        da.Fill(d);
                        int x = (int)d.Tables[0].Rows[0][0];
                        USERID = x;

                        adresa adresa = new adresa();
                        adresa.Show();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Neither of the textboxs can be blank!");
            }
        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection sql = new SqlConnection(data);

            SqlCommand com = new SqlCommand("CheckUser", sql);
            com.CommandType = CommandType.StoredProcedure;

            SqlParameter p1 = new SqlParameter("username", textbox_username.Text);
            SqlParameter p2 = new SqlParameter("password", hash.getHashSha256(passwordbox_password.Password));
            com.Parameters.Add(p1);
            com.Parameters.Add(p2);
            sql.Open();

            SqlDataReader rd = com.ExecuteReader();
            if (rd.HasRows)
            {
                rd.Read();
                MessageBox.Show("Login successful.");
                afisare_date afisare_Date = new afisare_date();
                user_name = textbox_username.Text;
                afisare_Date.Show();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
            sql.Close();
        }


    }
}
