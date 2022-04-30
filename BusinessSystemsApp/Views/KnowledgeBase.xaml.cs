using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace BusinessSystemsApp.Views
{
    public partial class KnowledgeBase : Page
    {
        public bool FirstLoad = true;
        
        public KnowledgeBase()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill grid column headers from resource depending on language
            kB_NotesDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.KB_Grid_Id;
            kB_NotesDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.KB_Grid_Date;
            kB_NotesDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.KB_Grid_Heading;
            kB_NotesDataGrid.Columns[8].Header = BusinessSystemsApp.Resources.BS_Resources.KB_Grid_Product;
            
        }

        //When KB is loaded
        private void kB_NotesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    kB_NotesDataGrid.SelectedItem = -1;
                    
                    dataPager1.Source = kB_NotesDataGrid.ItemsSource;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //Changed selection of KB item in data grid
        private void kB_NotesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (FirstLoad)
                {
                    kB_NotesDataGrid.SelectedItem = -1;

                    FirstLoad = false;
                }
                else
                {
                    ClearKBForm();

                    if (kB_NotesDataGrid.SelectedItem != null)
                    {
                        KBIdTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Id.ToString();
                        KBSolutionTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Description;
                        KBTitleTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Heading;
                        KBNoteTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Note;
                        KBAuthorTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).User.FirstName + " " + (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).User.LastName;
                        CsrId_TextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).CsrId.ToString();

                        //Load selected KB note attachments
                        kBAttachmenttAssignDomainDataSource.QueryParameters[0].Value = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Id;

                        kBAttachmenttAssignDomainDataSource.Load();

                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void CSReview_Expander_Header_Expanded(object sender, RoutedEventArgs e)
        {
            if (KnowledgeBase_Expander_Header != null)
            {

                KnowledgeBase_Expander_Header.Height = Double.NaN;
            }
        }

        private void CSReview_Expander_Header_Collapsed(object sender, RoutedEventArgs e)
        {
            if (KnowledgeBase_Expander_Header != null)
            {
                KnowledgeBase_Expander_Header.Height = 23;
            }
        }


        private void ClearKBForm()
        {
            KBIdTextBox.Text = String.Empty;
            KBSolutionTextBox.Text = String.Empty;
            KBTitleTextBox.Text = String.Empty;
            KBNoteTextBox.Text = String.Empty;
            KBAuthorTextBox.Text = String.Empty;
            CsrId_TextBox.Text = String.Empty;

            KBNotesAttachment_ListBox.Items.Clear();
        }


        private void CSRReview_Filter_Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                KnowledgeBase_Expander_Header.Height = 23;
                KnowledgeBase_Expander_Header.IsExpanded = false;

                ClearKBForm();


                kB_NotesDataGrid.ItemsSource = kB_NotesDomainDataSource1.Data;

                //FirstLoad = true;

                if (kB_NotesDomainDataSource1.CanLoad)
                    kB_NotesDomainDataSource1.Load();

            }

            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        private void productDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    Web.Product prod = new Web.Product();
                    prod.Id = 0;
                    prod.ProductName = "- None -";

                    productDomainDataSource.DataView.Add(prod);

                    productComboBox.SelectedValue = 0;


                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void kB_NotesDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    //if(kB_NotesDomainDataSource1.DataView.Count > 0)
                        //dataPager1.Source = kB_NotesDataGrid.ItemsSource;

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }

           

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Header.ToString())
            {
                case "Cut":
                    Clipboard.SetText(KBNoteTextBox.SelectedText);
                    KBNoteTextBox.SelectedText = "";
                    KBNoteTextBox.Focus();
                    break;
                case "Copy":
                    Clipboard.SetText(KBNoteTextBox.SelectedText);
                    KBNoteTextBox.Focus();
                    break;
                case "Paste":
                    KBNoteTextBox.SelectedText = Clipboard.GetText();
                    break;
                default:
                    break;
            }

            //contextMenu1.IsOpen = false;
        }

      
        private void KBNoteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void KBNoteTextBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //contextMenu1.Visibility = System.Windows.Visibility.Collapsed;

            //contextMenu1.IsOpen = false;

            //contextMenu1.Margin = new Thickness(e.GetPosition(LayoutRoot).X, e.GetPosition(LayoutRoot).Y, 0, 0);

            //contextMenu1.IsOpen = true;

            //contextMenu1.Visibility = System.Windows.Visibility.Visible;
        }

        private void KBNoteTextBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //contextMenu1.Visibility = System.Windows.Visibility.Visible;
        }

        private void KBSolutionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //contextMenu1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void KBNoteTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //contextMenu1.Visibility = System.Windows.Visibility.Collapsed;
        }


        private void KBNotesAttachment_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (KBNotesAttachment_ListBox.SelectedItem != null)
                {
                    var hostingWindow = System.Windows.Browser.HtmlPage.Window;

                    String path = "/Data/";

                    if (MainPage.AttachmentPath != null)
                        path = MainPage.AttachmentPath;

                    hostingWindow.Navigate(new Uri(path + (KBNotesAttachment_ListBox.SelectedItem as ListBoxItem).Name, UriKind.RelativeOrAbsolute), "_blank");

                    if (KBNotesAttachment_ListBox.Items.Count == 1)
                        KBNotesAttachment_ListBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        private void attachmenttAssignDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {

                    foreach (Web.AttachmenttAssign attach in kBAttachmenttAssignDomainDataSource.DataView)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Name = attach.Attachment.Url;
                        lbi.Content = attach.Attachment.AttachmentName;

                        KBNotesAttachment_ListBox.Items.Add(lbi);

                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

    }
}
