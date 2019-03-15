using CWDev.SLNTools.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GongSolutions.Wpf.DragDrop;
using Microsoft.Win32;
using SlnView.App.ViewModel.SlnItems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace SlnView.App.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged, IDropTarget
    {
        public SolutionFile CurrentSolution { get; private set; }
        public IEnumerable<SlnRoot> CurrentSolutionRoots => SlnRoot.FromSolution(CurrentSolution);

        public RelayCommand CommandOpen { get; private set; }
        public RelayCommand CommandSave { get; private set; }
        public RelayCommand CommandSaveAs { get; private set; }
        public RelayCommand CommandQuit { get; private set; }
        public RelayCommand<ISlnElement> CommandRevealInExplorer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ///

            CommandOpen = new RelayCommand(OpenExecute);
            CommandSave = new RelayCommand(SaveExecute);
            CommandSaveAs = new RelayCommand(SaveAsExecute);
            CommandQuit = new RelayCommand(() => Application.Current.Shutdown());

            CommandRevealInExplorer = new RelayCommand<ISlnElement>(RevealInExplorerExecute);
        }

        private void RevealInExplorerExecute(ISlnElement obj)
        {
            if (obj != null)
            {
                string cmd = "explorer.exe";
                string arg = $"/select, \"{obj.FullPath}\"";
                Process.Start(cmd, arg);
            }
        }

        private bool CheckHasSolution()
        {
            if (CurrentSolution == null)
            {
                MessageBox.Show("Nothing is open");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SaveAsExecute()
        {
            if (CheckHasSolution())
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "Visual Studio Solution (sln)|*.sln";
                if (dlg.ShowDialog() == true)
                {
                    try
                    {
                        CurrentSolution.SaveAs(dlg.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void SaveExecute()
        {
            if (CheckHasSolution())
            {
                CurrentSolution.Save();
            }
        }

        private void OpenExecute()
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "Visual Studio Solution (sln)|*.sln";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    CurrentSolution = SolutionFile.FromFile(dlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    CurrentSolution = null;
                }
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as SlnItem;
            var target = dropInfo.TargetItem as ISlnElement;

            if (sourceItem == null)
            {
                dropInfo.Effects = DragDropEffects.None;
            }
            else
            {
                if (target.ParentHierarchy.Contains(sourceItem))
                {
                    dropInfo.Effects = DragDropEffects.None;
                }
                else
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
                    dropInfo.Effects = DragDropEffects.Move;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            var sourceItem = dropInfo.Data as SlnItem;

            if (dropInfo.TargetItem is SlnRoot)
            {
                sourceItem.Item.ParentFolderGuid = null;
            }
            else
            {
                sourceItem.Item.ParentFolderGuid = ((SlnItem)dropInfo.TargetItem).Item.ProjectGuid;
            }

            sourceItem.Parent?.Refresh();
            ((ISlnElement)dropInfo.TargetItem).Refresh();
        }
    }
}