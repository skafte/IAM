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
      private string User;             // Name of user type
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      /// <summary>
      /// Collection of calls to all the insert data functions
      /// </summary>
      /// <param name="body_stckpnl">Power element</param>
      /// <param name="ePower">XML data for power</param>
      /// <param name="action">How to insert data: none=No existing data, add=add to already existing data, replace=replace existing data</param>
      #region Insert data ---------------------------------------------------------------------------
      private void DataIntoPowerBody_Intro(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBlock CreatorValue_txtblck = new TextBlock();
         TextBlock ReferenceValue_txtblck = new TextBlock();
         TextBlock CostValue_txtblck = new TextBlock();
         TextBlock DurationValue_txtblck = new TextBlock();
         TextBlock TypeValue_txtblck = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
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
                           CreatorValue_txtblck = (innerobj as TextBlock);
                           break;
                        case "ReferenceValue_txtblck":
                           ReferenceValue_txtblck = (innerobj as TextBlock);
                           break;
                        case "CostValue_txtblck":
                           CostValue_txtblck = (innerobj as TextBlock);
                           break;
                        case "DurationValue_txtblck":
                           DurationValue_txtblck = (innerobj as TextBlock);
                           break;
                        case "TypeValue_txtblck":
                           TypeValue_txtblck = (innerobj as TextBlock);
                           break;
                        default:
                           break;
                     }
                  }
               }
            }
         }

         // Insert data in objects
         string sCreator = "";
         string sReference = "";
         string sCost = "";
         string sDuration = "";
         string sType = "";
         if (ePower.Element("creator") != null)
            sCreator = ePower.Element("creator").Value;
         if (ePower.Element("reference") != null)
            sReference = ePower.Element("reference").Value;
         if (ePower.Element("cost") != null)
            sCost = ePower.Element("cost").Value;
         if (ePower.Element("duration") != null)
            sDuration = ePower.Element("duration").Value;
         if (ePower.Element("type") != null)
            sType = ePower.Element("type").Value;

         switch (action)
         {
            case "none":
               CreatorValue_txtblck.Text = "-";
               if (sCreator != "")
                  CreatorValue_txtblck.Text = sCreator;
               ReferenceValue_txtblck.Text = "-";
               if (sReference != "")
                  ReferenceValue_txtblck.Text = sReference;
               CostValue_txtblck.Text = "-";
               if (sCost != "")
                  CostValue_txtblck.Text = sCost;
               DurationValue_txtblck.Text = "-";
               if (sDuration != "")
                  DurationValue_txtblck.Text = sDuration;
               TypeValue_txtblck.Text = "-";
               if (sType != "")
                  TypeValue_txtblck.Text = sType;
               break;
            case "replace":
               CreatorValue_txtblck.Text = sCreator;
               ReferenceValue_txtblck.Text = sReference;
               CostValue_txtblck.Text = sCost;
               DurationValue_txtblck.Text = sDuration;
               TypeValue_txtblck.Text = sType;
               break;
            case "add":
               CreatorValue_txtblck.Text += " " + sCreator;
               ReferenceValue_txtblck.Text += " " + sReference;
               CostValue_txtblck.Text += " " + sCost;
               DurationValue_txtblck.Text += " " + sDuration;
               TypeValue_txtblck.Text += " " + sType;
               break;
            default:
               break;
         }
      }

      private void DataIntoPowerBody_Mins(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel Mins_wrppnl = new WrapPanel();
         TextBlock MinsTitle_txtblck = new TextBlock();
         TextBlock[] MinsName_txtblck = new TextBlock[amount];
         TextBlock[] MinsValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            MinsName_txtblck[i] = new TextBlock();
            MinsValue_txtblck[i] = new TextBlock();
         }

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Mins_wrppnl"))
            {
               Mins_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("skill").Any())
               {
                  foreach (object innerobj in Mins_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "MinsTitle_txtblck")
                           MinsTitle_txtblck = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MinsName_txtblck"))
                        {
                           try
                           {
                              MinsName_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MinsName_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MinsValue_txtblck"))
                        {
                           try
                           {
                              MinsValue_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MinsValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("skill").Any())
         {
            Mins_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
               {
                  MinsName_txtblck[i].Text = "";
                  MinsValue_txtblck[i].Text = "";
               }
            }

            int k = 0;
            MinsTitle_txtblck.Text = "Mins:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (MinsName_txtblck[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eSkill in ePower.Elements("skill"))
            {
               if (eSkill.Element("allowed") != null)
               {
                  foreach (XElement eSkillOK in eSkill.Element("allowed").Elements("ok"))
                     if (eSkillOK.Value == User)
                     {
                        if (k > 0)
                           MinsValue_txtblck[k - 1].Text += ",";
                        MinsName_txtblck[k].Text = eSkill.Element("name").Value;
                        MinsValue_txtblck[k].Text = eSkill.Element("value").Value;
                     }
               }
               else
               {
                  if (k > 0)
                     MinsValue_txtblck[k - 1].Text += ",";
                  MinsName_txtblck[k].Text = eSkill.Element("name").Value;
                  MinsValue_txtblck[k].Text = eSkill.Element("value").Value;
               }

               k++;
            }

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                  {
                     MinsName_txtblck[i].Text = "";
                     MinsValue_txtblck[i].Text = "";
                  }
               }
            }
         }
         else if (action == "none")
            Mins_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_Keyword(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel Keywords_wrppnl = new WrapPanel();
         TextBlock KeywordsTitle = new TextBlock();
         TextBlock[] KeywordsValue = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
            KeywordsValue[i] = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Keywords_wrppnl"))
            {
               Keywords_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("keyword").Any())
               {
                  foreach (object innerobj in Keywords_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "KeywordsTitle_txtblck")
                           KeywordsTitle = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("KeywordsValue_txtblck"))
                        {
                           try
                           {
                              KeywordsValue[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "KeywordsValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("keyword").Any())
         {
            Keywords_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
                  KeywordsValue[i].Text = "";
            }

            int k = 0;
            KeywordsTitle.Text = "Keywords:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (KeywordsValue[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eKeyword in ePower.Elements("keyword"))
            {
               if (k > 0)
                  KeywordsValue[k - 1].Text += ",";
               KeywordsValue[k].Text = eKeyword.Value;

               // set ToolTip
               XElement keyworddescription = new XElement("body");
               TextBlock tooltip_txtblck = new TextBlock();
               if (ePower.Name != "errata")
                  keyworddescription.Add((from vKeyword in Globals.TemporaryData.SelectedCharacterPowers.Element("body").Element("powerKeywords_" + ePower.Name.ToString()).Descendants("keyword")
                                          where eKeyword.Value.Contains(vKeyword.Element("name").Value)
                                          select vKeyword.Element("description").Value));
               else
                  keyworddescription.Add((from vKeyword in Globals.TemporaryData.SelectedCharacterPowers.Element("body").Element("powerKeywords_" + ePower.Parent.Name.ToString()).Descendants("keyword")
                                          where eKeyword.Value.Contains(vKeyword.Element("name").Value)
                                          select vKeyword.Element("description").Value));
               tooltip_txtblck.Text = keyworddescription.Value;
               tooltip_txtblck.MaxWidth = 400;
               tooltip_txtblck.TextWrapping = TextWrapping.Wrap;
               ToolTipService.SetToolTip(KeywordsValue[k], tooltip_txtblck);

               k++;
            }

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                     KeywordsValue[i].Text = "";
               }
            }
         }
         else if (action == "none")
            Keywords_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_Prerequest(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel Prerequest_wrppnl = new WrapPanel();
         TextBlock PrerequestTitle_txtblck = new TextBlock();
         TextBlock[] PrerequestValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
            PrerequestValue_txtblck[i] = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Prerequest_wrppnl"))
            {
               Prerequest_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("prerequest").Any())
               {
                  foreach (object innerobj in Prerequest_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "PrerequestTitle_txtblck")
                           PrerequestTitle_txtblck = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("PrerequestValue_txtblck"))
                        {
                           try
                           {
                              PrerequestValue_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "PrerequestValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("prerequest").Any())
         {
            Prerequest_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
                  PrerequestValue_txtblck[i].Text = "";
            }

            int k = 0;
            string sName, number;
            bool WasLastOr = false;
            PrerequestTitle_txtblck.Text = "Prerequest:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (PrerequestValue_txtblck[i].Text != "")
                     k++;
               }
            }

            foreach (XElement ePre in ePower.Elements("prerequest"))
            {
               number = "";
               if ((ePre.Element("number") != null) && (ePre.Element("number").Value != ""))
                  number = " (x" + ePre.Element("number").Value + ")";
               if ((ePre.Element("trouble") != null) && (ePre.Element("trouble").Element("replace") != null))
                  sName = ePre.Element("trouble").Element("replace").Value + number;
               else
                  sName = ePre.Element("name").Value + number;

               if ((ePre.Element("anyXof") != null) && (ePre.Element("anyXof").Value != ""))
               {
                  sName = "any " + ePre.Element("anyXof").Value + " of the following: " + sName;
                  if (WasLastOr)
                     InsertPrerequest(PrerequestValue_txtblck, k, sName, true);
                  else
                     InsertPrerequest(PrerequestValue_txtblck, k, sName, false);
                  WasLastOr = false;
               }
               else if ((ePre.Element("or") != null) && (ePre.Element("or").Value != ""))
               {
                  InsertPrerequest(PrerequestValue_txtblck, k, sName, false);
                  WasLastOr = true;
               }
               else
               {
                  if (WasLastOr)
                     InsertPrerequest(PrerequestValue_txtblck, k, sName, true);
                  else
                     InsertPrerequest(PrerequestValue_txtblck, k, sName, false);
                  WasLastOr = false;
               }
               k++;
            }
            if (WasLastOr)
               InsertPrerequest(PrerequestValue_txtblck, k, "", true);

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                     PrerequestValue_txtblck[i].Text = "";
               }
            }
         }
         else if (action == "none")
            Prerequest_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_Merged(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel Merged_wrppnl = new WrapPanel();
         TextBlock MergedTitle_txtblck = new TextBlock();
         TextBlock[] MergedValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
            MergedValue_txtblck[i] = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Merged_wrppnl"))
            {
               Merged_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("merged").Any())
               {
                  foreach (object innerobj in Merged_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "MergedTitle_txtblck")
                           MergedTitle_txtblck = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MergedValue_txtblck"))
                        {
                           try
                           {
                              MergedValue_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MergedValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("merged").Any())
         {
            Merged_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
                  MergedValue_txtblck[i].Text = "";
            }

            int k = 0;
            MergedTitle_txtblck.Text = "Merged:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (MergedValue_txtblck[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eMer in ePower.Elements("merged"))
            {
               if (k > 0)
                  MergedValue_txtblck[k - 1].Text += ",";
               MergedValue_txtblck[k].Text = eMer.Element("name").Value + " (" + eMer.Element("skill").Value + ")";

               k++;
            }

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                     MergedValue_txtblck[i].Text = "";
               }
            }
         }
         else if (action == "none")
            Merged_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_Martial(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel Martial_wrppnl = new WrapPanel();
         TextBlock MartialTitle_txtblck = new TextBlock();
         TextBlock[] MartialValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
            MartialValue_txtblck[i] = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Martial_wrppnl"))
            {
               Martial_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("martial").Any())
               {
                  foreach (object innerobj in Martial_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "MartialTitle_txtblck")
                           MartialTitle_txtblck = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MartialValue_txtblck"))
                        {
                           try
                           {
                              MartialValue_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MartialValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("martial").Any())
         {
            Martial_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
                  MartialValue_txtblck[i].Text = "";
            }

            int k = 0;
            MartialTitle_txtblck.Text = "Martial:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (MartialValue_txtblck[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eMar in ePower.Elements("martial"))
            {
               if (k > 0)
                  MartialValue_txtblck[k - 1].Text += ",";
               MartialValue_txtblck[k].Text = eMar.Value;

               k++;
            }

            if (action == "replace")
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                     MartialValue_txtblck[i].Text = "";
               }
            }
         }
         else if (action == "none")
            Martial_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_MartialReady(StackPanel body_stckpnl, XElement ePower, string action)
      {
         WrapPanel MartialReady_wrppnl = new WrapPanel();
         TextBlock MartialReadyTitle_txtblck = new TextBlock();
         TextBlock[] MartialReadyValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
            MartialReadyValue_txtblck[i] = new TextBlock();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "MartialReady_wrppnl"))
            {
               MartialReady_wrppnl = (outerobj as WrapPanel);

               if (ePower.Elements("martialReady").Any())
               {
                  foreach (object innerobj in MartialReady_wrppnl.Children)
                  {
                     if (innerobj.GetType().Name == "TextBlock")
                     {
                        if ((innerobj as TextBlock).Tag.ToString() == "MartialReadyTitle_txtblck")
                           MartialReadyTitle_txtblck = (innerobj as TextBlock);
                        else if ((innerobj as TextBlock).Tag.ToString().Contains("MartialReadyValue_txtblck"))
                        {
                           try
                           {
                              MartialReadyValue_txtblck[int.Parse((innerobj as TextBlock).Tag.ToString().Remove(0, "MartialReadyValue_txtblck".Length))] = (innerobj as TextBlock);
                           }
                           catch (Exception)
                           { }
                        }
                     }
                  }
               }

               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("martialReady").Any())
         {
            MartialReady_wrppnl.Visibility = Visibility.Visible;

            if (action == "none")                                                       // start by cleaning all values, if first created
            {
               for (int i = 0; i < amount; i++)
                  MartialReadyValue_txtblck[i].Text = "";
            }

            int k = 0;
            MartialReadyTitle_txtblck.Text = "Martial Ready:";

            if (action == "add")                                                    // find last entry, to add after
            {
               for (int i = 0; i < amount; i++)
               {
                  if (MartialReadyValue_txtblck[i].Text != "")
                     k++;
               }
            }

            foreach (XElement eMR in ePower.Elements("martialReady"))
            {
               if (k > 0)
                  MartialReadyValue_txtblck[k - 1].Text += ",";
               MartialReadyValue_txtblck[k].Text = eMR.Value;

               k++;
            }

            if (action == "replace")                                            // clear all old, leftover, entries
            {
               for (int i = 0; i < amount; i++)
               {
                  if (i >= k)
                     MartialReadyValue_txtblck[i].Text = "";
               }
            }
         }
         else if (action == "none")
            MartialReady_wrppnl.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_Description(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBox DescriptionText_txtbx = new TextBox();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "DescriptionText_txtbx"))
            {
               DescriptionText_txtbx = (outerobj as TextBox);
               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("description").Any())
         {
            switch (action)
            {
               case "none":
                  DescriptionText_txtbx.Text = "   " + ePower.Element("description").Value;
                  break;
               case "replace":
                  DescriptionText_txtbx.Text = "   " + ePower.Element("description").Value;
                  break;
               case "add":
                  DescriptionText_txtbx.Text += "\n" + ePower.Element("description").Value;
                  break;
               default:
                  break;
            }
         }
         else if (action == "none")
            DescriptionText_txtbx.Text = "-";
      }

      private void DataIntoPowerBody_Submodule(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBox SubmoduleText_txtbx = new TextBox();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "SubmoduleText_txtbx"))
            {
               SubmoduleText_txtbx = (outerobj as TextBox);
               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("submodule").Any())
         {
            SubmoduleText_txtbx.Visibility = Visibility.Visible;
            if (action != "add")
               SubmoduleText_txtbx.Text = "";

            foreach (XElement eSub in ePower.Elements("submodule"))
            {
               if (((action == "add") && (SubmoduleText_txtbx.Text != "")) || ((action != "add") && (eSub != ePower.Elements("submodule").First())))
                  SubmoduleText_txtbx.Text += "\n";

               SubmoduleText_txtbx.Text += "   " + eSub.Element("name").Value;
               if (((eSub.Elements("skill").Any()) && (eSub.Element("skill").Value != "")) || ((eSub.Elements("xpcost").Any()) && (eSub.Element("xpcost").Value != "")))
               {
                  SubmoduleText_txtbx.Text += " (";
                  foreach (XElement eMin in eSub.Elements("skill"))
                  {
                     if (eMin == eSub.Elements("skill").First())
                        SubmoduleText_txtbx.Text += eMin.Element("name").Value + " " + eMin.Element("value").Value;
                     else
                        SubmoduleText_txtbx.Text += ", " + eMin.Element("name").Value + " " + eMin.Element("value").Value;
                  }
                  if ((eSub.Elements("xpcost").Any()) && (eSub.Element("xpcost").Value != ""))
                  {
                     if ((eSub.Elements("skill").Any()) && (eSub.Element("skill").Value != ""))
                        SubmoduleText_txtbx.Text += ", ";
                     SubmoduleText_txtbx.Text += eSub.Element("xpcost").Value + "xp";
                  }
                  SubmoduleText_txtbx.Text += "): " + "\n";
               }
               else
                  SubmoduleText_txtbx.Text += "\n";

               if ((eSub.Elements("prerequest").Any()) && (eSub.Element("prerequest").Value != ""))
               {
                  SubmoduleText_txtbx.Text += "Prerequest: ";
                  foreach (XElement ePre in eSub.Elements("prerequest"))
                  {
                     SubmoduleText_txtbx.Text += ePre.Element("name").Value;
                     if (ePre != eSub.Elements("prerequest").Last())
                        SubmoduleText_txtbx.Text += ", ";
                  }
                  SubmoduleText_txtbx.Text += "\n";
               }
               SubmoduleText_txtbx.Text += eSub.Element("description").Value;
            }
         }
         else if (action == "none")
            SubmoduleText_txtbx.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_CrossRef(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBox MirrorText_txtbx = new TextBox();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "MirrorText_txtbx"))
            {
               MirrorText_txtbx = (outerobj as TextBox);
               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("crossRef").Any())
         {
            MirrorText_txtbx.Visibility = Visibility.Visible;

            foreach (XElement eCrossRef in ePower.Parent.Element("crossRefPowers_" + ePower.Name).Elements(ePower.Name))
            {
               if ((ePower.Element("crossRef").Element("type").Value == eCrossRef.Element("user").Value) &&
                   (ePower.Element("crossRef").Element("skill").Value == eCrossRef.Element("skill").Element("name").Value) &&
                   (ePower.Element("crossRef").Element("name").Value == eCrossRef.Element("name").Value))
               {
                  string mirrortext = "   " + eCrossRef.Element("description").Value;

                  foreach (XElement eErrata in eCrossRef.Elements("errata"))
                  {
                     if (eErrata.Element("description") != null)
                     {
                        switch (eErrata.Element("todo").Value)
                        {
                           case "replace":
                              mirrortext = "   " + eErrata.Element("description").Value;
                              break;
                           case "add":
                              mirrortext += "\n   " + eErrata.Element("description").Value;
                              break;
                           default:
                              break;
                        }
                     }
                     if (eErrata.Element("errText") != null)
                        mirrortext += "\n" + "Errata text: " + eErrata.Element("errText").Value;
                  }

                  if (MirrorText_txtbx.Text != "")          // if multiple crossrefs
                     mirrortext = "\n" + mirrortext;
                  MirrorText_txtbx.Text += mirrortext;
               }
            }
         }
         else if (action == "none")
            MirrorText_txtbx.Visibility = Visibility.Collapsed;
      }

      private void DataIntoPowerBody_ErrataTxtVersion(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBlock VersionObject_txtblck = new TextBlock();
         TextBox ErrataText_txtbx = new TextBox();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "WrapPanel") && ((outerobj as WrapPanel).Tag.ToString() == "Intro_wrppnl"))
            {
               foreach (object innerobj in (outerobj as WrapPanel).Children)
               {
                  if ((innerobj.GetType().Name == "TextBlock") && ((innerobj as TextBlock).Tag.ToString() == "VersionOfValue_txtblck"))
                  {
                     VersionObject_txtblck = (innerobj as TextBlock);
                     break;
                  }
               }
            }
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "ErrataText_txtbx"))
               ErrataText_txtbx = (outerobj as TextBox);
         }

         // Insert data in objects
         if (action != "none")
            VersionObject_txtblck.Text = ePower.Element("date").Value;

         if (ePower.Elements("errText").Any())
         {
            if (action != "add")
               ErrataText_txtbx.Text = "";

            foreach (XElement eErrata in ePower.Elements("errText"))
            {
               if (ErrataText_txtbx.Text != "")
                  ErrataText_txtbx.Text += "\n";
               ErrataText_txtbx.Text += "Errata text: " + eErrata.Value;
            }

            if (ErrataText_txtbx.Text == "")
               ErrataText_txtbx.Visibility = Visibility.Collapsed;
            else
               ErrataText_txtbx.Visibility = Visibility.Visible;
         }
         else if (action == "none")
         {
            VersionObject_txtblck.Text = "Original text";
            ErrataText_txtbx.Visibility = Visibility.Collapsed;
         }
      }

      private void DataIntoPowerBody_Comment(StackPanel body_stckpnl, XElement ePower, string action)
      {
         TextBox CommentText_txtbx = new TextBox();

         // find objects to use
         foreach (object outerobj in body_stckpnl.Children)
         {
            if ((outerobj.GetType().Name == "TextBox") && ((outerobj as TextBox).Tag.ToString() == "CommentText_txtbx"))
            {
               CommentText_txtbx = (outerobj as TextBox);
               break;
            }
         }

         // Insert data in objects
         if (ePower.Elements("comment").Any())
         {
            if (action != "add")
               CommentText_txtbx.Text = "";

            foreach (XElement eComment in ePower.Elements("comment"))
            {
               if (CommentText_txtbx.Text != "")
                  CommentText_txtbx.Text += "\n";
               CommentText_txtbx.Text += "Comment: " + eComment.Value;
            }

            if (CommentText_txtbx.Text == "")
               CommentText_txtbx.Visibility = Visibility.Collapsed;
            else
               CommentText_txtbx.Visibility = Visibility.Visible;
         }
         else if (action == "none")
            CommentText_txtbx.Visibility = Visibility.Collapsed;
      }
      #endregion ------------------------------------------------------------------------------------

      #region helper functions ----------------------------------------------------------------------
      /// <summary>
      /// Collection of calls to all the insert data functions
      /// </summary>
      /// <param name="body_stckpnl">Power element</param>
      /// <param name="ePower">XML data for power</param>
      /// <param name="action">How to insert data: none=No existing data, add=add to already existing data, replace=replace existing data</param>
      private void InsertPowerInfomation(StackPanel body_stckpnl, XElement ePower, string action)
      {
         DataIntoPowerBody_Intro(body_stckpnl, ePower, action);
         DataIntoPowerBody_Mins(body_stckpnl, ePower, action);
         DataIntoPowerBody_Keyword(body_stckpnl, ePower, action);
         DataIntoPowerBody_Prerequest(body_stckpnl, ePower, action);
         DataIntoPowerBody_Merged(body_stckpnl, ePower, action);
         DataIntoPowerBody_Martial(body_stckpnl, ePower, action);
         DataIntoPowerBody_MartialReady(body_stckpnl, ePower, action);
         DataIntoPowerBody_Description(body_stckpnl, ePower, action);
         DataIntoPowerBody_Submodule(body_stckpnl, ePower, action);
         DataIntoPowerBody_CrossRef(body_stckpnl, ePower, action);
         DataIntoPowerBody_ErrataTxtVersion(body_stckpnl, ePower, action);
         DataIntoPowerBody_Comment(body_stckpnl, ePower, action);
      }

      /// <summary>
      /// Insert extra words inbetween prerequests if they exist
      /// </summary>
      /// <param name="PrerequestValue">Array of all prerequest TextBlocks</param>
      /// <param name="k">Number currently selected prerequest TextBlock</param>
      /// <param name="sName">Prerequest text</param>
      /// <param name="LastOfOr">Are there any more prerequsts to this list of "or" prerequests</param>
      private static void InsertPrerequest(TextBlock[] PrerequestValue, int k, string sName, bool LastOfOr)
      {
         if ((k > 1) && (LastOfOr))
         {
            PrerequestValue[k - 2].Text = PrerequestValue[k - 2].Text.Remove(PrerequestValue[k - 2].Text.Length - 1) + " or";
            if (sName != "")
               PrerequestValue[k - 1].Text += ". And:";
         }
         else if (k > 0)
            PrerequestValue[k - 1].Text += ",";
         PrerequestValue[k].Text = sName;
      }
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------

      #region Public ----------------------------------------------------------------------------
      /// <summary>
      /// Function that will fill in all the fields in a power element
      /// </summary>
      /// <param name="expndr">Power element</param>
      /// <param name="ePower">XML data for power</param>
      /// <param name="user">Name of user type</param>
      public void InsertPowerInformationController(Expander expndr, XElement ePower, string user)
      {
         User = user;

         // set header
         (expndr.Header as Label).Content = ePower.Element("name").Value;

         // set body
         StackPanel body_stckpnl = new StackPanel();
         body_stckpnl = (expndr.Content as StackPanel);
         InsertPowerInfomation(body_stckpnl, ePower, "none");

         foreach (XElement eErrata in ePower.Elements("errata"))
         {
            InsertPowerInfomation(body_stckpnl, eErrata, eErrata.Element("todo").Value);
         }
      }
      #endregion ---------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
