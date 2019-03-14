using CWDev.SLNTools.Core;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SlnView.App.ViewModel.SlnItems
{
    public class SlnItem : ViewModelBase, INotifyPropertyChanged
    {
        private readonly Project item;

        public string Title => item.ProjectName;
        public string Type => item.ProjectTypeGuid;

        public IEnumerable<SlnItem> Items
        {
            get
            {
                return
                    from item in item.Childs
                    select SlnItem.FromItem(item);
            }
        }

        public SlnItem(Project item)
        {
            this.item = item ?? throw new System.ArgumentNullException(nameof(item));
        }

        public static SlnItem FromItem(Project item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            if (Guid.Parse(item.ProjectTypeGuid) == SlnFolder.TypeGuid)
            {
                return new SlnFolder(item);
            }
            else if (Guid.Parse(item.ProjectTypeGuid) == CppProject.TypeGuid)
            {
                return new CppProject(item);
            }
            else
            {
                return new SlnItem(item);
            }
        }
    }
}