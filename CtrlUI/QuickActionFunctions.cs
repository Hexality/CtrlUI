﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ArnoldVinkCode.AVImage;
using static ArnoldVinkCode.ProcessUwpFunctions;
using static CtrlUI.AppVariables;
using static LibraryShared.Classes;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Show the quick action prompt
        async Task QuickActionPrompt()
        {
            try
            {
                List<DataBindString> Answers = new List<DataBindString>();
                DataBindString AnswerQuickLaunch = new DataBindString();

                //Get the current quick launch application
                try
                {
                    DataBindApp QuickLaunchApp = CombineAppLists(false, false).Where(x => x.QuickLaunch).FirstOrDefault();
                    if (QuickLaunchApp != null)
                    {
                        AnswerQuickLaunch.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/App.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                        AnswerQuickLaunch.Name = "Quick launch " + QuickLaunchApp.Name;
                        Answers.Add(AnswerQuickLaunch);
                    }
                }
                catch { }

                DataBindString AnswerSortApps = new DataBindString();
                AnswerSortApps.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Sorting.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerSortApps.Name = "Sort applications by name or number and date";
                Answers.Add(AnswerSortApps);

                DataBindString AnswerLaunchExe = new DataBindString();
                AnswerLaunchExe.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Run.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerLaunchExe.Name = "Launch an executable file from disk";
                Answers.Add(AnswerLaunchExe);

                DataBindString AnswerLaunchUwp = new DataBindString();
                AnswerLaunchUwp.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/RunApp.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerLaunchUwp.Name = "Launch a Windows store application";
                Answers.Add(AnswerLaunchUwp);

                DataBindString AnswerFileManager = new DataBindString();
                AnswerFileManager.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Folder.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerFileManager.Name = "Show file browser and manager";
                Answers.Add(AnswerFileManager);

                DataBindString AnswerControlMedia = new DataBindString();
                AnswerControlMedia.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Media.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerControlMedia.Name = "Control playing media and volume";
                Answers.Add(AnswerControlMedia);

                DataBindString AnswerStartMenu = new DataBindString();
                AnswerStartMenu.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Windows.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerStartMenu.Name = "Show the Windows start menu";
                Answers.Add(AnswerStartMenu);

                DataBindString AnswerFpsOverlayer = new DataBindString();
                AnswerFpsOverlayer.ImageBitmap = FileToBitmapImage(new string[] { "Assets/Icons/Fps.png" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                AnswerFpsOverlayer.Name = "Show or hide the fps overlayer";
                Answers.Add(AnswerFpsOverlayer);

                //Check if the Xbox Companion app is installed
                DataBindString AnswerXboxApp = new DataBindString();
                if (UwpGetAppPackageByAppUserModelId("Microsoft.XboxApp_8wekyb3d8bbwe!Microsoft.XboxApp") != null)
                {
                    AnswerXboxApp.ImageBitmap = FileToBitmapImage(new string[] { "Xbox" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, -1, 0);
                    AnswerXboxApp.Name = "Open Xbox Companion app";
                    Answers.Add(AnswerXboxApp);
                }

                DataBindString messageResult = await Popup_Show_MessageBox("Quick action", "* You can change the quick launch application in the CtrlUI settings.", "", Answers);
                if (messageResult != null)
                {
                    if (messageResult == AnswerQuickLaunch)
                    {
                        await LaunchQuickLaunchApp();
                    }
                    else if (messageResult == AnswerSortApps)
                    {
                        SortAppLists(false, false);
                    }
                    else if (messageResult == AnswerLaunchExe)
                    {
                        await RunExecutableFile();
                    }
                    else if (messageResult == AnswerLaunchUwp)
                    {
                        await RunUwpApplication();
                    }
                    else if (messageResult == AnswerFileManager)
                    {
                        await ShowFileManager();
                    }
                    else if (messageResult == AnswerStartMenu)
                    {
                        ShowWindowStartMenu();
                    }
                    else if (messageResult == AnswerControlMedia)
                    {
                        await Popup_Show(grid_Popup_Media, grid_Popup_Media_PlayPause);
                    }
                    else if (messageResult == AnswerFpsOverlayer)
                    {
                        await CloseShowFpsOverlayer();
                    }
                    else if (messageResult == AnswerXboxApp)
                    {
                        await LaunchXboxCompanion();
                    }
                }
            }
            catch { }
        }

        ////Launch windows File Explorer
        //async Task LaunchWindowsFileExplorer()
        //{
        //    try
        //    {
        //        //await ProcessLauncherWin32Prepare(Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\explorer.exe", "", "", true, true, false);
        //        //await ProcessLauncherUwpPrepare("File Explorer", "c5e2524a-ea46-4f67-841f-6a9465d9d515_cw5n1h2txyewy!App", string.Empty, false, false);
        //    }
        //    catch { }
        //}

        //Launch windows Xbox Companion
        async Task LaunchXboxCompanion()
        {
            try
            {
                //Launch the UWP or Win32Store application
                await PrepareProcessLauncherUwpAndWin32StoreAsync("Xbox Companion", "Microsoft.XboxApp_8wekyb3d8bbwe!Microsoft.XboxApp", string.Empty, false, false, true);
            }
            catch { }
        }

        //Launch the set quick launch app
        async Task LaunchQuickLaunchApp()
        {
            try
            {
                DataBindApp quickLaunchApp = CombineAppLists(false, false).Where(x => x.QuickLaunch).FirstOrDefault();
                if (quickLaunchApp != null)
                {
                    //Check which launch method needs to be used
                    await LaunchProcessSelector(quickLaunchApp);
                }
                else
                {
                    Popup_Show_Status("App", "Please set a quick launch app");
                    Debug.WriteLine("Please set a quick launch app");
                }
            }
            catch
            {
                Popup_Show_Status("App", "Please set a quick launch app");
                Debug.WriteLine("Please set a quick launch app");
            }
        }
    }
}