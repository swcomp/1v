using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1v.Model
{
   public class ProductVersionNumber
   {
      public bool AutoIncrement { get; set; }
      public string Major { get; set; }
      public string Minor { get; set; }
      public string Build { get; set; }
      public string Revision { get; set; }

   }
}
