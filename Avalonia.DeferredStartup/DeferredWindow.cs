using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Rendering;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia.DeferredStartup
{
    public class DeferredWindow : Window, IStyleable
    {
        public static readonly StyledProperty<IControl> ShellProperty =
            AvaloniaProperty.Register<DeferredWindow, IControl>(nameof(Shell));

        public static readonly StyledProperty<Func<object?>> DeferredContentFactoryProperty =
            AvaloniaProperty.Register<DeferredWindow, Func<object?>>(nameof(DeferredContentFactory));

        private readonly CancellationTokenSource cts = new CancellationTokenSource();


        private object? nonDeferredFallback;

        static DeferredWindow()
        {
            ShellProperty.Changed.AddClassHandler<DeferredWindow>((sender, e) =>
            {
                sender.OnShellChanged();
            });
        }

        /// <summary>
        /// Fired when the <see cref="Shell"/> is loaded and rendered.
        /// </summary>
        public static event EventHandler? ShellLoaded;

        /// <summary>
        /// Gets a value indicating the current state.
        /// </summary>
        public DeferredState State { get; private set; }

        /// <summary>
        /// Gets or sets the intermediate shell control.
        /// </summary>
        public IControl Shell
        {
            get { return GetValue(ShellProperty); }
            set { SetValue(ShellProperty, value); }
        }

        /// <summary>
        /// Gets or sets a factory for the deferred content.
        /// </summary>
        /// <remarks>
        /// Invoked after the <see cref="ShellLoaded"/> event.
        /// </remarks>
        public Func<object?> DeferredContentFactory
        {
            get { return GetValue(DeferredContentFactoryProperty); }
            set { SetValue(DeferredContentFactoryProperty, value); }
        }

        public override void Show()
        {
            if (!State.IsOneOf(DeferredState.PendingShow, DeferredState.PendingShellRender))
            {
                // Redundant framework invocation.
                return;
            }

            ;
            bool isPendingShow = State == DeferredState.PendingShow;
            if (isPendingShow)
            {
                State++;
            }

            base.Show();
            if (isPendingShow)
            {
                Dispatcher.UIThread.MainLoop(cts.Token);
                State.Guard(DeferredState.ShellLoaded);
                ShellLoaded?.Invoke(this, EventArgs.Empty);
                Content = DeferredContentFactory?.Invoke() ?? nonDeferredFallback;
                nonDeferredFallback = null;
                State++;
                cts.Dispose();
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (State == DeferredState.PendingShellRender)
            {
                State++;
                ForceRenderWindow(context);
                cts.Cancel();
                State = DeferredState.ShellLoaded;
            }
        }

        private void ForceRenderWindow(DrawingContext dc)
        {
            Content = Shell;
            //using var r = new ImmediateRenderer(this);
            //r.Start();
            //r.RecalculateChildren(this);
            using var renderTarget = CreateRenderTarget();
            ImmediateRenderer.Render(this, renderTarget);

            // Paint window and exit.
            Dispatcher.UIThread.RunJobs();
        }

        private void OnShellChanged()
        {
            if (State == DeferredState.Creating)
            {
                State = DeferredState.PendingShow;
                Content = Shell;
            }
        }
    }
}
