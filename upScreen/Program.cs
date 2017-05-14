using System;
using System.Linq;
using System.Windows.Forms;
using upScreenLib.LogConsole;
using Microsoft.Win32;
using System.IO;
using upScreenLib;

namespace upScreen
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Allocate console
            if (args.Length > 0 && args.Contains("-console"))
                aConsole.Allocate();

            bool debug = args.Contains("-debug");

            Log.Init("Debug.html", l.Debug | l.Info | l.Warning | l.Error | l.Client, true, debug);

            foreach (string s in args)
                Log.Write(l.Info, "Argument: {0}", s);
            
            Settings.Load();
            Profile.FromFileMenu = CheckArgs(args);

            if (!args.Contains("-nomenus"))
                AddContextMenu();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmCapture());
        }

        #region Context Menus

        // Path to the .ico file of upScreen
        private static readonly string IconPath = string.Format("\"{0}\"", Path.Combine(Application.StartupPath, "upScreen.ico"));
        // The applies-to condition, used to display our menu item only when an image file is right-clicked
        const string AppliesTo = "System.FileExtension:\"jpg\" OR System.FileExtension:\"jpeg\" OR System.FileExtension:\"png\" OR System.FileExtension:\"gif\"";
        // The label of the menu item
        const string MUIVerb = "Upload with upScreen";
        // The command used to open upscreen.exe with the right-clicked file as arguement
        private static readonly string command = string.Format("\"{0}\" \"%1\"", Application.ExecutablePath);

        /// <summary>
        /// Create the necessary registry keys to add a 'Upload with upScreen'
        ///  option to the right-click menu of image files
        /// </summary>
        private static void AddContextMenu()
        {
            string regPath = "Software\\Classes\\*\\Shell\\upload_with_upscreen";
            RegistryKey key = Registry.CurrentUser;
            // Create a new key for our menu item
            key.CreateSubKey(regPath);
            key = Registry.CurrentUser.OpenSubKey(regPath, true);
            // Add MUIVerb, Icon and AppliesTo string values
            key.SetValue("MUIVerb", MUIVerb);
            key.SetValue("Icon", IconPath);
            key.SetValue("AppliesTo", AppliesTo);
            // Create the Command sub-key
            key.CreateSubKey("Command");
            key.Close();

            regPath = string.Format("{0}\\Command", regPath);
            key = Registry.CurrentUser.OpenSubKey(regPath, true);
            // put our command to (Default)
            key.SetValue("", command);
            key.Close();
        }

        /// <summary>
        /// Check if any of the arguements is a valid file path
        /// </summary>
        private static bool CheckArgs(string[] args)
        {
            foreach (string s in args.Where(File.Exists))
                Profile.ArgFiles.Add(s);

            return args.Any(File.Exists);
        }

        #endregion
    }
}
