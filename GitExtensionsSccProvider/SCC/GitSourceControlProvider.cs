using System;
using System.Collections.Generic;
using Microsoft.VisualStudio;
using System.Text;
using Microsoft.VisualStudio.Shell.Interop;
using System.Runtime.InteropServices;

namespace Henk.GitExtensionsSccProvider
{
    [Guid("B05265E5-7D44-4c2a-8275-A9F192CDE223")]
    public class GitSourceControlProvider : 
        IVsSccProvider,
        IVsSccManager2/*,
        /*IVsQueryEditQuerySave3,*/
        //IVsSccGlyphs
    {
        public bool Active { get; private set; }

        #region IVsSccProvider Members

        public int AnyItemsUnderSourceControl(out int pfResult)
        {
            // Set pfResult to false when the solution can change to an other scc provider
            pfResult = 1;
            return VSConstants.S_OK;
        }

        public int SetActive()
        {
            if (!this.Active)
            {
                this.Active = true;
            }

            return VSConstants.S_OK;
        }

        public int SetInactive()
        {
            if (this.Active)
            {
                this.Active = false;
            }

            return VSConstants.S_OK;
        }

        #endregion

        #region IVsSccManager2 Members

        public int BrowseForProject(out string pbstrDirectory, out int pfOK)
        {
            pbstrDirectory = null;
            pfOK = 0;

            return VSConstants.E_NOTIMPL;
        }

        public int CancelAfterBrowseForProject()
        {
            return VSConstants.E_NOTIMPL;
        }

        public int GetSccGlyph(int cFiles, string[] rgpszFullPaths, VsStateIcon[] rgsiGlyphs, uint[] rgdwSccStatus)
        {
            if (GitFileStatus.GetFileStatus(rgpszFullPaths[0]).IsChanged)
                rgsiGlyphs[0] = VsStateIcon.STATEICON_CHECKEDIN;
            else
            if (GitFileStatus.GetFileStatus(rgpszFullPaths[0]).IsDeleted)
                rgsiGlyphs[0] = VsStateIcon.STATEICON_CHECKEDOUT;
            else
            if (GitFileStatus.GetFileStatus(rgpszFullPaths[0]).IsNew)
                rgsiGlyphs[0] = VsStateIcon.STATEICON_DISABLED;
            else
            if (GitFileStatus.GetFileStatus(rgpszFullPaths[0]).IsTracked)
                rgsiGlyphs[0] = VsStateIcon.STATEICON_EXCLUDEDFROMSCC;

            rgdwSccStatus[0] = (uint)__SccStatus.SCC_STATUS_CONTROLLED;
            return VSConstants.S_OK;
        }

        public int GetSccGlyphFromStatus(uint dwSccStatus, VsStateIcon[] psiGlyph)
        {
            return VSConstants.S_OK;
        }

        public int IsInstalled(out int pbInstalled)
        {
            pbInstalled = 1;
            return VSConstants.S_OK;
        }

        public int RegisterSccProject(IVsSccProject2 pscp2Project, string pszSccProjectName, string pszSccAuxPath, string pszSccLocalPath, string pszProvider)
        {
            return VSConstants.S_OK;
        }

        public int UnregisterSccProject(IVsSccProject2 pscp2Project)
        {
            return VSConstants.S_OK;
        }

        #endregion
    }
}
