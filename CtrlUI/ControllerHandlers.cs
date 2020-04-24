﻿using ArnoldVinkCode;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputKeyboard;
using static CtrlUI.AppVariables;
using static LibraryShared.Classes;
using static LibraryShared.SoundPlayer;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Process controller input
        public async Task ControllerInteraction(ControllerInput ControllerInput)
        {
            try
            {
                if (await Controller_GlobalButtonPress(ControllerInput))
                {
                    return;
                }

                if (!vAppMinimized && vAppActivated)
                {
                    Controller_DPadPress(ControllerInput);
                    Controller_StickMovement(ControllerInput);
                    await Controller_ButtonPress(ControllerInput);
                    await Controller_TriggerPress(ControllerInput);
                }
            }
            catch { }
        }

        //Process global controller buttons
        async Task<bool> Controller_GlobalButtonPress(ControllerInput ControllerInput)
        {
            bool ControllerUsed = false;
            bool ControllerDelayShort = false;
            bool ControllerDelayLong = false;
            try
            {
                if (Environment.TickCount >= vControllerDelay_Global)
                {
                    if (ControllerInput.ButtonGuideShort)
                    {
                        Debug.WriteLine("Button Global - Guide Short - Switch apps");
                        await AVActions.ActionDispatcherInvokeAsync(async delegate { await AppWindow_HideShow(); });

                        ControllerUsed = true;
                        ControllerDelayLong = true;
                    }

                    if (ControllerDelayShort)
                    {
                        vControllerDelay_DPad = Environment.TickCount + vControllerDelayShortTicks;
                        vControllerDelay_Stick = Environment.TickCount + vControllerDelayShortTicks;
                        vControllerDelay_Trigger = Environment.TickCount + vControllerDelayShortTicks;
                        vControllerDelay_Button = Environment.TickCount + vControllerDelayShortTicks;
                        vControllerDelay_Activate = Environment.TickCount + vControllerDelayShortTicks;
                        vControllerDelay_Global = Environment.TickCount + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayLong)
                    {
                        vControllerDelay_DPad = Environment.TickCount + vControllerDelayLongTicks;
                        vControllerDelay_Stick = Environment.TickCount + vControllerDelayLongTicks;
                        vControllerDelay_Trigger = Environment.TickCount + vControllerDelayLongTicks;
                        vControllerDelay_Button = Environment.TickCount + vControllerDelayLongTicks;
                        vControllerDelay_Activate = Environment.TickCount + vControllerDelayLongTicks;
                        vControllerDelay_Global = Environment.TickCount + vControllerDelayLongTicks;
                    }
                }
            }
            catch { }
            return ControllerUsed;
        }

        //Process active XInput controller buttons
        async Task<bool> Controller_ButtonPress(ControllerInput ControllerInput)
        {
            bool ControllerUsed = false;
            bool ControllerDelayShort = false;
            bool ControllerDelayMedium = false;
            bool ControllerDelayLong = false;
            try
            {
                if (Environment.TickCount >= vControllerDelay_Button)
                {
                    if (ControllerInput.ButtonA)
                    {
                        Debug.WriteLine("Button: APressed");

                        AVActions.ActionDispatcherInvoke(delegate
                        {
                            FrameworkElement frameworkElement = (FrameworkElement)Keyboard.FocusedElement;
                            if (frameworkElement != null && frameworkElement.GetType() == typeof(TextBox))
                            {
                                //Launch the keyboard controller
                                if (vAppActivated && vControllerAnyConnected())
                                {
                                    LaunchKeyboardController(false);
                                }
                            }
                            else
                            {
                                //Press on the space bar
                                KeySendSingle((byte)KeysVirtual.Space, vProcessCurrent.MainWindowHandle);
                            }
                        });

                        ControllerUsed = true;
                        ControllerDelayMedium = true;
                    }
                    else if (ControllerInput.ButtonB)
                    {
                        Debug.WriteLine("Button: BPressed");

                        if (Popup_Any_Open())
                        {
                            KeySendSingle((byte)KeysVirtual.Escape, vProcessCurrent.MainWindowHandle);
                        }

                        ControllerUsed = true;
                        ControllerDelayMedium = true;
                    }
                    else if (ControllerInput.ButtonY)
                    {
                        Debug.WriteLine("Button: YPressed");

                        if (vFilePickerOpen)
                        {
                            KeySendSingle((byte)KeysVirtual.Back, vProcessCurrent.MainWindowHandle);
                        }
                        else
                        {
                            await AVActions.ActionDispatcherInvokeAsync(async delegate { await QuickActionPrompt(); });
                        }

                        ControllerUsed = true;
                        ControllerDelayMedium = true;
                    }
                    else if (ControllerInput.ButtonX)
                    {
                        Debug.WriteLine("Button: XPressed");

                        if (vTextInputOpen)
                        {
                            Debug.WriteLine("Resetting the text input popup.");
                            await AVActions.ActionDispatcherInvokeAsync(async delegate { await Popup_Reset_TextInput(true, string.Empty); });
                        }
                        else if (vSearchOpen)
                        {
                            Debug.WriteLine("Resetting the search popup.");
                            await AVActions.ActionDispatcherInvokeAsync(async delegate { await Popup_Reset_Search(true); });
                        }
                        else
                        {
                            KeySendSingle((byte)KeysVirtual.Delete, vProcessCurrent.MainWindowHandle);
                        }

                        ControllerUsed = true;
                        ControllerDelayMedium = true;
                    }
                    else if (ControllerInput.ButtonShoulderLeft)
                    {
                        Debug.WriteLine("Button: ShoulderLeftPressed");
                        PlayInterfaceSound("ClickRight", false);

                        await AVActions.ActionDispatcherInvokeAsync(async delegate
                         {
                             if (grid_Popup_Settings.Visibility == Visibility.Visible)
                             {
                                 int selectedIndex = Listbox_SettingsMenu.SelectedIndex;
                                 if (selectedIndex > 0)
                                 {
                                     Listbox_SettingsMenu.SelectedIndex = Listbox_SettingsMenu.SelectedIndex - 1;
                                     await Listbox_Settings_SingleTap();
                                 }
                             }
                             else
                             {
                                 //Improve: KeySendCombo((byte)KeysVirtual.Shift, (byte)KeysVirtual.Tab, vProcessCurrent.MainWindowHandle);
                                 KeyPressCombo((byte)KeysVirtual.Shift, (byte)KeysVirtual.Tab, false);
                             }
                         });

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ButtonShoulderRight)
                    {
                        Debug.WriteLine("Button: ShoulderRightPressed");
                        PlayInterfaceSound("ClickRight", false);

                        await AVActions.ActionDispatcherInvokeAsync(async delegate
                        {
                            if (grid_Popup_Settings.Visibility == Visibility.Visible)
                            {
                                Listbox_SettingsMenu.SelectedIndex = Listbox_SettingsMenu.SelectedIndex + 1;
                                await Listbox_Settings_SingleTap();
                            }
                            else
                            {
                                KeySendSingle((byte)KeysVirtual.Tab, vProcessCurrent.MainWindowHandle);
                            }
                        });

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ButtonBack)
                    {
                        Debug.WriteLine("Button: BackPressed / Show hide menu");
                        await AVActions.ActionDispatcherInvokeAsync(async delegate { await Popup_ShowHide_MainMenu(false); });

                        ControllerUsed = true;
                        ControllerDelayLong = true;
                    }
                    else if (ControllerInput.ButtonStart)
                    {
                        Debug.WriteLine("Button: StartPressed / Show hide search");
                        await AVActions.ActionDispatcherInvokeAsync(async delegate { await Popup_ShowHide_Search(false); });

                        ControllerUsed = true;
                        ControllerDelayLong = true;
                    }
                    else if (ControllerInput.ButtonThumbLeft)
                    {
                        Debug.WriteLine("Button: ThumbLeftPressed");
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.Home, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ButtonThumbRight)
                    {
                        Debug.WriteLine("Button: ThumbRightPressed");
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.End, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }

                    if (ControllerDelayShort)
                    {
                        vControllerDelay_Button = Environment.TickCount + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayMedium)
                    {
                        vControllerDelay_Button = Environment.TickCount + vControllerDelayMediumTicks;
                    }
                    else if (ControllerDelayLong)
                    {
                        vControllerDelay_Button = Environment.TickCount + vControllerDelayLongTicks;
                    }
                }
            }
            catch { }
            return ControllerUsed;
        }

        //Process XInput controller D-Pad
        bool Controller_DPadPress(ControllerInput ControllerInput)
        {
            bool ControllerUsed = false;
            bool ControllerDelayShort = false;
            bool ControllerDelayLong = false;
            try
            {
                if (Environment.TickCount >= vControllerDelay_DPad)
                {
                    if (ControllerInput.DPadLeft)
                    {
                        Debug.WriteLine("Button: DPadLeftPressed");

                        KeySendSingle((byte)KeysVirtual.Left, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.DPadUp)
                    {
                        Debug.WriteLine("Button: DPadUpPressed");

                        KeySendSingle((byte)KeysVirtual.Up, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.DPadRight)
                    {
                        Debug.WriteLine("Button: DPadRightPressed");

                        KeySendSingle((byte)KeysVirtual.Right, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.DPadDown)
                    {
                        Debug.WriteLine("Button: DPadDownPressed");

                        KeySendSingle((byte)KeysVirtual.Down, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }

                    if (ControllerDelayShort)
                    {
                        vControllerDelay_DPad = Environment.TickCount + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayLong)
                    {
                        vControllerDelay_DPad = Environment.TickCount + vControllerDelayLongTicks;
                    }
                }
            }
            catch { }
            return ControllerUsed;
        }

        //Process XInput controller triggers
        async Task<bool> Controller_TriggerPress(ControllerInput ControllerInput)
        {
            bool ControllerUsed = false;
            bool ControllerDelayShort = false;
            bool ControllerDelayLong = false;
            try
            {
                if (Environment.TickCount >= vControllerDelay_Trigger)
                {
                    //Check if the media popup is opened
                    bool MediaPopupOpen = false;
                    await AVActions.ActionDispatcherInvokeAsync(delegate { MediaPopupOpen = grid_Popup_Media.Visibility == Visibility.Visible; });

                    //Control the volume with controller triggers
                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["ShortcutVolume"]) || MediaPopupOpen)
                    {
                        if (ControllerInput.TriggerLeft > 0 && ControllerInput.TriggerRight > 0)
                        {
                            KeyPressSingle((byte)KeysVirtual.VolumeMute, false);

                            ControllerUsed = true;
                            ControllerDelayLong = true;
                        }
                        else if (ControllerInput.TriggerLeft > 0)
                        {
                            KeyPressSingle((byte)KeysVirtual.VolumeDown, false);

                            ControllerUsed = true;
                            ControllerDelayShort = true;
                        }
                        else if (ControllerInput.TriggerRight > 0)
                        {
                            KeyPressSingle((byte)KeysVirtual.VolumeUp, false);

                            ControllerUsed = true;
                            ControllerDelayShort = true;
                        }
                    }

                    if (ControllerDelayShort)
                    {
                        vControllerDelay_Trigger = Environment.TickCount + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayLong)
                    {
                        vControllerDelay_Trigger = Environment.TickCount + vControllerDelayLongTicks;
                    }
                }
            }
            catch { }
            return ControllerUsed;
        }

        //Process XInput controller sticks
        bool Controller_StickMovement(ControllerInput ControllerInput)
        {
            bool ControllerUsed = false;
            bool ControllerDelayShort = false;
            bool ControllerDelayLong = false;
            try
            {
                if (Environment.TickCount >= vControllerDelay_Stick)
                {
                    //Left stick movement
                    if (ControllerInput.ThumbLeftX < -10000 && Math.Abs(ControllerInput.ThumbLeftY) < 13000)
                    {
                        KeySendSingle((byte)KeysVirtual.Left, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbLeftY > 10000 && Math.Abs(ControllerInput.ThumbLeftX) < 13000)
                    {
                        KeySendSingle((byte)KeysVirtual.Up, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbLeftX > 10000 && Math.Abs(ControllerInput.ThumbLeftY) < 13000)
                    {
                        KeySendSingle((byte)KeysVirtual.Right, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbLeftY < -10000 && Math.Abs(ControllerInput.ThumbLeftX) < 13000)
                    {
                        KeySendSingle((byte)KeysVirtual.Down, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }

                    //Right stick movement
                    if (ControllerInput.ThumbRightX < -10000 && Math.Abs(ControllerInput.ThumbRightY) < 13000)
                    {
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.Prior, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbRightY > 10000 && Math.Abs(ControllerInput.ThumbRightX) < 13000)
                    {
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.Prior, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbRightX > 10000 && Math.Abs(ControllerInput.ThumbRightY) < 13000)
                    {
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.Next, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }
                    else if (ControllerInput.ThumbRightY < -10000 && Math.Abs(ControllerInput.ThumbRightX) < 13000)
                    {
                        PlayInterfaceSound("ClickRight", false);

                        KeySendSingle((byte)KeysVirtual.Next, vProcessCurrent.MainWindowHandle);

                        ControllerUsed = true;
                        ControllerDelayShort = true;
                    }

                    if (ControllerDelayShort)
                    {
                        vControllerDelay_Stick = Environment.TickCount + vControllerDelayShortTicks;
                    }
                    else if (ControllerDelayLong)
                    {
                        vControllerDelay_Stick = Environment.TickCount + vControllerDelayLongTicks;
                    }
                }
            }
            catch { }
            return ControllerUsed;
        }
    }
}