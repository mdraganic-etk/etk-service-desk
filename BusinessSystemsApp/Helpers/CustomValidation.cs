using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;


namespace BusinessSystemsApp.Helpers
{
    public class CustomValidation
    {

        private string message;

        public CustomValidation(string message)
        {
            this.message = message;
        }

        public bool ShowErrorMessage
        {
            get;
            set;
        }


        public object ValidationError
        {
            get
            {
                return null;
            }
            set
            {
                if (ShowErrorMessage)
                {
                    throw new ValidationException(message);
                }
            }
        }
    }
}
