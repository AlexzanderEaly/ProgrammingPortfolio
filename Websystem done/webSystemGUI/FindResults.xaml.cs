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
using System.Windows.Shapes;
using System.Data.SqlClient;


namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for FindResults.xaml
    /// </summary>
    /// 


    public partial class FindResults : Window
    {



        /*Makes the event handler be asscisable from other forms
        its also the handler you will call from other forms */
        public event EventHandler ResultIsReady;

        /*here to signal to the parent form that the data is reday*/
        protected virtual void OnChanged(EventArgs e)
        {
            EventHandler eh = ResultIsReady;
            if (eh != null)
            {
                eh(this, e);
            }
        }

        /*this just calls the method OnChanged which sends the signal to let the parent form know that data is reday*/
        private void DoSomethingToChangeData()
        {
            OnChanged(null);
        }

        public FindResults(String item, int type)
        {
            InitializeComponent();

            //gets the connection object ready to connect to the database
            SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

            try
            {
                //connects to the database
                con.Open();


                //the pointer to go through SQL qurey results
                SqlDataReader reader = null;

                switch (type)
                {
                    case 0:
                        SqlCommand findLink = new SqlCommand(
                            "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link LIKE '%" + item + "%'", con);
                        //creats a reader that will go through the query and read each row found
                        reader = findLink.ExecuteReader();
                        addResults(reader);
                        break;

                    case 1:
                        //runs SQL query that finds all links that macth what was typed into the textBox on the parent form
                        SqlCommand findFilter = new SqlCommand(
                            "SELECT DISTINCT WebSystem.dbo.linkOptions.filterParamerters FROM linkOptions WHERE WebSystem.dbo.linkOptions.filterParamerters LIKE '%" + item + "%'", con);
                        //creats a reader that will go through the query and read each row found
                        reader = findFilter.ExecuteReader();
                        addResults(reader);


                        //keywords filter
                        resultsListBox.Items.Add("html *:-soup-contains()");
                        break;

                    case 2:
                        //runs SQL query that finds all links that macth what was typed into the textBox on the parent form
                        SqlCommand findUser = new SqlCommand(
                            "SELECT dbo.users.email FROM users WHERE dbo.users.email LIKE '%" + item + "%'", con);
                        //creats a reader that will go through the query and read each row found
                        reader = findUser.ExecuteReader();
                        addResults(reader);
                        break;

                    case 3:
                        //runs SQL query that finds all links that macth what was typed into the textBox on the parent form
                        SqlCommand findCategory = new SqlCommand(
                            "SELECT dbo.linkSections.secName FROM linkSections WHERE dbo.linkSections.secName LIKE '%" + item + "%'", con);
                        //creats a reader that will go through the query and read each row found
                        reader = findCategory.ExecuteReader();
                        addResults(reader);
                        break;
                    case 4:
                        SqlCommand findCrawlLink = new SqlCommand(
                           "SELECT linkURL FROM linksFound WHERE linksFound.linkURL LIKE '%" + item + "%'", con);
                        //creats a reader that will go through the query and read each row found
                        reader = findCrawlLink.ExecuteReader();
                        addResults(reader);
                        break;

                    default:
                        MessageBox.Show("how did u get here!");

                        break;

                }
            }
            catch
            {
                MessageBox.Show("Database is down");
                resultsListBox.Items.Add("No Results");
            }
            finally
            {
                con.Close();
            }
        }

        private void addResults(SqlDataReader reader)
        {
            //checks to see if the query got any results if not it displays no links found
            if (reader.HasRows)
            {
                //going through the query and adding the results to the ListBox
                int count = 0;
                while (reader.Read())
                {
                    resultsListBox.Items.Add(reader[count].ToString());
                }

            }
            else
            {
                resultsListBox.Items.Add("No Results");
            }
        }


        private void resultFoundBtn_Click(object sender, RoutedEventArgs e)
        {
            if (resultsListBox.SelectedItem != null)
            {
                DoSomethingToChangeData();

            }
            else
            {
                MessageBox.Show("Please select an item or close the window");
            }
        }
    }
}
