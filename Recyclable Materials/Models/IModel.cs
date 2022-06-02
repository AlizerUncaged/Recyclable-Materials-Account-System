using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recyclable_Materials.Models
{
    public interface IModel<T>
    {
        IEnumerable<T> Search(IEnumerable<T> searchList, string query);
    }
}
