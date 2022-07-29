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
using System.Windows.Shapes;

namespace DataBase_Demo
{
    /// <summary>
    /// Interaction logic for afisare_useri.xaml
    /// </summary>
    public partial class afisare_useri : Window
    {

        public afisare_useri()
        {
            InitializeComponent();
            Loaded += MyWindow_Loaded;
        }

        int i;
        DataSet d, ds;
        SqlConnection sql;
        string user;

        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            string data = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=AppUsers;Integrated Security=True";
            sql = new SqlConnection(data);

            SqlCommand cmd = new SqlCommand("showInfoUser", sql);
            cmd.CommandType = CommandType.StoredProcedure;

            sql.Open();

            cmd.ExecuteNonQuery();

            SqlDataAdapter da = new SqlDataAdapter(cmd);

            d = new DataSet();
            da.Fill(d);
            sql.Close();

            Button button = new Button();
            button.Content = "Schimba adresa pentru ";

            for (i = 0; i < d.Tables[0].Rows.Count; i++)
            { 
                Label l = new Label()
                {
                    Content = d.Tables[0].Rows[i][1].ToString(),
                    
                };
                l.MouseDown += (st, ev) =>
                {

                    //GET USER ID AND ADDRESS
                    SqlCommand comd = new SqlCommand("showID_Address", sql);
                    comd.CommandType = CommandType.StoredProcedure;
                    SqlParameter p1 = new SqlParameter("user", l.Content);
                    user = l.Content.ToString();
                    comd.Parameters.Add(p1);

                    sql.Open();
                    comd.ExecuteNonQuery();

                    SqlDataAdapter dat = new SqlDataAdapter(comd);

                    ds = new DataSet();
                    dat.Fill(ds);
                    sql.Close();

                    label_address.Content = "Adresa : ";
                    label_address.Content += ds.Tables[0].Rows[0][1].ToString();

                    sql.Close();

                    button.Content = "Schimba adresa pentru " + user;

                };
                stack.Children.Add(l);
            }


            TextBox textBox = new TextBox();
            stack.Children.Add(textBox);
            textBox.Margin = new Thickness(10, 20, 10, 0);

            stack.Children.Add(button);
            button.Margin = new Thickness(50, 10, 50, 0);

            button.Click += (st, ev) =>
            {
                //CHANGE USER ADDRESS

                sql.Open();
                SqlCommand comd1 = new SqlCommand("changeAddress", sql);
                comd1.CommandType = CommandType.StoredProcedure;
                if (user != "")
                {
                    SqlParameter p11 = new SqlParameter("new_address", textBox.Text);
                    SqlParameter p22 = new SqlParameter("user_id", ds.Tables[0].Rows[0][0]);
                    comd1.Parameters.Add(p11);
                    comd1.Parameters.Add(p22);
                    comd1.ExecuteNonQuery();

                    label_address.Content = "Adresa : ";
                    label_address.Content += textBox.Text;

                    MessageBox.Show("Adresa schimbata cu succes!");
                    textBox.Text = "";
                }
                else
                {
                    MessageBox.Show("Adresa nu poate fi nula.");
                }
                sql.Close();
            };
        }

    }
}
