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
using System.Data.SqlTypes;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace webSystemGUI
{
    /// <summary>
    /// Interaction logic for AssignFilterToCrwaledLinks.xaml
    /// </summary>
    public partial class AssignFilterToCrwaledLinks : UserControl
    {
        public AssignFilterToCrwaledLinks()
        {
            InitializeComponent();
        }

        private bool insertDatabase(String url, string filter, bool pageIsDynamic, bool previewMode = false)
        {
            int pageIs = 0;

            if (pageIsDynamic)
            {
                pageIs = 1;
            }

            //put -i if you want to see error messages
            //path to the python script
            string arg = string.Format("C:\\Users\\lcs\\PycharmProjects\\PreviewSoup\\main.py {0} \"{1}\" {2}", url, filter, pageIs);
            try
            {
                Process p1 = new Process();
                p1.StartInfo = new ProcessStartInfo(@"C:\Users\lcs\PycharmProjects\PreviewSoup\venv\Scripts\python.exe ", arg);
                p1.StartInfo.UseShellExecute = false;
                p1.StartInfo.RedirectStandardOutput = true;
                p1.StartInfo.RedirectStandardError = true;
                p1.StartInfo.WorkingDirectory = @"C:\Users\lcs\PycharmProjects\PreviewSoup\";
                p1.Start();
                StreamReader output = p1.StandardOutput;
                string outputCode = output.ReadToEnd();
                p1.Kill();

                //holds a buffer of any errors thrown by the python script.
                StreamReader errorOutput = p1.StandardError;

                //moves the pointer forward by one line.
                errorOutput.ReadLine();

                //if pointer is at the end it means there is no error, otherwise there is an error.
                if (!(errorOutput.EndOfStream))
                {
                    String errorMsg = errorOutput.ReadToEnd();

                    //shows the last couple lines of the error.
                    MessageBox.Show(errorMsg);

                    //clears the buffer for the next time
                    errorOutput.DiscardBufferedData();
                    errorOutput.Close();

                    //returns false because there wasn error message.
                    return false;
                }

                if (outputCode.Length < 4)
                {
                    //tells user that their filter doesn't find any html on the webpage
                    MessageBox.Show("filter failed to find any HTML");

                    //returns false beacuse filter was empty aka over filtering or bad logical filter
                    return false;
                }

                if (previewMode && outputCode.Length > 4)
                {
                    //shows what was scraped off the website
                    PreviewLink linkFilter = new PreviewLink(outputCode);
                    linkFilter.Show();
                }

                //clears the buffer for the next time
                errorOutput.DiscardBufferedData();
                errorOutput.Close();

                return true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }

        }

        private bool ValidateLink(string filterSettings, string link, bool pageIsDynamic)
        {
            bool validLinkRecord = false;

            if (link == "")
            {
                MessageBox.Show("Please enter the URL");
                return validLinkRecord;
            }
            else if (filterSettings == "")
            {
                MessageBox.Show("Enter a Filter please");
                return validLinkRecord;
            }
            else if (!(insertDatabase(link, filterSettings, pageIsDynamic)))
            {
                return validLinkRecord;
            }
            else
            {
                validLinkRecord = true;
                return validLinkRecord;
            }
        }

        private void findLinkBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupLinks = new FindResults(linkTxt.Text, 0);

            /*This tells this form that when it recives the signal GetSelectedLink which is just a methods being called
             on some other form it should run the function listed inside the EventHandler()*/
            lookupLinks.ResultIsReady += new EventHandler(GetSelectedLink);
            lookupLinks.ShowDialog();
        }


        /*This pulls the currently selected item from the pop up form and closes*/
        private void GetSelectedLink(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                linkTxt.Text = "";
                try
                {
                    if (fc.resultsListBox.SelectedItem != null)
                    {
                        linkTxt.Text = fc.resultsListBox.SelectedItem.ToString();

                        SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                        con.Open();

                        SqlCommand getLink = new SqlCommand("SELECT linkOptions.pageIsDynamic, linkOptions.filterParamerters, linkOptions.crwalLinks " +
                                                            " FROM URLS JOIN crwalFilter ON URLS.linkID = crwalFilter.linkID JOIN linkOptions ON linkOptions.filterID = crwalFilter.FilterID WHERE URLS.linkID = (SELECT linkID FROM URLS WHERE link = @link)", con);
                        getLink.Parameters.AddWithValue("@link", linkTxt.Text);

                        SqlDataReader linkRecord = getLink.ExecuteReader();

                        if (linkRecord.Read())
                        {

                            pageIsDynamicCheck.IsChecked = (bool)linkRecord[0];

                            filterSettingsTxt.Text = linkRecord[1].ToString();

                            crwalLinksTrueCheck.IsChecked = (bool)linkRecord[2];

                            linkRecord.Close();

                            fc.Close();
                        }
                        else
                        {
                            fc.Close();
                            MessageBox.Show("No filter found for crwaled links.");
                        }

                    }
                }
                catch
                {
                    MessageBox.Show("Database is down");
                }
            }
        }

        private void addFilterToCrwaledLinksBtn_Click(object sender, RoutedEventArgs e)
        {
            //holds the input from each of the textBoxes on the GUI page AddLink
            String filterSettings = filterSettingsTxt.Text.Trim(),
                    link = linkTxt.Text.Trim();

            //gets all the statues of the checkBoxes on the GUI page AddLink.
            Boolean pageIsDynamic = (bool)pageIsDynamicCheck.IsChecked,
                    crwalLinks = (bool)crwalLinksTrueCheck.IsChecked;

            //how often the link will be monitored 1 being the most frequent and 4 being the least frequent

            int monitorRate = 1;

            try
            {
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand deleteLink = new SqlCommand(
                    "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);
                deleteLink.Parameters.AddWithValue("@link", link);

                SqlDataReader reader = deleteLink.ExecuteReader();
                /*sees if link exists*/
                if (reader.HasRows)
                {
                    reader.Close();

                    if (ValidateLink(filterSettingsTxt.Text, link, (bool)pageIsDynamicCheck.IsChecked))
                    {
                        SqlCommand linkID = new SqlCommand("SELECT * FROM crwalFilter WHERE crwalFilter.linkID = (SELECT linkID FROM URLS WHERE link = @link)", con);
                        linkID.Parameters.AddWithValue("@link", link);
                        SqlDataReader linkHasFilter = linkID.ExecuteReader();

                        if (!(linkHasFilter.HasRows))
                        {
                            linkHasFilter.Close();

                            SqlCommand addFilterToCrwaledLinks = new SqlCommand("Insert INTO linkOptions (filterParamerters, pageIsDynamic, monitorRate, crwalLinks) VALUES (@filter, @pageIsDynamic, @monitorRate, @crwalLinks) " +
                                                                                "DECLARE @lastRowAddedToLinkOptions INT = 0 " +
                                                                                "DECLARE @linkID INT = 0 " +
                                                                                "SELECT TOP 1 @lastRowAddedToLinkOptions = linkOptions.filterID FROM linkOptions ORDER BY linkOptions.filterID DESC " +
                                                                                "SELECT TOP 1 @linkID = linkID FROM URLS WHERE link = @link " +
                                                                                "INSERT INTO crwalFilter (filterID, linkID) VALUES (@lastRowAddedToLinkOptions, @linkID)", con);
                            addFilterToCrwaledLinks.Parameters.AddWithValue("@filter", filterSettings);
                            addFilterToCrwaledLinks.Parameters.AddWithValue("@pageIsDynamic", pageIsDynamic);
                            addFilterToCrwaledLinks.Parameters.AddWithValue("@monitorRate", monitorRate);
                            addFilterToCrwaledLinks.Parameters.AddWithValue("@crwalLinks", crwalLinks);
                            addFilterToCrwaledLinks.Parameters.AddWithValue("@link", link);

                            addFilterToCrwaledLinks.ExecuteNonQuery();

                            MessageBox.Show("Filter was assigned to crawled links.");

                        }
                        else
                        {
                            MessageBox.Show("Crwaled Links has a filter already assigned.");
                        }
                    }

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

        private void modifyFilterAssignToCrwaledLinksBtn_Click(object sender, RoutedEventArgs e)
        {

            //holds the input from each of the textBoxes on the GUI page AddLink
            String filterSettings = filterSettingsTxt.Text.Trim(),
                    link = linkTxt.Text.Trim();

            //gets all the statues of the checkBoxes on the GUI page AddLink.
            Boolean pageIsDynamic = (bool)pageIsDynamicCheck.IsChecked,
                    crwalLinks = (bool)crwalLinksTrueCheck.IsChecked;

            //how often the link will be monitored 1 being the most frequent and 4 being the least frequent

            int monitorRate = 1;
            try
            {
                SqlConnection con = new SqlConnection("Data Source=LCS-PC;Initial Catalog=WebSystem;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

                con.Open();

                SqlCommand deleteLink = new SqlCommand(
                    "SELECT dbo.URLS.link FROM URLS WHERE dbo.URLS.link = @link ", con);

                deleteLink.Parameters.AddWithValue("@link", linkTxt.Text);

                SqlDataReader reader = deleteLink.ExecuteReader();
                /*sees if link exists*/
                if (reader.HasRows)
                {
                    reader.Close();

                    if (ValidateLink(filterSettingsTxt.Text, linkTxt.Text, (bool)pageIsDynamicCheck.IsChecked))
                    {
                        SqlCommand linkID = new SqlCommand("SELECT * FROM crwalFilter WHERE crwalFilter.linkID = (SELECT linkID FROM URLS WHERE link = @link)", con);
                        linkID.Parameters.AddWithValue("@link", link);
                        SqlDataReader linkHasFilter = linkID.ExecuteReader();

                        if (linkHasFilter.HasRows)
                        {

                            linkHasFilter.Close();

                            SqlCommand modFilterToCrwaledLinks = new SqlCommand("Insert INTO linkOptions (filterParamerters, pageIsDynamic, monitorRate, crwalLinks) VALUES (@filter, @pageIsDynamic, @monitorRate, @crwalLinks) " +
                                                                                "DECLARE @lastRowAddedToLinkOptions INT = 0 " +
                                                                                "DECLARE @linkID INT = 0 " +
                                                                                "SELECT TOP 1 @lastRowAddedToLinkOptions = linkOptions.filterID FROM linkOptions ORDER BY linkOptions.filterID DESC " +
                                                                                "SELECT @linkID = linkID FROM URLS WHERE link = @link " +
                                                                                "UPDATE crwalFilter SET filterID = @lastRowAddedToLinkOptions WHERE linkID = @linkID", con);
                            modFilterToCrwaledLinks.Parameters.AddWithValue("@filter", filterSettings);
                            modFilterToCrwaledLinks.Parameters.AddWithValue("@pageIsDynamic", pageIsDynamic);
                            modFilterToCrwaledLinks.Parameters.AddWithValue("@monitorRate", monitorRate);
                            modFilterToCrwaledLinks.Parameters.AddWithValue("@crwalLinks", crwalLinks);
                            modFilterToCrwaledLinks.Parameters.AddWithValue("@link", link);

                            modFilterToCrwaledLinks.ExecuteNonQuery();

                            MessageBox.Show("Filter assignment updated");
                        }
                        else
                        {
                            MessageBox.Show("Crwaled Links do not have a filter assigned yet.");
                        }
                    }

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

        private void findFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            FindResults lookupFilter = new FindResults(filterSettingsTxt.Text, 1);
            lookupFilter.ResultIsReady += new EventHandler(GetSelectedFilter);
            lookupFilter.ShowDialog();
        }

        private void GetSelectedFilter(object sender, EventArgs e)
        {
            FindResults fc = sender as FindResults;
            if (fc != null)
            {
                filterSettingsTxt.Text = "";

                if (fc.resultsListBox.SelectedItem != null)
                {
                    filterSettingsTxt.Text = fc.resultsListBox.SelectedItem.ToString();
                    fc.Close();

                }
            }
        }
    }
}
