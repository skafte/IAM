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
using System.Xml.Linq;
using System.Linq;
using System.Windows.Data;

using IAM;

namespace IAM.Powers
{
   public class DisplayPowers
   {
      #region Properties ______________________________________________________________________
      #region Private _____________________________________________________________________________
      #endregion __________________________________________________________________________________

      #region Events ______________________________________________________________________________
      public delegate void fromDisplayPowersHandler(object sender, SelectionChangedEventArgs e);

      #region Event Signatures ________________________________________________________________________
      // Startup loading of informations
      public event fromDisplayPowersHandler selectedUser;
      #endregion ______________________________________________________________________________________
      #endregion __________________________________________________________________________________
      #endregion ______________________________________________________________________________

      #region Methods _________________________________________________________________________
      #region Events ______________________________________________________________________________

      private void lstbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
      {
         if (this.selectedUser != null)
            this.selectedUser(sender, e);
      }
      #endregion __________________________________________________________________________________
      #region Public ______________________________________________________________________________

      /// <summary>
      /// Create grid for the selected power
      /// </summary>
      /// <param name="sender">button of selected power</param>
      /// <param name="ParentGrid">LayoutRoot</param>
      public void CreatePowerLibraryGrid(object sender, Grid LayoutRoot)
      {
         // grid, outer
         Grid outer_grd = new Grid();
         outer_grd.HorizontalAlignment = HorizontalAlignment.Stretch;
         outer_grd.VerticalAlignment = VerticalAlignment.Stretch;
         RowDefinition r = new RowDefinition();
         r.Height = new GridLength(60, GridUnitType.Pixel);
         outer_grd.RowDefinitions.Add(r);
         r = new RowDefinition();
         r.Height = new GridLength(1, GridUnitType.Star);
         outer_grd.RowDefinitions.Add(r);
         outer_grd.Name = (sender as Button).Tag.ToString() + "Outer_grd";

         // label
         Label name_lbl = new Label();
         name_lbl.Foreground = (SolidColorBrush)Application.Current.Resources["Brush_Header"];
         name_lbl.FontFamily = new FontFamily("Segoe UI");
         name_lbl.FontSize = Globals.PtToPx(20);
         name_lbl.Content = ((sender as Button).Content as TextBox).Text;
         name_lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
         name_lbl.VerticalAlignment = VerticalAlignment.Bottom;
         name_lbl.Margin = new Thickness(60, 0, 20, 0);

         // scrollview
         ScrollViewer scrllvwr = new ScrollViewer();
         scrllvwr.Style = (Application.Current.Resources["Custom_ScrollViewerStyle"] as Style);
         scrllvwr.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
         scrllvwr.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
         scrllvwr.HorizontalAlignment = HorizontalAlignment.Stretch;
         scrllvwr.VerticalAlignment = VerticalAlignment.Stretch;
         Grid.SetRow(scrllvwr, 1);

         // grid, inner
         Grid inner_grd = new Grid();
         inner_grd.HorizontalAlignment = HorizontalAlignment.Stretch;
         inner_grd.VerticalAlignment = VerticalAlignment.Stretch;

// mangler power element liste og graph

         // connect the elements
         scrllvwr.Content = inner_grd;
         outer_grd.Children.Add(scrllvwr);
         outer_grd.Children.Add(name_lbl);
         LayoutRoot.Children.Insert(LayoutRoot.Children.Count - 10, outer_grd);
      }

      /// <summary>
      /// Will add users to combobox
      /// </summary>
      /// <param name="document"></param>
      public void DisplayPowerLibraryUsers(string powerSelected, StackPanel AppBar_stckpnl)
      {  
         // stackpanel, combobox parent
         StackPanel stckpnl = new StackPanel();
         stckpnl.Orientation = Orientation.Vertical;
         stckpnl.Margin = new Thickness(10);

         // label
         Label lbl = new Label();
         lbl.Foreground = (SolidColorBrush)Application.Current.Resources["Brush_Header"];
         lbl.FontFamily = new FontFamily("Segoe UI");
         lbl.FontSize = Globals.PtToPx(20);
         lbl.HorizontalAlignment = HorizontalAlignment.Stretch;
         lbl.VerticalAlignment = VerticalAlignment.Bottom;
         lbl.Margin = new Thickness(0, 0, 20, 0);
         lbl.Content = "User";

         // ListBox
         ListBox lstbx = new ListBox();
         lstbx.Background = null;
         lstbx.BorderBrush = null;
         lstbx.Foreground = new SolidColorBrush(Colors.White);
         lstbx.FontFamily = new FontFamily("Segoe UI");
         lstbx.FontSize = Globals.PtToPx(14);
         lstbx.SelectionChanged += lstbx_SelectionChanged;

         // add data
         foreach (string str in Globals.GameInformation.PowerIndexSingle.ElementAt(Globals.GameInformation.PowerIndex.IndexOf(powerSelected)))
            lstbx.Items.Add(str);

         // connect the elements
         stckpnl.Children.Add(lbl);
         stckpnl.Children.Add(lstbx);
         AppBar_stckpnl.Children.Add(stckpnl);
      }
      #endregion __________________________________________________________________________________
      #endregion ______________________________________________________________________________

   }
}
