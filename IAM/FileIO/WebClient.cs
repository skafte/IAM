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

namespace FileIONamespace
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
            getCharacterStats, getCharacterPowers, getEmptyCharacterSheet
            
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
        public event fromWebClientHandler gotCharacterPowers;
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
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Characters/index_Characters.xml");
                    break;
                case "Get list of equipment":
                    FileProcessTask = (int)FileProcessEnum.getListOfEquipment;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Equipment/index_Equipment.xml");
                    break;
                case "Get list of powers":
                    FileProcessTask = (int)FileProcessEnum.getListOfPowers;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Powers/index_Powers.xml");
                    break;
                case "Get types of equipment":
                    FileProcessTask = (int)FileProcessEnum.getTypesOfEquipment;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Equipment/" + File + "/index_" + File + ".xml");
                    break;
                case "Get types of power":
                    FileProcessTask = (int)FileProcessEnum.getTypesOfPower;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Powers/" + File + "/index_" + File + ".xml");
                    break;
                // Character sheet informations
                case "Get character stats":
                    FileProcessTask = (int)FileProcessEnum.getCharacterStats;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Characters/" + File + ".xml");
                    break;
                case "Get character powers":
                    FileProcessTask = (int)FileProcessEnum.getCharacterPowers;
                    foreach (XElement ePowerName in document.Descendants("powers"))
                    {
                        string powerType = ePowerName.Attribute("type").Value.ToString();

                        // find single power types
                        List<string> singlePowerIndex = (from vTypes in ePowerName.Descendants(powerType)
                                                       select vTypes.Element("type").Value).Distinct().ToList();
                        if (singlePowerIndex.Contains(""))
                            singlePowerIndex.Remove("");                                                          // Cleanup of list
                        // find for all power types for that kind of powers
                        List<string> forallPowerIndex = Globals.GameInformation.PowerIndexForAll.ElementAt(Globals.GameInformation.PowerIndex.IndexOf(File));

                        // number of files to load
                        Globals.TemporaryData.FilesStillToLoad += singlePowerIndex.Count + forallPowerIndex.Count;

                        singlePowerIndex.ForEach(delegate(string str)
                        {
                            LoadFile("./" + Globals.GameInformation.SelectedGame + "/Powers/" + powerType + "/" + str + ".xml");
                        });
                        forallPowerIndex.ForEach(delegate(string str)
                        {
                            LoadFile("./" + Globals.GameInformation.SelectedGame + "/Powers/" + powerType + "/" + str + ".xml");
                        });
                    }
                    break;
                case "Get empty character sheet":
                    FileProcessTask = (int)FileProcessEnum.getEmptyCharacterSheet;
                    LoadFile("./" + Globals.GameInformation.SelectedGame + "/Sheets/" + File + ".xml");
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
                    case (int)FileProcessEnum.getCharacterPowers:
                        this.gotCharacterPowers(XDocument.Load(e.Result));
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
