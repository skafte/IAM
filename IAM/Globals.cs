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
      public static void ResetTemporaryData()
      {
         Globals.TemporaryData.FilesStillToLoad = 0;
         Globals.TemporaryData.PowersXMLFiles.Clear();
         Globals.TemporaryData.SelectedCharacterPowers.RemoveNodes();
         Globals.TemporaryData.SelectedCharacterStats.RemoveNodes();
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
      }

      /// <summary>
      /// Styles to put on objects
      /// </summary>
      public struct ObjectStyles
      {
         static private Style textbox = new Style();
         static private Style button_up = new Style();
         static private Style button_down = new Style();

         static public Style Textbox
         {
            get
            {
               return textbox;
            }
            set
            {
               textbox = value;
            }
         }
         static public Style Button_Up
         {
            get
            {
               return button_up;
            }
            set
            {
               button_up = value;
            }
         }
         static public Style Button_Down
         {
            get
            {
               return button_down;
            }
            set
            {
               button_down = value;
            }
         }
      }
      #endregion ----------------------------------------------------------------------------
   }
}