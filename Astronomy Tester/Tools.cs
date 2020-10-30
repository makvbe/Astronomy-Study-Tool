using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Solution/Project:  Astronomy Tester/Astronomy Tester
//	File Name:         Choices.cs
//	Description:       A set of Tools for a Driver
//	Author:            Roland Mahe, maherp@etsu.edu, Dept. of Computing, East Tennessee State University
//	Created:           Sunday, September 27th, 2020
//	Copyright:         Roland Mahe, 2020               
//
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Astronomy_Tester
{
    /// <summary>
    ///   <para>A collection of Tools to be used for the Astronomy_Tester namespace</para>
    /// </summary>
    class Tools
    {
        /// <summary>Gets the questions.</summary>
        /// <param name="strIn">The string in.</param>
        /// <returns>
        ///   <para>Returns a list of strings (questions)</para>
        /// </returns>
        public static List<string> GetQuestions (string strIn)
        {
            List<String> parsedStrIn = new List<String> { };//a list string that will store each individual question and choices

            int delimIndex;//stores current index
            int startIndex = 0;//stores previous index and/or starting index
            int questionStartIndex = 0;
            char curChar; //stores current char
            bool questionStart;
            bool endQuestion = false;

            do
            {
                
                do
                {
                    questionStart = false;
                    delimIndex = strIn.IndexOf('\n', startIndex);//gets index of first character found from delims starting from the startIndex. Step 1
                    if (delimIndex > -1)//if a newline was found
                    {
                        curChar = strIn[delimIndex + 1];//stores the next character after the character found in step 1. Step 2
                        if (Char.IsDigit(curChar))//if next character is a digit
                        {
                            questionStart = true;
                        }
                        else if(curChar == 'A')
                        {
                            questionStart = true;
                            endQuestion = true;
                        }
                        else
                        {
                            startIndex = delimIndex + 1;
                        }
                    }
                    else { }
                } while (!questionStart); // while there isn't a question

                // if the start of the quesiton isn't the very first "\nDigit" found 
                if (questionStart && questionStartIndex != 0) 
                {
                    parsedStrIn.Add(strIn.Substring(questionStartIndex, delimIndex - questionStartIndex));
                }
                // if it is the very first "\nDigit" found 
                else if (questionStart)
                {
                    startIndex = delimIndex;
                }
                questionStartIndex = startIndex;
                startIndex = delimIndex + 1;
            } while (!endQuestion);//continue this loop until the very last question

            return parsedStrIn;
        }

        /// <summary>Gets the answers.</summary>
        /// <param name="strIn">The string in.</param>
        /// <returns>
        ///   <para>Returns a list of chars (answers)</para>
        /// </returns>
        public static List<char> GetAnswers (string strIn)
        {
            List<char> answers = new List<char> { };

            int answersIndex = strIn.IndexOf("Answers:");//index of "Answers:" string
            int answersStart = answersIndex + 9;//start index of answers list
            string answersString = strIn.Substring(answersStart, (strIn.Length - 1) - answersStart);
            string[] answerSplit = answersString.Split(new[] { ", " }, StringSplitOptions.None);
            string prevAnswer = "previous Answer";

            foreach (string answer in answerSplit)
            {
                if (prevAnswer != answer)
                {
                    answers.Add(answer[answer.Length - 1]);
                }
                prevAnswer = answer;
            }
            
            return answers;
        }

        /// <summary>Gets the grade.</summary>
        /// <param name="correctAnswers">The correct answers.</param>
        /// <param name="userAnswers">The user answers.</param>
        /// <returns>
        ///   <para>Returns the grade</para>
        /// </returns>
        public static double GetGrade (List<char> correctAnswers, List<char> userAnswers)
        {
            double grade = -1;

            if (correctAnswers.Count == userAnswers.Count)
            {
                grade = 0;
                for(int i = 0; i < correctAnswers.Count; i++)
                {
                    if(correctAnswers[i] == userAnswers[i])
                    {
                        grade++;
                    }
                }

                grade = (grade / correctAnswers.Count) * 100;
            }

            return grade;
        }

        public static bool IsChar(string userInput)
        {
            

            return true;
        }
    }
}
