using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWDev.SLNTools.Core;

namespace SlnView.App.ViewModel.SlnItems
{
    public class CppProject : SlnItem, INotifyPropertyChanged
    {
        public static Guid TypeGuid = Guid.Parse("{8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942}");

        public CppProject(Project item) : base(item)
        {
        }
    }
}
