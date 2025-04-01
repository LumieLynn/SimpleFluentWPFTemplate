using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTemplate.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        void ConfigurePages(IEnumerable<object> items);
    }
}
