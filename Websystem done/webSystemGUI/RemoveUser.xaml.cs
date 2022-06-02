using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for RemoveUser.xaml
    /// </summary>
    public partial class RemoveUser : UserControl
    {
        public RemoveUser()
        {
            InitializeComponent();
        }

        private void removeLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            /*grabs the URL placed in the textBox*/
            String user = userTxt.Text;

            try
            {
                /*opends a connection to the database and then excutes a query that looks up to see if the link exists 
                 * then askes the user to confirm the deletion of the link*/
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand deleteUser = new SqlCommand(
                    "SELECT dbo.users.email FROM users WHERE dbo.users.email = @user ", con);

                deleteUser.Parameters.AddWithValue("@user", user);

                SqlDataReader reader = deleteUser.ExecuteReader();
                /*sees if link exists*/
                if (reader.HasRows)
                {
                    /*moves the reader point forwards by one*/
                    reader.Read();
                    ConfirmBox confirmBox = new ConfirmBox((String)reader[0], 1);
                    confirmBox.ShowDialog();
                }
                else
                {
                    MessageBox.Show("User was not found.");
                }
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
        }

        private void closedEvent()
        {

        }

        private void findLinkBtn_Click(object sender, RoutedEventArgs e)
        {

            FindResults lookupUser = new FindResults(userTxt.Text, 2);
            lookupUser.ResultIsReady += new EventHandler(GetSelectedUser);
            lookupUser.ShowDialog();

        }

        private void GetSelectedUser(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                userTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    userTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                    fc.Close();
                }
            }
        }
    }
}
