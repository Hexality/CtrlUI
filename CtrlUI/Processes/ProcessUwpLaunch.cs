﻿using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using static ArnoldVinkCode.ProcessClasses;
using static ArnoldVinkCode.ProcessUwpFunctions;
using static LibraryShared.Classes;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Launch an UWP or Win32Store application manually
        async Task<bool> LaunchProcessManuallyUwpAndWin32Store(string appName, string pathExe, string argument, bool silent, bool allowMinimize)
        {
            try
            {
                //Check if the application exists
                if (UwpGetAppPackageFromAppUserModelId(pathExe) == null)
                {
                    Popup_Show_Status("Close", "Application not found");
                    Debug.WriteLine("Launch application not found, possibly uninstalled.");
                    return false;
                }

                //Show launching message
                if (!silent)
                {
                    Popup_Show_Status("App", "Launching " + appName);
                    Debug.WriteLine("Launching UWP or Win32Store: " + appName + "/" + pathExe);
                }

                //Minimize the CtrlUI window
                if (allowMinimize && ConfigurationManager.AppSettings["MinimizeAppOnShow"] == "True") { await AppMinimize(true); }

                //Launch the UWP or Win32Store application
                ProcessLauncherUwpAndWin32Store(pathExe, argument);
                return true;
            }
            catch
            {
                //Show failed launch messagebox
                await LaunchProcessFailed();
                return false;
            }
        }

        //Launch an UWP or Win32Store databind app
        async Task<bool> LaunchProcessDatabindUwpAndWin32Store(DataBindApp dataBindApp)
        {
            try
            {
                //Check if UWP or Win32Store process is running
                ProcessMulti processMulti = await GetProcessMultiFromDataBindApp(dataBindApp, true);
                if (processMulti != null)
                {
                    bool alreadyRunning = false;
                    if (processMulti.Type == ProcessType.UWP)
                    {
                        alreadyRunning = await CheckLaunchProcessUwp(processMulti, dataBindApp);
                    }
                    else if (processMulti.Type == ProcessType.Win32Store)
                    {
                        alreadyRunning = await CheckLaunchProcessWin32andWin32Store(processMulti, dataBindApp);
                    }

                    if (!alreadyRunning)
                    {
                        Debug.WriteLine(processMulti.Type + " process is already running, skipping the launch.");
                        return false;
                    }
                }

                //Check if the application exists
                if (UwpGetAppPackageFromAppUserModelId(dataBindApp.PathExe) == null)
                {
                    dataBindApp.StatusAvailable = Visibility.Visible;

                    Popup_Show_Status("Close", "Application not found");
                    Debug.WriteLine("Launch application not found, possibly uninstalled.");
                    return false;
                }
                else
                {
                    dataBindApp.StatusAvailable = Visibility.Collapsed;
                }

                //Show application launch message
                Popup_Show_Status("App", "Launching " + dataBindApp.Name);
                Debug.WriteLine("Launching UWP or Win32Store: " + dataBindApp.Name + " from: " + dataBindApp.Category + " path: " + dataBindApp.PathExe);

                //Launch the UWP application
                await LaunchProcessManuallyUwpAndWin32Store(dataBindApp.Name, dataBindApp.PathExe, dataBindApp.Argument, true, true);
                return true;
            }
            catch
            {
                //Show failed launch messagebox
                await LaunchProcessFailed();
                return false;
            }
        }
    }
}