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

namespace IAM
{
   // TODO: global catching of exceptions
   public class UnknownFileProcessException : ArgumentException
   {
      public UnknownFileProcessException(string Reason)
         : base(Reason)
      { }
   }
   public class FileNotFoundException : ArgumentException
   {
      public FileNotFoundException(string FileProcess, string FileToLoad)
         : base(FileProcess, FileToLoad)
      { }
   }
   public class UnknownObjectException : ArgumentException
   {
      public UnknownObjectException(string ObjectType)
         : base(ObjectType)
      { }
   }
}
