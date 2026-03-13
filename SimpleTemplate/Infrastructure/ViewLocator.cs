using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SimpleTemplate.Infrastructure
{
    public class ViewLocator : DataTemplateSelector
    {
        private readonly Dictionary<Type, DataTemplate> _templateCache = new();

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return base.SelectTemplate(item, container);

            var vmType = item.GetType();

            // 1. 如果缓存中已有该 ViewModel 的模板，直接返回
            if (_templateCache.TryGetValue(vmType, out var cachedTemplate))
            {
                return cachedTemplate;
            }

            // 2. 懒加载：仅在首次遇到该 ViewModel 时，通过命名约定推断 View 类型
            var viewTypeName = vmType.FullName?.Replace("ViewModels", "Views").Replace("ViewModel", "View");
            var viewType = vmType.Assembly.GetType(viewTypeName);

            if (viewType != null)
            {
                // 3. 动态生成 DataTemplate
                string xaml = $@"
                <DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                              xmlns:v=""clr-namespace:{viewType.Namespace};assembly={viewType.Assembly.GetName().Name}"">
                    <v:{viewType.Name} />
                </DataTemplate>";

                var template = (DataTemplate)XamlReader.Parse(xaml);

                // 4. 加入缓存并返回
                _templateCache[vmType] = template;
                return template;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
