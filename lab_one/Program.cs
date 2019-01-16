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
            return answerVal == this.correctIndex;
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
            return this.answeredCorrect;
        }

    }
    class Test
    {
        private List<Question> questions;
        private int totalCorrect;
        
        public Test(string[] questionFileContent)
        {
            questions = new List<Question>();
            foreach(string line in questionFileContent)
            {
                if (line.Contains("Question,Answer1,Answer2,Answer3,Answer4,CorrectIndex"))
                    continue;
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
        public void Take()
        {
            int questionIndex = 1;

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            foreach (Question currentQuestion in this.questions)
            {
                int leftDist = 1;
                int topDist = 3;
                Console.SetCursorPosition(leftDist, topDist++);
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
                Console.WriteLine("Please enter your answer below");
     
                var result = Console.ReadKey().KeyChar.ToString().ToUpper().ToCharArray(); //Converting char to string to make uppercase then base to char..
                Console.WriteLine(currentQuestion.checkAnswer(result[0] - 64));
                Console.ReadKey();
                questionIndex++;
            }
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
            catch (Exception)
            {
                Console.WriteLine("failed to load quiz", args[0]);
            }
            Console.WriteLine("Hello, welcome to the DotNetCore quiz");
            Console.WriteLine("Click enter to start the quiz!");
            

            
            Console.ReadKey();


            test.Take();

           
            

            Console.ReadKey();


        }
    }
}
