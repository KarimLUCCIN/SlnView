using CWDev.SLNTools.Core;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SlnView.App.ViewModel.SlnItems
{
    public class SlnItem : ViewModelBase, INotifyPropertyChanged, ISlnElement
    {
        public Project Item { get; private set; }

        public string FullPath => Item.FullPath;

        public string Title => Item.ProjectName;
        public string Type => Item.ProjectTypeGuid;

        public IEnumerable<SlnItem> Items
        {
            get
            {
                return
                    from item in Item.Childs
                    select SlnItem.FromItem(this, item);
            }
        }

        public ISlnElement Parent { get; private set; }

        public IEnumerable<ISlnElement> ParentHierarchy
        {
            get
            {
                if (Parent != null)
                {
                    yield return Parent;

                    foreach (var h in Parent.ParentHierarchy)
                    {
                        yield return h;
                    }
                }
            }
        }

        public SlnItem(ISlnElement parent, Project item)
        {
            this.Parent = parent;
            this.Item = item ?? throw new System.ArgumentNullException(nameof(item));
        }

        public static SlnItem FromItem(ISlnElement parent, Project item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }

            if (Guid.Parse(item.ProjectTypeGuid) == SlnFolder.TypeGuid)
            {
                return new SlnFolder(parent, item);
            }
            else if (Guid.Parse(item.ProjectTypeGuid) == CppProject.TypeGuid)
            {
                return new CppProject(parent, item);
            }
            else
            {
                return new SlnItem(parent, item);
            }
        }

        public void Refresh()
        {
            RaisePropertyChanged("");
        }
    }
}