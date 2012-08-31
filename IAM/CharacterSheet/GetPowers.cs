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

namespace IAM.CharacterSheet
{
   public class GetPowers
   {
      public void findCharacterPowers()
      {
         // get all the power "pages" from the sheet
         XElement sheetPowerList = new XElement("body", from vPower in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers")
                                                        select vPower);


         XElement ePowers = new XElement("body");

         foreach (XElement eSheetPowerList in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers"))       // get all the power "pages" from the sheet
         {
            string powerType = eSheetPowerList.Attribute("type").Value;                                                 // get type of power on page
            foreach (XElement eFilePowers in Globals.TemporaryData.PowersXML)                                           // search each power file found
            {
               if (powerType == eFilePowers.Attribute("type").Value)                      // check if power type from file is the same as the current page
               {
                  foreach (XElement eSheetPower in eSheetPowerList.Descendants(powerType))
                  {
                     ePowers.Add(from vPower in eFilePowers.Descendants(powerType)
                                 where ((vPower.Element("name").Value == eSheetPower.Element("name").Value) &&
                                        (vPower.Element("skill").Element("name").Value == eSheetPower.Element("skill").Value))
                                 select vPower);
                  }
               }
            }
         }



          //Globals.TemporaryData.PowersXML
      }
   }
}
