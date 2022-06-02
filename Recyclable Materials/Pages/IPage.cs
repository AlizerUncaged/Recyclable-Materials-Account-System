using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recyclable_Materials.Pages
{
    public interface IPage
    {
        /// <summary>
        /// Fired when the current page requests page change.
        /// </summary>
        event EventHandler<IPage> ChangePage;
    }
}
