using System.Text.Json;
namespace Packt.Shared;

/// <summary>
/// Guestbook is an object which enable users to add posts and delete posts via its methods
/// </summary>
public class Guestbook
{
    // Post object will store the id, name and message information
    public record Entries(int Id, string Name, string Message);

    // emtpy object as standard
    public List<Entries> entries = [];

    /// <summary>
    /// Setup the environment before executing the program, checks for the JSON file and creates it if needed
    /// </summary>
    public void Setup()
    {
        // check if JSON file exists
        if (File.Exists("book.json"))
        {
            WriteLine("File exists!");
            // read the JSON data
            string jsonData = File.ReadAllText("book.json");
            WriteLine(string.IsNullOrEmpty(jsonData));
            // check that the json file (string) that it is not empty 
            if (!string.IsNullOrEmpty(jsonData))
            {
                // store the JSON data deserialize already checked to not me null or empty
                entries = JsonSerializer.Deserialize<List<Entries>>(jsonData)!;
                WriteLine(entries[0]);
                WriteLine(entries.Count());
            }

        }
        else
        {
            // create the JSON file
            File.WriteAllText("book.json", "");

        }
    }


    /// <summary>
    /// Method for the user to be able to add a post
    /// </summary>
    public void AddPost()
    {
        // dummy variables to ensure that all conditions are fulfilled
        bool NameChecker = false;
        bool MessageChecker = false;
        // empty declared input variables, wont pass conditions.
        string inpAuthor = "";
        string inpContent = "";

        // first condition loop
        while (NameChecker == false)
        {
            // clear terminal
            Clear();
            // prompt user 
            WriteLine("Author name: ");
            // read and store user input
            inpAuthor = ReadLine()!;
            // check if the user input is null or empty
            if (string.IsNullOrEmpty(inpAuthor))
            {
                Clear();

                // write error message
                WriteLine("Please enter a valid name, press any key to continue!");
                ReadKey();
            }
            // check whether the users name is geq 2 characters
            else if (inpAuthor.Length < 2)
            {
                Clear();

                WriteLine("Please enter a name that includes atleast two characters, press any key to continue!");
                ReadKey();
            }
            else // conditions are met
            {
                // toggle dummy variable to true, break loop
                NameChecker = true;
            }
        }

        // second condition loop
        while (MessageChecker == false)
        {
            // clear terminal
            Clear();
            // prompt user
            WriteLine("Content: ");
            // read and store user input
            inpContent = ReadLine()!;
            // check user input is not null or empty
            if (string.IsNullOrEmpty(inpContent))
            {
                Clear();

                // write error message
                WriteLine("Please write your content, press any key to continue!");
                ReadKey();
            }
            // check length of the input, must be geq 5
            else if (inpContent.Length < 5)
            {
                Clear();

                // write error message
                WriteLine("Please write a content greater than 4 characters long, press any key to continue!");
                ReadKey();
            }
            else // conditions are met
            {
                // break loop
                MessageChecker = true;
            }
        }

        // double check that the dummy variables are ok
        if (MessageChecker && NameChecker)
        {
            Clear();
            // Prompt user the post was successfully created
            WriteLine($"Name: {inpAuthor}\nContent: {inpContent}\nPost successfully created, press any key to continue!");
            ReadKey();
            // store the user inputted value in a new Post object
            var newPost = new Entries(Id: PostId(), Name: inpAuthor, Message: inpContent);
            // append the new post to the entries
            entries.Add(newPost);
            // initialize method to save the entries to JSON file
            Save();

        }
    }

    /// <summary>
    /// Check which index the post should get when adding a new post
    /// </summary>
    /// <returns>The id number of the new post</returns>
    public int PostId()
    {
        // create a new list
        List<Entries> currentEntries = [];

        // get the data from the JSON file 
        string jsonContent = File.ReadAllText("book.json");
        // check whether the file is empty or not
        if (string.IsNullOrEmpty(jsonContent))
        {
            // if empty return id 1
            return 1;
        }
        else // check the idcount and return count + 1
        {
            currentEntries = JsonSerializer.Deserialize<List<Entries>>(jsonContent)!;
            // init the counter
            int count = 0;
            // loop through the entries
            foreach (Entries e in currentEntries)
            {
                // if the current counter is less than the loops id
                if (count < e.Id)
                {
                    // change the current count to that id
                    count = e.Id;
                }
            }
            // return the count + 1
            return count + 1;
        }
    }

    /// <summary>
    /// Save the content on the entries to the JSON file
    /// </summary>
    public void Save()
    {
        // Serialize the entries
        string json = JsonSerializer.Serialize(entries);
        // write the json data to the json file
        File.WriteAllText("book.json", json);
    }

    /// <summary>
    /// Delete a post given the inputted id 
    /// </summary>
    /// <param name="id">The id value of the post that you want to delete</param>
    public void DeletePost(int id)
    {
        // create a new list
        List<Entries> currentEntries = [];
        // create a secondary new list to store entries after "deletion"
        List<Entries> holdEntries = [];

        // get JSON data
        string jsonContent = File.ReadAllText("book.json");
        // insert to currentEntries
        currentEntries = JsonSerializer.Deserialize<List<Entries>>(jsonContent)!;

        // loop through all the entries 
        foreach (Entries c in currentEntries)
        {
            // condition to include all id's that were not given by the user 
            if (id != c.Id)
            {
                // append/add object to the holdEntries list
                holdEntries.Add(c);
            }


        }

        // control whether a post was removed by checking the counts, if old count > new count successful deletion
        if (currentEntries.Count > holdEntries.Count)
        {
            // store the new entries in the entries object
            entries = holdEntries;

            // save the new entries on JSON file
            Save();
            // write success message
            Clear();

            WriteLine($"Post id: {id} is now removed, press any key to continue!");
            ReadKey();
        }
        else // if no posts where removed
        {
            // write error message
            Clear();

            WriteLine($"Post id: {id} was not found, press any key to continue!");
            ReadKey();

        }



    }

    /// <summary>
    /// Prints all the posts in the JSON file in a decent format
    /// </summary>
    /// <returns>A string of all posts which can be printed via WriteLine()</returns>
    public string PrintPosts()
    {
        // create a new list
        List<Entries> currentEntries = [];
        // initialize with empty string
        string messageString = "";
        // get JSON data
        string jsonContent = File.ReadAllText("book.json");

        // check if the JSON data has any data
        if (jsonContent.Length == 0)
        {
            // inform user about no posts 
            messageString = "There are no posts in the guestbook!";
        }
        else // if posts exists
        {
            // insert to currentEntries
            currentEntries = JsonSerializer.Deserialize<List<Entries>>(jsonContent)!;

            // loop through all the entries
            foreach (Entries c in currentEntries)
            {
                // and add the id, name and content of the post to the string
                messageString += $"[{c.Id}] {c.Name}  -  {c.Message}\n";
            }
        }

        // return the string 
        return messageString;
    }

}
