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
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;

namespace BusinessSystemsApp.ChildForms
{
    public partial class CommunicationChannelDetails : ChildWindow
    {

        public CommunicationChannel NewComm { get; set; }

        public bool isNew = true;

        /// <summary>
        /// When adding new item. No params
        /// </summary>
        public CommunicationChannelDetails()
        {
            InitializeComponent();

            NewComm = new CommunicationChannel();

            isNew = true;
        }

        /// <summary>
        /// When editing selected item.
        /// </summary>
        /// <param name="type">Item for editing</param>
        public CommunicationChannelDetails(CommunicationChannel type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewComm = new CommunicationChannel();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.CommunicationChannelName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;
        }

        //When button ok is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NewComm.Id = Convert.ToInt32(idTextBox.Text);
            NewComm.CommunicationChannelName = nameTextBox.Text;
            NewComm.Description = descriptionTextBox.Text;

            this.DialogResult = true;
        }

        //When button cancel is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewComm = null;
            this.DialogResult = false;
        }
    }
}

