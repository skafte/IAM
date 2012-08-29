using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Windows.Media;

using IAM;
using FileIONamespace;

namespace CharacterSheetNamespace
{
   public class CreateSheet
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      enum DefaultItemEnum { enDefault, enCustom };
      enum CreateItemEnum { enColumn, enRow, enComboBox, enDot, enLabel, enLabelTitle, enNumericUpDown, enTextBox, enPowerBox };
      string[,] StyleLayout = new string[2, 9];
      #endregion --------------------------------------------------------------------------------

      #region Events ----------------------------------------------------------------------------

      #region Event Signatures ----------------------------------------------------------------------
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      #region Layout helper functions ---------------------------------------------------------------
      /// <summary>
      /// Reset layout values for the different object elements
      /// </summary>
      private void ResetLayoutValues()
      {
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enColumn] = "HorizontalAlignment=Left; VerticalAlignment=Top";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enRow] = "HorizontalAlignment=Left; VerticalAlignment=Top";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enComboBox] = "Height=24; MinWidth=50";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enDot] = "Height=24; HorizontalAlignment=Right";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enLabel] = "Height=24; MinWidth=100; FontSize=13";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enLabelTitle] = "HorizontalAlignment=Center; HorizontalContentAlignment=Center; FontSize=16; Foreground=SolidColorBrush, 255, 139, 54, 54";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enNumericUpDown] = "Height=24; MinWidth=100; FontSize=13";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enTextBox] = "Height=24; MinWidth=100";
         StyleLayout[(int)DefaultItemEnum.enDefault, (int)CreateItemEnum.enPowerBox] = "MaxHeight=300; MaxWidth=500";

         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enColumn] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enRow] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enComboBox] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enDot] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enLabel] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enLabelTitle] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enNumericUpDown] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enTextBox] = "";
         StyleLayout[(int)DefaultItemEnum.enCustom, (int)CreateItemEnum.enPowerBox] = "";
      }

      /// <summary>
      /// Find layout informations in attribute to node
      /// </summary>
      /// <param name="eValue">node to get attribute from</param>
      /// <param name="CreateItem">StyleLayout type to set layout informations for</param>
      /// <returns>An array of layout values to use</returns>
      private string[] GetLayoutAttributes(XElement eValue, int CreateItem)
      {
         string attribute = "";
         string[] seperatorPairs = new string[] { "; " };

         if (eValue.Attribute("layout") != null)
         {
            if (eValue.Attribute("layout").Value != "default")
               attribute = StyleLayout[(int)DefaultItemEnum.enCustom, CreateItem] = eValue.Attribute("layout").Value;      // set custom layout
            else if (eValue.Attribute("layout").Value == "default")
            {
               StyleLayout[(int)DefaultItemEnum.enCustom, CreateItem] = "";                                                // clear custom layout
               attribute = StyleLayout[(int)DefaultItemEnum.enDefault, CreateItem];                                        // layout to use equals default
            }
         }
         else if (StyleLayout[(int)DefaultItemEnum.enCustom, CreateItem] != "")                                              // custom layout already sat and not to use default
            attribute = StyleLayout[(int)DefaultItemEnum.enCustom, CreateItem];
         else
            attribute = StyleLayout[(int)DefaultItemEnum.enDefault, CreateItem];

         return attribute.Split(seperatorPairs, StringSplitOptions.RemoveEmptyEntries);
      }

      /// <summary>
      /// Find a given object from any panel depth
      /// </summary>
      /// <param name="pnl">Panel to search within</param>
      /// <param name="objectID">Object to search after</param>
      /// <param name="objectType">Object type to search after</param>
      /// <returns>Return null if object couldn't be found</returns>
      private static object FindGridElement(Panel pnl, string objectID, string objectType)
      {
         foreach (object obj in pnl.Children)
         {
            if ((obj.GetType().Name == objectType) && ((obj as Control).Name == objectID))
               return obj;
            else if ((obj.GetType().Name == "StackPanel") || (obj.GetType().Name == "WrapPanel"))
            {
               object FoundObj = FindGridElement((Panel)obj, objectID, objectType);
               if (FoundObj != null)
                  return FoundObj;
            }
         }

         return null;
      }
      #endregion ------------------------------------------------------------------------------------

      #region Layout --------------------------------------------------------------------------------
      /// <summary>
      /// Find the different objects to create in the XML
      /// </summary>
      /// <param name="ePage">Page (XML node) where objects are described</param>
      /// <param name="wrppnl">Wrappanel to create objects on</param>
      private void DiscectSheetXML(XElement ePage, object wrppnl)
      {
         string[] KeyValuePairArray;
         string[] KeyValuePair;
         string[] seperatorKeyValue = new string[] { "=" };

         XNode eNode = ePage.FirstNode;

         if (eNode != null)              // make sure are objects exits
         {
            do
            {
               if (eNode.NodeType == XmlNodeType.Element)
               {
                  switch ((eNode as XElement).Name.ToString())
                  {
                     case "column":
                        WrapPanel wrppnlColumnInner = new WrapPanel();
                        wrppnlColumnInner.Orientation = Orientation.Vertical;

                        KeyValuePairArray = GetLayoutAttributes((eNode as XElement), (int)CreateItemEnum.enColumn);

                        foreach (string str in KeyValuePairArray)
                        {
                           KeyValuePair = str.Split(seperatorKeyValue, StringSplitOptions.RemoveEmptyEntries);
                           SetObjectAttribute(wrppnlColumnInner, KeyValuePair);
                        }

                        DiscectSheetXML((eNode as XElement), wrppnlColumnInner);
                        if (wrppnl is WrapPanel)
                           (wrppnl as WrapPanel).Children.Add(wrppnlColumnInner);
                        else
                           (wrppnl as StackPanel).Children.Add(wrppnlColumnInner);
                        break;
                     case "row":
                        WrapPanel wrppnlRowInner = new WrapPanel();
                        wrppnlRowInner.Orientation = Orientation.Horizontal;

                        KeyValuePairArray = GetLayoutAttributes((eNode as XElement), (int)CreateItemEnum.enRow);

                        foreach (string str in KeyValuePairArray)
                        {
                           KeyValuePair = str.Split(seperatorKeyValue, StringSplitOptions.RemoveEmptyEntries);
                           SetObjectAttribute(wrppnlRowInner, KeyValuePair);
                        }

                        DiscectSheetXML((eNode as XElement), wrppnlRowInner);

                        if (wrppnl is WrapPanel)
                           (wrppnl as WrapPanel).Children.Add(wrppnlRowInner);
                        else
                           (wrppnl as StackPanel).Children.Add(wrppnlRowInner);
                        break;
                     case "name":
                        //CreateLabelTitle((eNode as XElement), wrppnl);
                        CreateElement((eNode as XElement), wrppnl);
                        break;
                     case "stat":
                        AddStatObject((eNode as XElement), wrppnl);
                        break;
                     default:
                        break;
                  }
               }
            }
            while ((eNode = eNode.NextNode) != null);
         }
      }

      /// <summary>
      /// Control creation of stat objects
      /// </summary>
      /// <param name="eStat">XML-node to create object on basic off</param>
      /// <param name="wrppnl">Wrappanel to place object on</param>
      private void AddStatObject(XElement eStat, object wrppnl)
      {
         WrapPanel wrppnlInner = new WrapPanel();
         wrppnlInner.HorizontalAlignment = HorizontalAlignment.Left;
         wrppnlInner.VerticalAlignment = VerticalAlignment.Top;
         wrppnlInner.Orientation = Orientation.Horizontal;

         foreach (XElement eValue in eStat.Elements("value"))
            CreateElement(eValue, wrppnlInner);

         (wrppnl as Panel).Children.Add(wrppnlInner);
      }

      /// <summary>
      /// Set values of object attribute
      /// </summary>
      /// <param name="sender">Object to set attribute on</param>
      /// <param name="KeyValuePair">Attribute to set and value(s) it shall have</param>
      private void SetObjectAttribute(object sender, string[] KeyValuePair)
      {
         string[] ValueArray;
         string[] seperatorValues = new string[] { ", " };
         int i = 0;

         switch (KeyValuePair[0])
         {
            case "RenderTransform":
               // RenderTransform=5e-1, 5e-1, 75e-2, 75e-2
               ValueArray = KeyValuePair[1].Split(seperatorValues, StringSplitOptions.RemoveEmptyEntries);

               sender.GetType().GetProperty("RenderTransformOrigin").SetValue(sender, new Point(double.Parse(ValueArray[0]), double.Parse(ValueArray[1])), null);

               ScaleTransform Scale = new ScaleTransform();
               Scale.ScaleX = double.Parse(ValueArray[2]);
               Scale.ScaleY = double.Parse(ValueArray[3]);
               sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, Scale, null);
               break;
            case "Background":
            case "BorderBrush":
            case "Foreground":
            case "OpacityMask":
               ValueArray = KeyValuePair[1].Split(seperatorValues, StringSplitOptions.RemoveEmptyEntries);

               switch (ValueArray[0])
               {
                  case "NoBrush":
                     // OpacityMask=NoBrush
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, null, null);
                     break;
                  case "SolidColorBrush":
                     // Background=SolidColorBrush, 170, 170, 170, 170
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, new SolidColorBrush(Color.FromArgb(byte.Parse(ValueArray[1]), byte.Parse(ValueArray[2]),
                                                                                                                        byte.Parse(ValueArray[3]), byte.Parse(ValueArray[4]))), null);
                     break;
                  case "LinearGradientBrush":
                     // Foreground=LinearGradientBrush, 0, 0, 1, 1, 255, 255, 255, 0, 2e-1, 255, 255, 165, 0, 5e-1, 255, 255, 0, 0, 8e-1
                     LinearGradientBrush LinGradient = new LinearGradientBrush();
                     LinGradient.StartPoint = new Point(double.Parse(ValueArray[1]), double.Parse(ValueArray[2]));
                     LinGradient.EndPoint = new Point(double.Parse(ValueArray[3]), double.Parse(ValueArray[4]));

                     i = 4;
                     while (i < ValueArray.Length - 1)
                     {
                        GradientStop stop = new GradientStop();
                        stop.Color = Color.FromArgb(byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]));
                        stop.Offset = double.Parse(ValueArray[++i]);
                        LinGradient.GradientStops.Add(stop);
                     }

                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, LinGradient, null);
                     break;
                  case "RadialGradientBrush":
                     // BorderBrush=RadialGradientBrush, 0, 0, 0, 0, 0, 255, 144, 144, 144, 1
                     RadialGradientBrush RadGradient = new RadialGradientBrush();

                     i = 0;
                     while (i < ValueArray.Length - 1)
                     {
                        GradientStop stop = new GradientStop();
                        stop.Color = Color.FromArgb(byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]), byte.Parse(ValueArray[++i]));
                        stop.Offset = double.Parse(ValueArray[++i]);
                        RadGradient.GradientStops.Add(stop);
                     }

                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, RadGradient, null);
                     break;
               }
               break;
            case "HorizontalAlignment":
            case "HorizontalContentAlignment":
            case "TextAlignment":
               // HorizontalAlignment=Right
               switch (KeyValuePair[1])
               {
                  case "Center":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, HorizontalAlignment.Center, null);
                     break;
                  case "Left":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, HorizontalAlignment.Left, null);
                     break;
                  case "Right":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, HorizontalAlignment.Right, null);
                     break;
                  case "Stretch":
                  default:
                     if (KeyValuePair[0] != "TextAlignment")
                        sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, HorizontalAlignment.Stretch, null);
                     break;
               }
               break;
            case "VerticalAlignment":
            case "VerticalContentAlignment":
               // VerticalContentAlignment=Bottom
               switch (KeyValuePair[1])
               {
                  case "Bottom":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, VerticalAlignment.Bottom, null);
                     break;
                  case "Center":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, VerticalAlignment.Center, null);
                     break;
                  case "Stretch":
                  default:
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, VerticalAlignment.Stretch, null);
                     break;
                  case "Top":
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, VerticalAlignment.Top, null);
                     break;
               }
               break;
            case "BorderThickness":
            case "Margin":
            case "Padding":
               // BorderThickness=0, 5, 0, 0
               ValueArray = KeyValuePair[1].Split(seperatorValues, StringSplitOptions.RemoveEmptyEntries);

               if (ValueArray.Length == 1)
                  sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, new Thickness(double.Parse(ValueArray[0])), null);
               else if (ValueArray.Length == 4)
                  sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, new Thickness(double.Parse(ValueArray[0]), double.Parse(ValueArray[1]), double.Parse(ValueArray[2]), double.Parse(ValueArray[3])), null);
               break;
            default:
               try
               {
                  if (KeyValuePair[1] == "null")
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, double.NaN, null);
                  else
                     sender.GetType().GetProperty(KeyValuePair[0]).SetValue(sender, double.Parse(KeyValuePair[1]), null);
               }
               finally { }
               break;
         }
      }

      /// <summary>
      /// Create an object on a Wrap/Stack-panel
      /// </summary>
      /// <param name="eValue">Value</param>
      /// <param name="pnlInner"></param>
      /// <param name="objType"></param>
      private void CreateElement(XElement eValue, object pnlInner)
      {
         string[] KeyValuePairArray;
         string[] KeyValuePair;
         string[] seperatorKeyValue = new string[] { "=" };

         // object specific setup
         object obj = null;
         if (eValue.Attribute("id").Value.ToString().Contains("_nmlbl"))
         {
            obj = new Label();
            (obj as Label).Content = eValue.Value;
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enLabelTitle);
         }
         else if (eValue.Attribute("id").Value.ToString().Contains("_lbl"))
         {
            obj = new Label();
            if (eValue.Element("start") != null)
               (obj as Label).Content = eValue.Element("start").Value;
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enLabel);
         }
         else if (eValue.Attribute("id").Value.ToString().Contains("_cmbbx"))
         {
            obj = new ComboBox();
            string start = "";
            List<string> lItem = new List<string>((from item in eValue.Elements("item")
                                                   select item.Value).ToList());
            if (eValue.Element("start") != null)
            {
               if (lItem.IndexOf(eValue.Element("start").Value) == -1)
                  lItem.Add(eValue.Element("start").Value);
               start = eValue.Element("start").Value;
            }
            lItem.Sort();
            lItem.ForEach(delegate(string itm)
            {
               (obj as ComboBox).Items.Add(itm);
            });
            (obj as ComboBox).SelectedItem = eValue.Element("start").Value;
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enComboBox);
         }
         else if (eValue.Attribute("id").Value.ToString().Contains("_txtbx"))
         {
            obj = new TextBox();
            if (eValue.Element("start") != null)
               (obj as TextBox).Text = eValue.Element("start").Value;
            (obj as TextBox).TextWrapping = TextWrapping.Wrap;
            if (eValue.Attribute("id").Value.ToString().Contains("_txtbxrdo"))
               (obj as TextBox).IsReadOnly = true;
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enTextBox);
         }
         else if (eValue.Attribute("id").Value.ToString().Contains("_dt"))
         {
            obj = new Rating();
            (obj as Rating).ItemCount = int.Parse(eValue.Element("maxshow").Value);
            if (eValue.Element("start") != null)
               (obj as Rating).Value = float.Parse(eValue.Element("start").Value) / (obj as Rating).ItemCount;
            switch (eValue.Element("look").Value)
            {
               // TODO
               // Get styles back again, they should probably be put in an external file..
               case "dot":
                  (obj as Rating).ItemContainerStyle = (Style)App.Current.Resources["RatingItemStyleDot"];
                  break;
               case "square":
                  (obj as Rating).ItemContainerStyle = (Style)App.Current.Resources["RatingItemStyleSquare"];
                  break;
               default:
                  break;
            }
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enDot);
         }
         else if (eValue.Attribute("id").Value.ToString().Contains("_nmud"))
         {
            obj = new NumericUpDown();
            if (eValue.Element("start") != null)
               (obj as NumericUpDown).Value = double.Parse(eValue.Element("start").Value);
            KeyValuePairArray = GetLayoutAttributes(eValue, (int)CreateItemEnum.enNumericUpDown);
         }
         else
            throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());

         // set object name
         (obj as Control).Name = eValue.Attribute("id").Value.ToString();

         // set layout of object
         foreach (string str in KeyValuePairArray)
         {
            KeyValuePair = str.Split(seperatorKeyValue, StringSplitOptions.RemoveEmptyEntries);
            SetObjectAttribute(obj, KeyValuePair);
         }

         (pnlInner as Panel).Children.Add((obj as Control));
      }

      #endregion -----------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------

      #region Public ----------------------------------------------------------------------------
      /// <summary>
      /// Create a grid for each page in the XML document and call object creating functions
      /// </summary>
      /// <param name="document">XML database with layout of character sheet</param>
      /// <param name="CharacterSheet_grd">Reference to CharacterSheet_grd</param>
      public void GetSheets(XDocument document, Grid CharacterSheet_grd)
      {
         CharacterSheet_grd.Children.Clear();

         foreach (XElement ePage in document.Descendants("page"))
         {
            Grid grd = new Grid();
            grd.Name = ePage.Element("menuTitle").Value.ToString() + "_grd";
            grd.Margin = new Thickness(0);
            grd.Visibility = Visibility.Collapsed;

            StackPanel wrppnl = new StackPanel();
            wrppnl.Name = ePage.Element("menuTitle").Value.ToString() + "_stckpnl";
            wrppnl.HorizontalAlignment = HorizontalAlignment.Left;
            wrppnl.VerticalAlignment = VerticalAlignment.Top;
            wrppnl.Orientation = Orientation.Vertical;

            if (ePage.Element("powers") != null)
            {
               // TODO
               
            }
            else
            {
               ResetLayoutValues();
               DiscectSheetXML(ePage, wrppnl);
            }
            grd.Children.Add(wrppnl);
            CharacterSheet_grd.Children.Add(grd);
         }
      }

      /// <summary>
      /// Setting values for all user fillable character sheet objects
      /// </summary>
      /// <param name="CharacterSheet_grd">Grid for Character sheet</param>
      public void InsertStats(Grid CharacterSheet_grd)
      {
         object objgrd = null;
         foreach (XElement ePage in Globals.TemporaryData.SelectedCharacterStats.Descendants("page"))
         {
            // Find grid of that ePage
            foreach (object obj in CharacterSheet_grd.Children)
            {
               if (obj.GetType().Name == "Grid")
               {
                  if ((obj as Grid).Name == ePage.Element("menuTitle").Value.ToString() + "_grd")
                  {
                     objgrd = obj;
                     break;
                  }
               }
            }
            // if grid exist, insert values
            if (objgrd != null)
            {
               // for stats and text
               foreach (XElement eValue in ePage.Descendants("value"))
               {
                  if (eValue.Attribute("id").Value.ToString().Contains("_cmbbx"))
                  {
                     ComboBox cmbbx = (ComboBox)FindGridElement((Panel)objgrd, eValue.Attribute("id").Value.ToString(), "ComboBox");
                     if (cmbbx != null)
                     {
                        if (cmbbx.Items.IndexOf(eValue.Element("start").Value.ToString()) == -1)
                        {
                           List<string> lItem = new List<string>();
                           foreach (string str in cmbbx.Items)
                              lItem.Add(str);
                           cmbbx.Items.Clear();
                           lItem.Add(eValue.Element("start").Value.ToString());
                           lItem.Sort();
                           lItem.ForEach(delegate(string itm)
                           {
                              cmbbx.Items.Add(itm);
                           });
                        }
                        cmbbx.SelectedItem = eValue.Element("start").Value;
                     }
                     else
                        throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());
                  }
                  else if (eValue.Attribute("id").Value.ToString().Contains("_txtbx"))
                  {
                     TextBox txtbx = (TextBox)FindGridElement((Panel)objgrd, eValue.Attribute("id").Value.ToString(), "TextBox");
                     if (txtbx != null)
                        txtbx.Text = eValue.Element("start").Value;
                     else
                        throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());
                  }
                  else if (eValue.Attribute("id").Value.ToString().Contains("_dt"))
                  {
                     Rating rtng = (Rating)FindGridElement((Panel)objgrd, eValue.Attribute("id").Value.ToString(), "Rating");
                     if (rtng != null)
                     {
                        if (eValue.Element("maxshow") != null)
                           rtng.ItemCount = int.Parse(eValue.Element("maxshow").Value);
                        rtng.Value = float.Parse(eValue.Element("start").Value) / rtng.ItemCount;
                     }
                     else
                        throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());
                  }
                  else if (eValue.Attribute("id").Value.ToString().Contains("_nmud"))
                  {
                     NumericUpDown nmricud = (NumericUpDown)FindGridElement((Panel)objgrd, eValue.Attribute("id").Value.ToString(), "NumericUpDown");
                     if (nmricud != null)
                     {
                        nmricud.Value = double.Parse(eValue.Element("start").Value);
                     }
                     else
                        throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());
                  }
                  else
                     throw new UnknownObjectException(eValue.Attribute("id").Value.ToString());
               }

               // for powers
               foreach (XElement eValue in ePage.Element("powers").Descendants(ePage.Element("menuTitle").Value.ToString()))
               {
               }
            }
         }
      }
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}