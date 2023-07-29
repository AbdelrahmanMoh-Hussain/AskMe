namespace AskMe
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PrintMainMenu();
                var key1 = Console.ReadLine();
                User user = new User();
                if(key1 == "1")
                {
                    user.LogIn();
                }
                else if(key1 == "2")
                {
                    user.SignUp();
                }
                else if(key1 == "3")
                {
                    break;
                }
                else
                {
                    continue;
                }

                while (true)
                {
                    PrintOptionsMenu();
                    var key2 = Console.ReadLine();
                    if(key2 == "1")
                    {
                        user.PrintQuestionsToMe();
                    }
                    else if(key2 == "2")
                    {
                        user.PrintQuestionsFromMe();
                    }
                    else if (key2 == "3")
                    {
                        user.AnswerQuestion();
                    }
                    else if (key2 == "4")
                    {
                        user.DeleteQuestion();
                    }
                    else if (key2 == "5")
                    {
                        user.AskQuestion();
                    }
                    else if (key2 == "6")
                    {
                        user.PrintUsers();
                    }
                    else if (key2 == "7")
                    {
                        user.PrintUserFeed();
                    }
                    else if (key2 == "8")
                    {
                        break;
                    }
                }
            }

        }

        static void PrintOptionsMenu()
        {
            Console.WriteLine("Menu: ");
            Console.WriteLine("1.  Print Questions To Me");
            Console.WriteLine("2.  Print Questions From Me");;
            Console.WriteLine("3.  Answer Question");
            Console.WriteLine("4.  Delete Question");
            Console.WriteLine("5.  Ask Question");
            Console.WriteLine("6.  List System Users");
            Console.WriteLine("7.  Feed");
            Console.WriteLine("8.  Logout");
            Console.WriteLine("Enter menu choice [1 - 8]");
        }

        static void PrintMainMenu()
        {
            Console.WriteLine("Menu: ");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Sign Up");
            Console.WriteLine("3. Exit");
            Console.WriteLine("Enter menu choice [1 - 3]");
        }
    }
}