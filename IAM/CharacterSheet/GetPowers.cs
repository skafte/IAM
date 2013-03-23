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

using IAM;
using IAM.FileIO;
using System.Collections.Generic;

namespace IAM.CharacterSheet
{
   public class GetPowers
   {
      #region Properties ______________________________________________________________________
      #region Private _____________________________________________________________________________
      private string categorySelected;
      private string skillSelected;
      private string treeSelected;
      private string subtreeSelected;
      private List<string> categoryCollection = new List<string>();
      private List<string> skillCollection = new List<string>();
      private List<string> treeCollection = new List<string>();
      private List<string> subtreeCollection = new List<string>();
      #endregion __________________________________________________________________________________
      #endregion ______________________________________________________________________________

      #region Methods _________________________________________________________________________
      #region Private _____________________________________________________________________________
      /// <summary>
      /// Will clean collections of power data
      /// It will fall down from a parent collection to all its children and clean them all
      /// </summary>
      /// <param name="AmountToClean">Name of collection to clean from</param>
      private void CleanCollections(string AmountToClean)
      {
         switch (AmountToClean)
         {
            case "user":
               categoryCollection.Clear();
               categorySelected = "";
               skillCollection.Clear();
               skillSelected = "";
               //dTypeCharms.Document.Element("body").ReplaceWith(new XElement("body", ""));
               goto case "category";
            case "category":
            case "skill":
               treeCollection.Clear();
               treeSelected = "";
               goto case "tree";
            case "tree":
               subtreeCollection.Clear();
               subtreeSelected = "";
               goto case "subtree";
            case "subtree":
            default:
               break;
         }
      }
      #endregion __________________________________________________________________________________

      #region Public ______________________________________________________________________________
      #region Character _______________________________________________________________________________
      /// <summary>
      /// Will find the specific powers belonging to the selected sheet
      /// </summary>
      /// <returns>true: if and crossRef powers are needed, else return false</returns>
      public void findCharacterPowers()
      {
         foreach (XElement eSheetPowerList in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers"))       // get all the power "pages" from the sheet
         {
            string powerType = eSheetPowerList.Attribute("type").Value;                                                 // get type of power on page
            foreach (XElement eFilePowers in Globals.TemporaryData.PowersXMLFiles)                                           // search each power file found
            {
               if (powerType == eFilePowers.Attribute("type").Value)                      // check if power type from file is the same as the current page
               {
                  foreach (XElement eSheetPower in eSheetPowerList.Descendants(powerType))
                  {
                     Globals.TemporaryData.SelectedCharacterPowers.Element("body").Add(from vPower in eFilePowers.Descendants(powerType)
                                                                                       where ((vPower.Element("name").Value == eSheetPower.Element("name").Value) &&
                                                                                              (vPower.Element("skill").Element("name").Value == eSheetPower.Element("skill").Value))
                                                                                       select vPower);
                  }
               }
            }
         }
      }

      /// <summary>
      /// Will find all needed crossRef powers and add them to SelectedCharacterPowers
      /// </summary>
      public void findCharacterCrossRefPowers()
      {
         XElement CrossRefPower = new XElement("body");

         foreach (string powerType in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers").Attributes("type"))              // get the power type name, for each type of power
         {
            XElement CrossRefPowers = new XElement("crossRefPowers_" + powerType);
            foreach (XElement eSheetPowerCrossRef in Globals.TemporaryData.SelectedCharacterPowers.Descendants(powerType).Elements("crossRef"))   // get each crossRef
            {
               foreach (XElement eFilePowers in Globals.TemporaryData.PowersXMLFiles)                                                     // search through all the XML data files after power matching the crossRef
               {
                  CrossRefPower.ReplaceAll(from vCrossRefPower in eFilePowers.Descendants(powerType)
                                           where ((eSheetPowerCrossRef.Element("user").Value == eFilePowers.Attribute("user").Value) &&
                                                  (eSheetPowerCrossRef.Element("name").Value == vCrossRefPower.Element("name").Value) &&
                                                  (eSheetPowerCrossRef.Element("skill").Value == vCrossRefPower.Element("skill").Element("name").Value))
                                           select vCrossRefPower);
                  if (CrossRefPower.Value != "")
                  {
                     CrossRefPower.Element(powerType).Add(new XElement("user", eFilePowers.Attribute("user").Value));
                     CrossRefPowers.Add(CrossRefPower.FirstNode);
                  }
               }
            }
            Globals.TemporaryData.SelectedCharacterPowers.Element("body").Add(CrossRefPowers);
         }
      }

      /// <summary>
      /// Will find all needed keywords and add them to SelectedCharacterPowers
      /// </summary>
      public void findCharacterPowerKeywords()
      {
         XElement PowerKeyword = new XElement("body");

         foreach (string powerType in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers").Attributes("type"))
         {
            XElement PowerKeywords = new XElement("powerKeywords_" + powerType);

            foreach (XElement eFileKeywords in Globals.TemporaryData.PowersXMLFiles)
            {
               if (eFileKeywords.Attribute("type").Value == powerType)
               {
                  ((from vKeywords in Globals.TemporaryData.SelectedCharacterPowers.Descendants("keyword")
                    select vKeywords.Value).Distinct().ToList()).ForEach(delegate(string keywordname)
                  {
                     PowerKeyword.ReplaceAll(from vKeywords in eFileKeywords.Descendants("keyword")
                                             where keywordname.Contains(vKeywords.Element("name").Value)
                                             select vKeywords);

                     if ((new XElement("body", (from vKeyword in PowerKeywords.Descendants("name")
                                                where (vKeyword.Value == PowerKeyword.Element("keyword").Element("name").Value)
                                                select vKeyword))).Value == "")
                        PowerKeywords.Add(PowerKeyword.FirstNode);
                  });
               }
            }

            Globals.TemporaryData.SelectedCharacterPowers.Element("body").Add(PowerKeywords);
         }
      }
      #endregion ______________________________________________________________________________________
      #region Library _________________________________________________________________________________

      public void UserIsSelected(string user)
      {
         CleanCollections("user");
      }
      public void CategoryIsSelected(string user)
      {
         CleanCollections("category");
      }
      public void SkillIsSelected(string user)
      {
         CleanCollections("skill");
      }
      public void TreeIsSelected(string user)
      {
         CleanCollections("tree");
      }
      public void SubtreeIsSelected(string user)
      {
         CleanCollections("subtree");
      }
      #endregion ______________________________________________________________________________________
      #endregion __________________________________________________________________________________
      #endregion ______________________________________________________________________________
   }
}
