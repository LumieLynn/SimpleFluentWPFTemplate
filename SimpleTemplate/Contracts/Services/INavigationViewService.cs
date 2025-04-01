using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationViewService
    {
        void Initialize(NavigationView navigationView,IEnumerable<object>? menuItems,IEnumerable<object>? footerItems);

        NavigationViewItem? GetCurrentSelectedItem();
    }
}
