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

                //foreach (var proj in sln.Projects)
                //{
                //    if(proj.ProjectName == "json.Shared")
                //    {
                //        proj.ParentFolderGuid = sln.Projects.FindByFullName("props").ProjectGuid;
                //    }
                //}

                //sln.SaveAs(dlg.FileName + "2.sln");
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Highlight;
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public void Drop(IDropInfo dropInfo)
        {

        }
    }
}