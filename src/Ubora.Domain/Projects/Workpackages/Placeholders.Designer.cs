﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ubora.Domain.Projects.Workpackages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Placeholders {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Placeholders() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ubora.Domain.Projects.Workpackages.Placeholders", typeof(Placeholders).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Here you should describe the clinical need that is the target of your device.
        ///•	Example 1: write the common or technical name of the pathology, describe the main symptoms and if it is acute/chronic.
        ///Example 2: write the common or technical name of the handicap, describe if it is innate or caused by incidents or a consequence of a pathology
        ///Example 3: describe what is the patient need in terms of cost / improvement of life conditions / improvement of wellbeing
        ///•	Worked example 1: Down syndrome is a syndr [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ClinicalNeeds {
            get {
                return ResourceManager.GetString("ClinicalNeeds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Here you should describe what are the devices or therapies on the market; if possible, list some pros and cons of the existing solutions.
        ///•	Example: list medical, surgical or technical solutions available; define the criteria to choose amongst the have different options. Comment on residual side effects or contraindications that may be improved.
        ///•	Worked example: congenital club foot is usually treated by braces (mild cases) or surgery. Both solutions require long healing times and frequent specialistic f [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ExistingSolutions {
            get {
                return ResourceManager.GetString("ExistingSolutions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Here you should describe what your end user prefers or needs; this may be the same or different than the patient need and relates more to &quot;easy and friendly to use&quot; that to patient state of health.
        ///•	Example: describe if the device should be lightweight to be portable; describe the sounds and colors that are expected; describe handling and visual requirements
        ///•	Worked example 1: a defibrillator for use in airports and other meeting places should be very intuitive to use and should give vocal commands in t [rest of string was truncated]&quot;;.
        /// </summary>
        public static string IntendedUsers {
            get {
                return ResourceManager.GetString("IntendedUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Here you should describe the most important operating physical principle of your medical device.
        ///•	Example 1: Contactless Infrared Thermometer
        ///•	Worked example 1: An infrared thermometer is a device that measures the infrared radiation – a type of electromagnetic radiation below the visible spectrum of light - emitted by an object. The most basic design of infrared thermometers consists of a lens to focus the infrared thermal radiation onto a detector, which converts the radiant energy into an electric si [rest of string was truncated]&quot;;.
        /// </summary>
        public static string PhysicalPrinciples {
            get {
                return ResourceManager.GetString("PhysicalPrinciples", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Here you should describe all the requirements to certain product. It is written to allow people to understand what a product should do.
        ///Typical components of a product requirements document are:
        ///•	Functional requirements 
        ///o	Describe how your product will meet and solve the clinical need.
        ///	Example 1: describe how the device interacts with the human body and why this interaction is beneficial.
        ///	Worked example 1: the syringe allows to deliver liquid substances, for example drugs, to the peripheral blood [rest of string was truncated]&quot;;.
        /// </summary>
        public static string ProductRequirements {
            get {
                return ResourceManager.GetString("ProductRequirements", resourceCulture);
            }
        }
    }
}
