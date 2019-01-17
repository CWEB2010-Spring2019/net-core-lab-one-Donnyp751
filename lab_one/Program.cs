using System;
using System.Collections.Generic;
using System.IO;

namespace lab_one
{
    class Question
    {
        string question;
        int correctIndex;
        bool answeredCorrect = false;
        List<string> answers;
        int selectedAnswer;
        public Question(string question, List<string> answers, int correctIndex)
        {
            this.question = question;
            this.correctIndex = correctIndex;
            this.answers = answers;
        }
        public Question()
        {
            this.answers = new List<string>();
        }
        public void AddQuestion(string question)
        {
            this.question = question;
        }
        public void AddAnswers(string answer)
        {
            this.answers.Add(answer);
        }

        public void AddCorrectAnswer(int correctIndex)
        {
            this.correctIndex = correctIndex;
        }

        public bool checkAnswer(char answer)
        {
            answer = char.ToUpper(answer);
            int answerVal = answer - 40;

            this.selectedAnswer = answerVal;

            return answerVal == this.correctIndex;
        }
        public int getSelectedAnswer()
        {
            return this.selectedAnswer;
        }
        public int getCorrectAnswer()
        {
            return this.correctIndex;
        }

        
        public string getQuestion()
        {
            return this.question;
        }
        public List<string> getAnswers()
        {
            return this.answers;
        }
        public bool checkAnswer(int index)
        {
            this.answeredCorrect = this.correctIndex == index;
            this.selectedAnswer = index;
            return this.answeredCorrect;
        }
        public bool checkResult()
        {
            return this.answeredCorrect;
        }

    }
    class Test
    {
        private List<Question> questions;
        private int totalCorrect;
        private int totalQuestions;
        public Test(string[] questionFileContent)
        {
            questions = new List<Question>();
            this.totalQuestions = 0;
            foreach(string line in questionFileContent)
            {
                if (line.Contains("Question,Answer1,Answer2,Answer3,Answer4,CorrectIndex"))
                    continue;

                this.totalQuestions++;
                var segments = line.Split(",");

                Question NewQuestion = new Question();
                NewQuestion.AddQuestion(segments[0]);
                for(int i = 1; i < 5; i++)
                {   
                    NewQuestion.AddAnswers(segments[i]);
                }
                NewQuestion.AddCorrectAnswer(Convert.ToInt32(segments[5]));

                this.questions.Add(NewQuestion);
            }
        }

        public void resetResults()
        {
            totalCorrect = 0;
        }

        private void resetConsoleColors()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }
        public void Take()
        {
            int questionIndex = 1;

            resetConsoleColors();

            foreach (Question currentQuestion in this.questions)
            {
                Console.Clear();
                resetConsoleColors();

                int leftDist = 0;
                int topDist = 3;
                Console.SetCursorPosition(leftDist, topDist++);


                Console.Write(questionIndex);
                Console.Write(". ");
                
                Console.Write(currentQuestion.getQuestion());

                char answerLetter = 'A';

                foreach(string answer in currentQuestion.getAnswers())
                {
                    Console.SetCursorPosition(leftDist, topDist);
                    Console.Write("(");
                    Console.Write(answerLetter ++);
                    Console.Write(")");
                    Console.SetCursorPosition(leftDist + 4, topDist++);
                    Console.Write(answer);
                }
                Console.SetCursorPosition(leftDist, topDist + 2);
                Console.WriteLine("Please enter your answer below\n");
                
     
                var result = Console.ReadKey().KeyChar.ToString().ToUpper().ToCharArray(); //Converting char to string to make uppercase then base to char..

                var isCorrect = currentQuestion.checkAnswer(result[0] - 64);
                Console.WriteLine();
                if (isCorrect)
                {
                    totalCorrect++;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("Correct!");
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Incorrect!");
                }
                resetConsoleColors();
      
                Console.WriteLine("\nPress any key to continue");

                Console.ReadKey();
                questionIndex++;
            }
        }
        private float getTestPercentage()
        {
            return this.totalCorrect != 0 ? ((float)this.totalCorrect/(float)this.totalQuestions) * 100: 0;
        }

        private void writeSpace(int count)
        {
            for(int i = 0; i < count; i++)
            {
                Console.Write(" ");
            }
        }
        private void printTableLine(string firstColumn, string secondColumn, string thirdColumn)
        {
            int firstColumnBuffer = 4;
            int secondColumnBuffer = 15;
            int thirdColumnBuffer = 17;

            writeSpace((firstColumnBuffer - firstColumn.Length) / 2);
            Console.Write(firstColumn);
            writeSpace((firstColumnBuffer - firstColumn.Length) / 2);
            Console.Write("|");

            writeSpace((secondColumnBuffer - secondColumn.Length) / 2);
            Console.Write(secondColumn);
            writeSpace((secondColumnBuffer - secondColumn.Length) / 2);
            Console.Write("|");

            writeSpace((thirdColumnBuffer - thirdColumn.Length) / 2);
            Console.Write(thirdColumn);
            writeSpace((thirdColumnBuffer - thirdColumn.Length) / 2);

        }
        public void Results()
        {
            resetConsoleColors();
            Console.Clear();
            int leftPos = 0;
            int topPos = 2;
            Console.SetCursorPosition(leftPos, topPos);
            printTableLine("#", "You answered ", "Correct answer");

            int questionCount = 0;
            foreach(Question question in this.questions)
            {
                questionCount++;
                topPos++;
                Console.SetCursorPosition(leftPos, topPos);
                bool correct = question.checkResult();
                if (correct)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    printTableLine(questionCount.ToString(), Convert.ToChar(question.getSelectedAnswer() + 64).ToString(), Convert.ToChar(question.getCorrectAnswer() + 64).ToString());
                }
                else
                { 
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    printTableLine(questionCount.ToString(), Convert.ToChar(question.getSelectedAnswer() + 64).ToString(), Convert.ToChar(question.getCorrectAnswer() + 64).ToString());
                }

            }

            Console.SetCursorPosition(leftPos, topPos + 2);

            if(getTestPercentage() > 70)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("Congrats you passed with " + getTestPercentage() .ToString() + "%");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Congrats you failed with " + getTestPercentage().ToString() + "%");
            }
            resetConsoleColors();

        }
        public Question getFirstQuestion()
        {
            if(this.questions != null)
            {
                return questions[0];
            }else
                return null;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Test test = null;

            try
            {
                test = new Test(File.ReadAllLines(args[0]));
            }
            catch (Exception e)
            {
                Console.WriteLine("failed to load quiz: "+ e.ToString());
            }
            Console.WriteLine("Hello, welcome to the DotNetCore quiz");
            Console.WriteLine("Click enter to start the quiz!");

            bool Testing = true;

            while (Testing)
            {
                test.resetResults();
                test.Take();
                test.Results();

                Console.WriteLine("Enter y to test again, else any other key to end");

                Testing = Console.ReadKey().Key == ConsoleKey.Y;

            }



        }
    }
}
