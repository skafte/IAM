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
         SetData();

         clWebClientManager.PrepareFilePaths("Get list of games");
      }

      private void SetGUI()
      {
         LoadingData_bsind.Visibility = Visibility.Visible;

         // Collaps all grids except the menu, to make sure there ain't an visible grids from the design process
         ShowCollapsOuterGrids(UserMenuOuter_grd.Name, LayoutRoot);
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

      private void SetData()
      {
         AppBar_grd.Tag = 1;
         CharmBar_grd.Tag = 2;
         NavigationBar_grd.Tag = 1;
         Unused_Bar_grd.Tag = 0;
      }
      #endregion ----------------------------------------------------------------------------

      #region Events ------------------------------------------------------------------------
      #region from XAML -------------------------------------------------------------------------
      /// <summary>
      /// Jumps back to last grid - or at least that is what it is suppose to do, doesn't do it yet
      /// </summary>
      private void Back_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         ShowCollapsOuterGrids(UserMenuOuter_grd.Name, LayoutRoot);
         // TODO: should probably do something more than just go back to Home
      }

      /// <summary>
      /// Open side-bar
      /// </summary>
      private void BarCollapsed_grd_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
      {
         var grd = ((sender as Grid).Parent as Grid).FindName((sender as Grid).Name.Replace("Collapsed", ""));

         if ((int)(grd as Grid).Tag >= 2)             // might need to be a higher number
            (grd as Grid).Visibility = Visibility.Visible;
      }

      /// <summary>
      /// Close side-bar
      /// </summary>
      private void Bar_grd_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
      {
         (sender as Grid).Visibility = Visibility.Collapsed;
      }

      /// <summary>
      /// Go to Search menu
      /// </summary>
      private void CharmSearch_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Go to settings menu
      /// </summary>
      private void CharmSettings_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         throw new NotImplementedException();
      }

      /// <summary>
      /// Will jump to grid for selected button
      /// </summary>
      /// <param name="sender">Navigation button clicked</param>
      /// <param name="e"></param>
      private void NagivationBar_btn_click(object sender, System.Windows.RoutedEventArgs e)
      {
         LoadingData_bsind.IsBusy = true;
         ShowCollapsOuterGrids((sender as Button).Tag.ToString() + "Outer_grd", LayoutRoot);
         LoadingData_bsind.IsBusy = false;
      }

      /// <summary>
      /// Hide selection menu panel and show Unhide button
      /// </summary>
      /// <param name="sender">Used to find panel/button to hide/show</param>
      private void Selection_Hide_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         ((sender as Button).Parent as Grid).Visibility = Visibility.Collapsed;
         ((((sender as Button).Parent as Grid).Parent as Grid).FindName((sender as Button).Name.Replace("Hide", "Unhide")) as Button).Visibility = Visibility.Visible;         
      }

      /// <summary>
      /// Unhide selection menu panel and hide Unhdie button
      /// </summary>
      /// <param name="sender">Used to find panel/button to show/hide</param>
      private void Selection_Unhide_btn_Click(object sender, System.Windows.RoutedEventArgs e)
      {
         (sender as Button).Visibility = Visibility.Collapsed;
         (((sender as Button).Parent as Grid).FindName((sender as Button).Name.Replace("Unhide_btn", "grd")) as Grid).Visibility = Visibility.Visible;
      }

      /// <summary>
      /// Open the grid corrosponding to the selected menu point
      /// </summary>
      private void SecondaryMenu_lstbx_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
      {
         if (sender is ListBox)
         {
            switch ((sender as ListBox).Name)
            {
               case "CharacterSheetMenu_lstbx":
                  ShowCollapsOuterGrids(CharacterSheetMenu_lstbx.SelectedItem.ToString() + "_grd", CharacterSheetInner_grd);
                  break;
               default:
                  break;
            }
         }
      }

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
      /// Event that will switch to the right []Outer_grd and open the clicked page
      /// </summary>
      /// <param name="sender">Clicked on button</param>
      private void UserMenu_btn_Clicked(object sender, RoutedEventArgs e)
      {
         LoadingData_bsind.IsBusy = true;

         if (AddNavigationBarButton(sender))
         {
            switch (((sender as Button).Parent as WrapPanel).Name)
            {
               case "CharacterSheetMenu_wrppnl":
                  UserMenu_Character(sender);
                  break;
               case "PowerLibraryMenu_wrppnl":
                  UserMenu_Power(sender);
                  break;
               case "EquipmentLibraryMenu_wrppnl":
                  UserMenu_Equipment(sender);
                  break;
               default:
                  LoadingData_bsind.IsBusy = false;
                  throw new IAM.UnknownObjectException("This Object should not exist: " + sender.ToString());
            }
         }
         else
            NagivationBar_btn_click(sender, e);
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
         CharacterMenu_grd.Visibility = Visibility.Visible;
         CharacterSheetMenu_wrppnl.Children.Clear();

         foreach (XElement eCharacter in document.Descendants("character"))
            CreateAndFillButton(eCharacter, CharacterSheetMenu_wrppnl);

         // next step in loading process after selecting a game
         clWebClientManager.PrepareFilePaths("Get list of equipment");
      }

      /// <summary>
      /// Equipment list loaded form database
      /// Filling equipment list in EquipmentLibraryMenu_lstbx
      /// </summary>
      /// <param name="document">Index over equipment list</param>
      private void clWebClientManager_gotListOfEquipment(XDocument document)
      {
         EquipmentMenu_grd.Visibility = Visibility.Visible;
         EquipmentLibraryMenu_wrppnl.Children.Clear();

         Globals.TemporaryData.FilesStillToLoad = document.Descendants("equipment").Count();
         foreach (XElement eEquipment in document.Descendants("equipment"))
         {
            CreateAndFillButton(eEquipment, EquipmentLibraryMenu_wrppnl);

            clWebClientManager.PrepareFilePaths("Get types of equipment", eEquipment.Element("name").Value);
         }
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
            clWebClientManager.PrepareFilePaths("Get list of powers");        // last step in loading process after selecting a game
      }

      /// <summary>
      /// Power list loaded from database
      /// Filling power list in PowerLibraryMenu_lstbx
      /// </summary>
      /// <param name="document">Index over power list</param>
      private void clWebClientManager_gotListOfPowers(XDocument document)
      {
         PowerMenu_grd.Visibility = Visibility.Visible;
         PowerLibraryMenu_wrppnl.Children.Clear();

         Globals.TemporaryData.FilesStillToLoad = document.Descendants("power").Count();
         foreach (XElement ePower in document.Descendants("power"))
         {
            CreateAndFillButton(ePower, PowerLibraryMenu_wrppnl);

            clWebClientManager.PrepareFilePaths("Get types of power", ePower.Element("name").Value);
         }
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
            LoadingData_bsind.IsBusy = false;
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
         CharacterSheetMenu_lstbx.Items.Clear();

         foreach (XElement ePage in document.Descendants("page"))
            CharacterSheetMenu_lstbx.Items.Add(ePage.Element("menuTitle").Value);

         // create sheet layout
         clCreateSheet.GetEmptySheets(document, CharacterSheetInner_grd);

         // fill in stats
         if (Globals.TemporaryData.SelectedCharacterStats.ToString() != "")
            clCreateSheet.InsertStats(CharacterSheetInner_grd);

         SheetFinished();
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
      /// <summary>
      /// Create and set text of a menu button
      /// </summary>
      /// <param name="eButtonText">XElement button text</param>
      /// <returns>The finished button element</returns>
      private void CreateAndFillButton(XElement eButtonText, WrapPanel ParentPanel)
      {
         // get characters name and stuff
         string itemToAdd = eButtonText.Element("name").Value.ToString();
         itemToAdd = ToUpperCaseProper(itemToAdd);

         // insert extra info
         if (eButtonText.Element("type") != null)
         {
            itemToAdd += "\n";

            foreach (XElement eType in eButtonText.Descendants("type"))       // list all types (caste, clan...)
               itemToAdd += eType.Value + ", ";
            itemToAdd = itemToAdd.Remove(itemToAdd.Length - 2);         // remove last, inused, comma
         }

         // create button
         TextBox txtbx = new TextBox();
         txtbx.Style = (Application.Current.Resources["Custom_TextBoxStyle_Generic"] as Style);
         txtbx.Text = itemToAdd;
         Button btn = new Button();
         btn.Style = (Application.Current.Resources["Custom_ButtonStyle_Generic"] as Style);
         btn.Content = txtbx;
         btn.Click += new RoutedEventHandler(UserMenu_btn_Clicked);

         //create gridname
         if (ParentPanel.Name.ToString() != "CharacterSheetMenu_wrppnl")
         {
            string gridname = itemToAdd.Replace("\n", "_");
            gridname = gridname.Replace(", ", "_");
            gridname = gridname.Replace(" ", "_");
            btn.Tag = gridname;
         }
         else
            btn.Tag = "CharacterSheet";

         ParentPanel.Children.Add(btn);
      }

      private static string ToUpperCaseProper(string toConvert)
      {
         char FirstLetter = ' ';
         string haveConverted = "";
         foreach (char SecondLetter in toConvert)
         {
            if (FirstLetter == ' ')
               haveConverted += SecondLetter.ToString().ToUpper();
            else
               haveConverted += SecondLetter.ToString();
            FirstLetter = SecondLetter;
         }
         return haveConverted;
      }

      /// <summary>
      /// Display the first page of the character sheet after it is loaded
      /// </summary>
      private void SheetFinished()
      {
         ShowCollapsOuterGrids(CharacterSheetOuter_grd.Name, LayoutRoot);
         CharacterSheetMenu_lstbx.SelectedIndex = 0;
         LoadingData_bsind.IsBusy = false;
      }

      private bool AddNavigationBarButton(object sender)
      {
         if ((from vButton in NavigationBar_stckpnl.Children                              // make sure button doesn't already exist
              where (((vButton as Button).Content as TextBox).Text == ((sender as Button).Content as TextBox).Text)
              select vButton).ToList().Count == 0)
         {
            // create button
            TextBox txtbx = new TextBox();
            txtbx.Style = (Application.Current.Resources["Custom_TextBoxStyle_Generic"] as Style);
            txtbx.Text = ((sender as Button).Content as TextBox).Text;
            Button btn = new Button();
            btn.Style = (Application.Current.Resources["Custom_ButtonStyle_Generic"] as Style);
            btn.Content = txtbx;
            btn.Click += new RoutedEventHandler(NagivationBar_btn_click);
            btn.Tag = (sender as Button).Tag;

            NavigationBar_stckpnl.Children.Add(btn);
            NavigationBar_grd.Tag = int.Parse(NavigationBar_grd.Tag.ToString()) + 1;

            return true;
         }

         return false;
      }

      public bool AddAppBarStackPanel(object sender)
      {
         string gridname = ((sender as Button).Content as TextBox).Text.Replace("\n", "_");
         gridname = gridname.Replace(", ", "_");
         gridname = gridname.Replace(" ", "_");
         gridname += "AppBar_grd";

         if ((from vGrid in AppBar_grd.Children                              // make sure button doesn't already exist
              where ((vGrid as Grid).Name.ToString() == gridname)
              select vGrid).ToList().Count == 0)
         {
            // grid, outer
            Grid grd = new Grid();
            grd.HorizontalAlignment = HorizontalAlignment.Stretch;
            grd.VerticalAlignment = VerticalAlignment.Stretch;
            grd.Name = gridname;

            // stackpanel
            StackPanel stckpnl = new StackPanel();
            stckpnl.Orientation = Orientation.Horizontal;
            stckpnl.HorizontalAlignment = HorizontalAlignment.Left;
            stckpnl.VerticalAlignment = VerticalAlignment.Stretch;

            // connect the elements
            grd.Children.Add(stckpnl);
            AppBar_grd.Children.Add(grd);

            return true;
         }

         return false;
      }

      #region ShowHide ------------------------------------------------------------------------------
      /// <summary>
      /// Event that will switch to CharacterSheetOuter_grd and open the clicked character sheet
      /// </summary>
      /// <param name="sender">Clicked on button</param>
      private void UserMenu_Character(object sender)
      {
         // get and format character name to match file name
         string characterName = ((sender as Button).Content as TextBox).Text;
         if (characterName.Contains("\n"))
         {
            CharacterName_lbl.Content = characterName.Substring(0, characterName.IndexOf("\n"));
            characterName = characterName.Replace("\n", " (") + ")";
         }
         else
            CharacterName_lbl.Content = characterName;

         clWebClientManager.PrepareFilePaths("Get character stats", characterName);
      }

      /// <summary>
      /// Event that will switch to PowersOuter_grd and load the clicked power list and graph
      /// </summary>
      /// <param name="sender">Clicked on button</param>
      private void UserMenu_Power(object sender)
      {
         if (AddAppBarStackPanel(sender))
         {
            clDisplayPowers.CreatePowerLibraryGrid(sender, LayoutRoot);
         }

         ShowCollapsOuterGrids((sender as Button).Tag.ToString() + "Outer_grd", LayoutRoot);
         ShowCollapsOuterGrids((sender as Button).Tag.ToString() + "AppBar_grd", AppBar_grd);
         LoadingData_bsind.IsBusy = false;
      }

      /// <summary>
      /// Event that will switch to EquipmentsOuter_grd and load the clicked equipment list
      /// </summary>
      /// <param name="sender">Clicked on button</param>
      private void UserMenu_Equipment(object sender)
      {
         EquipmentName_lbl.Content = ((sender as Button).Content as TextBox).Text;

         ShowCollapsOuterGrids(EquipmentLibraryOuter_grd.Name, LayoutRoot);
         LoadingData_bsind.IsBusy = false;
      }

      /// <summary>
      /// Collaps all child-grids in ParentGrid, except VisualGrid
      /// </summary>
      /// <param name="VisualGrid">Name of grid to show</param>
      private void ShowCollapsOuterGrids(string VisualGrid, Grid ParentGrid)
      {
         foreach (object obj in ParentGrid.Children)
         {
            if (obj.GetType().Name == "Grid")
            {
               if ((obj as Grid).Name == VisualGrid)
                  (obj as Grid).Visibility = Visibility.Visible;
               else if (!(obj as Grid).Name.Contains("Collapsed_grd"))    // should always be "visible"
                  (obj as Grid).Visibility = Visibility.Collapsed;
            }
         }

         if (VisualGrid == "UserMenuOuter_grd")
            Back_btn.Visibility = Visibility.Collapsed;
         else
            Back_btn.Visibility = Visibility.Visible;

         AppBar_grd.Tag = 1;
         if (ParentGrid.Name.ToString() == "AppBar_grd")
            AppBar_grd.Tag = 2;
      }
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
