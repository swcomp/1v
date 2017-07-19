using _1v.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1v
{
   public class ProductVersionRootFile
   {
      private readonly XmlDocument _rootXmlDoc;
      private readonly string _rootXmlDocPath;
      private readonly Logger _logger;
      private readonly Product _product;

      public ProductVersionRootFile(Product prod, XmlDocument rootVersionXmlDoc)
      {
         _product = prod;
         _rootXmlDoc = rootVersionXmlDoc;
         _rootXmlDocPath = _product.VersionRootFilePath;
         _logger = LogManager.GetLogger("1v");

      }

      public enum SetVersionMethods
      {
         AssemblyAndFile,
         FileOnly
      }

      /// <summary>
      /// Sets version number part with passed value or triggers auto increment.
      /// </summary>
      /// <param name="args"></param>
      public void SetVersionPart(string[] args)
      {
         ProductVersionNumber pv = new ProductVersionNumber();

         switch (args[1])
         {
            case "/m":

               break;

            case "/i":

               break;

            case "/b":
               if (args.Count() > 2)
               {
                  pv.Build = args[2];
               }
               else
               {
                  pv.AutoIncrement = true;
               }
               Set(pv);
               break;

            case "/r":

               break;
         }

      }

      /// <summary>
      /// Allows command line update of the ProductVersionRoot.xml file.
      /// TODO: Add command line switches for updating specific parts of the Assembly Version.
      /// </summary>
      /// <param name="productVersion"></param>
      /// <param name="setVersionMethod"></param>
      public void Set(string productVersionStr, SetVersionMethods setVersionMethod = SetVersionMethods.AssemblyAndFile)
      {
         string[] v = productVersionStr.Split('.');

         ProductVersionNumber pv = new ProductVersionNumber() { Build = v[0], Major = v[1], Minor = v[2], Revision = v[3] };

         Set(pv, setVersionMethod);
      }

      /// <summary>
      /// Allows the Build Server and/or SVN Hooks to modify the configured productVersionFilename ( root ) version file.
      /// TODO: Add command line switches for updating specific parts of the Assembly Version.
      /// </summary>
      /// <param name="productVersion"></param>
      /// <param name="setVersionMethod"></param>
      public void Set(ProductVersionNumber productVersion, SetVersionMethods setVersionMethod = SetVersionMethods.AssemblyAndFile)
      {

         _logger.Log(LogLevel.Info, "Updating root product version file [" + _rootXmlDocPath + "]");

         // Update parts of AssemblyVersion number passed in productVersion.
         if (setVersionMethod != SetVersionMethods.FileOnly)
         {
            XmlNode av = _rootXmlDoc.SelectSingleNode("//productVersion/assemblyVersion");
            av["major"].InnerText = productVersion.Major ?? av["major"].InnerText;
            av["minor"].InnerText = productVersion.Minor ?? av["minor"].InnerText;
            if (productVersion.AutoIncrement)
            {
               av["build"].InnerText = (int.Parse(av["build"].InnerText) + 1).ToString();
            }
            else
            {
               av["build"].InnerText = productVersion.Build ?? av["build"].InnerText;
            }
            
            av["revision"].InnerText = productVersion.Revision ?? av["revision"].InnerText;

            /* Log */
            _logger.Log(LogLevel.Info, "Set AssemblyVersion= " +
                        String.Concat(productVersion.Major, '.',
                                       productVersion.Minor, '.',
                                       productVersion.Build, '.',
                                       productVersion.Revision));
         }

         // Update parts of AssemblyFileVersion number passed in productVersion.
         XmlNode afv = _rootXmlDoc.SelectSingleNode("//productVersion/assemblyFileVersion");
         afv["major"].InnerText = productVersion.Major ?? afv["major"].InnerText;
         afv["minor"].InnerText = productVersion.Minor ?? afv["minor"].InnerText;

         if (productVersion.AutoIncrement)
         {
            afv["build"].InnerText = (int.Parse(afv["build"].InnerText) + 1).ToString();
         }
         else
         {
            if (productVersion.Build != null)
            {
               afv["build"].InnerText = productVersion.Build;
            }
         }
         
         /* Log */
         _logger.Log(LogLevel.Info, "Set AssemblyFileVersion= " +
                     String.Concat(productVersion.Major, '.',
                                    productVersion.Minor, '.',
                                    productVersion.Build, '.',
                                    productVersion.Revision));

         _rootXmlDoc.Save(_rootXmlDocPath);
      }

      public string GetAssemblyVersion()
      {

         // Update AssemblyVersion number.
         XmlNode av = _rootXmlDoc.SelectSingleNode("//productVersion/assemblyVersion");

         string result = String.Concat(av["major"].InnerText, '.',
                                       av["minor"].InnerText, '.',
                                       av["build"].InnerText, '.',
                                       av["revision"].InnerText);

         return result;
      }

      public string GetAssemblyFileVersion()
      {

         // Update AssemblyFileVersion number.
         XmlNode afv = _rootXmlDoc.SelectSingleNode("//productVersion/assemblyFileVersion");

         string result = String.Concat(afv["major"].InnerText, '.',
                                       afv["minor"].InnerText, '.',
                                       afv["build"].InnerText, '.',
                                       afv["revision"].InnerText);

         return result;
      }

   }
}
