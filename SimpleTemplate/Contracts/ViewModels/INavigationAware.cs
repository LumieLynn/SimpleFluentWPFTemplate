namespace SimpleTemplate.Contracts.ViewModels
{
    /// <summary>
    /// Receives navigation lifecycle callbacks from <see cref="Services.INavigationService"/>.
    /// Both methods fire only after a navigation succeeds, and they are symmetric:
    /// a forward or back navigation always notifies the page being left and the
    /// page being shown.
    /// </summary>
    public interface INavigationAware
    {
        /// <summary>
        /// Called after the page becomes the current one (forward or back navigation).
        /// </summary>
        /// <param name="parameter">The parameter passed to NavigateTo, or null for back navigation.</param>
        void OnNavigatedTo(object? parameter);

        /// <summary>
        /// Called after the page stops being the current one.
        /// </summary>
        void OnNavigatedFrom();
    }
}
