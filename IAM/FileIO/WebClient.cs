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
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

using IAM;
using System.IO;

namespace IAM.FileIO
{
   public class WebClientManager
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      //private List<string> forallFiles = new List<string>();

      // file to process properties
      enum FileProcessEnum
      {
      none,
      // Startup loading of informations
      getListOfGames, getListOfCharacters, getListOfEquipment, getListOfPowers,
      getTypesOfEquipment, getTypesOfPower,
      // Character sheet informations
      getCharacterStats, getCharacterPowerFiles, getCharacterPowerCrossRefs, getCharacterPowerKeywords, getEmptyCharacterSheet
            
      };
      int FileProcessTask;

      #endregion --------------------------------------------------------------------------------

      #region Events ----------------------------------------------------------------------------
      public delegate void fromWebClientHandler(XDocument document);

      #region Event Signatures ----------------------------------------------------------------------
      // Startup loading of informations
      public event fromWebClientHandler gotListOfGames;
      public event fromWebClientHandler gotListOfCharacters;
      public event fromWebClientHandler gotListOfEquipment;
      public event fromWebClientHandler gotListOfPowers;
      public event fromWebClientHandler gotTypesOfEquipment;
      public event fromWebClientHandler gotTypesOfPower;
      // Character sheet informations
      public event fromWebClientHandler gotCharacterStats;
      public event fromWebClientHandler gotCharacterPowerFiles;
      public event fromWebClientHandler gotCharacterPowerCrossRefs;
      public event fromWebClientHandler gotCharacterPowerKeywords;
      public event fromWebClientHandler gotEmptyCharacterSheet;
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Public ----------------------------------------------------------------------------
      /// <summary>
      /// Prepare, with path, file(s) and call LoadFile for async downloading
      /// </summary>
      /// <param name="Reason">What files to prepade</param>
      /// <param name="File">If a specific file within a group of alike files. Default: ""</param>
      /// <param name="document">Character stat XML, to get correct power files from. Default: null</param>
      public void PrepareFilePaths(string Reason, string File = "", XDocument document = null)
      {
         switch (Reason)
         {
            // Startup loading of informations
            case "Get list of games":
               FileProcessTask = (int)FileProcessEnum.getListOfGames;
               LoadFile("./index.xml");
               break;
            case "Get list of characters":
               FileProcessTask = (int)FileProcessEnum.getListOfCharacters;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/characters/index_characters.xml");
               break;
            case "Get list of equipment":
               FileProcessTask = (int)FileProcessEnum.getListOfEquipment;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/equipment/index_equipment.xml");
               break;
            case "Get list of powers":
               FileProcessTask = (int)FileProcessEnum.getListOfPowers;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/powers/index_powers.xml");
               break;
            case "Get types of equipment":
               FileProcessTask = (int)FileProcessEnum.getTypesOfEquipment;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/equipment/" + File + "/index_" + File + ".xml");
               break;
            case "Get types of power":
               FileProcessTask = (int)FileProcessEnum.getTypesOfPower;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/powers/" + File + "/index_" + File + ".xml");
               break;
            // Character sheet informations
            case "Get character stats":
               FileProcessTask = (int)FileProcessEnum.getCharacterStats;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/characters/" + File + ".xml");
               break;
            case "Get character powers":
               FileProcessTask = (int)FileProcessEnum.getCharacterPowerFiles;
               Globals.TemporaryData.FilesStillToLoad = 0;

               foreach (XElement ePowerName in document.Descendants("powers"))
               {
                  string powerType = ePowerName.Attribute("type").Value;

                  // find single power types
                  List<string> PowerIndex = (from vTypes in ePowerName.Descendants(powerType)
                                             select vTypes.Element("type").Value).Distinct().ToList();
                  PowerIndex.AddRange(Globals.GameInformation.PowerIndexForAll.ElementAt(Globals.GameInformation.PowerIndex.IndexOf(powerType)));  // find general powers
                  PowerIndex.Remove("");                                                          // Cleanup of list

                  // number of files to load
                  Globals.TemporaryData.FilesStillToLoad += PowerIndex.Count;

                  PowerIndex.ForEach(delegate(string str)
                  {
                     LoadFile("./" + Globals.GameInformation.SelectedGame + "/powers/" + powerType + "/" + str + ".xml");
                  });
               }
               break;
            case "Get character power crossRefs":
               FileProcessTask = (int)FileProcessEnum.getCharacterPowerCrossRefs;
               Globals.TemporaryData.FilesStillToLoad = 0;

               List<string> UserFiles = new List<string>();

               foreach (XElement eCrossRef in document.Descendants("crossRef"))
               {
                  string parentName = eCrossRef.Parent.Name.ToString();
                  string typeName = eCrossRef.Element("type").Value;
               
                  if (UserFiles.IndexOf(parentName + ": " + typeName) == -1)                                                     // file hasn't been called to load yet
                  {
                     UserFiles.Add(parentName + ": " + typeName);                                                                // add file to has been called to load list
                     Globals.TemporaryData.FilesStillToLoad++;                                                                   // NB. because FilesStillToLoad isn't set the exact number from the start it can potentially be counter down to 0 (and new new functions called) before all files are send to loading
                     LoadFile("./" + Globals.GameInformation.SelectedGame + "/powers/" + parentName + "/" + typeName + ".xml");
                  }
               }
               break;
            case "Get character power keywords":
               FileProcessTask = (int)FileProcessEnum.getCharacterPowerKeywords;
               Globals.TemporaryData.FilesStillToLoad = 0;

               //string powerType = "";
               foreach (string ePowerType in Globals.TemporaryData.SelectedCharacterStats.Descendants("powers").Attributes("type"))
               {
                  if ((from vKeywords in Globals.TemporaryData.SelectedCharacterPowers.Descendants(ePowerType).Elements("keyword")
                       select vKeywords.Value).ToList().Count > 0)
                  {
                     Globals.TemporaryData.FilesStillToLoad++;
                     LoadFile("./" + Globals.GameInformation.SelectedGame + "/mechanic/" + ePowerType + "_keywords.xml");
                  }
               }
               break;
            case "Get empty character sheet":
               FileProcessTask = (int)FileProcessEnum.getEmptyCharacterSheet;
               LoadFile("./" + Globals.GameInformation.SelectedGame + "/sheets/" + File + ".xml");
               break;
            default:
               throw new UnknownFileProcessException(Reason);
         }
      }

