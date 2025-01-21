using System.Text;
namespace IFN645_SOS
{
    public interface IMenuComponent
    {
        void Display();
    }

    public class Menu : IMenuComponent 
    {
        public string Name { get; }
        public Dictionary<string, MenuCommands> menuCommands = new Dictionary<string, MenuCommands>();
        public List<string> menuOrder = new List<string>();

        public Menu(string name) 
        {
            this.Name = name;
        }
        public void Add(string menuName, MenuCommands menuCommand) 
        {
            try
            {
                menuCommands.ContainsKey(menuName);
                menuCommands[menuName] = menuCommand;
                menuOrder.Insert(0,menuName);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("An exception occurred while trying to a entry in a menu: " + ex.Message);
            }
        }
        public void Display() 
        {
            /*here the goal is to display options , first select a game, then a second menu to select new or load a game, and if load game is selected
            display the available saves for the selectected game*/
            int selectedIndex = 0;
            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;
            int centerX = consoleWidth / 2;
            int centerY = consoleHeight / 2;
            Console.Clear();
            //select the game
            Console.SetCursorPosition(centerX - ("Select an Option".Length / 2), centerY - menuOrder.Count / 2 - 1);
            Console.WriteLine("Select an Option:");
            int menuCenterY = centerY - menuOrder.Count / 2;
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(centerX - ("Select an Option".Length / 2), centerY - menuOrder.Count / 2 - 1);
                Console.WriteLine("Select an Option:"); ;
                for (int i = 0; i < menuOrder.Count; i++)
                {
                    string label = menuOrder[i];
                    if (i == selectedIndex)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.SetCursorPosition(centerX - label.Length / 2, menuCenterY + i);
                    Console.WriteLine(label);
                    Console.ResetColor();
                }


                ConsoleKeyInfo keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex - 1 + menuOrder.Count) % menuOrder.Count;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex + 1) % menuOrder.Count;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    string selectedOption = menuOrder[selectedIndex];
                    //
                    Console.Clear();
                    Console.WriteLine($"Selected Option: {selectedOption}");
                    //will look in the command dict for the correct one, should execute a method here, can be a game,new game,load game, quit
                    if (menuCommands[selectedOption] is MenuCommands gameCommand)
                    {
                        gameCommand.Execute();
                        return;
                       
                    }
                    else
                    {
                        throw new Exception("Menu command do not exist");
                    }
                }
            }
        }
    
    }

    public class UserInterface
    {
        private Dictionary<string, Menu> menus = new Dictionary<string, Menu>();
        public Match Match { get; set; } = null;

       
        public void DispayMenu()
        {
            //display each menu, a check is included in the start and saves menu to ensure they are display if necessary only
            foreach (KeyValuePair<string, Menu> kvp in menus)
            {
               kvp.Value.Display();
            }

        }
        public void CreateMatch()
        {
            if (this.Match == null)
            {
                this.Match = new Match();
            }
            else
            {
                throw new InvalidOperationException("game already created");

            }
        }
        public void AddMenu(Menu newMenu) 
        {
            if (menus.TryGetValue(newMenu.Name, out Menu menu))
            {
                throw new InvalidOperationException("Menu already added to interface found!");
            }
            else
            {
                menus[newMenu.Name] =newMenu;
            }
        }
        public void AddCommand(string menuName, string commandName, MenuCommands newCommand)
        {
            if (menus.TryGetValue(menuName, out Menu menu))
            {
                menu.Add(commandName, newCommand);
            }
            else
            {
                throw new InvalidOperationException("Menu not found!");
            }
        }
        public void addGame(string gameName, Game newGame, GameStrategyManager gameStrategyManager, IGameLogic gamelogic)
        {
            StartMatchCommand gameCommand = new StartMatchCommand(newGame,this.Match, gameStrategyManager, gamelogic);
            try
            {
                AddCommand("Main Menu", gameName, gameCommand);
            }
            catch
            {
                throw new Exception("Game not added");
            }
        }
        public void initializeGame() 
        {
            Match.InitializeMatch();
        }


        public Dictionary<string, string> GetListOfSaves()
        //for each line, create a 
        {
            const char DELIM = ',';
            const string FILENAME = "saves.txt";
            const string MOVEDELIM = "Moves";
            //implementation of game selection
            Dictionary<string, string> savedGames = new Dictionary<string, string>();
            using (FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(inFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(DELIM);
                    if (parts.Length >= 2)
                    {
                        string gameName = parts[1] + DELIM + parts[0];
                        string gameData = string.Join(DELIM.ToString(), parts.Skip(2));
                        savedGames[gameName] = gameData;
                    }
                }
            }
            return savedGames;
        }
        public Dictionary<string, MenuCommands> GetCommandDict(Dictionary<string, string> saves)
        {
            Dictionary<string, MenuCommands> savesCommands = new Dictionary<string, MenuCommands>();
            foreach (KeyValuePair<string, string> kvp in saves)
            {
                LoadGameCommand save = new LoadGameCommand(this.Match, kvp.Value);

                savesCommands[kvp.Key] = save;
            }
            Console.WriteLine("Dict if saves created");
            return savesCommands;
        }
        public List<string> CreaterOrderList(Dictionary<string, MenuCommands> dictionnaire)
        {
            List<string> orderMenu = new List<string>();
            foreach (KeyValuePair<string, MenuCommands> kvpair in dictionnaire)
            {
                orderMenu.Add(kvpair.Key);
            }
            return orderMenu;
        }
        public void UpdateSaveMenu() 
        {
            foreach (KeyValuePair<string, MenuCommands> kvp in GetCommandDict(GetListOfSaves())) 
            {
                AddCommand("Saves", kvp.Key, kvp.Value);
            }
        }
     
        public void LaunchApp()
        {
            DispayMenu();

        }

    }

    public interface IUserInterfaceGenerator 
    {
        void CreateInterface(UserInterface userInterface);
    }

    public class DefaultUserInterfaceConfigurator : IUserInterfaceGenerator
    {
        public void CreateInterface(UserInterface userInterface)
        {
            userInterface.AddCommand("Exit Game", "Exit Game", new ExiteGameCommand());
     
        }
    }
    public class Ass2InterfaceCreator : IUserInterfaceGenerator 
    {
        public void CreateInterface(UserInterface userInterface)
        {
            userInterface.CreateMatch();
            userInterface.AddMenu(new Menu("Main Menu"));
            userInterface.AddCommand("Main Menu", "Exit Game", new ExiteGameCommand());
            //add the necessary strategy available for players
            GameStrategyManager strategyManager = new GameStrategyManager();
            strategyManager.AddStategy(new AiMoveStategyEasy());
            strategyManager.AddStategy(new HumanSosMoveStategy());
            //add the game to the main menu
            userInterface.addGame("SOS", new ConcreteSOS(), strategyManager, new SosGameLogic());
            userInterface.AddMenu(new Menu("Start Menu"));
            userInterface.AddCommand("Start Menu", "New Game", new NewGameCommand(userInterface.Match));
            userInterface.AddCommand("Start Menu", "Load Game", new LoadSavesCommand(userInterface));
            userInterface.AddMenu(new Menu("Saves"));
            
        }
    }
 
}
