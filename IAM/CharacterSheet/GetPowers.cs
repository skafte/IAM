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
         XElement CrossRefPowers = new XElement("crossRefPowers");
         XElement CrossRefPower = new XElement("body");

         foreach (string powerType in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers").Attributes("type"))              // get the power type name, for each type of power
         {
            foreach (XElement eSheetPowerCrossRef in Globals.TemporaryData.SelectedCharacterPowers.Descendants(powerType).Elements("crossRef"))   // get each crossRef
            {
               foreach (XElement eFilePowers in Globals.TemporaryData.PowersXMLFiles)                                                     // search through all the XML data files after power matching the crossRef
               {
                  CrossRefPower.ReplaceAll(from vCrossRefPower in eFilePowers.Descendants(powerType)
                                           where ((eSheetPowerCrossRef.Element("type").Value == eFilePowers.Attribute("user").Value) &&
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
         }
         Globals.TemporaryData.SelectedCharacterPowers.Element("body").Add(CrossRefPowers);
      }

      /// <summary>
      /// Will find all needed keywords and add them to SelectedCharacterPowers
      /// </summary>
      public void findCharacterPowerKeywords()
      {

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
                     PowerKeywords.Add(from vKeywords in eFileKeywords.Descendants("keyword")
                                       where keywordname.Contains(vKeywords.Element("name").Value)
                                       select vKeywords);
                  });
               }
            }

            Globals.TemporaryData.SelectedCharacterPowers.Element("body").Add(PowerKeywords);
         }
      }
   }
}
