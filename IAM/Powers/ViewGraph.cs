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
using GraphLight.Graph;
using GraphLight.Layout;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

using IAM;

namespace ShowPowersNamespace
{
   public class ViewGraph: BaseViewModel
   {
      #region Properties --------------------------------------------------------------------
      #region Private ---------------------------------------------------------------------------
      private DrawingGraph graph;
      #endregion --------------------------------------------------------------------------------

      #region Public ----------------------------------------------------------------------------
      public DrawingGraph Graph
      {
         get { return graph; }
         set
         {
            graph = value;
            RaisePropertyChanged("Graph");
         }
      }
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Events ------------------------------------------------------------------------
      #region Delegates -------------------------------------------------------------------------
      public delegate void GraphReadyHandler(DrawingGraph grph, string graphtxt);
      #endregion --------------------------------------------------------------------------------

      #region Event Signatures ------------------------------------------------------------------
      /// <summary>
      /// Indicates that the graph has been updated
      /// </summary>
      public event GraphReadyHandler GraphReady;
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------

      #region Methods -----------------------------------------------------------------------
      #region Public ----------------------------------------------------------------------------
      public void LoadGraph(XElement eCharms)
      {
         string nodes, edges;
         string tab = "    ";

         // first lines
         nodes = "digraph Test" + "\n" + "{" + "\n" + tab + "nodes:" + "\n";
         edges = tab + "edges:" + "\n";

         foreach (XElement eCharm in eCharms.Elements("charm"))
         {
            DoCharmPrerequests(ref nodes, ref edges, eCharm, "");
         }
         CreateGraph(nodes + "\n" + edges + "}");
      }

      public void ReloadGraph(XElement eCharms, string graphtxt, string CharmName, string ErrataDate)
      {
         int e;
         string nodes, edges;
         string tab = "    ";

         // find nodes and edges
         e = graphtxt.IndexOf(tab + "edges:" + "\n");
         nodes = graphtxt.Substring(0, e - 1);
         edges = graphtxt.Substring(e, graphtxt.Length - e - 2);

         DeleteEdge(ref edges, ReplaceChars(CharmName));

         foreach (XElement eCharm in eCharms.Elements("charm"))
         {
            if (eCharm.Element("name").Value == CharmName)
            {
               DoCharmPrerequests(ref nodes, ref edges, eCharm, ErrataDate);

               break;
            }
         }

         CreateGraph(nodes + "\n" + edges + "}");
      }
      #endregion --------------------------------------------------------------------------------

      #region Private ---------------------------------------------------------------------------
      private void CreateGraph(string graphtxt)
      {
         try
         {
            var graph = DrawingGraph.ReadFromFile(new MemoryStream(Encoding.UTF8.GetBytes(graphtxt)));
            var engine = new GraphVizLayoutBatch();
            engine.Execute(graph);
            Graph = graph;
            this.GraphReady(Graph, graphtxt);
         }
         catch
         {
            this.GraphReady(null, null);
         }
      }

      #region Graph text file creation --------------------------------------------------------------
      private static void DoCharmPrerequests(ref string nodes, ref string edges, XElement eCharm, string ErrataDate)
      {
         string tab = "    ";
         string graphnodename;

         // Add node name
         graphnodename = ReplaceChars(eCharm.Element("name").Value);
         if (!nodes.Contains(graphnodename))
            nodes += tab + tab + graphnodename + " [label=\"" + eCharm.Element("name").Value + "\"];" + "\n";

         // write prerequests
         foreach (XElement ePre in eCharm.Elements("prerequest"))
         {
            if ((ePre.Element("name").Value != "None") && (ePre.Element("name").Value != "none") && (ePre.Element("name").Value != ""))
            {
               WriteNode(ref nodes, ref edges, graphnodename, ePre);               // Prerequest
               graphWriteTroublePrere(ref nodes, ref edges, ePre);                 // if any prerequests redirect to node
            }
         }

         // do errata if any
         if (ErrataDate != "Original text")
         {
            foreach (XElement eErr in eCharm.Elements("errata"))
            {
               foreach (XElement eErrPre in eErr.Elements("prerequest"))
               {
                  if (eErrPre.Element("name").Value != "")
                  {
                     if ((eErrPre.Element("name").Value != "None") && (eErrPre.Element("name").Value != "none"))
                     {
                        if (eErr.Element("todo").Value == "replace")
                           DeleteEdge(ref edges, graphnodename);

                        WriteNode(ref nodes, ref edges, graphnodename, eErrPre);               // Prerequest
                        graphWriteTroublePrere(ref nodes, ref edges, eErrPre);                 // if any prerequests redirect to node
                     }
                     else
                        DeleteEdge(ref edges, graphnodename);
                  }
               }

               if (eErr.Element("date").Value == ErrataDate)
                  break;
            }
         }
      }