      /// <summary>
      /// Create async download
      /// </summary>
      /// <param name="file">File to download</param>
      private void LoadFile(string file)
      {
         WebClient LoadFile_wc = new WebClient();
         LoadFile_wc.OpenReadCompleted += LoadFile_wc_OpenReadCompleted;
         LoadFile_wc.OpenReadAsync(new Uri(file, UriKind.Relative), file);
      }

      /// <summary>
      /// Sends correct event when file has been downloaded
      /// </summary>
      private void LoadFile_wc_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
      {
         if (e.Error != null)
            throw new IAM.FileNotFoundException(Enum.GetName(typeof(FileProcessEnum), FileProcessTask), (string)e.UserState);
         else
         {
            switch (FileProcessTask)
            {
               // Startup loading of informations
               case (int)FileProcessEnum.getListOfGames:
                  this.gotListOfGames(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getListOfCharacters:
                  this.gotListOfCharacters(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getListOfEquipment:
                  this.gotListOfEquipment(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getTypesOfEquipment:
                  this.gotTypesOfEquipment(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getListOfPowers:
                  this.gotListOfPowers(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getTypesOfPower:
                  this.gotTypesOfPower(XDocument.Load(e.Result));
                  break;
               // Character sheet informations
               case (int)FileProcessEnum.getCharacterStats:
                  this.gotCharacterStats(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getCharacterPowerFiles:
                  this.gotCharacterPowerFiles(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getCharacterPowerCrossRefs:
                  this.gotCharacterPowerCrossRefs(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getCharacterPowerKeywords:
                  this.gotCharacterPowerKeywords(XDocument.Load(e.Result));
                  break;
               case (int)FileProcessEnum.getEmptyCharacterSheet:
                  this.gotEmptyCharacterSheet(XDocument.Load(e.Result));
                  break;
               default:
                  throw new UnknownFileProcessException(Enum.GetName(typeof(FileProcessEnum), FileProcessTask));
            }
         }
      }
      #endregion --------------------------------------------------------------------------------

      #region Private ---------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
