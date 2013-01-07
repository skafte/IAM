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

namespace IAM
{
   static class Globals
   {
      #region Methods -----------------------------------------------------------------------
      /// <summary>
      /// Will clear all data from Globals.TemporaryData
      /// This function is first called after a character sheet is actually found, in case the sheet doesn't exist anyway
      /// </summary>
      public static void ResetTemporaryData(string varToReset = "")
      {
         if ((varToReset == "FilesStillToLoad") || (varToReset == ""))
            Globals.TemporaryData.FilesStillToLoad = 0;
         if ((varToReset == "PowersXMLFiles") || (varToReset == ""))
            Globals.TemporaryData.PowersXMLFiles.Clear();
         if ((varToReset == "SelectedCharacterStats") || (varToReset == ""))
            Globals.TemporaryData.SelectedCharacterStats.RemoveNodes();
         if ((varToReset == "SelectedCharacterPowers") || (varToReset == ""))
         {
            Globals.TemporaryData.SelectedCharacterPowers.RemoveNodes();
            Globals.TemporaryData.SelectedCharacterPowers.Add(new XElement("body"));
         }
      }

      /// <summary>
      /// Used to convert from GUI font size (points) to WPF xaml size (pixel)
      /// </summary>
      /// <param name="pt">point size to convert</param>
      /// <returns>converted to pixel</returns>
      public static float PtToPx(int pt = 1)
      {
         float px = (float)pt * 96/72;
         return px;
      }
      #endregion ----------------------------------------------------------------------------

      #region Properties --------------------------------------------------------------------
      /// <summary>
      /// Path to folders corrently in use
      /// </summary>
      public struct GameInformation
      {
         static private string selectedgame = "";
         static private List<string> equipmentindex = new List<string>();
         static private List<List<string>> equipmentindexsingle = new List<List<string>>();
         static private List<List<string>> equipmentindexforall = new List<List<string>>();
         static private List<string> powerindex = new List<string>();
         static private List<List<string>> powerindexsingle = new List<List<string>>();
         static private List<List<string>> powerindexforall = new List<List<string>>();

         /// <summary>
         /// Game selected from UserMenu_grd -> GameSelection_grd
         /// </summary>
         static public string SelectedGame
         {
            get
            {
               return selectedgame;
            }
            set
            {
               selectedgame = value;
            }
         }

         /// <summary>
         /// List of equipment index
         /// </summary>
         static public List<string> EquipmentIndex
         {
            get
            {
               return equipmentindex;
            }
            set
            {
               equipmentindex = value;
            }
         }
         /// <summary>
         /// List of equipment index's to single types
         /// </summary>
         static public List<List<string>> EquipmentIndexSingle
         {
            get
            {
               return equipmentindexsingle;
            }
            set
            {
               equipmentindexsingle = value;
            }
         }
         /// <summary>
         /// List of equipment index's for all types
         /// </summary>
         static public List<List<string>> EquipmentIndexForAll
         {
            get
            {
               return equipmentindexforall;
            }
            set
            {
               equipmentindexforall = value;
            }
         }

         /// <summary>
         /// List of powers index
         /// </summary>
         static public List<string> PowerIndex
         {
            get
            {
               return powerindex;
            }
            set
            {
               powerindex = value;
            }
         }
         /// <summary>
         /// List of powers index's to single types
         /// </summary>
         static public List<List<string>> PowerIndexSingle
         {
            get
            {
               return powerindexsingle;
            }
            set
            {
               powerindexsingle = value;
            }
         }
         /// <summary>
         /// List of power index's for all types
         /// </summary>
         static public List<List<string>> PowerIndexForAll
         {
            get
            {
               return powerindexforall;
            }
            set
            {
               powerindexforall = value;
            }
         }
      }

      /// <summary>
      /// Information that is needed across classes for a limited time
      /// </summary>
      public struct TemporaryData
      {
         static private XDocument selectedcharacterstats = new XDocument();
         static private int filesstilltoload = 0;
         static private List<XElement> powersxmlfiles = new List<XElement>();
         static private XDocument selectedcharacterpowers = new XDocument();

         static private string selectedpowerlibrary = "";

         /// <summary>
         /// Used for temporary storing of data while fetching the sheet layout for that character type
         /// </summary>
         static public XDocument SelectedCharacterStats
         {
            get
            {
               return selectedcharacterstats;
            }
            set
            {
               selectedcharacterstats = value;
            }
         }

         /// <summary>
         /// Number of files still in 'not loaded' as part of a given task
         /// </summary>
         static public int FilesStillToLoad
         {
            get
            {
               return filesstilltoload;
            }
            set
            {
               filesstilltoload = value;
            }
         }

         /// <summary>
         /// XML data files containing character powers
         /// </summary>
         static public List<XElement> PowersXMLFiles
         {
            get
            {
               return powersxmlfiles;
            }
            set
            {
               powersxmlfiles = value;
            }
         }

         /// <summary>
         /// Used for temporary storing of data while fetching the sheet layout for that characters type
         /// </summary>
         static public XDocument SelectedCharacterPowers
         {
            get
            {
               return selectedcharacterpowers;
            }
            set
            {
               selectedcharacterpowers = value;
            }
         }

         /// <summary>
         /// Used for temporary storing of data while fetching and creating power library
         /// </summary>
         static public string SelectedPowerLibrary
         {
            get
            {
               return selectedpowerlibrary;
            }
            set
            {
               selectedpowerlibrary = value;
            }
         }
      }
      #endregion ----------------------------------------------------------------------------
   }
}