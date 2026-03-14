namespace SimpleTemplate.Models
{
    public enum MenuItemType
    {
        Item,
        Header,
        Separator
    }
    public class MenuConfigItem
    {
        public MenuItemType Type { get; set; } = MenuItemType.Item;

        public string? Title { get; set; }

        public string? Icon { get; set; }

        public string? TargetPage { get; set; }

        public bool IsExpanded { get; set; }
        public bool? IsSelectable { get; set; }

        public List<MenuConfigItem> Children { get; set; } = new();
    }
}
