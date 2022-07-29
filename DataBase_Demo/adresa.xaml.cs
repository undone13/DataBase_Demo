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
    /// Interaction logic for adresa.xaml
    /// </summary>
    public partial class adresa : Window
    {
        public adresa()
        {
            InitializeComponent();
        }

        private void button_submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string data = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=AppUsers;Integrated Security=True";

                SqlConnection sql = new SqlConnection(data);

                //AddUserAdress
                SqlCommand com = new SqlCommand("userAdressInsert", sql);
                com.CommandType = CommandType.StoredProcedure;
                SqlParameter id = new SqlParameter("val_id", DataBase_Demo.MainWindow.USERID);
                SqlParameter address = new SqlParameter("val_address", textbox_address.Text);
                SqlParameter number = new SqlParameter("val_phone", textbox_pnumber.Text);
                SqlParameter email = new SqlParameter("val_email", textbox_email.Text);

                com.Parameters.Add(id);
                com.Parameters.Add(address);
                com.Parameters.Add(number);
                com.Parameters.Add(email);

                sql.Open();
                com.ExecuteNonQuery();

                MessageBox.Show("Success!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
