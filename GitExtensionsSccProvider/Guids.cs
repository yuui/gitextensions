// Guids.cs
// MUST match guids.h
using System;

namespace Henk.GitExtensionsSccProvider
{
    static class GuidList
    {
        public const string guidGitExtensionsSccProviderPkgString = "a9eae1e6-b97b-4d38-a954-d611b8bcbe6c";
        public const string guidGitExtensionsSccProviderCmdSetString = "1b77d609-8cc2-4e5a-ad35-23c89df5cfe6";
        public const string guidToolWindowPersistanceString = "43a46293-f6df-4d84-964e-7d8bd45edeb7";

        public const string guidSccProviderString = "B05265E5-7D44-4c2a-8275-A9F192CDE223";
        public const string guidSccProviderServiceString = "EF4C3A69-4761-455b-90E5-6FF364156B27";

        public static readonly Guid guidGitExtensionsSccProviderPkg = new Guid(guidGitExtensionsSccProviderPkgString);
        public static readonly Guid guidGitExtensionsSccProviderCmdSet = new Guid(guidGitExtensionsSccProviderCmdSetString);

        public static readonly Guid guidSccProvider = new Guid(guidSccProviderString);
        public static readonly Guid guidSccProviderService = new Guid(guidSccProviderServiceString);

    };
}