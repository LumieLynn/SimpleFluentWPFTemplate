using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Contracts.ViewModels
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object? parameter);
        void OnNavigatedFrom();
    }
}
