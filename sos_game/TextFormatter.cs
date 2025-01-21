
using System.Text;


namespace IFN645_SOS
{
    public class TextFormatter
    {
        public void DisplayCenteredText(string text)
        {
            Console.Clear();
            string gameRules = text;
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            int textWidth = Math.Min(consoleWidth, 80); 
            int paddingHoriz = (consoleWidth - textWidth) / 2;
            int paddingVert = (consoleHeight - gameRules.Split('\n').Length) / 2;

            string[] chunks = SplitTextIntoLines(gameRules, textWidth);
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < paddingVert; i++)
            {
                Console.WriteLine();
            }

            foreach (string chunk in chunks)
            {
                string centeredChunk = chunk.PadLeft(paddingHoriz + chunk.Length).PadRight(consoleWidth);
                Console.WriteLine(centeredChunk);
            }

            ConsoleKeyInfo keyInfo;

            do
            {
              
                keyInfo = Console.ReadKey();
            } while (keyInfo.Key != ConsoleKey.Enter);
        }
        static string[] SplitTextIntoLines(string text, int lineLength)

        {
            text += ".....Press enter to proceed.";
            string[] words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var lines = new List<string>();
            var currentLine = new StringBuilder();

            foreach(string word in words)
        {
                if (currentLine.Length + word.Length + 1 <= lineLength)
                {
                    if (currentLine.Length > 0)
                    {
                        currentLine.Append(' ');
                    }
                    currentLine.Append(word);
                }
                else
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear().Append(word);
                }
            }

            lines.Add(currentLine.ToString());
            return lines.ToArray();
        }
    }
    
}
