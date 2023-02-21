using System.Runtime.CompilerServices;

namespace DelfosseA4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string file = "movies.csv";
            string choice;
            do
            {
                Console.WriteLine("1) List Movies.");
                Console.WriteLine("2) Add Movies.");
                Console.WriteLine("Enter any other key to exit");
                choice = Console.ReadLine();

                // create parallel lists of movie details
                // lists must be used since we do not know number of lines of data
                List<UInt64> MovieIds = new List<UInt64>();
                List<string> MovieTitles = new List<string>();
                List<string> MovieGenres = new List<string>();

                StreamReader sr = new StreamReader(file);
                // first line contains column headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    // first look for quote(") in string
                    // this indicates a comma(,) in movie title
                    int idx = line.IndexOf('"');
                    if (idx == -1)
                    {
                        // no quote = no comma in movie title
                        // movie details are separated with comma(,)
                        string[] movieDetails = line.Split(',');
                        // 1st array element contains movie id
                        MovieIds.Add(UInt64.Parse(movieDetails[0]));
                        // 2nd array element contains movie title
                        MovieTitles.Add(movieDetails[1]);
                        // 3rd array element contains movie genre(s)
                        // replace "|" with ", "
                        MovieGenres.Add(movieDetails[2].Replace("|", ", "));
                    }
                    else
                    {
                        // quote = comma in movie title
                        // extract the movieId
                        MovieIds.Add(UInt64.Parse(line.Substring(0, idx - 1)));
                        // remove movieId and first quote from string
                        line = line.Substring(idx + 1);
                        // find the next quote
                        idx = line.IndexOf('"');
                        // extract the movieTitle
                        MovieTitles.Add(line.Substring(0, idx));
                        // remove title and last comma from the string
                        line = line.Substring(idx + 2);
                        // replace the "|" with ", "
                        MovieGenres.Add(line.Replace("|", ", "));
                    }
                }
                // close file when done
                sr.Close();

                if (choice == "1")
                {

                    //logic to read
                    sr = new StreamReader(file);
                    int page = 25;
                    bool nextPage = true;

                    sr.ReadLine();
                    while (nextPage)
                    {
                        for (int i = 0; i < page; i++)
                        {
                            if (!sr.EndOfStream)
                            {
                                var line = sr.ReadLine();
                                string[] arr = line.Split(',');
                                //movieId,title,genres
                                Console.WriteLine($"{arr[0]},{arr[1]},{arr[2]}");
                            }
                        }

                        if (sr.EndOfStream)
                        {
                            Console.WriteLine("You've reached the end");
                            Console.WriteLine("Press enter to continue");
                        }
                        else
                        {
                            Console.WriteLine("Choose next page? (Y/N)");
                        }
                        string userInput = Console.ReadLine().ToUpper();


                        if (userInput == "Y" && !sr.EndOfStream)
                        {
                            nextPage = true;
                        }
                        else
                        {
                            nextPage = false;
                        }
                    }
                    sr.Close(); //Always close!!
                }
                else if (choice == "2")

                { // Add Movie
                  // ask user to input movie title
                    Console.WriteLine("Enter the movie title");
                    // input title
                    string movieTitle = Console.ReadLine();
                    // check for duplicate title
                    List<string> LowerCaseMovieTitles = MovieTitles.ConvertAll(t => t.ToLower());
                    if (LowerCaseMovieTitles.Contains(movieTitle.ToLower()))
                    {
                        Console.WriteLine("That movie already exists in this list");
                    }
                    else
                    {
                        // generate movie id - use max value in MovieIds + 1
                        UInt64 movieId = MovieIds.Max() + 1;
                        // input genres
                        List<string> genres = new List<string>();
                        string genre;
                        do
                        {
                            // ask user to enter genre
                            Console.WriteLine("Enter genre (or done to quit)");
                            // input genre
                            genre = Console.ReadLine();
                            // if user enters "done"
                            // or does not enter a genre do not add it to list
                            if (genre != "done" && genre.Length > 0)
                            {
                                genres.Add(genre);
                            }
                        } while (genre != "done");
                        // specify if no genres are entered
                        if (genres.Count == 0)
                        {
                            genres.Add("(no genres listed)");
                        }
                        // use "|" as delimeter for genres
                        string genresString = string.Join("|", genres);
                        // if there is a comma(,) in the title, wrap it in quotes
                        movieTitle = movieTitle.IndexOf(',') != -1 ? $"\"{movieTitle}\"" : movieTitle;
                        // display movie id, title, genres
                        Console.WriteLine($"{movieId},{movieTitle},{genresString}");
                        // create file from data
                        StreamWriter sw = new StreamWriter(file, true);
                        sw.WriteLine($"{movieId},{movieTitle},{genresString}");
                        sw.Close();
                        // add movie details to Lists
                        MovieIds.Add(movieId);
                        MovieTitles.Add(movieTitle);
                        MovieGenres.Add(genresString);
                    }
                }

            } while (choice == "1" || choice == "2");
        }
    }
}