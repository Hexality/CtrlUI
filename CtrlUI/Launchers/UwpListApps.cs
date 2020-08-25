﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.ApplicationModel;
using Windows.Gaming.Preview.GamesEnumeration;
using Windows.Management.Deployment;
using static ArnoldVinkCode.AVImage;
using static ArnoldVinkCode.ProcessClasses;
using static ArnoldVinkCode.ProcessUwpFunctions;
using static CtrlUI.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.Enums;

namespace CtrlUI
{
    partial class WindowMain
    {
        async Task UwpScanAddLibrary()
        {
            try
            {
                //Get all the installed uwp apps
                PackageManager deployPackageManager = new PackageManager();
                string currentUserIdentity = WindowsIdentity.GetCurrent().User.Value;
                IEnumerable<Package> appPackages = deployPackageManager.FindPackagesForUser(currentUserIdentity);
                foreach (Package appPackage in appPackages)
                {
                    try
                    {
                        //Filter out system apps and others
                        if (appPackage.IsBundle) { continue; }
                        if (appPackage.IsOptional) { continue; }
                        if (appPackage.IsFramework) { continue; }
                        if (appPackage.IsResourcePackage) { continue; }
                        if (appPackage.SignatureKind != PackageSignatureKind.Store) { continue; }

                        //Get detailed application information
                        AppxDetails appxDetails = UwpGetAppxDetailsFromAppPackage(appPackage);

                        //Check if executable name is valid
                        if (string.IsNullOrWhiteSpace(appxDetails.ExecutableName))
                        {
                            continue;
                        }

                        //Check if application name is valid
                        if (string.IsNullOrWhiteSpace(appxDetails.DisplayName) || appxDetails.DisplayName.StartsWith("ms-resource"))
                        {
                            continue;
                        }

                        //Check if the application is a game
                        IEnumerable<GameListEntry> uwpGameList = (await GameList.FindAllAsync(appPackage.Id.FamilyName)).Where(x => x.Category == GameListCategory.ConfirmedBySystem);
                        if (!uwpGameList.Any())
                        {
                            //Debug.WriteLine(appPackage.Id.FamilyName + " is not an uwp game.");
                            continue;
                        }

                        await UwpAddApplication(appPackage, appxDetails);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed adding uwp games library: " + ex.Message);
            }
        }

        async Task UwpAddApplication(Package appPackage, AppxDetails appxDetails)
        {
            try
            {
                //Get application name
                string appName = appxDetails.DisplayName;

                //Get basic application information
                string runCommand = appPackage.Id.FamilyName;
                vLauncherAppAvailableCheck.Add(runCommand);

                //Check if application is already added
                DataBindApp launcherExistCheck = List_Launchers.Where(x => x.PathExe.ToLower() == runCommand.ToLower()).FirstOrDefault();
                if (launcherExistCheck != null)
                {
                    //Debug.WriteLine("UWP app already in list: " + appIds);
                    return;
                }

                //Load the application image
                BitmapImage iconBitmapImage = FileToBitmapImage(new string[] { appxDetails.SquareLargestLogoPath, appxDetails.WideLargestLogoPath, appName, "Microsoft" }, vImageSourceFolders, vImageBackupSource, IntPtr.Zero, 90, 0);

                //Add the application to the list
                DataBindApp dataBindApp = new DataBindApp()
                {
                    Type = ProcessType.UWP,
                    Launcher = AppLauncher.UWP,
                    Category = AppCategory.Launcher,
                    Name = appName,
                    ImageBitmap = iconBitmapImage,
                    PathExe = runCommand,
                    StatusLauncher = vImagePreloadMicrosoft
                };

                await ListBoxAddItem(lb_Launchers, List_Launchers, dataBindApp, false, false);
                //Debug.WriteLine("Added UWP app: " + appName);
            }
            catch
            {
                Debug.WriteLine("Failed adding UWP app: " + appxDetails.DisplayName);
            }
        }
    }
}