      private static string ReplaceChars(string name)
      {
         name = name.Replace(" ", "_space");
         name = name.Replace("(", "_open");
         name = name.Replace(")", "_close");
         name = name.Replace("+", "_plus");
         name = name.Replace("-", "_dash");
         name = name.Replace("*", "_star");
         name = name.Replace("/", "_slash");
         name = name.Replace(".", "_period");
         name = name.Replace(",", "_comma");
         name = name.Replace("'", "_quote");
         name = name.Replace("\"", "_dquote");
         name = name.Replace(":", "_colon");
         name = name.Replace(";", "_scolon");
         name = name.Replace("?", "_query");
         name = name.Replace("!", "_bang");
         name = name.Replace("&", "_amper");
         name = name.Replace("•", "_dot");
         name = name.Replace("0", "_0");
         name = name.Replace("1", "_1");
         name = name.Replace("2", "_2");
         name = name.Replace("3", "_3");
         name = name.Replace("4", "_4");
         name = name.Replace("5", "_5");
         name = name.Replace("6", "_6");
         name = name.Replace("7", "_7");
         name = name.Replace("8", "_8");
         name = name.Replace("9", "_9");
         name = name.Replace("á", "_Aquote");
         name = name.Replace("à", "_Abquote");
         name = name.Replace("é", "_Equote");
         name = name.Replace("è", "_Ebquote");

         return name;
      }

      private static void DeleteEdge(ref string edges, string edgename)
      {
         int PreN, NextN, k;
         k = edges.IndexOf(edgename + ";");
         while (k != -1)
         {
            PreN = edges.LastIndexOf("\n", k);
            NextN = edges.IndexOf("\n", k);
            if (NextN == -1)
               NextN = edges.Length - 1;

            edges = edges.Remove(PreN + 1, NextN - PreN);

            k = edges.IndexOf(edgename + ";");
         }
      }

      /// <summary>
      /// written: ePre -> graphnodename
      /// </summary>
      /// <param name="nodes">string of nodes in graph</param>
      /// <param name="edges">string of edges in graph</param>
      /// <param name="graphnodename">top tier node</param>
      /// <param name="ePre">lower tier node</param>
      private static void WriteNode(ref string nodes, ref string edges, string graphnodename, XElement ePre)
      {
         string prenodename;
         string tab = "    ";

         if (ePre.Element("trouble") != null)
         {
            if (ePre.Element("trouble").Element("replace") != null)
               prenodename = ReplaceChars(ePre.Element("trouble").Element("replace").Value);
            else
               prenodename = ReplaceChars(ePre.Element("name").Value);
         }
         else
            prenodename = ReplaceChars(ePre.Element("name").Value);
         if (!edges.Contains(tab + tab + prenodename + " -> " + graphnodename + ";" + "\n"))
            edges += tab + tab + prenodename + " -> " + graphnodename + ";" + "\n";

         // comment out for debug charm names
         /*if (!nodes.Contains(prenodename))
         nodes += tab + tab + prenodename + " [label=\"" + ePre.Element("name").Value + "\"];" + "\n";
         // till here */
      }

      private static void graphWriteTroublePrere(ref string nodes, ref string edges, XElement ePre)
      {
         string trblgraphnodename;
         if (ePre.Element("trouble") != null)
         {
            if (ePre.Element("trouble").Element("prerequest") != null)
            {
               trblgraphnodename = ReplaceChars(ePre.Element("name").Value);
               foreach (XElement eTrPre in ePre.Element("trouble").Elements("prerequest"))
               {
                  WriteNode(ref nodes, ref edges, trblgraphnodename, eTrPre);
               }
            }
         }
      }
      #endregion ------------------------------------------------------------------------------------
      #endregion --------------------------------------------------------------------------------
      #endregion ----------------------------------------------------------------------------
   }
}
