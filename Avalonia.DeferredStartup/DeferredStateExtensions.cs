namespace Avalonia.DeferredStartup
{
    internal static class DeferredStateExtensions
    {
        public static void Guard(this DeferredState sender, DeferredState state, string message)
        {
            if (sender != state)
            {
                throw new InvalidOperationException(message);
            }
        }

        public static void Guard(this DeferredState sender, DeferredState state)
        {
            Guard(sender, state, $"Expected state: {state}; acutal state: {sender}.");
        }

        public static bool IsOneOf(this DeferredState sender, params DeferredState[] states)
        {
            bool isOneOf = false;
            foreach (var state in states)
            {
                isOneOf |= sender == state;
            }

            return isOneOf;
        }

        public static void GuardOneOf(this DeferredState sender, params DeferredState[] states)
        {
            if (!IsOneOf(sender, states))
            {
                throw new InvalidOperationException($"Expected state one of: {string.Join(',', states)}; acutal state: {sender}.");
            }
        }
    }
}
