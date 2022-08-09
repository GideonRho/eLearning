namespace LocalTerminal
{
    internal static class Program
    {
        private static void Main(string[] args)
        {

            var terminal = new TerminalController();
            terminal.InputCommand();

        }
    }
}