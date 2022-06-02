using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Recyclable_Materials.Utilities
{
    public static class ControlUtilities
    {
        /// <summary>
        /// Gets all the children and subchildren of a container control.
        /// </summary>
        /// <typeparam name="T">The type of children to find.</typeparam>
        /// <param name="depObj">The root control.</param>
        /// <returns>All children and subchildren of type T.</returns>
        public static IEnumerable<T> FindVisualChilds<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) yield return (T)Enumerable.Empty<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
                if (ithChild == null) continue;
                if (ithChild is T t) yield return t;
                foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
            }
        }
    }
}
