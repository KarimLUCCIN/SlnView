using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CWDev.SLNTools.Core;
using GalaSoft.MvvmLight;

namespace SlnView.App.ViewModel.SlnItems
{
    public class SlnRoot : ViewModelBase, INotifyPropertyChanged
    {
        private readonly SolutionFile solution;

        public string Title => Path.GetFileName(solution.SolutionFullPath);

        public SlnRoot(SolutionFile solution)
        {
            this.solution = solution ?? throw new ArgumentNullException(nameof(solution));
        }

        public IEnumerable<SlnItem> Items
        {
            get
            {
                return
                    from item in solution.Childs
                    select SlnItem.FromItem(item);
            }
        }

        public static IEnumerable<SlnRoot> FromSolution(SolutionFile solution)
        {
            if (solution == null)
            {
                yield break;
            }
            else
            {
                yield return new SlnRoot(solution);
            }
        }
    }
}