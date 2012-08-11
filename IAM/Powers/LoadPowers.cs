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
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.ComponentModel;

using IAM;

namespace ShowPowersNamespace
{
    public class LoadPowers : INotifyPropertyChanged
    {
        #region Properties --------------------------------------------------------------------
        #region Public ----------------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion --------------------------------------------------------------------------------

        #region Events ----------------------------------------------------------------------------
        #region Event Signatures ----------------------------------------------------------------------
        #region INotifyPropertyChanged Members ------------------------------------------------------------
        private void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion ----------------------------------------------------------------------------------------
        #endregion ------------------------------------------------------------------------------------
        #endregion --------------------------------------------------------------------------------
        #endregion ----------------------------------------------------------------------------
    }
}
