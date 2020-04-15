﻿using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.ProcessClasses;
using static ArnoldVinkCode.ProcessFunctions;
using static ArnoldVinkCode.ProcessUwpFunctions;
using static CtrlUI.AppVariables;
using static CtrlUI.ImageFunctions;
using static LibraryShared.Classes;
using static LibraryShared.SoundPlayer;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Check the top or bottom listbox
        ListBox TopVisibleListBoxWithItems()
        {
            try
            {
                if (sp_Games.Visibility == Visibility.Visible && lb_Games.Items.Count > 0) { return lb_Games; }
                else if (sp_Apps.Visibility == Visibility.Visible && lb_Apps.Items.Count > 0) { return lb_Apps; }
                else if (sp_Emulators.Visibility == Visibility.Visible && lb_Emulators.Items.Count > 0) { return lb_Emulators; }
                else if (sp_Shortcuts.Visibility == Visibility.Visible && lb_Shortcuts.Items.Count > 0) { return lb_Shortcuts; }
                else if (sp_Processes.Visibility == Visibility.Visible && lb_Processes.Items.Count > 0) { return lb_Processes; }
            }
            catch { }
            return null;
        }
        ListBox BottomVisibleListBox()
        {
            try
            {
                if (sp_Processes.Visibility == Visibility.Visible) { return lb_Processes; }
                else if (sp_Shortcuts.Visibility == Visibility.Visible) { return lb_Shortcuts; }
                else if (sp_Emulators.Visibility == Visibility.Visible) { return lb_Emulators; }
                else if (sp_Apps.Visibility == Visibility.Visible) { return lb_Apps; }
                else if (sp_Games.Visibility == Visibility.Visible) { return lb_Games; }
            }
            catch { }
            return null;
        }

        //Register Interface Handlers
        void RegisterInterfaceHandlers()
        {
            try
            {
                //Monitor window state changes
                SizeChanged += CheckWindowStateAndSize;
                StateChanged += CheckWindowStateAndSize;

                //Main menu functions
                grid_Popup_MainMenu_button_Close.Click += Button_Popup_Close_Click;
                listbox_MainMenu.PreviewKeyUp += ListBox_Menu_KeyPressUp;
                listbox_MainMenu.PreviewMouseUp += ListBox_Menu_MousePressUp;
                button_MenuHamburger.Click += Button_MenuHamburger_Click;
                button_MenuSearch.Click += Button_MenuSearch_Click;
                button_MenuSorting.Click += Button_MenuSorting_Click;

                //App list functions
                lb_Search.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Search.PreviewMouseUp += ListBox_Apps_MousePressUp;
                lb_Games.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Games.PreviewMouseUp += ListBox_Apps_MousePressUp;
                lb_Apps.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Apps.PreviewMouseUp += ListBox_Apps_MousePressUp;
                lb_Emulators.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Emulators.PreviewMouseUp += ListBox_Apps_MousePressUp;
                lb_Shortcuts.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Shortcuts.PreviewMouseUp += ListBox_Apps_MousePressUp;
                lb_Processes.PreviewKeyUp += ListBox_Apps_KeyPressUp;
                lb_Processes.PreviewMouseUp += ListBox_Apps_MousePressUp;

                //MessageBox list functions
                lb_MessageBox.PreviewKeyUp += ListBox_MessageBox_KeyPressUp;
                lb_MessageBox.PreviewMouseUp += ListBox_MessageBox_MousePressUp;

                //Manage functions
                btn_Manage_ResetAppLogo.Click += Button_Manage_ResetAppLogo_Click;
                btn_Manage_SaveEditApp.Click += Button_Manage_SaveEditApp_Click;
                btn_Manage_MoveAppLeft.Click += Btn_Manage_MoveAppLeft_Click;
                btn_Manage_MoveAppRight.Click += Btn_Manage_MoveAppRight_Click;
                lb_Manage_AddAppCategory.SelectionChanged += Lb_Manage_AddAppCategory_SelectionChanged;

                //Media functions
                grid_Popup_Media_Previous.Click += Button_Media_PreviousItem;
                grid_Popup_Media_PlayPause.Click += Button_Media_PlayPause;
                grid_Popup_Media_Next.Click += Button_Media_NextItem;
                grid_Popup_Media_VolumeMute.Click += Button_Media_VolumeMute;
                grid_Popup_Media_VolumeDown.Click += Button_Media_VolumeDown;
                grid_Popup_Media_VolumeUp.Click += Button_Media_VolumeUp;

                //Popup functions
                grid_Popup_FilePicker_button_ControllerRight.Click += Button_FilePicker_button_ControllerRight_Click;
                grid_Popup_FilePicker_button_ControllerLeft.Click += Button_FilePicker_button_ControllerLeft_Click;
                grid_Popup_FilePicker_button_ControllerUp.Click += Button_FilePicker_button_ControllerUp_Click;
                grid_Popup_Media_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_Manage_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_Settings_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_Help_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_MessageBox_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_Welcome_button_Close.Click += Button_Popup_Close_Click;

                //Search functions
                lb_Search.PreviewKeyDown += ListBox_Search_KeyPressDown;
                grid_Popup_Search_textbox.TextChanged += Grid_Popup_Search_textbox_TextChanged;
                grid_Popup_Search_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_Search_button_KeyboardControllerIcon.Click += Button_SearchKeyboardController_Click;
                grid_Popup_Search_button_Reset.Click += Grid_Popup_Search_button_Reset_Click;

                //Text Input functions
                grid_Popup_TextInput_button_Close.Click += Button_Popup_Close_Click;
                grid_Popup_TextInput_textbox.PreviewKeyUp += Grid_Popup_TextInput_textbox_PreviewKeyUp;
                grid_Popup_TextInput_button_KeyboardControllerIcon.Click += Button_TextInputKeyboardController_Click;
                grid_Popup_TextInput_button_Reset.Click += Grid_Popup_TextInput_button_Reset_Click;
                grid_Popup_TextInput_button_ConfirmText.Click += Button_TextInputConfirmText_Click;

                //File Picker functions
                lb_FilePicker.PreviewKeyUp += ListBox_FilePicker_KeyPressUp;
                lb_FilePicker.PreviewKeyDown += ListBox_FilePicker_KeyPressDown;
                lb_FilePicker.PreviewMouseUp += ListBox_FilePicker_MousePressUp;
                lb_FilePicker.SelectionChanged += Lb_FilePicker_SelectionChanged;
                grid_Popup_FilePicker_button_SelectFolder.Click += Grid_Popup_FilePicker_button_SelectFolder_Click;
                btn_AddAppLogo.Click += Button_ShowFilePicker;
                btn_AddAppExePath.Click += Button_ShowFilePicker;
                btn_AddAppPathLaunch.Click += Button_ShowFilePicker;
                btn_AddAppPathRoms.Click += Button_ShowFilePicker;
                btn_Settings_ChangeBackgroundImage.Click += Button_ShowFilePicker;
                btn_Settings_ChangeBackgroundVideo.Click += Button_ShowFilePicker;
                btn_Settings_InterfaceSoundPackName.Click += Button_ShowFilePicker;

                //Profile Manager functions
                grid_Popup_ProfileManager_button_ControllerRight.Click += Button_Popup_Close_Click;
                grid_Popup_ProfileManager_button_ChangeProfile.Click += Grid_Popup_ProfileManager_button_ChangeProfile_Click;
                grid_Popup_ProfileManager_button_ProfileAdd.Click += Grid_Popup_ProfileManager_button_ProfileAdd_Click;
                grid_Popup_ProfileManager_textbox_ProfileString1.KeyDown += grid_Popup_ProfileManager_textbox_ProfileString_KeyDown;
                grid_Popup_ProfileManager_textbox_ProfileString2.KeyDown += grid_Popup_ProfileManager_textbox_ProfileString_KeyDown;
                lb_ProfileManager.PreviewKeyUp += ListBox_ProfileManager_KeyPressUp;
                lb_ProfileManager.PreviewKeyDown += ListBox_ProfileManager_KeyPressDown;
                lb_ProfileManager.PreviewMouseUp += ListBox_ProfileManager_MousePressUp;

                //Color Picker functions
                grid_Popup_ColorPicker_button_ControllerRight.Click += Button_Popup_Close_Click;
                lb_ColorPicker.PreviewKeyUp += ListBox_ColorPicker_KeyPressUp;
                lb_ColorPicker.PreviewMouseUp += ListBox_ColorPicker_MousePressUp;

                //Welcome functions
                grid_Popup_Welcome_button_LaunchDirectXInput.Click += Button_LaunchDirectXInput_Click;
                grid_Popup_Welcome_button_Start.Click += Button_Popup_Close_Click;
                grid_Popup_Welcome_button_Edge.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Kodi.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Spotify.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Steam.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Origin.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Uplay.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_GoG.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_Battle.Click += Button_ShowFilePicker;
                grid_Popup_Welcome_button_PS4Remote.Click += Button_ShowFilePicker;

                //Settings functions
                btn_Settings_AppQuickLaunch.Click += Button_ShowStringPicker;
                btn_Settings_LaunchDirectXInput.Click += Button_LaunchDirectXInput_Click;
                btn_Settings_CheckControllers.Click += Button_Settings_CheckControllers_Click;
                btn_Settings_CheckForUpdate.Click += Button_Settings_CheckForUpdate_Click;
                btn_Settings_AddGeforceExperience.Click += Button_Settings_AddGeforceExperience_Click;
                btn_Settings_ColorPickerAccent.Click += Button_Settings_ColorPickerAccent;

                //Help functions
                btn_Help_ProjectWebsite.Click += Button_Help_ProjectWebsite_Click;
                btn_Help_OpenDonation.Click += Button_Help_OpenDonation_Click;

                //MediaElement functions
                grid_Video_Background.MediaEnded += Grid_Video_Background_MediaEnded;
                grid_Video_Background.MediaFailed += Grid_Video_Background_MediaFailed;

                //Global functions
                this.PreviewMouseMove += WindowMain_MouseMove;
                this.PreviewMouseDown += WindowMain_PreviewMouseDown;
                this.PreviewKeyUp += WindowMain_KeyPressUp;

                Debug.WriteLine("Registered all the interface handlers.");
            }
            catch { }
        }

        //Update the user interface clock
        void UpdateClock()
        {
            try
            {
                //Update the time and image
                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        //Update the clock image
                        string clockImageNumber = DateTime.Now.ToString("hmm");
                        string currentImage = img_Main_Time.Source.ToString();
                        string updatedImage = "pack://application:,,,/Assets/Clock/" + clockImageNumber + ".png";
                        if (currentImage.ToLower() != updatedImage.ToLower())
                        {
                            img_Main_Time.Source = FileToBitmapImage(new string[] { updatedImage }, IntPtr.Zero, -1, 0);
                        }

                        //Change the time format
                        if (vMainMenuOpen)
                        {
                            txt_Main_Date.Text = DateTime.Now.ToString("d MMMM");
                            txt_Main_Time.Text = DateTime.Now.ToShortTimeString();
                        }
                        else
                        {
                            txt_Main_Date.Text = string.Empty;
                            txt_Main_Time.Text = DateTime.Now.ToShortTimeString();
                        }
                    }
                    catch { }
                });
            }
            catch { }
        }

        //Update the current window status
        async Task UpdateWindowStatus()
        {
            try
            {
                vProcessDirectXInput = GetProcessByNameOrTitle("DirectXInput", false);
                vProcessKeyboardController = GetProcessByNameOrTitle("KeyboardController", false);
                int focusedAppId = GetFocusedProcess().Identifier;
                bool appActivated = false;

                AVActions.ActionDispatcherInvoke(delegate
                {
                    try
                    {
                        if (WindowState == WindowState.Maximized) { vAppMaximized = true; } else { vAppMaximized = false; }
                        if (WindowState == WindowState.Minimized) { vAppMinimized = true; } else { vAppMinimized = false; }
                        if (vProcessCurrent.Id == focusedAppId)
                        {
                            //Play background media
                            grid_Video_Background.Play();

                            //Hide window status message
                            grid_WindowActive.Opacity = 0;
                            grid_App.IsHitTestVisible = true;

                            if (!vAppActivated) { appActivated = true; }
                            vAppActivated = true;
                        }
                        else
                        {
                            //Pause background media
                            grid_Video_Background.Pause();

                            //Show window status message
                            grid_WindowActive.Opacity = 0.80;
                            grid_App.IsHitTestVisible = false;

                            vAppActivated = false;
                        }
                    }
                    catch { }
                });

                //Check if application window activated
                if (appActivated)
                {
                    await AppWindowActivated();
                }
            }
            catch { }
        }

        //Application window activated event
        async Task AppWindowActivated()
        {
            try
            {
                Debug.WriteLine("Welcome back to the application.");

                //Hide the mouse cursor
                await MouseCursorHide();
            }
            catch { }
        }

        //Check the applications running status
        void CheckAppRunningStatus(IEnumerable<Process> processesList)
        {
            try
            {
                //Check if processes list is provided
                if (processesList == null)
                {
                    processesList = Process.GetProcesses();
                }

                //Update main menu launchers status
                if (processesList.Any(x => x.ProcessName.ToLower() == "steam"))
                {
                    AVActions.ElementSetValue(img_Menu_SteamStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_SteamStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "origin"))
                {
                    AVActions.ElementSetValue(img_Menu_OriginStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_OriginStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "galaxyclient"))
                {
                    AVActions.ElementSetValue(img_Menu_GoGStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_GoGStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "upc"))
                {
                    AVActions.ElementSetValue(img_Menu_UplayStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_UplayStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "bethesdanetlauncher"))
                {
                    AVActions.ElementSetValue(img_Menu_BethesdaStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_BethesdaStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "epicgameslauncher"))
                {
                    AVActions.ElementSetValue(img_Menu_EpicStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_EpicStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "battle.net"))
                {
                    AVActions.ElementSetValue(img_Menu_BlizzardStatus, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_BlizzardStatus, OpacityProperty, 0.40);
                }

                if (processesList.Any(x => x.ProcessName.ToLower() == "directxinput"))
                {
                    AVActions.ElementSetValue(img_Menu_DirectXInput, OpacityProperty, 1.00);
                }
                else
                {
                    AVActions.ElementSetValue(img_Menu_DirectXInput, OpacityProperty, 0.40);
                }
            }
            catch { }
        }

        //Show the mouse cursor
        void MouseCursorShow()
        {
            try
            {
                //Update the last mouse interaction time
                vMouseLastInteraction = Environment.TickCount;

                //Set the mouse cursor when not visible
                AVActions.ActionDispatcherInvoke(delegate
                {
                    if (this.Cursor == Cursors.None)
                    {
                        Mouse.SetCursor(Cursors.Arrow);
                    }
                });
            }
            catch { }
        }

        //Hide the mouse cursor
        async Task MouseCursorHide()
        {
            try
            {
                //Update the last mouse interaction time
                vMouseLastInteraction = Environment.TickCount;

                //Check if the mouse hide setting is enabled
                if (ConfigurationManager.AppSettings["HideMouseCursor"] == "False")
                {
                    return;
                }

                //Check if the application is active and any controller is connected
                if (vAppActivated && vControllerAnyConnected() && vProcessKeyboardController == null)
                {
                    //Move the mouse cursor
                    Point LocationFromScreen = new Point();
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        LocationFromScreen = this.PointToScreen(new Point(255, 44));
                    });

                    int TargetX = Convert.ToInt32(LocationFromScreen.X);
                    int TargetY = Convert.ToInt32(LocationFromScreen.Y);
                    SetCursorPos(TargetX, TargetY);
                    await Task.Delay(10);

                    //Hide the mouse cursor
                    AVActions.ActionDispatcherInvoke(delegate
                    {
                        Mouse.SetCursor(Cursors.None);
                    });

                    //Debug.WriteLine("Hiding the mouse cursor.");
                }
            }
            catch { }
        }

        //Check if the mouse cursor has moved
        async Task MouseCursorCheckMovement()
        {
            try
            {
                //Get the current mouse position
                GetCursorPos(out PointWin MouseCurrentPosition);

                //Check if the mouse has moved since the last time
                bool LastInteraction = Environment.TickCount - vMouseLastInteraction > 5000;
                bool LastMovement = MouseCurrentPosition.X == vMousePreviousPosition.X && MouseCurrentPosition.Y == vMousePreviousPosition.Y;
                if (LastInteraction && LastMovement)
                {
                    await MouseCursorHide();
                }

                //Update the previous mouse position
                vMousePreviousPosition = MouseCurrentPosition;
            }
            catch { }
        }

        //Hide or recover the CtrlUI application
        async Task AppWindow_HideShow()
        {
            try
            {
                Debug.WriteLine("Show or hide the CtrlUI window.");

                //Get the current focused application
                ProcessMulti foregroundProcess = GetFocusedProcess();

                if (vAppMinimized || !vAppActivated)
                {
                    PlayInterfaceSound("PopupOpen", false);

                    //Check previous focused application
                    try
                    {
                        //Check if application title or process is blacklisted
                        bool titleBlacklisted = vCtrlIgnoreProcessName.Any(x => x.String1.ToLower() == foregroundProcess.Title.ToLower());
                        bool processBlacklisted = vCtrlIgnoreProcessName.Any(x => x.String1.ToLower() == foregroundProcess.Name.ToLower());
                        if (!titleBlacklisted && !processBlacklisted)
                        {
                            //Save the previous focused application
                            vPrevFocusedProcess = foregroundProcess;
                        }
                    }
                    catch { }

                    //Disable top most window from foreground process
                    try
                    {
                        Debug.WriteLine("Disabling top most from process: " + foregroundProcess.Name);
                        SetWindowPos(foregroundProcess.WindowHandle, (IntPtr)WindowPosition.NoTopMost, 0, 0, 0, 0, (int)WindowSWP.NOMOVE | (int)WindowSWP.NOSIZE);
                    }
                    catch { }

                    //Force focus on CtrlUI
                    FocusProcessWindowPrepare("CtrlUI", vProcessCurrent.Id, vProcessCurrent.MainWindowHandle, 0, false, true, false, false);
                }
                else
                {
                    //Disable top most window from foreground process
                    try
                    {
                        Debug.WriteLine("Disabling top most from process: " + foregroundProcess.Name);
                        SetWindowPos(foregroundProcess.WindowHandle, (IntPtr)WindowPosition.NoTopMost, 0, 0, 0, 0, (int)WindowSWP.NOMOVE | (int)WindowSWP.NOSIZE);
                    }
                    catch { }

                    //Force focus on CtrlUI
                    FocusProcessWindowPrepare("CtrlUI", vProcessCurrent.Id, vProcessCurrent.MainWindowHandle, 0, false, true, false, false);

                    //Check if a previous process is available
                    if (vPrevFocusedProcess == null)
                    {
                        Popup_Show_Status("Close", "No app to show found");
                        Debug.WriteLine("Previous process not found.");
                        return;
                    }

                    //Check if application name is blacklisted
                    if (vCtrlIgnoreProcessName.Any(x => x.String1.ToLower() == vPrevFocusedProcess.Name.ToLower()))
                    {
                        Popup_Show_Status("Close", "App is blacklisted");
                        Debug.WriteLine("Previous process name is blacklisted: " + vPrevFocusedProcess.Name);
                        return;
                    }

                    //Check if application title is blacklisted
                    if (vCtrlIgnoreProcessName.Any(x => x.String1.ToLower() == vPrevFocusedProcess.Title.ToLower()))
                    {
                        Popup_Show_Status("Close", "App is blacklisted");
                        Debug.WriteLine("Previous process title is blacklisted: " + vPrevFocusedProcess.Title);
                        return;
                    }

                    //Check if application process is still running
                    if (!CheckRunningProcessByNameOrTitle(vPrevFocusedProcess.Name, false))
                    {
                        Popup_Show_Status("Close", "App no longer running");
                        Debug.WriteLine("Previous process is no longer running.");
                        return;
                    }

                    List<DataBindString> Answers = new List<DataBindString>();
                    DataBindString Answer1 = new DataBindString();
                    Answer1.ImageBitmap = FileToBitmapImage(new string[] { "pack://application:,,,/Assets/Icons/Switch.png" }, IntPtr.Zero, -1, 0);
                    Answer1.Name = "Return to application";
                    Answers.Add(Answer1);

                    DataBindString Answer2 = new DataBindString();
                    Answer2.ImageBitmap = FileToBitmapImage(new string[] { "pack://application:,,,/Assets/Icons/Closing.png" }, IntPtr.Zero, -1, 0);
                    Answer2.Name = "Close the application";
                    Answers.Add(Answer2);

                    DataBindString Answer3 = new DataBindString();
                    Answer3.ImageBitmap = FileToBitmapImage(new string[] { "pack://application:,,,/Assets/Icons/Minimize.png" }, IntPtr.Zero, -1, 0);
                    Answer3.Name = "Minimize CtrlUI";
                    Answers.Add(Answer3);

                    DataBindString messageResult = await Popup_Show_MessageBox("Return to previous application or minimize?", "", "You can always return to " + vPrevFocusedProcess.Title + " later on.", Answers);
                    if (messageResult != null)
                    {
                        if (messageResult == Answer1)
                        {
                            //Minimize the CtrlUI window
                            if (ConfigurationManager.AppSettings["MinimizeAppOnShow"] == "True")
                            {
                                await AppMinimize(true);
                            }

                            //Check keyboard controller launch
                            string fileNameNoExtension = Path.GetFileNameWithoutExtension(vPrevFocusedProcess.Name);
                            bool keyboardProcess = vCtrlKeyboardProcessName.Any(x => x.String1.ToLower() == fileNameNoExtension.ToLower());
                            bool keyboardLaunch = keyboardProcess && vControllerAnyConnected();

                            //Force focus on the app
                            FocusProcessWindowPrepare(vPrevFocusedProcess.Title, vPrevFocusedProcess.Identifier, vPrevFocusedProcess.WindowHandle, 0, false, false, false, keyboardLaunch);
                        }
                        else if (messageResult == Answer2)
                        {
                            Popup_Show_Status("Closing", "Closing " + vPrevFocusedProcess.Title);
                            Debug.WriteLine("Closing process: " + vPrevFocusedProcess.Title + " / " + vPrevFocusedProcess.Identifier + " / " + vPrevFocusedProcess.WindowHandle);

                            //Check if the application is UWP or Win32
                            if (CheckProcessIsUwp(vPrevFocusedProcess.WindowHandle))
                            {
                                bool ClosedProcess = await CloseProcessUwpByWindowHandleOrProcessId(vPrevFocusedProcess.Title, vPrevFocusedProcess.Identifier, vPrevFocusedProcess.WindowHandle);
                                if (ClosedProcess)
                                {
                                    Popup_Show_Status("Closing", "Closed " + vPrevFocusedProcess.Title);
                                    Debug.WriteLine("Closed process: " + vPrevFocusedProcess.Title + " / " + vPrevFocusedProcess.Identifier + " / " + vPrevFocusedProcess.WindowHandle);
                                    vPrevFocusedProcess = null;
                                }
                            }
                            else
                            {
                                bool ClosedProcess = CloseProcessById(vPrevFocusedProcess.Identifier);
                                if (ClosedProcess)
                                {
                                    Popup_Show_Status("Closing", "Closed " + vPrevFocusedProcess.Title);
                                    Debug.WriteLine("Closed process: " + vPrevFocusedProcess.Title + " / " + vPrevFocusedProcess.Identifier + " / " + vPrevFocusedProcess.WindowHandle);
                                    vPrevFocusedProcess = null;
                                }
                            }
                        }
                        else if (messageResult == Answer3)
                        {
                            //Minimize the CtrlUI window
                            await AppMinimize(false);
                        }
                    }
                }
            }
            catch
            {
                Popup_Show_Status("Close", "Failed to minimize or show app");
                Debug.WriteLine("Failed to minimize or show application.");
            }
        }

        //Minimize the application and save previous state
        async Task AppMinimize(bool Delay)
        {
            try
            {
                Debug.WriteLine("Minimizing the CtrlUI window.");

                //Save the CtrlUI window state
                vAppActivated = false;
                vAppMinimized = true;

                //Disable the CtrlUI window
                grid_WindowActive.Opacity = 0.80;
                grid_App.IsHitTestVisible = false;

                //Minimize the CtrlUI application
                WindowState = WindowState.Minimized;

                //Wait for application to minimize
                if (Delay)
                {
                    await Task.Delay(1000);
                }
            }
            catch { }
        }

        //Switch application between fullscreen and windowed
        async Task AppSwitchScreenMode(bool ForceMaximized, bool ForceNormal)
        {
            try
            {
                if (!ForceNormal && (ForceMaximized || WindowState != WindowState.Maximized))
                {
                    Debug.WriteLine("Maximizing CtrlUI window.");

                    WindowStyle = WindowStyle.None;
                    WindowState = WindowState.Maximized;

                    ////Hide the Windows taskbar
                    //IntPtr hWnd = FindWindow("Shell_TrayWnd", string.Empty);
                    //ShowWindow(hWnd, (int)WindowShowCmd.Hide);

                    vAppMaximized = true;
                }
                else
                {
                    Debug.WriteLine("Restoring CtrlUI window.");

                    WindowStyle = WindowStyle.SingleBorderWindow;
                    WindowState = WindowState.Normal;

                    ////Show the Windows taskbar
                    //IntPtr hWnd = FindWindow("Shell_TrayWnd", string.Empty);
                    //ShowWindow(hWnd, (int)WindowShowCmd.Normal);

                    vAppMaximized = false;
                }

                //Hide the mouse cursor
                await MouseCursorHide();
            }
            catch { }
        }

        //Adjust the application font size
        void AdjustApplicationFontSize()
        {
            try
            {
                int FontSize = Convert.ToInt32(ConfigurationManager.AppSettings["AppFontSize"]);
                Debug.WriteLine("Adjusting the font size to: " + FontSize);

                double TextSizeTiny = 10;
                double TextSizeInterface = 16;
                double TextSizeSmall = 18;
                double TextSizeMedium = 20;
                double TextSizeLarge = 24;
                double TextSizeHuge = 28;
                double TextSizePreTitle = 50;
                double TextSizeSubTitle = 60;
                double TextSizeTitle = 75;

                Application.Current.Resources["TextSizeTiny"] = TextSizeTiny + FontSize;
                Application.Current.Resources["TextSizeInterface"] = TextSizeInterface + FontSize;
                Application.Current.Resources["TextSizeSmall"] = TextSizeSmall + FontSize;
                Application.Current.Resources["TextSizeMedium"] = TextSizeMedium + FontSize;
                Application.Current.Resources["TextSizeLarge"] = TextSizeLarge + FontSize;
                Application.Current.Resources["TextSizeHuge"] = TextSizeHuge + FontSize;
                Application.Current.Resources["TextSizePreTitle"] = TextSizePreTitle + FontSize;
                Application.Current.Resources["TextSizeSubTitle"] = TextSizeSubTitle + FontSize;
                Application.Current.Resources["TextSizeTitle"] = TextSizeTitle + FontSize;
            }
            catch { }
        }

        //Set content and resource images with Cache OnLoad
        void SetContentResourceXamlImages()
        {
            try
            {
                img_Menu_SteamStatus.Source = FileToBitmapImage(new string[] { "Steam" }, IntPtr.Zero, 30, 0);
                img_Menu_UplayStatus.Source = FileToBitmapImage(new string[] { "Uplay" }, IntPtr.Zero, 30, 0);
                img_Menu_OriginStatus.Source = FileToBitmapImage(new string[] { "Origin" }, IntPtr.Zero, 30, 0);
                img_Menu_GoGStatus.Source = FileToBitmapImage(new string[] { "GoG" }, IntPtr.Zero, 30, 0);
                img_Menu_DirectXInput.Source = FileToBitmapImage(new string[] { "DirectXInput" }, IntPtr.Zero, 30, 0);
                img_Menu_BethesdaStatus.Source = FileToBitmapImage(new string[] { "Bethesda" }, IntPtr.Zero, 30, 0);
                img_Menu_EpicStatus.Source = FileToBitmapImage(new string[] { "Epic" }, IntPtr.Zero, 30, 0);
                img_Menu_BlizzardStatus.Source = FileToBitmapImage(new string[] { "Battle.net" }, IntPtr.Zero, 30, 0);

                //Check if the first launch logo's need to be loaded
                if (ConfigurationManager.AppSettings["AppFirstLaunch"] == "True")
                {
                    grid_Popup_Welcome_img_Edge.Source = FileToBitmapImage(new string[] { "Microsoft Edge" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Kodi.Source = FileToBitmapImage(new string[] { "Kodi" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Spotify.Source = FileToBitmapImage(new string[] { "Spotify" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Steam.Source = FileToBitmapImage(new string[] { "Steam" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Origin.Source = FileToBitmapImage(new string[] { "Origin" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Uplay.Source = FileToBitmapImage(new string[] { "Uplay" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_GoG.Source = FileToBitmapImage(new string[] { "GoG" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_Battle.Source = FileToBitmapImage(new string[] { "Battle.net" }, IntPtr.Zero, 75, 0);
                    grid_Popup_Welcome_img_PS4Remote.Source = FileToBitmapImage(new string[] { "Remote Play" }, IntPtr.Zero, 75, 0);
                }
            }
            catch { }
        }
    }
}