using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Xml.Linq;

using IAM.CharacterSheet;
using IAM.FileIO;
using IAM.Powers;

namespace IAM
{
   public partial class MainPage : UserControl
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      private CreateSheet clCreateSheet = new CreateSheet();
      private GetPowers clGetPowers = new GetPowers();
      private WebClientManager clWebClientManager = new WebClientManager();
      private DisplayPowers clDisplayPowers = new DisplayPowers();
      private LoadPowers clLoadPowers = new LoadPowers();
      private CreatePowerElement clCreatePowerElement = new CreatePowerElement();
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Default Behavior --------------------------------------------------------------
      public MainPage()
      {
         InitializeComponent();

         LoadingData_bsind.IsBusy = true;

         SetGUI();
         SetEvents();

         clWebClientManager.PrepareFilePaths("Get list of games");
      }

      private void SetGUI()
      {
         LoadingData_bsind.Visibility = Visibility.Visible;

         // Collaps all grids in Grid_grd, to make sure there ain't an visible grids from the design process
         //ShowCollapsGrids("", Grid_grd, false);
         //foreach (object obj in Grid_grd.Children)
         //{
         //   if (obj.GetType().Name == "Grid")
         //      ShowCollapsGrids("", (obj as Grid), false);
         //}

         //ShowCollapsGrids("GameSelection_grd", UserMenu_grd, true);
         //ShowCollapsMenues("All");
      }

      private void SetEvents()
      {
         // from WebClient
         // Startup loading of informations
         this.clWebClientManager.gotListOfGames += new WebClientManager.fromWebClientHandler(clWebClientManager_gotListOfGames);
         this.clWebClientManager.gotListOfCharacters += new WebClientManager.fromWebClientHandler(clWebClientManager_gotListOfCharacters);
         this.clWebClientManager.gotListOfEquipment += new WebClientManager.fromWebClientHandler(clWebClientManager_gotListOfEquipment);
         this.clWebClientManager.gotTypesOfEquipment += new WebClientManager.fromWebClientHandler(clWebClientManager_gotTypesOfEquipment);
         this.clWebClientManager.gotListOfPowers += new WebClientManager.fromWebClientHandler(clWebClientManager_gotListOfPowers);
         this.clWebClientManager.gotTypesOfPower += new WebClientManager.fromWebClientHandler(clWebClientManager_gotTypesOfPower);
         // Character sheet informations
         this.clWebClientManager.gotCharacterStats += new WebClientManager.fromWebClientHandler(clWebClientManager_gotCharacterStats);
         this.clWebClientManager.gotCharacterPowerFiles += new WebClientManager.fromWebClientHandler(clWebClientManager_gotCharacterPowerFiles);
         this.clWebClientManager.gotCharacterPowerCrossRefs += new WebClientManager.fromWebClientHandler(clWebClientManager_gotCharacterPowerCrossRefs);
         this.clWebClientManager.gotCharacterPowerKeywords += new WebClientManager.fromWebClientHandler(clWebClientManager_gotCharacterPowerKeywords);
         this.clWebClientManager.gotEmptyCharacterSheet += new WebClientManager.fromWebClientHandler(clWebClientManager_gotEmptyCharacterSheet);

         // from CreatePowerElement
         this.clCreatePowerElement.Version_btn_click += clCreatePowerElement_Version_btn_click;
      }
      #endregion ----------------------------------------------------------------------------

      #region Events ------------------------------------------------------------------------
      #region internal --------------------------------------------------------------------------
      /// <summary>
      /// Making sure all other Menu expanders except the selected one collapses
      /// </summary>
      private void Expanders_ExpandCollaps(object sender, System.Windows.RoutedEventArgs e)
      {
         if (sender is Expander)
         {
            foreach (object obj in ((sender as Expander).Parent as StackPanel).Children)
            {
               if (obj is Expander)
               {
                  if (obj != sender)
                     (obj as Expander).IsExpanded = false;
               }
            }
         }
      }
      #endregion --------------------------------------------------------------------------------

      #region from XAML -------------------------------------------------------------------------
      /// <summary>
      /// Open the grid corrosponding to the selected menu point
      /// </summary>
      //private void SecondaryMenu_lstbx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      //{
         //if (sender is ListBox)
         //{
         //   switch ((sender as ListBox).Name)
         //   {
         //      case "CharacterSheetMenu_lstbx":
         //         ShowCollapsGrids(CharacterSheetMenu_lstbx.SelectedItem.ToString() + "_grd", CharacterSheetOuter_grd, true);
         //         break;
         //      case "EquipmentLibraryMenu_lstbx":
         //         ShowCollapsGrids(EquipmentLibraryMenu_lstbx.SelectedItem.ToString() + "_grd", EquipmentsOuter_grd, true);
         //         break;
         //      case "PowerLibraryMenu_lstbx":
         //         ShowCollapsGrids(PowerLibraryMenu_lstbx.SelectedItem.ToString() + "_grd", PowersOuter_grd, true);
         //         break;
         //      default:
         //         break;
         //   }
         //}
      //}

