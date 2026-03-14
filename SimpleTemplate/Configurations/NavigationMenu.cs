using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Models;
using SimpleTemplate.ViewModels;

namespace SimpleTemplate.Configurations
{
    public class NavigationMenu : IMenuDefinition
    {
        public (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) Build()
        {
            var main = new LuminaMenuBuilder()
                .AddItem("Home", "Home", typeof(HomePageViewModel))

                .AddSeparator()

                .AddHeader("Sample Modules")

                .AddExpandableItem("Apps", "OEM", sub =>
                {
                    sub.AddItem("App List", "List", typeof(AppsPageViewModel));
                })
                .Build();

            var footer = new LuminaMenuBuilder()
                .AddItem("Settings", "Settings", typeof(SettingsPageViewModel))
                .AddItem("About", "Info", typeof(AboutPageViewModel))
                .Build();

            return (main, footer);
        }
    }
}