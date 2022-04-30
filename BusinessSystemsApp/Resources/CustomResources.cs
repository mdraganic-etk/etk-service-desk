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
using System.ComponentModel;

namespace BusinessSystemsApp.Resources
{
    public class CustomResources: INotifyPropertyChanged
    {
        private static Resources.BS_Resources _strings = new Resources.BS_Resources();

        public Resources.BS_Resources LocalizedStrings
        {
            get { return _strings; }
            set { OnPropertyChanged("LocalizedStrings"); }
        }

        #region INotifyPropertyChanged Members
    
       public event PropertyChangedEventHandler PropertyChanged;
    
       private void OnPropertyChanged(string propertyName)
       {
           if (PropertyChanged != null)
           {
              PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
           }
       }
       #endregion    

    }
}