      /// <summary>
      /// If selection name contains 'Selection' then the corrosponding grid will be opened
      /// else if it contains 'Library' then it will open that secondary menu
      /// </summary>
      //private void UserMenu_lstbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
      //{
         //if (UserMenu_lstbx.SelectedIndex != -1)
         //{
         //   if ((UserMenu_lstbx.SelectedItem as ListBoxItem).Name.Contains("Selection"))
         //      ShowCollapsGrids((UserMenu_lstbx.SelectedItem as ListBoxItem).Name.ToString() + "_grd", UserMenu_grd, true);
         //   else if ((UserMenu_lstbx.SelectedItem as ListBoxItem).Name.Contains("Library"))
         //      ShowCollapsMenues((UserMenu_lstbx.SelectedItem as ListBoxItem).Name.ToString() + "Menu_expndr");
         //}
      //}

      /// <summary>
      /// When a game has been selected, this will start the chain of data to be loaded in order to setup all the grids connected to that game
      /// </summary>
      private void GamesList_lstbx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         if ((sender as ListBox).SelectedIndex != -1)
         {
            LoadingData_bsind.IsBusy = true;

            Globals.GameInformation.SelectedGame = (sender as ListBox).SelectedValue.ToString();

            // first step in loading process after selecting a game
            clWebClientManager.PrepareFilePaths("Get list of characters");
         }
      }

      /// <summary>
      /// Event that will open the selected character sheet
      /// </summary>
      private void CharacterList_lstbx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         LoadingData_bsind.IsBusy = true;

         //string name = CharacterList_lstbx.SelectedValue.ToString();
         //CharacterName_txtbx.Text = "";                              // name on Character menu
         //for (int i = 0; i < name.Length; i++)
         //{
         //   if (!((name[i].ToString().Equals(" ")) && (name[i + 1].ToString().Equals("("))))                 // TODO: this will cause problems if no " (" exist
         //   {
         //      CharacterName_txtbx.Text += name[i] + "\n";
         //   }
         //   else
         //   {
         //      CharacterName_txtbx.Text = CharacterName_txtbx.Text.Remove(CharacterName_txtbx.Text.Length - 1);    // remove last /n
         //      break;
         //   }
         //}

         //clWebClientManager.PrepareFilePaths("Get character stats", CharacterList_lstbx.SelectedValue.ToString());
      }

      /// <summary>
      /// Event that will open a new, empty character sheet
      /// </summary>
      private void NewChar_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         // TODO: Create all the stuff for a new blank character
         throw new NotImplementedException();
      }

      #endregion --------------------------------------------------------------------------------

      #region from WebClient --------------------------------------------------------------------
      /// <summary>
      /// Games loaded from database
      /// Filling games into GameList_lstbx
      /// </summary>
      /// <param name="document">Index over games</param>
      private void clWebClientManager_gotListOfGames(XDocument document)
      {
         GameList_lstbx.Items.Clear();

         foreach (XElement eGame in document.Descendants("game"))
            GameList_lstbx.Items.Add(eGame.Value);
         
         LoadingData_bsind.IsBusy = false;
      }

      /// <summary>
      /// Characters loaded from database
      /// Filling Characters into CharacterList_lstbx
      /// </summary>
      /// <param name="document">Index over characters</param>
      private void clWebClientManager_gotListOfCharacters(XDocument document)
      {
         //CharacterList_lstbx.Items.Clear();

         //string itemToAdd;

         //foreach (XElement eChar in document.Descendants("character"))
         //{
         //   itemToAdd = eChar.Element("name").Value;

         //   if (eChar.Element("type") != null)
         //   {
         //      itemToAdd += " (";

         //      foreach (XElement eType in eChar.Descendants("type"))       // list all types (caste, clan...)
         //         itemToAdd += eType.Value + ", ";

         //      itemToAdd = itemToAdd.Remove(itemToAdd.Length - 2);         // remove last, inused, comma
         //      itemToAdd += ")";
         //   }

         //   CharacterList_lstbx.Items.Add(itemToAdd);
         //}

         //// next step in loading process after selecting a game
         //clWebClientManager.PrepareFilePaths("Get list of equipment");
      }

      /// <summary>
      /// Equipment list loaded form database
      /// Filling equipment list in EquipmentLibraryMenu_lstbx
      /// </summary>
      /// <param name="document">Index over equipment list</param>
      private void clWebClientManager_gotListOfEquipment(XDocument document)
      {
         //EquipmentLibraryMenu_lstbx.Items.Clear();

         //Globals.TemporaryData.FilesStillToLoad = document.Descendants("equipment").Count();
         //foreach (XElement eEquipment in document.Descendants("equipment"))
         //{
         //   clWebClientManager.PrepareFilePaths("Get types of equipment", eEquipment.Value);
         //   EquipmentLibraryMenu_lstbx.Items.Add(eEquipment.Value);
         //}
      }
      /// <summary>
      /// Specific equipment types loaded from database
      /// </summary>
      /// <param name="document">Index over specific equipment types</param>
      private void clWebClientManager_gotTypesOfEquipment(XDocument document)
      {
         List<string> single = (from vTypes in document.Descendants("singles").Elements("type")
                                select vTypes.Value).Distinct().ToList();
         List<string> forall = (from vTypes in document.Descendants("forall").Elements("type")
                                select vTypes.Value).Distinct().ToList();
         Globals.GameInformation.EquipmentIndex.Add(document.Element("body").Element("title").Value);
         Globals.GameInformation.EquipmentIndexSingle.Add(single);
         Globals.GameInformation.EquipmentIndexForAll.Add(forall);

         Globals.TemporaryData.FilesStillToLoad--;
         if (Globals.TemporaryData.FilesStillToLoad == 0)
            // last step in loading process after selecting a game
            clWebClientManager.PrepareFilePaths("Get list of powers");
      }

      /// <summary>
      /// Power list loaded from database
      /// Filling power list in PowerLibraryMenu_lstbx
      /// </summary>
      /// <param name="document">Index over power list</param>
      private void clWebClientManager_gotListOfPowers(XDocument document)
      {
         //PowerLibraryMenu_lstbx.Items.Clear();

         //Globals.TemporaryData.FilesStillToLoad = document.Descendants("power").Count();
         //foreach (XElement ePower in document.Descendants("power"))
         //{
         //   clWebClientManager.PrepareFilePaths("Get types of power", ePower.Value);
         //   PowerLibraryMenu_lstbx.Items.Add(ePower.Value);
         //}
      }
      /// <summary>
      /// Specific power types loaded from database
      /// </summary>
      /// <param name="document">Index over specific power types</param>
      private void clWebClientManager_gotTypesOfPower(XDocument document)
      {
         List<string> single = (from vTypes in document.Descendants("singles").Elements("type")
                                select vTypes.Value).Distinct().ToList();
         List<string> forall = (from vTypes in document.Descendants("forall").Elements("type")
                                select vTypes.Value).Distinct().ToList();
         Globals.GameInformation.PowerIndex.Add(document.Element("body").Element("title").Value);
         Globals.GameInformation.PowerIndexSingle.Add(single);
         Globals.GameInformation.PowerIndexForAll.Add(forall);

         Globals.TemporaryData.FilesStillToLoad--;
         if (Globals.TemporaryData.FilesStillToLoad == 0)
         {
            ShowCollapsMenues("UserMenu_expndr");
            LoadingData_bsind.IsBusy = false;
         }
      }

      /// <summary>
      /// Save stats for later use before calling for a sheet layout to that type of character
      /// </summary>
      /// <param name="document">XML with stats on character</param>
      private void clWebClientManager_gotCharacterStats(XDocument document)
      {
         Globals.ResetTemporaryData();
         Globals.TemporaryData.SelectedCharacterStats = document;

         if (document.Descendants("powers").Count() != 0)
            clWebClientManager.PrepareFilePaths("Get character powers", "", Globals.TemporaryData.SelectedCharacterStats);
         else
            clWebClientManager.PrepareFilePaths("Get empty character sheet", Globals.TemporaryData.SelectedCharacterStats.Element("body").Attribute("type").Value.ToString());
      }

      /// <summary>
      /// Get and temporarely store power data files
      /// Will call functions to process data files when all are loaded
      /// </summary>
      /// <param name="document">XML with powers</param>
      private void clWebClientManager_gotCharacterPowerFiles(XDocument document)
      {
         Globals.TemporaryData.PowersXMLFiles.Add(document.Element("body"));
         if (--Globals.TemporaryData.FilesStillToLoad == 0)
         {
            clGetPowers.findCharacterPowers();
            Globals.ResetTemporaryData("PowersXMLFiles");

            clWebClientManager.PrepareFilePaths("Get character power crossRefs", "", Globals.TemporaryData.SelectedCharacterPowers);
         }
      }

      /// <summary>
      /// Get and temporarely store power (crossRef) data files
      /// Will call functions to process data files when all are loaded
      /// </summary>
      /// <param name="document">XML with crossRef powers</param>
      void clWebClientManager_gotCharacterPowerCrossRefs(XDocument document)
      {
         Globals.TemporaryData.PowersXMLFiles.Add(document.Element("body"));
         if (--Globals.TemporaryData.FilesStillToLoad == 0)
         {
            clGetPowers.findCharacterCrossRefPowers();
            Globals.ResetTemporaryData("PowersXMLFiles");

            clWebClientManager.PrepareFilePaths("Get character power keywords", "", Globals.TemporaryData.SelectedCharacterPowers);
         }
      }

      /// <summary>
      /// Get and temporarely store power keyword data and will call function to process data
      /// </summary>
      /// <param name="document">XML with power keywords</param>
      private void clWebClientManager_gotCharacterPowerKeywords(XDocument document)
      {
         Globals.TemporaryData.PowersXMLFiles.Add(document.Element("body"));
         clGetPowers.findCharacterPowerKeywords();
         Globals.ResetTemporaryData("PowersXMLFiles");

         clWebClientManager.PrepareFilePaths("Get empty character sheet", Globals.TemporaryData.SelectedCharacterStats.Element("body").Attribute("type").Value.ToString());
      }

      /// <summary>
      /// Call CreateSheet and afterwards FillInCharacterStats, if stats exist
      /// </summary>
      /// <param name="document">XML with sheet layout data</param>
      private void clWebClientManager_gotEmptyCharacterSheet(XDocument document)
      {
         // fill in character sheet menu
         //CharacterSheetMenu_lstbx.Items.Clear();

         //foreach (XElement ePage in document.Descendants("page"))
         //   CharacterSheetMenu_lstbx.Items.Add(ePage.Element("menuTitle").Value);

         //// create sheet layout
         //clCreateSheet.GetEmptySheets(document, CharacterSheet_grd);

         //// fill in stats
         //if (Globals.TemporaryData.SelectedCharacterStats.ToString() != "")
         //   clCreateSheet.InsertStats(CharacterSheet_grd);

         //SheetFinished();
      }
      #endregion --------------------------------------------------------------------------------

      #region from CreatePowerElement -----------------------------------------------------------
      /// <summary>
      /// Trigger whenever one of the version buttons in a power element have been clicked
      /// </summary>
      /// <param name="sender">This button object</param>
      /// <param name="e"></param>
      private void clCreatePowerElement_Version_btn_click(object sender, RoutedEventArgs e)
      {
         throw new NotImplementedException();
      }

      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      #region ShowHide ------------------------------------------------------------------------------
      /// <summary>
      /// Collaps all child-grids in ParentGrid, except VisualGrid
      /// </summary>
      /// <param name="VisualGrid">Selected grid to show</param>
      /// <param name="ParentGrid">Parent grid</param>
      /// <param name="DoPrimaryGrid">Recursive ShowCollapsGrids with ParentGrid=Grid_grd and VisualGrid=ParentGrid</param>
      private void ShowCollapsGrids(string VisualGrid, Grid ParentGrid, bool DoPrimaryGrid)
      {
         //if (DoPrimaryGrid)
         //   ShowCollapsGrids(ParentGrid.Name.ToString(), Grid_grd, false);

         //foreach (object obj in ParentGrid.Children)
         //{
         //   if (obj.GetType().Name == "Grid")
         //   {
         //      if ((obj as Grid).Name == VisualGrid)
         //         (obj as Grid).Visibility = Visibility.Visible;
         //      else
         //         (obj as Grid).Visibility = Visibility.Collapsed;
         //   }
         //   else if ((obj.GetType().Name == "ScrollViewer") && (DoPrimaryGrid))
         //      ShowCollapsGrids(VisualGrid, ((obj as ScrollViewer).Content as Grid), false);
         //}
      }

      /// <summary>
      /// Collaps all Expander Menues.
      /// UserMenu_expndr will never be collaped
      /// </summary>
      /// <param name="VisualMenu">Expander menu not to collaps, if = 'All menues' UserMenu_expndr will also be collapsed</param>
      private void ShowCollapsMenues(string VisualMenu)
      {
         //foreach (object obj in MainMenu_stckpnl.Children)
         //{
         //   if (obj.GetType().Name == "Expander")
         //   {
         //      if ((obj as Expander).Name == VisualMenu)
         //      {
         //         (obj as Expander).IsExpanded = true;
         //         (obj as Expander).Visibility = Visibility.Visible;
         //      }
         //      else if (((obj as Expander).Name == "UserMenu_expndr") && (VisualMenu != "All menues"))
         //         (obj as Expander).Visibility = Visibility.Visible;
         //      else
         //         (obj as Expander).Visibility = Visibility.Collapsed;
         //   }
         //}
      }

      /// <summary>
      /// Display the first page of the character sheet after it is loaded
      /// </summary>
      private void SheetFinished()
      {
         //ShowCollapsMenues("CharacterSheetMenu_expndr");
         //CharacterSheetMenu_lstbx.SelectedIndex = 0;
         //LoadingData_bsind.IsBusy = false;
      }
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
