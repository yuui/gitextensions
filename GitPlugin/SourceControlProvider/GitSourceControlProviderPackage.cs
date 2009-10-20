using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Shell;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Globalization;
using System.Diagnostics;

namespace GitPlugin.SourceControlProvider
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the registration utility (regpkg.exe) that this class needs
    // to be registered as package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // A Visual Studio component can be registered under different regitry roots; for instance
    // when you debug your package you want to register it in the experimental hive. This
    // attribute specifies the registry root to use if no one is provided to regpkg.exe with
    // the /root switch.
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\9.0")]
    // This attribute is used to register the informations needed to show the this package
    // in the Help/About dialog of Visual Studio.
    //[InstalledProductRegistration(false, "#110", "#112", "1.0", IconResourceID = 400)]
    // In order be loaded inside Visual Studio in a machine that has not the VS SDK installed, 
    // package needs to have a valid load key (it can be requested at 
    // http://msdn.microsoft.com/vstudio/extend/). This attributes tells the shell that this 
    // package has a load key embedded in its resources.
    [ProvideLoadKey("Standard", "0.1", "Git Menu", "GitExtensions", 1)]
    [ProvideService(typeof(GitSourceControlProvider), ServiceName = "GitExtensions Source Control Provider Service")]
    [ProvideSourceControlProvider("Git Source Control Provider", "#100")]
    //[ProvideOptionPage(typeof(Options), "Git Menu", "General", 101, 106, true)]
    //[ProvideProfile(typeof(Options), "Git Menu", "General", 101, 106, true)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    //[ProvideMenuResource(1000, 1)]
    // Pre-load the package when the command UI context is asserted (the provider will be automatically loaded after restarting the shell if it was active last time the shell was shutdown)
    [ProvideAutoLoad("{313395F9-5635-4ba8-92AD-268D5951182A}")]
    // Declare the package guid
    [Guid("313395F9-5635-4ba8-92AD-268D5951182A")]
    public sealed class GitExtensionsPackage : Package
    {

        public GitSourceControlProvider GitSourceControlProvider { get; private set; }

        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public GitExtensionsPackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));

        }



        /////////////////////////////////////////////////////////////////////////////
        // Overriden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initilaization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            this.GitSourceControlProvider = new GitSourceControlProvider();
            ((IServiceContainer)this).AddService(typeof(GitSourceControlProvider), this.GitSourceControlProvider, true);

            IVsRegisterScciProvider rscp = (IVsRegisterScciProvider)GetService(typeof(IVsRegisterScciProvider));
            rscp.RegisterSourceControlProvider(new Guid("{313395F9-5635-4ba8-92AD-268D5951182A}"));

            //var commands = new CommandSet(this);
            //commands.Initialize();

            //Settings.Instance.LoadSettings(this);
        }

        protected override int QueryClose(out bool canClose)
        {
            //Settings.Instance.SaveSettings(this);
            return base.QueryClose(out canClose);
        }
        #endregion
    }
}
