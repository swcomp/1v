﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _1v.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\CI_Tools\\1v\\")]
        public string installationDir {
            get {
                return ((string)(this["installationDir"]));
            }
            set {
                this["installationDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("D:\\_git\\gh-pu\\1v\\CI_Tools\\CI_Config\\ProductVersionNumbering\\")]
        public string productsVersionRootDir {
            get {
                return ((string)(this["productsVersionRootDir"]));
            }
            set {
                this["productsVersionRootDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ProductVersionNumberingConfig.xml")]
        public string productsVersionConfig {
            get {
                return ((string)(this["productsVersionConfig"]));
            }
            set {
                this["productsVersionConfig"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Config\\")]
        public string regexPatternsRootDir {
            get {
                return ((string)(this["regexPatternsRootDir"]));
            }
            set {
                this["regexPatternsRootDir"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1vRegexPatterns.xml")]
        public string regexPatternsFile {
            get {
                return ((string)(this["regexPatternsFile"]));
            }
            set {
                this["regexPatternsFile"] = value;
            }
        }
    }
}
