using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlnView.App.ViewModel.SlnItems
{
    public interface ISlnElement
    {
        string Title { get; }
        IEnumerable<SlnItem> Items { get; }

        IEnumerable<ISlnElement> ParentHierarchy { get; }

        string FullPath { get; }

        void Refresh();
    }
}
