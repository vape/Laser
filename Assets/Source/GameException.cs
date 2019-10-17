using System;

namespace Laser
{
    public class GameException : Exception
    {
        public static string GetHelperMessage(int code)
        {
            var msg = default(string);
            switch (code)
            {
                case 0:
                case 1:
                    msg = "Couldn't save game progress.";
                    break;
                case 2:
                case 3:
                    msg = "Couldn't load game progress.";
                    break;
                default:
                    msg = $"Unknown error.";
                    break;
            }

            return $"{msg} Code: {code}";
        }

        public string HelpMessage
        { get; private set; }

        public readonly int Code;

        public GameException(int code)
            : this(code, null, null)
        { }

        public GameException(int code, string debugMessage)
            : this(code, null, debugMessage)
        { }

        public GameException(string debugMessage)
            : this(-1, null, debugMessage)
        { }

        public GameException(int code, Exception innerException, string debugMessage = null)
            : base(debugMessage == null ? "Game exception" : debugMessage, innerException)
        {
            Code = code;
            HelpMessage = GetHelperMessage(code);
        }
    }
}
