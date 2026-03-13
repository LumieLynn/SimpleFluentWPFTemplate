namespace SimpleTemplate.Models
{
    public class MenuConfigItem
    {
        /// <summary>
        /// 类型："Item", "Separator", "Header"
        /// </summary>
        public string Type { get; set; } = "Item";

        public string Title { get; set; }

        /// <summary>
        /// 图标名称，需匹配 SegoeFluentIcons 枚举
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 目标 ViewModel 的完整类名 (对应 PageKey)
        /// </summary>
        public string TargetPage { get; set; }

        public bool IsExpanded { get; set; }

        /// <summary>
        /// 子菜单项
        /// </summary>
        public List<MenuConfigItem> Children { get; set; } = new();
    }
}
