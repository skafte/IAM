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
using Microsoft.Expression.Controls;
using Microsoft.Expression.Media;

using IAM;

namespace IAM.Powers
{
   public class CreatePowerElement
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      private int amount = 10;
      private CustomControlStyles usCustomControlStyles = new CustomControlStyles();
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Private --------------------------------------------------------------------------
      /// <summary>
      /// Creates the header for a power element
      /// </summary>
      /// <returns>a Label</returns>
      private object CreateHeader()
      {
         Label lbl = new Label();

         lbl.Content = "power header";
         lbl.Margin = new Thickness(0);
         lbl.IsHitTestVisible = false;

         return lbl;
      }

      /// <summary>
      /// Creates the body for a power element
      /// </summary>
      /// <returns>a StackPanel</returns>
      private object CreateContent()
      {
         StackPanel stckpnl = new StackPanel();

         CreatePowerBody_Intro(stckpnl);
         CreatePowerBody_CostDurationType(stckpnl);
         CreatePowerBody_Mins(stckpnl);
         CreatePowerBody_Keywords(stckpnl);
         CreatePowerBody_Prerequest(stckpnl);
         CreatePowerBody_Merged(stckpnl);
         CreatePowerBody_Martial(stckpnl);
         CreatePowerBody_MartialReady(stckpnl);

         // draw a line
         LineArrow Devider_lnrrw = new LineArrow();
         Devider_lnrrw.Fill = new SolidColorBrush(Color.FromArgb(byte.Parse("255"), byte.Parse("244"), byte.Parse("244"), byte.Parse("245")));
         Devider_lnrrw.Stroke = new SolidColorBrush(Color.FromArgb(byte.Parse("128"), byte.Parse("128"), byte.Parse("128"), byte.Parse("128")));
         Devider_lnrrw.Height = 1;
         Devider_lnrrw.Margin = new Thickness(1, 0, 1, 0);
         Devider_lnrrw.VerticalAlignment = VerticalAlignment.Top;
         Devider_lnrrw.EndArrow = ArrowType.NoArrow;
         stckpnl.Children.Add(Devider_lnrrw);

         CreatePowerBody_Description(stckpnl);
         CreatePowerBody_Submodule(stckpnl);
         CreatePowerBody_Mirror(stckpnl);
         CreatePowerBody_Errata(stckpnl);
         CreatePowerBody_Comment(stckpnl);

         return stckpnl;
      }

      #region Create body sections -----------------------------------------------------------------
      #region Intro ------------------------------------------------------------------------------------
      private void CreatePowerBody_Intro(StackPanel stckpnl)
      {
         WrapPanel Intro_wrppnl = new WrapPanel();
         Intro_wrppnl.Tag = "Intro_wrppnl";
         CreatePowerBody_Title(Intro_wrppnl);
         CreaPowerBody_Reference(Intro_wrppnl);
         CreatePowerBody_VersionOf(Intro_wrppnl);
         stckpnl.Children.Add(Intro_wrppnl);
      }

      private void CreatePowerBody_Title(WrapPanel Intro_wrppnl)
      {
         TextBlock CreatorTitle_txtblck = new TextBlock();
         TextBlock CreatorValue_txtblck = new TextBlock();
         CreatorTitle_txtblck.Tag = "CreatorTitle_txtblck";
         CreatorValue_txtblck.Tag = "CreatorValue_txtblck";
         CreatorTitle_txtblck.Text = "Creator:";
         CreatorValue_txtblck.Text = "-";
         CreatorTitle_txtblck.FontWeight = FontWeights.Bold;
         CreatorTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         CreatorValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         Intro_wrppnl.Children.Add(CreatorTitle_txtblck);
         Intro_wrppnl.Children.Add(CreatorValue_txtblck);
      }

      private void CreaPowerBody_Reference(WrapPanel Intro_wrppnl)
      {
         TextBlock ReferenceTitle_txtblck = new TextBlock();
         TextBlock ReferenceValue_txtblck = new TextBlock();
         ReferenceTitle_txtblck.Tag = "ReferenceTitle_txtblck";
         ReferenceValue_txtblck.Tag = "ReferenceValue_txtblck";
         ReferenceTitle_txtblck.Text = "Reference:";
         ReferenceValue_txtblck.Text = "-";
         ReferenceTitle_txtblck.FontWeight = FontWeights.Bold;
         ReferenceTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         ReferenceValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         Intro_wrppnl.Children.Add(ReferenceTitle_txtblck);
         Intro_wrppnl.Children.Add(ReferenceValue_txtblck);
      }

