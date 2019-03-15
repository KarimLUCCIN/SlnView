using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWDev.SLNTools.Core;

namespace SlnView.App.ViewModel.SlnItems
{
    public class SlnFolder : SlnItem, INotifyPropertyChanged
    {
        public static Guid TypeGuid = Guid.Parse("{2150E333-8FDC-42A3-9474-1A3956D46DE8}");

        public SlnFolder(ISlnElement parent, Project item) : base(parent, item)
        {
        }
    }
}
