﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Apprefine.MattermostBots.PollBot.Resources {
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
    internal class Langs {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Langs() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Apprefine.MattermostBots.PollBot.Resources.Langs", typeof(Langs).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Only one open poll can be active per channel for answering simplicity. You can have many closed polls if you want..
        /// </summary>
        internal static string ActiveOpenPollExists {
            get {
                return ResourceManager.GetString("ActiveOpenPollExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your answer in poll {0} has been updated. Thx :+1:.
        /// </summary>
        internal static string AnswerUpdated {
            get {
                return ResourceManager.GetString("AnswerUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage: `/poll answer YOUR ANSWER`. This will only work if there&apos;s an open poll in the channel. You can create new polls with `/poll new`..
        /// </summary>
        internal static string AnswerUsage {
            get {
                return ResourceManager.GetString("AnswerUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage: `/poll new open DESCRIPTION` - creates a new open poll. &quot;Open&quot; means that users can give text answers..
        /// </summary>
        internal static string NewOpenPollUsage {
            get {
                return ResourceManager.GetString("NewOpenPollUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage:
        ///- `/poll new open DESCRIPTION` - creates a new open poll. &quot;Open&quot; means that users can give text answers.
        ///- `/poll new closed ANSWERS DESCRIPTION` - NOT IMPLEMENTED.
        /// </summary>
        internal static string NewPollUsage {
            get {
                return ResourceManager.GetString("NewPollUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no active open polls in the channel..
        /// </summary>
        internal static string NoActivePolls {
            get {
                return ResourceManager.GetString("NoActivePolls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to There are no active open polls which you own in the channel..
        /// </summary>
        internal static string NoActivePollsByYou {
            get {
                return ResourceManager.GetString("NoActivePollsByYou", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No answers found or there&apos;s no such poll in this channel..
        /// </summary>
        internal static string NoAnswersFound {
            get {
                return ResourceManager.GetString("NoAnswersFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not implemented yet, stay tuned :).
        /// </summary>
        internal static string NotImplemented {
            get {
                return ResourceManager.GetString("NotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} has closed the poll {1}. Everyone can display results with `/poll results`..
        /// </summary>
        internal static string OpenPollClosed {
            get {
                return ResourceManager.GetString("OpenPollClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} has created a poll &quot;{1}&quot;. The ID is {2}..
        /// </summary>
        internal static string OpenPollCreated {
            get {
                return ResourceManager.GetString("OpenPollCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage (brackets denote optional parameters):
        ///- `/poll results [ID]` - displays results of a poll. If ID is supplied - a specific poll, otherwise latest..
        /// </summary>
        internal static string ResultsUsage {
            get {
                return ResourceManager.GetString("ResultsUsage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Usage (brackets denote optional parameters):
        ///- `/poll answer YOUR ANSWER`. This will only work if there&apos;s an open poll in the channel. You can create new polls with `/poll new`.
        ///- `/poll new open DESCRIPTION` - creates a new open poll. &quot;Open&quot; means that users can give text answers.
        ///- `/poll new closed ANSWERS DESCRIPTION` - NOT IMPLEMENTED
        ///- `/poll close` - closes your active poll. Nobody will be able to add more answers.
        ///- `/poll results [ID]` - displays results of a poll. If ID is supplied - a specif [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Usage {
            get {
                return ResourceManager.GetString("Usage", resourceCulture);
            }
        }
    }
}