      private void CreatePowerBody_VersionOf(WrapPanel Intro_wrppnl)
      {
         TextBlock VersionOfTitle_txtblck = new TextBlock();
         TextBlock VersionOfValue_txtblck = new TextBlock();
         VersionOfTitle_txtblck.Tag = "VersionOfTitle_txtblck";
         VersionOfValue_txtblck.Tag = "VersionOfValue_txtblck";
         VersionOfTitle_txtblck.Text = "Version of:";
         VersionOfValue_txtblck.Text = "-";
         VersionOfTitle_txtblck.FontWeight = FontWeights.Bold;
         VersionOfTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         VersionOfValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         Intro_wrppnl.Children.Add(VersionOfTitle_txtblck);
         Intro_wrppnl.Children.Add(VersionOfValue_txtblck);
         StackPanel VersionOf_stckpnl = new StackPanel();
         Button VersionOfPre_btn = new Button();
         Button VersionOfNext_btn = new Button();
         VersionOfPre_btn.Tag = "VersionOfPre_btn";
         VersionOfNext_btn.Tag = "VersionOfNext_btn";
         VersionOfPre_btn.Style = (Style)usCustomControlStyles.Resources["Custom_ButtonStyle_UpArrow"];
         VersionOfNext_btn.Style = (Style)usCustomControlStyles.Resources["Custom_ButtonStyle_DownArrow"];
         VersionOfPre_btn.Height = 8;
         VersionOfPre_btn.Width = 11;
         VersionOfNext_btn.Height = 8;
         VersionOfNext_btn.Width = 11;
         VersionOfNext_btn.IsEnabled = false;
         VersionOfPre_btn.IsEnabled = false;
         VersionOf_stckpnl.Children.Add(VersionOfPre_btn);
         VersionOf_stckpnl.Children.Add(VersionOfNext_btn);
         Intro_wrppnl.Children.Add(VersionOf_stckpnl);
      }
      #endregion ---------------------------------------------------------------------------------------

      #region CostDurationType -------------------------------------------------------------------------
      private void CreatePowerBody_CostDurationType(StackPanel stckpnl)
      {
         WrapPanel CostDurationType_wrppnl = new WrapPanel();
         CostDurationType_wrppnl.Tag = "CostDurationType_wrppnl";
         CreatePowerBody_Cost(CostDurationType_wrppnl);
         CreatePowerBody_Duration(CostDurationType_wrppnl);
         CreatePowerBody_Type(CostDurationType_wrppnl);
         stckpnl.Children.Add(CostDurationType_wrppnl);
      }

      private void CreatePowerBody_Cost(WrapPanel CostDurationType_wrppnl)
      {
         TextBlock CostTitle_txtblck = new TextBlock();
         TextBlock CostValue_txtblck = new TextBlock();
         CostTitle_txtblck.Tag = "CostTitle_txtblck";
         CostValue_txtblck.Tag = "CostValue_txtblck";
         CostTitle_txtblck.Text = "Cost:";
         CostValue_txtblck.Text = "-";
         CostTitle_txtblck.FontWeight = FontWeights.Bold;
         CostTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         CostValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         CostDurationType_wrppnl.Children.Add(CostTitle_txtblck);
         CostDurationType_wrppnl.Children.Add(CostValue_txtblck);
      }

      private void CreatePowerBody_Duration(WrapPanel CostDurationType_wrppnl)
      {
         TextBlock DurationTitle_txtblck = new TextBlock();
         TextBlock DurationValue_txtblck = new TextBlock();
         DurationTitle_txtblck.Tag = "DurationTitle_txtblck";
         DurationValue_txtblck.Tag = "DurationValue_txtblck";
         DurationTitle_txtblck.Text = "Duration:";
         DurationValue_txtblck.Text = "-";
         DurationTitle_txtblck.FontWeight = FontWeights.Bold;
         DurationTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         DurationValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         CostDurationType_wrppnl.Children.Add(DurationTitle_txtblck);
         CostDurationType_wrppnl.Children.Add(DurationValue_txtblck);
      }

