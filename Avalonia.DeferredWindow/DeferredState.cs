using Avalonia.Controls;

namespace Avalonia.DeferredWindow
{
    /// <summary>
    /// States for the <see cref="DeferredWindow"/> FSM.
    /// </summary>
    public enum DeferredState
    {
        /// <summary>
        /// Creating deferred window.
        /// </summary>
        Creating,

        /// <summary>
        /// Pending <see cref="Window.Show"/> invocation.
        /// </summary>
        PendingShow,

        /// <summary>
        /// Pending shell render.
        /// </summary>
        PendingShellRender,

        /// <summary>
        /// Rendering startup shell to platform render target.
        /// </summary>
        RenderingShell,

        /// <summary>
        /// Startup shell is loaded.
        /// </summary>
        ShellLoaded,

        /// <summary>
        /// Terminal state.
        /// </summary>
        Initialized
    }
}
