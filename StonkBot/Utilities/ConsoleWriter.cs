namespace StonkBot.StonkBot.Utilities
{
    public class ConsoleWriter
    {
        private static object _messageLock = new object();

        
        public void WriteMessage(string message)
        {
            var textColor = ConsoleColor.White;
            var backgroundColor = ConsoleColor.Black;

            lock (_messageLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ]  ");

                Console.ForegroundColor = (ConsoleColor)textColor;
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void WriteMessage(string message, ConsoleColor textColor)
        {
            var backgroundColor = ConsoleColor.Black;

            lock (_messageLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ]  ");

                Console.ForegroundColor = (ConsoleColor)textColor;
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
        
        public void WriteMessage(string message, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            lock (_messageLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[ ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ]  ");

                Console.ForegroundColor = (ConsoleColor)textColor;
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void WriteProgress(decimal current, decimal total)
        {
            var textColor = ConsoleColor.Gray;
            var backgroundColor = ConsoleColor.Black;

            lock (_messageLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\r[ ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ]  ");

                Console.ForegroundColor = (ConsoleColor)textColor;
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
                Console.Write($"{(current/total).ToString("P")}");
                Console.ResetColor();
            }
        }

        public void WriteProgressComplete(string message)
        {
            var textColor = ConsoleColor.Gray;
            var backgroundColor = ConsoleColor.Black;

            lock (_messageLock)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\r[ ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"{DateTime.Now:hh:mm:ss tt}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" ]  ");

                Console.ForegroundColor = (ConsoleColor)textColor;
                Console.BackgroundColor = (ConsoleColor)backgroundColor;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}