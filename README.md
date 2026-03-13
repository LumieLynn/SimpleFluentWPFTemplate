# LumiereWPF
LumiereWPF is a modern, lightweight WPF template built on the Fluent Design System.

## 🌟 Key Features
- **Automated DI Registration**: Simply decorate your ViewModels with the `[RegisterView]` attribute. The template automatically scans and registers View/ViewModel pairs with the DI container, managing their lifecycles seamlessly.
- **Strict MVVM Architecture**: Built with a clean separation of concerns, utilizing dedicated services for Navigation, Page mapping, and Menu configuration.
- **Modern Fluent UX**: Native integration with [iNKORE.UI.WPF.Modern](https://github.com/iNKORE-NET/UI.WPF.Modern/) (iUWM), offering smooth NavigationView transitions and Windows 11 visual effects like Acrylic and Mica.

## 🚀 Getting Started
Adding a new page to LumiereWPF is a streamlined two-step process:
1. **Define and Register**
Apply the `[RegisterView]` attribute to your ViewModel to specify its paired View and DI lifetime (e.g., Transient or Singleton).
```csharp
[RegisterView(typeof(MyPageView), ServiceLifetime.Transient)]
public partial class MyPageViewModel : ObservableObject 
{ 
    // Your business logic here
}
```
2. **Configure the Menu**
Define your navigation hierarchy in Configurations/NavigationMenu.cs.
```csharp
public (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) Build()
{
    return new LuminaMenuBuilder()
        .AddItem("Home", "Home", typeof(HomePageViewModel))
        .AddSeparator()
        .AddExpandableItem("Features", "Setting", sub => {
            sub.AddItem("New Page", "Document", typeof(MyPageViewModel));
        })
        .Build(); // Generates the menu model for the UI
}
```

## 📂 Project Structure
- `Infrastructure/`: Contains the automated DI logic and custom attributes.
- `Configurations/`: The centralized hub for app-wide settings, including the `NavigationMenu.cs` definition.
- `Services/`: Implementation of core services such as Navigation and Page management.
- `Models/`: Core data models used for UI rendering and configuration.

## 🤝 Credits
- [iNKORE.UI.WPF.Modern](https://github.com/iNKORE-NET/UI.WPF.Modern/)
- [Microsoft Template Studio](https://github.com/microsoft/TemplateStudio)
