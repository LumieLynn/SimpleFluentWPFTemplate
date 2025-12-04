# SimpleFluentWPFTemplate
A simple Fluent style WPF Template with a NavigationView using 'iUWM'([iNKORE.UI.WPF.Modern](https://github.com/iNKORE-NET/UI.WPF.Modern/)).

## Feature
This template utilizes the MVVM architecture, inspired by Template Studio for WinUI, and incorporates IUWM for enhanced FluentUI effects in WPF. It reduces maintenance costs for adding or removing pages while maintaining design consistency. 

## Adding a New Page to NavigationView
To add a page to the NavigationView component:

1. Navigate to the NavigationRootViewModel.cs file in the project;
2. Create a new NavigationViewItem instance;
3. Set the Tag property of this NavigationViewItem to the ViewModel type of your target page (e.g., typeof(YourNewPageViewModel));
4. Add the instance to the navigation items collection (typically the NavigationItems observable collection in the view model).

This tagging mechanism maps the navigation item to the corresponding page, ensuring the correct page is loaded when the item is selected.

Due to the dependency injection (DI) automatic registration mechanism, when using this template, any newly added Views and ViewModels must follow specific naming conventions: "PageName" + "PageView"/"PageViewModel".

## Credits
- [iNKORE.UI.WPF.Modern](https://github.com/iNKORE-NET/UI.WPF.Modern/)
- [Microsoft Template Studio](https://github.com/microsoft/TemplateStudio)