      private void CreatePowerBody_Type(WrapPanel CostDurationType_wrppnl)
      {
         TextBlock TypeTitle_txtblck = new TextBlock();
         TextBlock TypeValue_txtblck = new TextBlock();
         TypeTitle_txtblck.Tag = "TypeTitle_txtblck";
         TypeValue_txtblck.Tag = "TypeValue_txtblck";
         TypeTitle_txtblck.Text = "Type:";
         TypeValue_txtblck.Text = "-";
         TypeTitle_txtblck.FontWeight = FontWeights.Bold;
         TypeTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         TypeValue_txtblck.Margin = new Thickness(0, 0, 8, 0);
         CostDurationType_wrppnl.Children.Add(TypeTitle_txtblck);
         CostDurationType_wrppnl.Children.Add(TypeValue_txtblck);
      }
      #endregion ---------------------------------------------------------------------------------------

      #region the rest ---------------------------------------------------------------------------------
      private void CreatePowerBody_Mins(StackPanel stckpnl)
      {
         WrapPanel Mins_wrppnl = new WrapPanel();
         Mins_wrppnl.Tag = "Mins_wrppnl";
         TextBlock MinsTitle_txtblck = new TextBlock();
         MinsTitle_txtblck.Tag = "MinsTitle_txtblck";
         MinsTitle_txtblck.Text = "Mins:";
         MinsTitle_txtblck.FontWeight = FontWeights.Bold;
         MinsTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         Mins_wrppnl.Children.Add(MinsTitle_txtblck);
         TextBlock[] MinsName_txtblck = new TextBlock[amount];
         TextBlock[] MinsValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            MinsName_txtblck[i] = new TextBlock();
            MinsValue_txtblck[i] = new TextBlock();
            MinsName_txtblck[i].Tag = "MinsName_txtblck" + i.ToString();
            MinsValue_txtblck[i].Tag = "MinsValue_txtblck" + i.ToString();
            MinsName_txtblck[i].Margin = new Thickness(0, 0, 3, 0);
            MinsValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            Mins_wrppnl.Children.Add(MinsName_txtblck[i]);
            Mins_wrppnl.Children.Add(MinsValue_txtblck[i]);
         }
         stckpnl.Children.Add(Mins_wrppnl);
      }

      private void CreatePowerBody_Keywords(StackPanel stckpnl)
      {
         WrapPanel Keywords_wrppnl = new WrapPanel();
         Keywords_wrppnl.Tag = "Keywords_wrppnl";
         TextBlock KeywordsTitle_txtblck = new TextBlock();
         KeywordsTitle_txtblck.Tag = "KeywordsTitle_txtblck";
         KeywordsTitle_txtblck.Text = "Keyword:";
         KeywordsTitle_txtblck.FontWeight = FontWeights.Bold;
         KeywordsTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         Keywords_wrppnl.Children.Add(KeywordsTitle_txtblck);
         TextBlock[] KeywordsValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            KeywordsValue_txtblck[i] = new TextBlock();
            KeywordsValue_txtblck[i].Tag = "KeywordsValue_txtblck" + i.ToString();
            KeywordsValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            Keywords_wrppnl.Children.Add(KeywordsValue_txtblck[i]);
         }
         stckpnl.Children.Add(Keywords_wrppnl);
      }

      private void CreatePowerBody_Prerequest(StackPanel stckpnl)
      {
         WrapPanel Prerequest_wrppnl = new WrapPanel();
         Prerequest_wrppnl.Tag = "Prerequest_wrppnl";
         TextBlock PrerequestTitle_txtblck = new TextBlock();
         PrerequestTitle_txtblck.Tag = "PrerequestTitle_txtblck";
         PrerequestTitle_txtblck.Text = "Prerequest:";
         PrerequestTitle_txtblck.FontWeight = FontWeights.Bold;
         PrerequestTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         Prerequest_wrppnl.Children.Add(PrerequestTitle_txtblck);
         TextBlock[] PrerequestValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            PrerequestValue_txtblck[i] = new TextBlock();
            PrerequestValue_txtblck[i].Tag = "PrerequestValue_txtblck" + i.ToString();
            PrerequestValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            Prerequest_wrppnl.Children.Add(PrerequestValue_txtblck[i]);
         }
         stckpnl.Children.Add(Prerequest_wrppnl);
      }

      private void CreatePowerBody_Merged(StackPanel stckpnl)
      {
         WrapPanel Merged_wrppnl = new WrapPanel();
         Merged_wrppnl.Tag = "Merged_wrppnl";
         TextBlock MergedTitle_txtblck = new TextBlock();
         MergedTitle_txtblck.Tag = "MergedTitle_txtblck";
         MergedTitle_txtblck.Text = "Merged:";
         MergedTitle_txtblck.FontWeight = FontWeights.Bold;
         MergedTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         Merged_wrppnl.Children.Add(MergedTitle_txtblck);
         TextBlock[] MergedValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            MergedValue_txtblck[i] = new TextBlock();
            MergedValue_txtblck[i].Tag = "MergedValue_txtblck" + i.ToString();
            MergedValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            Merged_wrppnl.Children.Add(MergedValue_txtblck[i]);
         }
         stckpnl.Children.Add(Merged_wrppnl);
      }

      private void CreatePowerBody_Martial(StackPanel stckpnl)
      {
         WrapPanel Martial_wrppnl = new WrapPanel();
         Martial_wrppnl.Tag = "Martial_wrppnl";
         TextBlock MartialTitle_txtblck = new TextBlock();
         MartialTitle_txtblck.Tag = "MartialTitle_txtblck";
         MartialTitle_txtblck.Text = "Martial:";
         MartialTitle_txtblck.FontWeight = FontWeights.Bold;
         MartialTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         Martial_wrppnl.Children.Add(MartialTitle_txtblck);
         TextBlock[] MartialValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            MartialValue_txtblck[i] = new TextBlock();
            MartialValue_txtblck[i].Tag = "MartialValue_txtblck" + i.ToString();
            MartialValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            Martial_wrppnl.Children.Add(MartialValue_txtblck[i]);
         }
         stckpnl.Children.Add(Martial_wrppnl);
      }

      private void CreatePowerBody_MartialReady(StackPanel stckpnl)
      {
         WrapPanel MartialReady_wrppnl = new WrapPanel();
         MartialReady_wrppnl.Tag = "MartialReady_wrppnl";
         TextBlock MartialReadyTitle_txtblck = new TextBlock();
         MartialReadyTitle_txtblck.Tag = "MartialReadyTitle_txtblck";
         MartialReadyTitle_txtblck.Text = "MartialReady:";
         MartialReadyTitle_txtblck.FontWeight = FontWeights.Bold;
         MartialReadyTitle_txtblck.Margin = new Thickness(0, 0, 3, 0);
         MartialReady_wrppnl.Children.Add(MartialReadyTitle_txtblck);
         TextBlock[] MartialReadyValue_txtblck = new TextBlock[amount];
         for (int i = 0; i < amount; i++)
         {
            MartialReadyValue_txtblck[i] = new TextBlock();
            MartialReadyValue_txtblck[i].Tag = "MartialReadyValue_txtblck" + i.ToString();
            MartialReadyValue_txtblck[i].Margin = new Thickness(0, 0, 8, 0);
            MartialReady_wrppnl.Children.Add(MartialReadyValue_txtblck[i]);
         }
         stckpnl.Children.Add(MartialReady_wrppnl);
      }

      private void CreatePowerBody_Description(StackPanel stckpnl)
      {
         TextBox DescriptionText_txtbx = new TextBox();
         DescriptionText_txtbx.Tag = "DescriptionText_txtbx";
         DescriptionText_txtbx.IsReadOnly = true;
         DescriptionText_txtbx.TextWrapping = TextWrapping.Wrap;
         DescriptionText_txtbx.Style = (Style)usCustomControlStyles.Resources["Custom_TextBoxStyle"];
         DescriptionText_txtbx.Background = new SolidColorBrush(Colors.White);
         DescriptionText_txtbx.BorderBrush = null;
         DescriptionText_txtbx.Padding = new Thickness(0);
         DescriptionText_txtbx.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         DescriptionText_txtbx.MaxHeight = 200;
         DescriptionText_txtbx.Margin = new Thickness(2, 0, 1, 0);
         stckpnl.Children.Add(DescriptionText_txtbx);
      }

      private void CreatePowerBody_Submodule(StackPanel stckpnl)
      {
         TextBox SubmoduleText_txtbx = new TextBox();
         SubmoduleText_txtbx.Tag = "SubmoduleText_txtbx";
         SubmoduleText_txtbx.IsReadOnly = true;
         SubmoduleText_txtbx.TextWrapping = TextWrapping.Wrap;
         SubmoduleText_txtbx.Style = (Style)usCustomControlStyles.Resources["Custom_TextBoxStyle"];
         SubmoduleText_txtbx.Background = new SolidColorBrush(Color.FromArgb(byte.Parse("63"), byte.Parse("210"), byte.Parse("210"), byte.Parse("210")));
         SubmoduleText_txtbx.BorderBrush = null;
         SubmoduleText_txtbx.Padding = new Thickness(0);
         SubmoduleText_txtbx.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         SubmoduleText_txtbx.MaxHeight = 200;
         SubmoduleText_txtbx.Margin = new Thickness(2, 0, 1, 0);
         stckpnl.Children.Add(SubmoduleText_txtbx);
      }

      private void CreatePowerBody_Mirror(StackPanel stckpnl)
      {
         TextBox MirrorText_txtbx = new TextBox();
         MirrorText_txtbx.Tag = "MirrorText_txtbx";
         MirrorText_txtbx.IsReadOnly = true;
         MirrorText_txtbx.TextWrapping = TextWrapping.Wrap;
         MirrorText_txtbx.Style = (Style)usCustomControlStyles.Resources["Custom_TextBoxStyle"];
         MirrorText_txtbx.Background = new SolidColorBrush(Color.FromArgb(byte.Parse("63"), byte.Parse("210"), byte.Parse("210"), byte.Parse("210")));
         MirrorText_txtbx.BorderBrush = null;
         MirrorText_txtbx.Padding = new Thickness(0);
         MirrorText_txtbx.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         MirrorText_txtbx.MaxHeight = 200;
         MirrorText_txtbx.Margin = new Thickness(2, 0, 1, 0);
         stckpnl.Children.Add(MirrorText_txtbx);
      }

      private void CreatePowerBody_Errata(StackPanel stckpnl)
      {
         TextBox ErrataText_txtbx = new TextBox();
         ErrataText_txtbx.Tag = "ErrataText_txtbx";
         ErrataText_txtbx.IsReadOnly = true;
         ErrataText_txtbx.TextWrapping = TextWrapping.Wrap;
         ErrataText_txtbx.Style = (Style)usCustomControlStyles.Resources["Custom_TextBoxStyle"];
         ErrataText_txtbx.Background = new SolidColorBrush(Color.FromArgb(byte.Parse("63"), byte.Parse("192"), byte.Parse("192"), byte.Parse("210")));
         ErrataText_txtbx.BorderBrush = null;
         ErrataText_txtbx.Padding = new Thickness(0);
         ErrataText_txtbx.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         ErrataText_txtbx.MaxHeight = 200;
         ErrataText_txtbx.Margin = new Thickness(2, 0, 1, 0);
         stckpnl.Children.Add(ErrataText_txtbx);
      }

      private void CreatePowerBody_Comment(StackPanel stckpnl)
      {
         TextBox CommentText_txtbx = new TextBox();
         CommentText_txtbx.Tag = "CommentText_txtbx";
         CommentText_txtbx.IsReadOnly = true;
         CommentText_txtbx.TextWrapping = TextWrapping.Wrap;
         CommentText_txtbx.Style = (Style)usCustomControlStyles.Resources["Custom_TextBoxStyle"];
         CommentText_txtbx.Background = new SolidColorBrush(Color.FromArgb(byte.Parse("252"), byte.Parse("41"), byte.Parse("41"), byte.Parse("47")));
         CommentText_txtbx.Foreground = new SolidColorBrush(Colors.White);
         CommentText_txtbx.BorderBrush = null;
         CommentText_txtbx.Padding = new Thickness(0);
         CommentText_txtbx.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         CommentText_txtbx.MaxHeight = 200;
         CommentText_txtbx.Margin = new Thickness(2, 0, 1, 0);
         stckpnl.Children.Add(CommentText_txtbx);
      }
      #endregion ---------------------------------------------------------------------------------------
      #endregion -----------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------

      #region Public ----------------------------------------------------------------------------
      /// <summary>
      /// public function that created a power element
      /// </summary>
      /// <param name="itemcount">used to uniquily identify the element</param>
      /// <param name="powertype">used to uniquily identify the element</param>
      /// <returns>created power element</returns>
      public Expander CreateElement(int itemcount = 0, string powertype = "power")
      {
         Expander expndr = new Expander();

         expndr.Name = powertype + "_" + itemcount.ToString() + "_expndr";
         expndr.HorizontalAlignment = HorizontalAlignment.Stretch;

         expndr.Header = CreateHeader();
         expndr.Content = CreateContent();

         return expndr;
      }
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
