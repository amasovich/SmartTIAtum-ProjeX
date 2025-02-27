using Microsoft.Win32;
using Siemens.Engineering;
using Siemens.Engineering.HW;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartTIAtum_ProjeX.Library
{
    public class TIAConnector
    {
        public static TiaPortal instanceTIA;
        public static Project projectTIA;
        public static Device plcDevice;

        /// <summary>
        /// Open a new instance of TIA Portal V18 with/without user interface
        /// </summary>
        /// <param name="enableGuiTIA"></param>
        public void CreateTIAinstance(bool enableGuiTIA)
        {
            // Set whitelist entry
            SetWhiteList(System.Diagnostics.Process.GetCurrentProcess().ProcessName, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            // Open new TIA instance with user interface
            if (enableGuiTIA)
            {
                instanceTIA = new TiaPortal(TiaPortalMode.WithUserInterface);
            }
            // Open new TIA instance without user interface
            else
            {
                instanceTIA = new TiaPortal(TiaPortalMode.WithoutUserInterface);
            }
        }

        /// <summary>
        /// Create new TIA project at given project path with given project name
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="projectName"></param>
        public void CreateTIAprject(string projectPath, string projectName)
        {
            // Create new directory info
            DirectoryInfo targetDirectory = new DirectoryInfo(projectPath);

            // Create new TIA project
            projectTIA = instanceTIA.Projects.Create(targetDirectory, projectName);
        }

        /// <summary>
        /// Open TIA project at given project path with given project name
        /// </summary>
        /// <param name="projectPath"></param>
        /// <param name="projectName"></param>
        public void OpenTIAproject(string projectPath, string projectName)
        {
            // Create new file info
            FileInfo targetDirectory = new FileInfo(projectPath + "\\" + projectName + ".ap18");

            // Open exiting TIA project
            projectTIA = instanceTIA.Projects.Open(targetDirectory);
        }

        /// <summary>
        /// Create new PLC within project
        /// </summary>
        /// <param name="plcName"></param>
        public void CreatePLC(string plcName)
        {
            string plcVersion = "V3.0";
            string plcArticle = "6ES7 518-4FP00-0AB0";
            string plcIdent = "OrderNumber:" + plcArticle + "/" + plcVersion;
            string plcStation = "station" + plcName;

            // Create new S7-1518F PLC within project
            plcDevice = projectTIA.Devices.CreateWithItem(plcIdent, plcName, plcStation);
        }

        /// <summary>
        /// Set whitelist entry for TIA Portal in registry
        /// </summary>
        /// <param name="ApplicationName"></param>
        /// <param name="ApplicationStartupPath"></param>
        public void SetWhiteList(string ApplicationName, string ApplicationStartupPath)
        {
            RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey software = null;
            try
            {
                software = key.OpenSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .OpenSubKey("18.0")
                    .OpenSubKey("Whitelist")
                    .OpenSubKey(ApplicationName + ".exe")
                    .OpenSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryRights.FullControl);
            }
            catch (Exception)
            {

                //Eintrag in der Whitelist ist nicht vorhanden
                //Entry in whitelist is not available
                software = key.CreateSubKey(@"SOFTWARE\Siemens\Automation\Openness")
                    .CreateSubKey("18.0")
                    .CreateSubKey("Whitelist")
                    .CreateSubKey(ApplicationName + ".exe")
                    .CreateSubKey("Entry", RegistryKeyPermissionCheck.ReadWriteSubTree, RegistryOptions.None);
            }

            string lastWriteTimeUtcFormatted = string.Empty;
            DateTime lastWriteTimeUtc;
            HashAlgorithm hashAlgorithm = SHA256.Create();
            FileStream stream = File.OpenRead(ApplicationStartupPath);
            byte[] hash = hashAlgorithm.ComputeHash(stream);
            // this is how the hash should appear in the .reg file
            string convertedHash = Convert.ToBase64String(hash);
            software.SetValue("FileHash", convertedHash);
            lastWriteTimeUtc = new FileInfo(ApplicationStartupPath).LastWriteTimeUtc;
            // this is how the last write time should be formatted
            lastWriteTimeUtcFormatted = lastWriteTimeUtc.ToString(@"yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            software.SetValue("DateModified", lastWriteTimeUtcFormatted);
            software.SetValue("Path", ApplicationStartupPath);
        }
    }
}
