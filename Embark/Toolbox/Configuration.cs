using Embark.Models.ViewModels;
using System.Windows;

namespace Embark.Toolbox
{
    public static class Configuration
    {
        public static MainViewModel MainModelRef;
        public static FrameworkElement Framework = new();
        public static bool LoadComplete = false;

        
    }
}
