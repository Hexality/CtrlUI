﻿using ArnoldVinkCode;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using static ArnoldVinkCode.AVInputOutputClass;
using static ArnoldVinkCode.AVInputOutputKeyboard;
using static ArnoldVinkCode.AVInterface;
using static CtrlUI.AppVariables;

namespace CtrlUI
{
    partial class WindowMain
    {
        //Bind the lists to the listbox elements
        void ListBoxBindLists()
        {
            try
            {
                lb_Games.IsTextSearchEnabled = true;
                lb_Games.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Games, "Name");
                lb_Games.ItemsSource = List_Games;

                lb_Apps.IsTextSearchEnabled = true;
                lb_Apps.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Apps, "Name");
                lb_Apps.ItemsSource = List_Apps;

                lb_Emulators.IsTextSearchEnabled = true;
                lb_Emulators.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Emulators, "Name");
                lb_Emulators.ItemsSource = List_Emulators;

                lb_Shortcuts.IsTextSearchEnabled = true;
                lb_Shortcuts.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Shortcuts, "Name");
                lb_Shortcuts.ItemsSource = List_Shortcuts;

                lb_Processes.IsTextSearchEnabled = true;
                lb_Processes.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Processes, "Name");
                lb_Processes.ItemsSource = List_Processes;

                lb_ColorPicker.ItemsSource = List_ColorPicker;

                lb_Search.IsTextSearchEnabled = true;
                lb_Search.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_Search, "Name");
                lb_Search.ItemsSource = List_Search;

                lb_FilePicker.IsTextSearchEnabled = true;
                lb_FilePicker.IsTextSearchCaseSensitive = false;
                TextSearch.SetTextPath(lb_FilePicker, "Name");
                lb_FilePicker.ItemsSource = List_FilePicker;

                lb_ProfileManager.IsTextSearchEnabled = true;
                lb_ProfileManager.IsTextSearchCaseSensitive = false;
            }
            catch { }
        }

        //Select the first listbox item
        void ListBoxResetIndexes()
        {
            try
            {
                lb_Games.SelectedIndex = 0;
                lb_Apps.SelectedIndex = 0;
                lb_Emulators.SelectedIndex = 0;
                lb_Shortcuts.SelectedIndex = 0;
                lb_Processes.SelectedIndex = 0;
                lb_FilePicker.SelectedIndex = 0;
                lb_ColorPicker.SelectedIndex = 0;
                lb_MessageBox.SelectedIndex = 0;
                lb_Search.SelectedIndex = 0;
            }
            catch { }
        }

        //Listbox focus or select an index
        async Task ListBoxFocusOrSelectIndex(ListBox focusListBox, bool firstIndex, bool lastIndex, int indexNumber)
        {
            try
            {
                await AVActions.ActionDispatcherInvokeAsync(async delegate
                {
                    //Get the currently focused element
                    FrameworkElement frameworkElement = (FrameworkElement)Keyboard.FocusedElement;

                    //Check if focused element is disconnected
                    bool disconnectedSource = frameworkElement.DataContext == BindingOperations.DisconnectedSource;

                    //Focus on the listbox or select index
                    if (disconnectedSource || frameworkElement == null || frameworkElement == focusListBox)
                    {
                        await ListboxFocusIndex(focusListBox, firstIndex, lastIndex, indexNumber);
                    }
                    else
                    {
                        ListBoxSelectIndex(focusListBox, firstIndex, lastIndex, indexNumber);
                    }
                });
            }
            catch { }
        }

        //Force focus on a listbox
        async Task ListboxFocusIndex(ListBox focusListBox, bool firstIndex, bool lastIndex, int indexNumber)
        {
            try
            {
                await AVActions.ActionDispatcherInvokeAsync(async delegate
                {
                    //Select first available listbox
                    if (focusListBox != null && focusListBox.IsEnabled && focusListBox.Visibility == Visibility.Visible && focusListBox.Items.Count > 0)
                    {
                        //Update the listbox layout
                        focusListBox.UpdateLayout();

                        //Select a listbox item index
                        ListBoxSelectIndex(focusListBox, firstIndex, lastIndex, indexNumber);

                        //Focus on the listbox and item
                        int selectedIndex = focusListBox.SelectedIndex;

                        //Scroll to the listbox item
                        object scrollListBoxItem = focusListBox.Items[selectedIndex];
                        focusListBox.ScrollIntoView(scrollListBoxItem);

                        //Force focus on element
                        ListBoxItem focusListBoxItem = (ListBoxItem)focusListBox.ItemContainerGenerator.ContainerFromInd‌​ex(selectedIndex);
                        await FocusOnElement(focusListBoxItem, false, vProcessCurrent.MainWindowHandle);

                        Debug.WriteLine("Focusing on listbox index: " + selectedIndex);
                    }
                    else
                    {
                        Debug.WriteLine("Listbox cannot be focused on, pressing tab key.");
                        await KeySendSingle((byte)KeysVirtual.Tab, vProcessCurrent.MainWindowHandle);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed focusing on the listbox, pressing tab key: " + ex.Message);
                await KeySendSingle((byte)KeysVirtual.Tab, vProcessCurrent.MainWindowHandle);
            }
        }

        //Select a listbox index
        void ListBoxSelectIndex(ListBox focusListBox, bool firstIndex, bool lastIndex, int indexNumber)
        {
            try
            {
                AVActions.ActionDispatcherInvoke(delegate
                {
                    //Update the listbox layout
                    focusListBox.UpdateLayout();

                    //Select the requested index
                    if (firstIndex)
                    {
                        focusListBox.SelectedIndex = 0;
                        Debug.WriteLine("Selecting first listbox index.");
                    }
                    else if (lastIndex)
                    {
                        focusListBox.SelectedIndex = focusListBox.Items.Count - 1;
                        Debug.WriteLine("Selecting last listbox index.");
                    }
                    else if (indexNumber != -1)
                    {
                        if (indexNumber >= focusListBox.Items.Count)
                        {
                            focusListBox.SelectedIndex = focusListBox.Items.Count - 1;
                            Debug.WriteLine("Selecting last listbox index.");
                        }
                        else
                        {
                            focusListBox.SelectedIndex = indexNumber;
                            Debug.WriteLine("Selecting listbox index: " + indexNumber);
                        }
                    }

                    //Check the selected index
                    if (focusListBox.SelectedIndex == -1)
                    {
                        focusListBox.SelectedIndex = 0;
                        Debug.WriteLine("No selection, selecting first listbox index.");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed selecting the listbox index: " + ex.Message);
            }
        }

        //Select a listbox item
        void ListBoxSelectItem(ListBox focusListBox, object selectItem)
        {
            try
            {
                AVActions.ActionDispatcherInvoke(delegate
                {
                    //Update the listbox layout
                    focusListBox.UpdateLayout();

                    //Select the listbox item
                    if (selectItem != null)
                    {
                        try
                        {
                            focusListBox.SelectedItem = selectItem;
                        }
                        catch { }
                    }

                    //Check the selected index
                    if (focusListBox.SelectedIndex == -1)
                    {
                        focusListBox.SelectedIndex = 0;
                        Debug.WriteLine("No selection, selecting first listbox index.");
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed selecting the listbox item: " + ex.Message);
            }
        }

        //Add listbox item to a list
        async Task ListBoxAddItem<T>(ListBox listBox, Collection<T> listCollection, T addItem, bool insertItem, bool selectItem)
        {
            try
            {
                await AVActions.ActionDispatcherInvokeAsync(async delegate
                {
                    //Debug.WriteLine("Adding item to list collection: " + listCollection);

                    //Add or insert the item to the list
                    if (insertItem)
                    {
                        //Debug.WriteLine(listCollection + " listbox item has been inserted.");
                        listCollection.Insert(0, addItem);
                    }
                    else
                    {
                        //Debug.WriteLine(listCollection + " listbox item has been added.");
                        listCollection.Add(addItem);
                    }

                    //Select the item in the listbox
                    if (listBox != null && selectItem)
                    {
                        if (insertItem)
                        {
                            await ListBoxFocusOrSelectIndex(listBox, true, false, -1);
                        }
                        else
                        {
                            await ListBoxFocusOrSelectIndex(listBox, false, true, -1);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed adding listbox item: " + ex.Message);
            }
        }

        //Remove listbox item from a listbox
        async Task ListBoxRemoveItem<T>(ListBox listBox, Collection<T> listCollection, T removeItem, bool selectItem)
        {
            try
            {
                await AVActions.ActionDispatcherInvokeAsync(async delegate
                {
                    //Store the current listbox items count
                    int listBoxItemCount = listBox.Items.Count;

                    //Store the currently selected index
                    int listBoxSelectedIndex = listBox.SelectedIndex;

                    //Remove the listbox item from list
                    listCollection.Remove(removeItem);

                    //Check if there is a listbox item removed
                    if (listBoxItemCount != listBox.Items.Count)
                    {
                        Debug.WriteLine(listBox.Name + " listbox item has been removed.");
                        if (selectItem)
                        {
                            await ListBoxFocusOrSelectIndex(listBox, false, false, listBoxSelectedIndex);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed removing item from the listbox: " + ex.Message);
            }
        }

        //Remove all matching items from a listbox
        async Task ListBoxRemoveAll<T>(ListBox listBox, Collection<T> listCollection, Func<T, bool> removeCondition)
        {
            try
            {
                await AVActions.ActionDispatcherInvokeAsync(async delegate
                {
                    //Store the current listbox items count
                    int listBoxItemCount = listBox.Items.Count;

                    //Store the currently selected index
                    int listBoxSelectedIndex = listBox.SelectedIndex;

                    //Remove the listbox items from list
                    listCollection.ListRemoveAll(removeCondition);

                    //Check if there is a listbox item removed
                    if (listBoxItemCount != listBox.Items.Count)
                    {
                        Debug.WriteLine(listBox.Name + " " + (listBoxItemCount - listBox.Items.Count) + " items have been removed.");
                        await ListBoxFocusOrSelectIndex(listBox, false, false, listBoxSelectedIndex);
                    }
                });
            }
            catch
            {
                Debug.WriteLine("Failed removing all from the listbox.");
            }
        }
    }
}