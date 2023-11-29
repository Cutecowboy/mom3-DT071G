using Packt.Shared;

partial class Program
{
    static void Main()
    {
        // instantize the object
        Guestbook guestbook = new();

        WriteLine(guestbook);
        // setup neccessary files 
        guestbook.Setup();

        Menu();
        // run the program
        void Menu()
        {
            while (true)
            {
                // clear the terminal
                Clear();
                // informational text for user prompts
                WriteLine("N A V I D ' S\t G U E S T B O O K\n\n1. Write in the guestbook\n2. Remove post\nX. Exit\n");
                // print out the current posts in the json file
                WriteLine(guestbook.PrintPosts());
                // check the users prompt
                string userInp = ReadLine()!;
                // if user enters nothing
                if (string.IsNullOrEmpty(userInp))
                {
                    Clear();

                    // error message
                    WriteLine("Enter a valid input, press any key to continue");
                    ReadKey();
                }
                // if enter option 1
                else if (userInp == "1")
                {
                    // addPost method
                    guestbook.AddPost();
                }
                // if enter option 2 
                else if (userInp == "2")
                {
                    Option2();
                }
                // cancel program if user inputs x
                else if (userInp.ToUpper() == "X")
                {
                    Clear();
                    WriteLine("Thank you for visiting my guestbook!");
                    break;
                }
                else // if user inputs anything else
                {
                    Clear();

                    // error message
                    WriteLine("Enter a valid input, press any key to continue");
                    ReadKey();
                }
            }




        }

        // option 2 seperated due to being able to circulate user back to that menu
        void Option2()
        {
            // clear terminal
            Clear();
            // rewrite the terminal
            WriteLine("N A V I D ' S\t G U E S T B O O K\nREMOVE POST\n");
            // print the posts in the json file
            WriteLine(guestbook.PrintPosts());
            WriteLine("X. Go back\n");
            WriteLine("Enter the id of the post you wish to delete");

            // check for user input
            string userInp2 = ReadLine()!;
            // declare empty id variable
            int id;
            // check user input if its null or empty
            if (string.IsNullOrEmpty(userInp2))
            {
                Clear();

                // error message
                WriteLine("Enter a valid input, press any key to continue");
                ReadKey();
                // back to the menu
                Option2();

            }
            // check if user wants to go back
            else if (userInp2.ToUpper() == "X")
            {
                // go back to the meny
                Menu();
            }
            // check if user has inputted an id, tryparse to check if the string inputted is valid int
            else if (int.TryParse(userInp2, out id))
            {
                // if valid int, try deleting the id given
                guestbook.DeletePost(id);
                // back to the menu
                Option2();
            }
            else
            {
                Clear();
                //  error message
                WriteLine("Enter a valid input, press any key to continue");
                ReadKey();
                // back to the menu
                Option2();
            }
        }

    }
}