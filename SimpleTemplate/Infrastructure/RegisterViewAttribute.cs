using Microsoft.Extensions.DependencyInjection;

namespace SimpleTemplate.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RegisterViewAttribute : Attribute
    {
        public Type ViewType { get; }
        public ServiceLifetime ViewLifetime { get; }
        public ServiceLifetime ViewModelLifetime { get; }

        public RegisterViewAttribute(
            Type viewType,
            ServiceLifetime viewLifetime = ServiceLifetime.Transient,
            ServiceLifetime viewModelLifetime = ServiceLifetime.Transient)
        {
            ViewType = viewType ?? throw new ArgumentNullException(nameof(viewType));
            ViewLifetime = viewLifetime;
            ViewModelLifetime = viewModelLifetime;
        }
    }
}
