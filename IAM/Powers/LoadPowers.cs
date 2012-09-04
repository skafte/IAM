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

namespace IAM.Powers
{
   public class LoadPowers
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      private int amount = 10;
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      private static void DataIntoPowerBody_Intro(StackPanel pwrd_stckpnl, XElement eC, string action)
      {
         TextBlock CreatorValue = new TextBlock();
         TextBlock ReferenceValue = new TextBlock();
         TextBlock CostValue = new TextBlock();
         TextBlock DurationValue = new TextBlock();
         TextBlock TypeValue = new TextBlock();

         foreach (object outerobj in pwrd_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && (((outerobj as WrapPanel).Tag.ToString() == "Intro_wrppnl") || ((outerobj as WrapPanel).Tag.ToString() == "CostDurationType_wrppnl")))
            {
               foreach (object innerobj in (outerobj as WrapPanel).Children)
               {
                  if (innerobj.GetType().Name == "TextBlock")
                  {
                     switch ((innerobj as TextBlock).Tag.ToString())
                     {
                        case "CreatorValue_txtblck":
                           CreatorValue = (innerobj as TextBlock);
                           break;
                        case "ReferenceValue_txtblck":
                           ReferenceValue = (innerobj as TextBlock);
                           break;
                        case "CostValue_txtblck":
                           CostValue = (innerobj as TextBlock);
                           break;
                        case "DurationValue_txtblck":
                           DurationValue = (innerobj as TextBlock);
                           break;
                        case "TypeValue_txtblck":
                           TypeValue = (innerobj as TextBlock);
                           break;
                        default:
                           break;
                     }
                  }
               }
            }
         }

