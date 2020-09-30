using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityNamespace;
using System.Windows.Forms;
using System.IO;


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Solution/Project:  Astronomy Tester/Astronomy Tester
//	File Name:         Driver.cs
//	Description:       A driver to control everthing from the other classes
//	Author:            Roland Mahe, maher1@etsu.edu, Deptartment of Computing, East Tennessee State University
//	Created:           Sunday, September 27th, 2020
//	Copyright:         Roland Mahe, 2020
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Astronomy_Tester
{

    class Program
    {
        /// <summary>Defines the entry point of the application.</summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        static void Main(string[] args)
        {
            #region Window Setup
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Title = "Astronomy Study Tool";
            #endregion

            #region Variable setup
            String original = "";//a string to hold the original untranslated string
            List<String> questions = null;//a string to hold the original untranslated tokenized string
            String strDelims = "0123456789";//delimiters to watch out for
            List<int> questionNums = null; // a string to hold the question num's given
            string userInput;
            int quizLength = 10;//default length of 10
            bool restart = true;
            int counter = 1;
            List<char> userAnswers = new List<char> { };// a list that will store the user's inputed answers
            List<char> correctAnswers = new List<char> { };// a list that will store the correct answers relevant to the user
            List<char> answers = null; //stores all the correct answers
            double grade = -1;

            UtilityNamespace.Menu menu = new UtilityNamespace.Menu("Welcome to the Astomony Study Tool!");
            menu = menu + "Open a file" + "Start a new quiz" + "Review last quiz (Under Construction)" + "Average Grade (Under Construction)" + "Quit";

            Choices choice = (Choices)menu.GetChoice();
            #endregion

            #region Menu control
            while (choice != Choices.QUIT)
            {
                switch (choice)
                {
                    case Choices.OPEN:
                        Console.WriteLine("You selected Open");
                        original = getFile();//use the getFile() method to retrieve the path to the file
                        Console.WriteLine("Finished");
                        Console.ReadKey();
                        break;

                    case Choices.START:
                        Console.WriteLine("You chose to start a new quiz");
                        if (original.Equals(""))//if original is empty (a file hasnt been selected in this case)
                        {
                            Console.WriteLine("Please select a file first");
                        }
                        else
                        {
                            questions = Tools.GetQuestions(original);//storing the tokenized version of original in questions
                            answers = Tools.GetAnswers(original);//storing the answers from original in answers

                            do
                            {
                                restart = true;//in the case that more than one quiz is taken, restart is set back to true here to make sure we get a digit input
                                Console.Clear();
                                Console.Write("Quiz length: ");
                                Console.ForegroundColor = ConsoleColor.Red;
                                userInput = Console.ReadLine();
                                Console.ForegroundColor = ConsoleColor.White;

                                if (userInput.All(char.IsDigit))//if all characters in userInput are digits
                                {
                                    quizLength = Convert.ToInt32(userInput);
                                    restart = false;
                                    questionNums = selectQuestions(questions.Count, quizLength);
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("\aPlease only input numbers...");
                                    Console.ReadLine();
                                }
                            } while (restart); // gets quizLength val from user, and makes sure they input only digits


                            counter = 1;
                            foreach (int questionNumber in questionNums)
                            {

                                do
                                {
                                    Console.Clear();

                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("\n\n\t   " + ("Question " + counter));
                                    Console.Write("\t   ");
                                    for (int n = 0; n < ("Question " + counter).Length; n++)
                                        Console.Write("-");
                                    Console.WriteLine("\n");
                                    Console.ForegroundColor = ConsoleColor.White;

                                    Console.WriteLine(questions[questionNumber]);

                                    Console.Write("The answer is: ");
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    userInput = Console.ReadLine();
                                    Console.ForegroundColor = ConsoleColor.White;


                                    restart = true;
                                    if (userInput.Length == 1)// if userInput is only equal to one character
                                    {
                                        userAnswers.Add(Convert.ToChar(userInput));
                                        restart = false;
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("\aPlease input only one character...");
                                        Console.ReadLine();
                                    }
                                } while (restart); //does this until we get an answer for the question
                                counter++;
                            } // presents all the questions and gathers all the users answers

                            foreach(int number in questionNums)
                            {
                                correctAnswers.Add(answers[number]);
                            }

                            Console.Clear();
                            Console.WriteLine($"Total Questions: {questions.Count}\n" +
                                              $"Total Answers: {answers.Count}\n" +
                                              $"User Answers: {userAnswers.Count}\n" +
                                              $"Correct Answers: {correctAnswers.Count}");
                            Console.ReadLine();

                            #region Display Grade

                            Console.Clear();
                            grade = Tools.GetGrade(correctAnswers, userAnswers);

                            Console.Write($"You scored a ");
                            if(grade > 84)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else if(grade > 70)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }

                            Console.Write($"{Math.Round(grade, 2)}%");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("!");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\t\t╔═══════════════════════╦═══════════════════════╦═══════════════════════╗");
                            Console.Write("\t\t║\t");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Question");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t║\t");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Correct Answer");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("\t║\t");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Given Answer");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("\t║");
                            // 1 "╔═══════════════════════╦═══════════════════════╦═══════════════════════╗"
                            // 2 "║       Question        ║       Correct Answer  ║       Given Answer    ║"
                            // 3 "╠═══════════════════════╬═══════════════════════╬═══════════════════════╣"
                            // 4 "║       Question        ║       Correct Answer  ║       Given Answer    ║"
                            // 5 "╚═══════════════════════╩═══════════════════════╩═══════════════════════╝"

                            for (int i = 0; i < correctAnswers.Count; i++) //prints the "in betweeen sections of the table... i.e. rows 2 - 4 on the table above.
                            {
                                Console.WriteLine("\t\t╠═══════════════════════╬═══════════════════════╬═══════════════════════╣");
                                Console.Write("\t\t║\t");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write($"({i + 1}){questionNums[i] + 1}");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("\t\t║\t");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(correctAnswers[i]);
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.Write("\t\t║\t");
                                
                                if(correctAnswers[i] == userAnswers[i])
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }

                                Console.Write(userAnswers[i]);
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("\t\t║");

                            }
                            Console.WriteLine("\t\t╚═══════════════════════╩═══════════════════════╩═══════════════════════╝");
                            #endregion

                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\nUnder construction: Viewing individual questions with the correct answer and your given answer");
                            counter = 0;
                            userAnswers = new List<char> { };
                            correctAnswers = new List<char> { };
                        }
                        Console.ReadLine();
                        break;
                }//end of switch

                choice = (Choices)menu.GetChoice();
                #endregion
            } //end of while

        }//end of main

        /// <summary>Gets the file.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="FileNotFoundException">The file '{filePath}</exception>
        public static String getFile()
        {
            OpenFileDialog OpenDlg = new OpenFileDialog();//creating new object OpenDlg and initializing it
            OpenDlg.Filter = "text files|*.txt;*.text|all files|*.*";//giving the instructions on what files to show
            OpenDlg.InitialDirectory = Application.StartupPath;//setting the initial directory to the apps start up path
            OpenDlg.Title = "Select your desired text file";//setting the title text
            String filePath = "";//filePath will store the path to the file

            if (DialogResult.OK == OpenDlg.ShowDialog())//if the dialogs result is equal to OK
            {
                filePath = OpenDlg.FileName;//set the value of filepath to the path to the file chosen
            }

            if (!File.Exists(filePath))//if the file doesnt exist
            {
                throw new FileNotFoundException($"The file '{filePath}' was not found and could not be opened.");//throw an exception
            }

            StreamReader rdr = new StreamReader(filePath);//creating a new streamreader object called rdr
            String strWholeFile = rdr.ReadToEnd();//reading the whole file and storing it in  strWholeFile
            return strWholeFile;//return the whole file
        }

        /// <summary>Selects a random questions</summary>
        /// <param name="questionLimit">
        ///   <br />
        /// </param>
        /// <param name="quizLength">
        ///   <br />
        /// </param>
        /// <returns>Returns list of question numbers that have been randomly selected</returns>
        public static List<int> selectQuestions(int questionLimit, int quizLength)
        {
            List<int> questionNums = new List<int> { };// a list to store question numbers that have been randomly selected
            Random rnd = new Random();// random numbers

            for(int i = 0; i < quizLength; i++)//loop runs and adds questions numbers to list to the disired length of the quiz
            {
                questionNums.Add(rnd.Next(questionLimit));//adding randomly selected number to question number list
            }

            return questionNums;//returning list of question numbers
        }
    }//end of class


}//end of namespace

