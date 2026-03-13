using SimpleTemplate.Models;

namespace SimpleTemplate.Infrastructure
{
    public class LuminaMenuBuilder
    {
        private readonly List<MenuConfigItem> _items = new();

        public LuminaMenuBuilder AddItem(string title, string icon, Type viewModelType)
        {
            _items.Add(new MenuConfigItem
            {
                Type = "Item",
                Title = title,
                Icon = icon,
                TargetPage = viewModelType.FullName
            });
            return this;
        }

        public LuminaMenuBuilder AddHeader(string title)
        {
            _items.Add(new MenuConfigItem { Type = "Header", Title = title });
            return this;
        }

        public LuminaMenuBuilder AddSeparator()
        {
            _items.Add(new MenuConfigItem { Type = "Separator" });
            return this;
        }

        public LuminaMenuBuilder AddExpandableItem(string title, string icon, Action<LuminaMenuBuilder> childrenBuilder)
        {
            var subBuilder = new LuminaMenuBuilder();
            childrenBuilder(subBuilder);

            _items.Add(new MenuConfigItem
            {
                Type = "Item",
                Title = title,
                Icon = icon,
                IsExpanded = true,
                Children = subBuilder.Build()
            });
            return this;
        }

        public List<MenuConfigItem> Build() => _items;
    }
}
