using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BDO_Spirit.Util
{
    public class UIHelper
    {

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static T FindVisualChildByName<T>(DependencyObject dependencyObject, string controlName) where T : DependencyObject
        {
            foreach(T t in FindVisualChildren<T>(dependencyObject))
            {
                var item = t as FrameworkElement;

                if(item.Name == controlName)
                {
                    return t;
                }
            }
            return null;
        }

        public static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindVisualParent<T>(parentObject);
            }
        }

        public static T FindVisualParentByName<T>(DependencyObject dependencyObject, string controlName) where T : DependencyObject
        {
            T result = UIHelper.FindVisualParent<T>(dependencyObject);

            var s = result as FrameworkElement;

            if (s.Name == controlName)
            {
                return result;
            }
            else
            {
                return FindVisualParentByName<T>(result, controlName);
            }
        }
    }
}
