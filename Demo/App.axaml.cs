using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Demo.ViewModels;
using Demo.Views;
using System.Threading;

namespace Demo
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DeferredContentFactory = () =>
                    {
                        // Simulate main thread initialization.
                        // Load styles, themes and assemblies...
                        Thread.Sleep(2_000);
                        return new MainViewModel();
                    }
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
