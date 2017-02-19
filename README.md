# 1v
Windows shell utility for centralised management of .NET assembly file version numbers.

Will support multiple solutions in one or more CVS repositories. 

Integration with CI servers for build number increments is straightforward.

### Notes

Note: You will need to update the paths to the 1v Config directory to match the location of the 1v app folders in the following places:

D:\_git\gh-pu\1v\CI_Tools\1v\1v\Properties\Settings.settings -> productsVersionRootDir

D:\_git\gh-pu\1v\CI_Tools\CI_Config\ProductVersionNumbering\ProductVersionNumberingConfig.xml

Note: To use the 1v app to version your project files for Release builds you need to add the 1v build diretory to your SYSTEM PATH* and the following command line Pre-Build event to your project files:

IF NOT $(ConfigurationName) == Debug 1v "$(SolutionName)" "$(MSBuildProjectDirectory)" "$(MSBuildProjectExtension)"

* ( for development use your 1v bin\debug path, eg. "D:\_git\gh-pu\1v\CI_Tools\1v\1v\bin\Debug")

This will allow debug builds during development without assembly versions being updated and then on the build server the release build will result in the assemblyInfo version numbers being updated.



