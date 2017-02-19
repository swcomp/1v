using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using _1v.Data.Files;

namespace _1v.Model
{
   public class Product
   {
      // TODO: Add IEnumerable property for sub products.
      
      public Product(string name)
      {
         Name = name;
         VersionRootFile = new ProductVersionRootFile(this, GetProductVersionRootFile());
      }

      public string Name { get; private set; }
      public string VersionRootFilePath { get; private set; }
      public IList<RegexPatternsFile> OtherVersionedFiles { get; private set; }
      public ProductVersionRootFile VersionRootFile { get; private set; }


      public void AddSubProduct()
      {

      }

      public XmlDocument GetProductVersionRootFile()
      {
         XmlDocument vProductsConfig = new XmlDocument();

         // Product version config filename and directory are user configurable.
         string prodsVersionConfigPath = Properties.Settings.Default.productsVersionRootDir +
                                         Properties.Settings.Default.productsVersionConfig;                                         

         vProductsConfig.Load(prodsVersionConfigPath);

         // TODO: 

            

         // Now find the path to the productVersonRootFile for the product.
         XmlDocument prodVersionNumRootConfig = new XmlDocument();
         XmlNodeList products = vProductsConfig.SelectNodes("//products/product");
         var prod = products.Cast<XmlNode>().FirstOrDefault(p => p.Attributes["name"].Value == Name);

         // Store productVersonRootFile path for save operaitons.
         XmlNode prodVersionNumRoot = prod.SelectSingleNode("//productVersionRootFile");
         VersionRootFilePath = prodVersionNumRoot.Attributes["path"].Value;

         // Load current product's rootVersionFile
         prodVersionNumRootConfig.Load(VersionRootFilePath);

         return prodVersionNumRootConfig;
      }
   }
}
