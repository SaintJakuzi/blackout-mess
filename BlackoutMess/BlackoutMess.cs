using System;
using System.ComponentModel.Design;

namespace BlackoutMess
{
    internal static class BlackoutMess
    {
        enum States
        {
            AvailableChatsView,
            ChatView,
            SettingsView
        }

        private static States _currentState;
        private static int id_currentChat;

        private static bool shouldExit = false;

        public static void Main(string[] args)
        {
            var dbHelper = new DBHelper();
            var settings = new Settings();
            var chatDataManager = new ChatDataManager(dbHelper, settings);

            int result;
            do
            {
                Console.WriteLine("Authenticating...");
                Console.Write("Enter username: ");
                string userName = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();

                result = chatDataManager.AuthUser(userName, password);
            } while (result != 0);

            chatDataManager.PrintAvailableChats();
            _currentState = States.AvailableChatsView;

            Printer.PrintHelp();

            do
            {
                string input = Console.ReadLine();

                if (input != null && input.StartsWith("/"))
                {
                    string[] decomposedInput = input.Split();

                    if (decomposedInput[0] == "/open" || decomposedInput[0] == "/o")
                    {
                        int id_targetUser = int.Parse(decomposedInput[1].ToString());
                        int returned = chatDataManager.PrintChat(id_targetUser);
                        if (returned == -1)
                        {
                            Console.WriteLine("There is no chat with this ID.");
                        }
                        else
                        {
                            id_currentChat = returned;
                            _currentState = States.ChatView;
                        }
                    }
                }
            } while (!shouldExit);
        }
    }
}