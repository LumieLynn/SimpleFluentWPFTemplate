namespace SimpleTemplate.Models
{
    public class MenuConfigItem
    {
        public string Type { get; set; } = "Item";

        public string? Title { get; set; }

        public string? Icon { get; set; }

        public string? TargetPage { get; set; }

        public bool IsExpanded { get; set; }

        public List<MenuConfigItem> Children { get; set; } = new();
    }
}
