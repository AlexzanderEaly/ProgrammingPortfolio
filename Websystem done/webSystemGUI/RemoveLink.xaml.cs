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
    /// Interaction logic for RemoveLink.xaml
    /// </summary>
    public partial class RemoveLink : UserControl
    {
        public RemoveLink()
        {
            InitializeComponent();
        }

        /*This will pop up a smaller box and allow the user to search for links stored in the database
         after the user searches for the link and selects one the box should close and auto fill the input box
        lable linkTxt*/
        private void findLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupLinks = new FindResults(linkTxt.Text, 0);

            /*This tells this form that when it recives the signal GetSelectedLink which is just a methods being called
             on some other form it should run the function listed inside the EventHandler()*/
            lookupLinks.ResultIsReady += new EventHandler(GetSelectedLink);
            lookupLinks.Show();

        }

        /*This pulls the currently selected item from the pop up form and closes*/
        private void GetSelectedLink(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc.resultsListBox.SelectedItem != null)
            {
                linkTxt.Text = "";
                linkTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                fc.Close();
            }
        }

        /*This will delete the link record looked up by the URL and then notifiy the user that its been deleted.*/
        private void removeLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            /*grabs the URL placed in the textBox*/
            String link = linkTxt.Text;

            try
            {
                /*opends a connection to the database and then excutes a query that looks up to see if the link exists 
                 * then askes the user to confirm the deletion of the link*/
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand deleteLink = new SqlCommand(
                    "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);

                deleteLink.Parameters.AddWithValue("@link", link);

                SqlDataReader reader = deleteLink.ExecuteReader();
                /*sees if link exists*/
                if (reader.HasRows)
                {
                    /*moves the reader point forwards by one*/
                    reader.Read();
                    ConfirmBox confirmBox = new ConfirmBox((String)reader[0], 0);
                    reader.Close();
                    con.Close();
                    confirmBox.ShowDialog();
                    linkTxt.Text = "";
                }
                else
                {
                    MessageBox.Show("Link was not found.");
                    reader.Close();
                    con.Close();

                }
            }
            catch
            {
                MessageBox.Show("Database is down");
            }
        }
    }
}
