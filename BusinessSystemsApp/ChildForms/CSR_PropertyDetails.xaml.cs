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
    public partial class CSR_PropertyDetails : ChildWindow
    {
        public bool isNew = true;

        public CSR_Property currentProperty;

        public Frequency NewFrequency { get; set; }

        public Csr_Status NewStatus { get; set; }

        public Severity NewSeverity { get; set; }

        public Priority NewPriority { get; set; }

        /// <summary>
        /// Edit frequency
        /// </summary>
        /// <param name="type"></param>
        public CSR_PropertyDetails(Frequency type)
        {
            InitializeComponent();

            isNew = false;
            idTextBox.IsReadOnly = true;
            currentProperty = CSR_Property.Frequency;

            NewFrequency = new Frequency();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.FrequencyName;
            if(type.Description != null)
                descriptionTextBox.Text = type.Description.ToString();
        }

        /// <summary>
        /// Edit CSR status
        /// </summary>
        /// <param name="type"></param>
        public CSR_PropertyDetails(Csr_Status type)
        {
            InitializeComponent();

            isNew = false;
            idTextBox.IsReadOnly = true;
            currentProperty = CSR_Property.Status;

            NewStatus = new Csr_Status();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.StatusName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;
        }


        /// <summary>
        /// Edit severity
        /// </summary>
        /// <param name="type"></param>
        public CSR_PropertyDetails(Severity type)
        {
            InitializeComponent();

            isNew = false;
            idTextBox.IsReadOnly = true;
            currentProperty = CSR_Property.Severity;

            NewSeverity = new Severity();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.SeverityName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;
        }

        /// <summary>
        /// Edit priority
        /// </summary>
        /// <param name="type"></param>
        public CSR_PropertyDetails(Priority type)
        {
            InitializeComponent();

            isNew = false;
            idTextBox.IsReadOnly = true;
            currentProperty = CSR_Property.Priority;

            NewPriority = new Priority();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.PriorityName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;
        }


        /// <summary>
        /// New CSR property
        /// </summary>
        /// <param name="property">
        /// Parameter determins which CSR property is.
        /// </param>
        public CSR_PropertyDetails(CSR_Property property)
        {
            InitializeComponent();

            if (property == CSR_Property.Frequency)
            {
                NewFrequency = new Frequency();
                currentProperty = CSR_Property.Frequency;
            }
            else if (property == CSR_Property.Priority)
            {
                NewPriority = new Priority();
                currentProperty = CSR_Property.Priority;
            }
            else if (property == CSR_Property.Severity)
            {
                NewSeverity = new Severity();
                currentProperty = CSR_Property.Severity;
            }
            else if (property == CSR_Property.Status)
            {
                NewStatus = new Csr_Status();
                currentProperty = CSR_Property.Status;
            }
            
            isNew = true;
        }


        //When button OK is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentProperty == CSR_Property.Frequency)
                {
                    NewFrequency.Id = Convert.ToInt32(idTextBox.Text);
                    NewFrequency.FrequencyName = nameTextBox.Text;
                    NewFrequency.Description = descriptionTextBox.Text;
                }
                else if (currentProperty == CSR_Property.Priority)
                {
                    NewPriority.Id = Convert.ToInt32(idTextBox.Text);
                    NewPriority.PriorityName = nameTextBox.Text;
                    NewPriority.Description = descriptionTextBox.Text;
                }
                else if (currentProperty == CSR_Property.Severity)
                {
                    NewSeverity.Id = Convert.ToInt32(idTextBox.Text);
                    NewSeverity.SeverityName = nameTextBox.Text;
                    NewSeverity.Description = descriptionTextBox.Text;
                }
                else if (currentProperty == CSR_Property.Status)
                {
                    NewStatus.Id = Convert.ToInt32(idTextBox.Text);
                    NewStatus.StatusName = nameTextBox.Text;
                    NewStatus.Description = descriptionTextBox.Text;
                }

                this.DialogResult = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.ToString());
            }
        }

        //When cancel button is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentProperty == CSR_Property.Frequency)
            {
                NewFrequency = null;
            }
            else if (currentProperty == CSR_Property.Priority)
            {
                NewPriority = null;
            }
            else if (currentProperty == CSR_Property.Severity)
            {
                NewSeverity = null;
            }
            else if (currentProperty == CSR_Property.Status)
            {
                NewStatus = null;
            }

            this.DialogResult = false;
        }

        private void CsrPropertyDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.CsrPropertyDetails_Window_Title;
        }
    }
}

