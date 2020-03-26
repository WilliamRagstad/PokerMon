using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace PokerMon
{
    public static class Functions
    {
        #region Console Color Management
        public static void WriteColor(string text, ConsoleColor foreground, ConsoleColor background)
        {
            ConsoleColor fg = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(text);
            Console.ForegroundColor = fg;
            Console.BackgroundColor = bg;
        }
        public static void WriteColor(string text, ConsoleColor foreground) => WriteColor(text, foreground, Console.BackgroundColor);
        public static void WriteColor(string text) => WriteColor(text, Console.ForegroundColor, Console.BackgroundColor);

        public static void WriteLineColor(string text, ConsoleColor foreground, ConsoleColor background) => WriteColor(text + Environment.NewLine, foreground, background);
        public static void WriteLineColor(string text, ConsoleColor foreground) => WriteColor(text + Environment.NewLine, foreground, Console.BackgroundColor);
        public static void WriteLineColor(string text) => WriteColor(text + Environment.NewLine, Console.ForegroundColor, Console.BackgroundColor);
        #endregion
        #region Console Input
        public static string ReadInput(string text, object defaultValue = null) => ReadInput<string>(text, defaultValue);
        public static T ReadInput<T>(string text, object defaultValue = null)
        {
            WriteColor(text, ConsoleColor.Yellow);
            if (defaultValue != null)
                WriteColor($" ({defaultValue})", ConsoleColor.Cyan);
            Console.Write(": ");
            string r = Console.ReadLine();

            if (r.Length == 0)
            {
                WriteLineColor($"Value must not be empty", ConsoleColor.Red);
                return ReadInput<T>(text); // Re-prompt
            }

            try
            {
                return (T)Convert.ChangeType(r, typeof(T));
            }
            catch
            {
                string type = typeof(T).Name;
                if (typeof(T) == typeof(int)) type = "Integer Number";
                if (typeof(T) == typeof(double) || typeof(T) == typeof(decimal) || typeof(T) == typeof(float)) type = "Decimal Number";
                if (typeof(T) == typeof(string)) type = "Text";
                if (typeof(T) == typeof(bool)) type = "Boolean";

                WriteLineColor($"Value must be of type {type}!", ConsoleColor.Red);
                return ReadInput<T>(text); // Re-prompt
            }
        }
        #endregion
        #region Loading animations
        public static void Loading(string message, int duration, int updatesPerSecond = 2, int dots = 3)
        {
            for (int i = 0; i < duration * updatesPerSecond; i++)
            {
                string suffix = "";
                for (int j = 0; j < i % dots + 1; j++) suffix += '.';
                for (int j = 0; j < dots - i % dots - 1; j++) suffix += ' ';

                WriteColor(message + suffix, ConsoleColor.Gray);
                System.Threading.Thread.Sleep(1000 / updatesPerSecond);
                Console.CursorLeft = 0;
            }
            Console.WriteLine(); // Newline
        }
        private static int counter = 0;
        public static void LoadingStep(string message, int dots = 3)
        {
            string suffix = "";
            for (int j = 0; j < counter % dots + 1; j++) suffix += '.';
            for (int j = 0; j < dots - counter % dots - 1; j++) suffix += ' ';
            WriteColor(message + suffix, ConsoleColor.Gray);
            Console.CursorLeft = 0;

            counter++;
        }
        #endregion
    }
}
