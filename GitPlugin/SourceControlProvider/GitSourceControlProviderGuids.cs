using System;
using System.Collections.Generic;
using System.Text;

namespace GitPlugin.SourceControlProvider
{
    class GitSourceControlProviderGuids
    {
        // Unique ID of the source control provider; this is also used as the command UI context to show/hide the pacakge UI
        public static readonly Guid guidSccProvider = new Guid("{2A39F2AC-8662-4fd8-ACAF-02C6050EB182}");
        // The guid of the source control provider service (implementing IVsSccProvider interface)
        public static readonly Guid guidSccProviderService = new Guid("{E116265D-3096-4349-86B6-AF5EE79599BB}");
        // The guid of the source control provider package (implementing IVsPackage interface)
        public static readonly Guid guidSccProviderPkg = new Guid("{15D66BFE-4B6F-485d-9B82-60AC23407CED}");
        // Other guids for menus and commands
        public static readonly Guid guidSccProviderCmdSet = new Guid("{A4EA87B6-0D29-46df-B2A9-9A17DD29B304}");


    }
}
