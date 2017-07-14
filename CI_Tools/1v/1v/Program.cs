using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using _1v.Model;

namespace _1v
{
   class Program
   {
      const string CS_ASSEMBLY_INFO_PART_PATH = @"\Properties\AssemblyInfo.cs";
      const string VB_ASSEMBLY_INFO_PART_PATH = @"\My Project\AssemblyInfo.vb";

      private static NLog.Logger _logger = LogManager.GetLogger("1v");
      
      static void Main(string[] args)
      {
         Product prod;
         ProductVersionRootFile pvr;

         // TODO: Add validation on the product initialisation - ie check that the product name is defined in ProductVersionNumberingConfig.xml
         try
         {
            // First parameter is always the product name.
            prod = new Product(args[0]);
            pvr = prod.VersionRootFile;

         }
         catch (Exception e)
         {
            // TODO: Add handlers.
            throw e;
         }

         // Command line switches to update single parts of the version number.
         // Major /ma, Minor /mi, Build /b, Revision /r
         // Syntax is > 1v {product name} /{switch} {optional: value}  If a value is not passed then the value will be incremented in the Product Root Version file.
         if ("/m /i /b /r".IndexOf(args[1], StringComparison.Ordinal) > -1)
         {
            pvr.SetVersionPart(args);
         }
         else
         {

            // Project file pre-build event calls to update Assembly Info.
            if (args.Length < 1)
            {
               Console.WriteLine("Parameters required, path: Valid path to project file, project-file-extension: csproj or vbproj");
            }
            else if (args[0] == "/?")
            {
               Console.WriteLine("Parameter required, path= Valid path to project file.");
            }
            else
            {
               try
               {
                  // Default syntax in project file prebuild command is: IF NOT $(ConfigurationName) == Debug 1v "$(SolutionName)" "$(MSBuildProjectDirectory)" "$(MSBuildProjectExtension)"
                  UpdateAssemblyInfoVersionNumbers(pvr, args[1], args[2]);
               }
               catch (Exception e)
               {
                  _logger.Log(LogLevel.Error, "Product Name=" + args[0] + "Check for invalid parameters project path = " + args[1] + " project_file_extension = " + args[2]);
               }

            }

         }

      }

      static void UpdateAssemblyInfoVersionNumbers(ProductVersionRootFile pvr, string projectPath, string projectFileExt)
      {
         
         const string CS_ASSEMBLY_VERSION_PATTERN = @"\[assembly: AssemblyVersion\("".*""\)\]";
         const string CS_ASSEMBLY_FILEVERSION_PATTERN = @"\[assembly: AssemblyFileVersion\("".*""\)\]";
         const string VB_ASSEMBLY_VERSION_PATTERN = @"<Assembly: AssemblyVersion\("".*""\)>";
         const string VB_ASSEMBLY_FILEVERSION_PATTERN = @"<Assembly: AssemblyFileVersion\("".*""\)>";

         // TODO: Performance?
         // If issue cache these values in an environment variable or MSBuild variable or use the linked assembly_info file or MSBuild import approach.

         string assemblyVersion;
         string assemblyFileVersion;
         string assemblyInfoFile = string.Empty;
         string fileText = string.Empty;


         if (projectFileExt == ".csproj")
         {
            /* C# Project */
            assemblyVersion = String.Format("[assembly: AssemblyVersion(\"{0}\")]", pvr.GetAssemblyVersion());
            assemblyFileVersion = String.Format("[assembly: AssemblyFileVersion(\"{0}\")]", pvr.GetAssemblyFileVersion());
            assemblyInfoFile = String.Concat(projectPath, CS_ASSEMBLY_INFO_PART_PATH);

            fileText = File.ReadAllText(assemblyInfoFile);

            Regex rgx = new Regex(CS_ASSEMBLY_VERSION_PATTERN);
            fileText = rgx.Replace(fileText, assemblyVersion);

            rgx = new Regex(CS_ASSEMBLY_FILEVERSION_PATTERN);
            fileText = rgx.Replace(fileText, assemblyFileVersion);
         }
         else if (projectFileExt == ".vbproj")
         {
            /* VB Project */
            assemblyVersion = String.Format("<Assembly: AssemblyVersion(\"{0}\")>", pvr.GetAssemblyVersion());
            assemblyFileVersion = String.Format("<Assembly: AssemblyFileVersion(\"{0}\")>", pvr.GetAssemblyFileVersion());
            assemblyInfoFile = String.Concat(projectPath, VB_ASSEMBLY_INFO_PART_PATH);

            try
            {
               fileText = File.ReadAllText(assemblyInfoFile);
            }
            catch (FileNotFoundException)
            {
               _logger.Log(LogLevel.Info, @"AssemblyInfo.vb not found in \My Project directory, looking in project root...");
               /* RSLocalisation has its AssemblyInfo.vb in the project root directory not "My Project" */
               assemblyInfoFile = String.Concat(projectPath, @"\AssemblyInfo.vb");
               fileText = File.ReadAllText(assemblyInfoFile);
            }
            Regex rgx = new Regex(VB_ASSEMBLY_VERSION_PATTERN);
            fileText = rgx.Replace(fileText, assemblyVersion);

            rgx = new Regex(VB_ASSEMBLY_FILEVERSION_PATTERN);
            fileText = rgx.Replace(fileText, assemblyFileVersion);
         }
         else
         {
            throw new Exception("Unsupported project file extension: [" + projectFileExt + "]");
         }


         File.WriteAllText(assemblyInfoFile, fileText);

         _logger.Log(LogLevel.Info, "Updated [" + assemblyInfoFile + "]");

         //TODO: Produces false positives if AssemblyVersion attribute is not present in file, however if not present we're not worried.
         // Eg. RSLocalisation does not have the AssemblyFileVersion attribute using <Assembly: System.Security.AllowPartiallyTrustedCallers()>  instead.
         _logger.Log(LogLevel.Info, "Set " + assemblyVersion);
         _logger.Log(LogLevel.Info, "Set " + assemblyFileVersion);

      }
   }
}
