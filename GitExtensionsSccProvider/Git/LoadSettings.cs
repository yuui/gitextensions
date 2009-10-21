using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitCommands;
using Microsoft.Win32;
using System.Windows.Forms;

namespace Henk.GitExtensionsSccProvider.Git
{
    public static class LoadSettings
    {
        public static void Load()
        {
            Settings.GitDir = GetFromRegistry("gitdir");
        }

        private static string GetFromRegistry(string key)
        {
            string gitPath = GetRegistryValue(Registry.CurrentUser, "Software\\GitExtensions\\GitExtensions\\1.0.0.0", key);

            if (string.IsNullOrEmpty(gitPath))
                gitPath = GetRegistryValue(Registry.Users, "Software\\GitExtensions\\GitExtensions\\1.0.0.0", key);
            return gitPath;
        }

        private static string GetRegistryValue(RegistryKey root, string subkey, string key)
        {
            try
            {
                RegistryKey rk;
                rk = root.OpenSubKey(subkey, false);

                string value = "";

                if (rk != null && rk.GetValue(key) is string)
                {
                    value = rk.GetValue(key).ToString();
                    rk.Flush();
                    rk.Close();
                }

                return value;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show("GitExtensions has insufficient permisions to check the registry.");
            }
            return "";
        }
    }
}