         switch (action)
         {
            case "none":
               try
               {
                  CreatorValue.Text = eC.Element("creator").Value;
               }
               catch
               {
                  CreatorValue.Text = "-";
               }
               try
               {
                  ReferenceValue.Text = eC.Element("reference").Value;
               }
               catch
               {
                  ReferenceValue.Text = "-";
               }
               try
               {
                  CostValue.Text = eC.Element("cost").Value;
               }
               catch
               {
                  CostValue.Text = "-";
               }
               try
               {
                  DurationValue.Text = eC.Element("duration").Value;
               }
               catch
               {
                  DurationValue.Text = "-";
               }
               try
               {
                  TypeValue.Text = eC.Element("type").Value;
               }
               catch
               {
                  TypeValue.Text = "-";
               }
               break;
            case "replace":
               try
               {
                  CreatorValue.Text = eC.Element("creator").Value;
               }
               catch { }
               try
               {
                  ReferenceValue.Text = eC.Element("reference").Value;
               }
               catch { }
               try
               {
                  CostValue.Text = eC.Element("cost").Value;
               }
               catch { }
               try
               {
                  DurationValue.Text = eC.Element("duration").Value;
               }
               catch { }
               try
               {
                  TypeValue.Text = eC.Element("type").Value;
               }
               catch { }
               break;
            case "add":
               try
               {
                  CreatorValue.Text += " " + eC.Element("creator").Value;
               }
               catch { }
               try
               {
                  ReferenceValue.Text += " " + eC.Element("reference").Value;
               }
               catch { }
               try
               {
                  CostValue.Text += " " + eC.Element("cost").Value;
               }
               catch { }
               try
               {
                  DurationValue.Text += " " + eC.Element("duration").Value;
               }
               catch { }
               try
               {
                  TypeValue.Text += " " + eC.Element("type").Value;
               }
               catch { }
               break;
            default:
               break;
         }
      }

      private void DataIntoPowerBody_Mins(StackPanel pwrd_stckpnl, XElement eC, string action)
      {
         if (eC.Elements("skill").Any())
         {
            TextBlock MinsTitle = new TextBlock();
            TextBlock[] MinsName = new TextBlock[amount];
            TextBlock[] MinsValue = new TextBlock[amount];
            for (int i = 0; i < amount; i++)
            {
               MinsName[i] = new TextBlock();
               MinsValue[i] = new TextBlock();
            }

            foreach (object outerobj in pwrd_stckpnl.Children)
            {
               if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Mins_wrppnl"))
               {
                  foreach (object innerobj in (outerobj as WrapPanel).Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "MinsTitle_txtblck")
                           MinsTitle = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MinsName_txtblck"))
                        {
                           try
                           {
                              MinsName[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MinsName_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MinsValue_txtblck"))
                        {
                           try
                           {
                              MinsValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MinsValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }

                  (outerobj as WrapPanel).Visibility = Visibility.Visible;
                  break;
               }
            }

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
               {
                  MinsName[i].Text = "";
                  MinsValue[i].Text = "";
               }
            }

            int k = 0;
            MinsTitle.Text = "Mins:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (MinsName[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eSkill in eC.Elements("skill"))
            {
               if (eSkill.Element("allowed") != null)
               {
                  string User = (new XElement("body", (from vUser in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers")
                                 where vUser.Attribute("type").Value == eC.Name.ToString()
                                 select vUser.Attribute("user").Value))).Value;

                  foreach (XElement eSkillOK in eSkill.Element("allowed").Elements("ok"))
                     if (eSkillOK.Value == User)
                     {
                        if (k > 0)
                           MinsValue[k - 1].Text += ",";
                        MinsName[k].Text = eSkill.Element("name").Value;
                        MinsValue[k].Text = eSkill.Element("value").Value;
                     }
               }
               else
               {
                  if (k > 0)
                     MinsValue[k - 1].Text += ",";
                  MinsName[k].Text = eSkill.Element("name").Value;
                  MinsValue[k].Text = eSkill.Element("value").Value;
               }

               k++;
            }

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                  {
                     MinsName[i].Text = "";
                     MinsValue[i].Text = "";
                  }
               }
            }
         }
         else if (action == "none")
         {
            foreach (object outerobj in pwrd_stckpnl.Children)
            {
               if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Mins_wrppnl"))
               {
                  (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
                  break;
               }
            }
         }
      }

      //private void DataIntoPowerBody_Keyword(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("keyword").Any())
      //   {
      //      TextBlock KeywordsTitle = new TextBlock();
      //      TextBlock[] KeywordsValue = new TextBlock[amount];
      //      for (int i = 0; i < amount; i++)
      //         KeywordsValue[i] = new TextBlock();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Keywords_wrppnl"))
      //         {
      //            foreach (object innerobj in (outerobj as WrapPanel).Children)
      //            {
      //               if (innerobj.GetType().Name == "TextBlock")
      //               {
      //                  if ((innerobj as TextBlock).Tag.ToString() == "KeywordsTitle_txtblck")
      //                     KeywordsTitle = (innerobj as TextBlock);
      //                  else if ((innerobj as TextBlock).Tag.ToString().Contains("KeywordsValue_txtblck"))
      //                  {
      //                     try
      //                     {
      //                        KeywordsValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "KeywordsValue_txtblck".Length))] = (innerobj as TextBlock);
      //                     }
      //                     catch (Exception)
      //                     { }
      //                  }
      //               }
      //            }

      //            (outerobj as WrapPanel).Visibility = Visibility.Visible;
      //            break;
      //         }
      //      }

      //      if (action == "none")                                                       // start by cleaning all values, if first created
      //      {
      //         for (int i = 0; i < amount; i++)
      //            KeywordsValue[i].Text = "";
      //      }

      //      int k = 0;
      //      KeywordsTitle.Text = "Keywords:";

      //      if (action == "add")                                                    // find last entry, to add after
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (KeywordsValue[i].Text != "")
      //               k++;
      //         }
      //      }

      //      foreach (XElement eKey in eC.Elements("keyword"))
      //      {
      //         if (k > 0)
      //            KeywordsValue[k - 1].Text += ",";
      //         KeywordsValue[k].Text = eKey.Value;

      //         k++;
      //      }

      //      if (action == "replace")
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (i >= k)
      //               KeywordsValue[i].Text = "";
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Keywords_wrppnl"))
      //         {
      //            (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private void DataIntoPowerBody_Prerequest(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("prerequest").Any())
      //   {
      //      TextBlock PrerequestTitle = new TextBlock();
      //      TextBlock[] PrerequestValue = new TextBlock[amount];
      //      for (int i = 0; i < amount; i++)
      //         PrerequestValue[i] = new TextBlock();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Prerequest_wrppnl"))
      //         {
      //            foreach (object innerobj in (outerobj as WrapPanel).Children)
      //            {
      //               if (innerobj.GetType().Name == "TextBlock")
      //               {
      //                  if ((innerobj as TextBlock).Tag.ToString() == "PrerequestTitle_txtblck")
      //                     PrerequestTitle = (innerobj as TextBlock);
      //                  else if ((innerobj as TextBlock).Tag.ToString().Contains("PrerequestValue_txtblck"))
      //                  {
      //                     try
      //                     {
      //                        PrerequestValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "PrerequestValue_txtblck".Length))] = (innerobj as TextBlock);
      //                     }
      //                     catch (Exception)
      //                     { }
      //                  }
      //               }
      //            }

      //            (outerobj as WrapPanel).Visibility = Visibility.Visible;
      //            break;
      //         }
      //      }

      //      if (action == "none")                                                       // start by cleaning all values, if first created
      //      {
      //         for (int i = 0; i < amount; i++)
      //            PrerequestValue[i].Text = "";
      //      }

      //      int k = 0;
      //      string sName, number;
      //      bool WasLastOr = false;
      //      PrerequestTitle.Text = "Prerequest:";

      //      if (action == "add")                                                    // find last entry, to add after
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (PrerequestValue[i].Text != "")
      //               k++;
      //         }
      //      }

      //      foreach (XElement ePre in eC.Elements("prerequest"))
      //      {
      //         number = "";
      //         if ((ePre.Element("number") != null) && (ePre.Element("number").Value != ""))
      //            number = " (x" + ePre.Element("number").Value + ")";
      //         if ((ePre.Element("trouble") != null) && (ePre.Element("trouble").Element("replace") != null))
      //            sName = ePre.Element("trouble").Element("replace").Value + number;
      //         else
      //            sName = ePre.Element("name").Value + number;

      //         if ((ePre.Element("anyXof") != null) && (ePre.Element("anyXof").Value != ""))
      //         {
      //            sName = "any " + ePre.Element("anyXof").Value + " of the following: " + sName;
      //            if (WasLastOr)
      //               InsertPrerequest(PrerequestValue, k, sName, true);
      //            else
      //               InsertPrerequest(PrerequestValue, k, sName, false);
      //            WasLastOr = false;
      //         }
      //         else if ((ePre.Element("or") != null) && (ePre.Element("or").Value != ""))
      //         {
      //            InsertPrerequest(PrerequestValue, k, sName, false);
      //            WasLastOr = true;
      //         }
      //         else
      //         {
      //            if (WasLastOr)
      //               InsertPrerequest(PrerequestValue, k, sName, true);
      //            else
      //               InsertPrerequest(PrerequestValue, k, sName, false);
      //            WasLastOr = false;
      //         }
      //         k++;
      //      }
      //      if (WasLastOr)
      //         InsertPrerequest(PrerequestValue, k, "", true);

      //      if (action == "replace")
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (i >= k)
      //               PrerequestValue[i].Text = "";
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Prerequest_wrppnl"))
      //         {
      //            (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private void DataIntoPowerBody_Merged(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("merged").Any())
      //   {
      //      TextBlock MergedTitle = new TextBlock();
      //      TextBlock[] MergedValue = new TextBlock[amount];
      //      for (int i = 0; i < amount; i++)
      //         MergedValue[i] = new TextBlock();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Merged_wrppnl"))
      //         {
      //            foreach (object innerobj in (outerobj as WrapPanel).Children)
      //            {
      //               if (innerobj.GetType().Name == "TextBlock")
      //               {
      //                  if ((innerobj as TextBlock).Tag.ToString() == "MergedTitle_txtblck")
      //                     MergedTitle = (innerobj as TextBlock);
      //                  else if ((innerobj as TextBlock).Tag.ToString().Contains("MergedValue_txtblck"))
      //                  {
      //                     try
      //                     {
      //                        MergedValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MergedValue_txtblck".Length))] = (innerobj as TextBlock);
      //                     }
      //                     catch (Exception)
      //                     { }
      //                  }
      //               }
      //            }

      //            (outerobj as WrapPanel).Visibility = Visibility.Visible;
      //            break;
      //         }
      //      }

      //      if (action == "none")                                                       // start by cleaning all values, if first created
      //      {
      //         for (int i = 0; i < amount; i++)
      //            MergedValue[i].Text = "";
      //      }

      //      int k = 0;
      //      MergedTitle.Text = "Merged:";

      //      if (action == "add")                                                    // find last entry, to add after
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (MergedValue[i].Text != "")
      //               k++;
      //         }
      //      }

      //      foreach (XElement eMer in eC.Elements("merged"))
      //      {
      //         if (k > 0)
      //            MergedValue[k - 1].Text += ",";
      //         MergedValue[k].Text = eMer.Element("name").Value + " (" + eMer.Element("skill").Value + ")";

      //         k++;
      //      }

      //      if (action == "replace")
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (i >= k)
      //               MergedValue[i].Text = "";
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Merged_wrppnl"))
      //         {
      //            (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private void DataIntoPowerBody_Martial(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("martial").Any())
      //   {
      //      TextBlock MartialTitle = new TextBlock();
      //      TextBlock[] MartialValue = new TextBlock[amount];
      //      for (int i = 0; i < amount; i++)
      //         MartialValue[i] = new TextBlock();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Martial_wrppnl"))
      //         {
      //            foreach (object innerobj in (outerobj as WrapPanel).Children)
      //            {
      //               if (innerobj.GetType().Name == "TextBlock")
      //               {
      //                  if ((innerobj as TextBlock).Tag.ToString() == "MartialTitle_txtblck")
      //                     MartialTitle = (innerobj as TextBlock);
      //                  else if ((innerobj as TextBlock).Tag.ToString().Contains("MartialValue_txtblck"))
      //                  {
      //                     try
      //                     {
      //                        MartialValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MartialValue_txtblck".Length))] = (innerobj as TextBlock);
      //                     }
      //                     catch (Exception)
      //                     { }
      //                  }
      //               }
      //            }

      //            (outerobj as WrapPanel).Visibility = Visibility.Visible;
      //            break;
      //         }
      //      }

      //      if (action == "none")                                                       // start by cleaning all values, if first created
      //      {
      //         for (int i = 0; i < amount; i++)
      //            MartialValue[i].Text = "";
      //      }

      //      int k = 0;
      //      MartialTitle.Text = "Martial:";

      //      if (action == "add")                                                    // find last entry, to add after
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (MartialValue[i].Text != "")
      //               k++;
      //         }
      //      }

      //      foreach (XElement eMar in eC.Elements("martial"))
      //      {
      //         if (k > 0)
      //            MartialValue[k - 1].Text += ",";
      //         MartialValue[k].Text = eMar.Value;

      //         k++;
      //      }

      //      if (action == "replace")
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (i >= k)
      //               MartialValue[i].Text = "";
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Martial_wrppnl"))
      //         {
      //            (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private void DataIntoPowerBody_MartialReady(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("martialReady").Any())
      //   {
      //      TextBlock MartialReadyTitle = new TextBlock();
      //      TextBlock[] MartialReadyValue = new TextBlock[amount];
      //      for (int i = 0; i < amount; i++)
      //         MartialReadyValue[i] = new TextBlock();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "MartialReady_wrppnl"))
      //         {
      //            foreach (object innerobj in (outerobj as WrapPanel).Children)
      //            {
      //               if (innerobj.GetType().Name == "TextBlock")
      //               {
      //                  if ((innerobj as TextBlock).Tag.ToString() == "MartialReadyTitle_txtblck")
      //                     MartialReadyTitle = (innerobj as TextBlock);
      //                  else if ((innerobj as TextBlock).Tag.ToString().Contains("MartialReadyValue_txtblck"))
      //                  {
      //                     try
      //                     {
      //                        MartialReadyValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MartialReadyValue_txtblck".Length))] = (innerobj as TextBlock);
      //                     }
      //                     catch (Exception)
      //                     { }
      //                  }
      //               }
      //            }

      //            (outerobj as WrapPanel).Visibility = Visibility.Visible;
      //            break;
      //         }
      //      }

      //      if (action == "none")                                                       // start by cleaning all values, if first created
      //      {
      //         for (int i = 0; i < amount; i++)
      //            MartialReadyValue[i].Text = "";
      //      }

      //      int k = 0;
      //      MartialReadyTitle.Text = "Martial Ready:";

      //      if (action == "add")                                                    // find last entry, to add after
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (MartialReadyValue[i].Text != "")
      //               k++;
      //         }
      //      }

      //      foreach (XElement eMR in eC.Elements("martialReady"))
      //      {
      //         if (k > 0)
      //            MartialReadyValue[k - 1].Text += ",";
      //         MartialReadyValue[k].Text = eMR.Value;

      //         k++;
      //      }

      //      if (action == "replace")                                            // clear all old, leftover, entries
      //      {
      //         for (int i = 0; i < amount; i++)
      //         {
      //            if (i >= k)
      //               MartialReadyValue[i].Text = "";
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "MartialReady_wrppnl"))
      //         {
      //            (outerobj as WrapPanel).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      private static void DataIntoPowerBody_Description(StackPanel pwrd_stckpnl, XElement eC, string action)
      {
         TextBox DescriptionText = new TextBox();

         foreach (object outerobj in pwrd_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "DescriptionText_txtbx"))
            {
               DescriptionText = (outerobj as TextBox);
               break;
            }
         }

         if (eC.Elements("description").Any())
         {
            switch (action)
            {
               case "none":
                  DescriptionText.Text = "   " + eC.Element("description").Value;
                  break;
               case "replace":
                  DescriptionText.Text = "   " + eC.Element("description").Value;
                  break;
               case "add":
                  DescriptionText.Text += "\n" + eC.Element("description").Value;
                  break;
               default:
                  break;
            }
         }
         else if (action == "none")
            DescriptionText.Text = "-";
      }

      //private static void DataIntoPowerBody_Submodule(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("submodule").Any())
      //   {
      //      TextBox SubmoduleText = new TextBox();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "SubmoduleText_txtbx"))
      //         {
      //            SubmoduleText = (outerobj as TextBox);
      //            break;
      //         }
      //      }
      //      SubmoduleText.Visibility = Visibility.Visible;
      //      if (action != "add")
      //         SubmoduleText.Text = "";

      //      foreach (XElement eSub in eC.Elements("submodule"))
      //      {
      //         if (((action == "add") && (SubmoduleText.Text != "")) || ((action != "add") && (eSub != eC.Elements("submodule").First())))
      //            SubmoduleText.Text += "\n";

      //         SubmoduleText.Text += "   " + eSub.Element("name").Value;
      //         if (((eSub.Elements("skill").Any()) && (eSub.Element("skill").Value != "")) || ((eSub.Elements("xpcost").Any()) && (eSub.Element("xpcost").Value != "")))
      //         {
      //            SubmoduleText.Text += " (";
      //            foreach (XElement eMin in eSub.Elements("skill"))
      //            {
      //               if (eMin == eSub.Elements("skill").First())
      //                  SubmoduleText.Text += eMin.Element("name").Value + " " + eMin.Element("value").Value;
      //               else
      //                  SubmoduleText.Text += ", " + eMin.Element("name").Value + " " + eMin.Element("value").Value;
      //            }
      //            if ((eSub.Elements("xpcost").Any()) && (eSub.Element("xpcost").Value != ""))
      //            {
      //               if ((eSub.Elements("skill").Any()) && (eSub.Element("skill").Value != ""))
      //                  SubmoduleText.Text += ", ";
      //               SubmoduleText.Text += eSub.Element("xpcost").Value + "xp";
      //            }
      //            SubmoduleText.Text += "): " + "\n";
      //         }
      //         else
      //            SubmoduleText.Text += "\n";

      //         if ((eSub.Elements("prerequest").Any()) && (eSub.Element("prerequest").Value != ""))
      //         {
      //            SubmoduleText.Text += "Prerequest: ";
      //            foreach (XElement ePre in eSub.Elements("prerequest"))
      //            {
      //               SubmoduleText.Text += ePre.Element("name").Value;
      //               if (ePre != eSub.Elements("prerequest").Last())
      //                  SubmoduleText.Text += ", ";
      //            }
      //            SubmoduleText.Text += "\n";
      //         }
      //         SubmoduleText.Text += eSub.Element("description").Value;
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "SubmoduleText_txtbx"))
      //         {
      //            (outerobj as TextBox).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private static void DataIntoPowerBody_CrossRef(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("crossRef").Any())
      //   {
      //      TextBox MirrorText = new TextBox();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "MirrorText_txtbx"))
      //         {
      //            MirrorText = (outerobj as TextBox);
      //            break;
      //         }
      //      }
      //      MirrorText.Visibility = Visibility.Visible;

      //      foreach (XElement eCrossRef in eC.Parent.Elements("powercrossRef"))
      //      {
      //         if (eC.Element("crossRef").Element("name").Value == eCrossRef.Element(Globals.PowerXML.PowerName).Element("name").Value)
      //         {
      //            MirrorText.Text = "   " + eCrossRef.Element(Globals.PowerXML.PowerName).Element("description").Value;

      //            foreach (XElement eErrata in eCrossRef.Element(Globals.PowerXML.PowerName).Elements("errata"))
      //            {
      //               switch (eErrata.Element("todo").Value)
      //               {
      //                  case "replace":
      //                     if (eErrata.Element("description") != null)
      //                        MirrorText.Text = "   " + eErrata.Element("description").Value;
      //                     break;
      //                  case "add":
      //                     if (eErrata.Element("description") != null)
      //                        MirrorText.Text += "\n" + "   " + eErrata.Element("description").Value;
      //                     break;
      //                  default:
      //                     break;
      //               }

      //               if (eErrata.Element("errText") != null)
      //                  MirrorText.Text += "\n" + "Errata text: " + eErrata.Element("errText").Value;
      //            }
      //         }
      //      }
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "MirrorText_txtbx"))
      //         {
      //            (outerobj as TextBox).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private static void DataIntoPowerBody_Errata(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("errText").Any())
      //   {
      //      TextBox ErrataText = new TextBox();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "ErrataText_txtbx"))
      //         {
      //            ErrataText = (outerobj as TextBox);
      //            break;
      //         }
      //      }

      //      if (action != "add")
      //         ErrataText.Text = "";

      //      foreach (XElement eErrata in eC.Elements("errText"))
      //      {
      //         if (ErrataText.Text != "")
      //            ErrataText.Text += "\n";
      //         ErrataText.Text += "Errata text: " + eErrata.Value;
      //      }

      //      if (ErrataText.Text == "")
      //         ErrataText.Visibility = Visibility.Collapsed;
      //      else
      //         ErrataText.Visibility = Visibility.Visible;
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "ErrataText_txtbx"))
      //         {
      //            (outerobj as TextBox).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}

      //private static void DataIntoPowerBody_Comment(StackPanel pwrd_stckpnl, XElement eC, string action)
      //{
      //   if (eC.Elements("comment").Any())
      //   {
      //      TextBox CommentText = new TextBox();

      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "CommentText_txtbx"))
      //         {
      //            CommentText = (outerobj as TextBox);
      //            break;
      //         }
      //      }

      //      if (action != "add")
      //         CommentText.Text = "";

      //      foreach (XElement eComment in eC.Elements("comment"))
      //      {
      //         if (CommentText.Text != "")
      //            CommentText.Text += "\n";
      //         CommentText.Text += "Comment: " + eComment.Value;
      //      }

      //      if (CommentText.Text == "")
      //         CommentText.Visibility = Visibility.Collapsed;
      //      else
      //         CommentText.Visibility = Visibility.Visible;
      //   }
      //   else if (action == "none")
      //   {
      //      foreach (object outerobj in pwrd_stckpnl.Children)
      //      {
      //         if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "CommentText_txtbx"))
      //         {
      //            (outerobj as TextBox).Visibility = Visibility.Collapsed;
      //            break;
      //         }
      //      }
      //   }
      //}
      #endregion --------------------------------------------------------------------------------

      #region Public ----------------------------------------------------------------------------
      public void InsertPowerInformation(Expander expndr, XElement ePower)
      {
         // set header
         (expndr.Header as Label).Content = ePower.Element("name").Value;

         string action = "none";       // er ikke sikker påhvad denne string er til endnu

         // set body
         StackPanel body_stckpnl = new StackPanel();
         body_stckpnl = (expndr.Content as StackPanel);

         DataIntoPowerBody_Intro(body_stckpnl, ePower, action);
         DataIntoPowerBody_Mins(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Keyword(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Prerequest(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Merged(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Martial(body_stckpnl, ePower, action);
         //DataIntoPowerBody_MartialReady(body_stckpnl, ePower, action);
         DataIntoPowerBody_Description(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Submodule(body_stckpnl, ePower, action);
         //DataIntoPowerBody_CrossRef(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Errata(body_stckpnl, ePower, action);
         //DataIntoPowerBody_Comment(body_stckpnl, ePower, action);
      }
      #endregion ---------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
