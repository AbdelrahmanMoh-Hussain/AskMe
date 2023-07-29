using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AskMe
{
    internal class User
    {
        //Genrate new id for each user
        private static int idSequance = 1;

        private string id;
        private string name;
        private string password;
        private string email;
        private string userName;
        private bool allowAnonymousQuestions;
        public LinkedList<Question> Questions = new LinkedList<Question>();

        public User()
        {
            RetriveQuestionsData();
        }

        public User(string id, string name, string password, string email, string userName, bool allowAnonymousQuestions)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.email = email;
            this.userName = userName;
            this.allowAnonymousQuestions = allowAnonymousQuestions;
        }

        /// <summary>
        /// Get all the information from question.txt file
        /// </summary>
        private void RetriveQuestionsData()
        {
            var path = "F:\\C#\\AskMe\\AskMe\\data\\questions.txt";
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (line.Length <= 0) continue;
                var contents = line.Split(',');
                Question q = new Question(contents[2], contents[3], contents[5], contents[6], contents[4] == "1" ? true : false);
                q.Id = contents[0];
                this.Questions.AddLast(q);
                if (contents[1] != "-1")
                {
                    foreach (var question in Questions)
                    {
                        if (question.Id == contents[1])
                        {
                            question.Threads.Add(q);
                        }
                    }
                }
            }
        }

        public void LogIn()
        {
            //Get login info
            Console.Write("Enter User name: ");
            var userName = Console.ReadLine();
            Console.Write("Enter Password: ");
            var password = Console.ReadLine();

            //Open the user.txt file & check this user exist
            var path = "F:\\C#\\AskMe\\AskMe\\data\\users.txt";
            var lines = File.ReadAllLines(path);
            bool LoginInFlag = false;
            foreach (var line in lines)
            {
                var contents = line.Split(',');
                if (contents[2] == password && contents[3] == userName)
                {
                    LoginInFlag = true;
                    Console.WriteLine("Login successfully");
                    Console.WriteLine($"Welcome {contents[1]}");
                   this.id = contents[0];
                   this.name = contents[1];
                   this.password = contents[2];
                   this.userName = contents[3];
                   this.email = contents[4];
                   this.allowAnonymousQuestions = contents[5] == "yes" ? true : false;
                    break;
                }
            }
            if (!LoginInFlag) //Login faild
            {
                Console.WriteLine("Wrong user name or password");
                Console.WriteLine("to login press 1, to sign up press 2");
                var key = Console.ReadLine();
                if(key == "1")
                {
                    LogIn();
                }
                else
                {
                    SignUp();
                }
            }
        }
        public void SignUp()
        {
            //get new user infor
            Console.Write("Enter user name ");
            var userName = Console.ReadLine();
            Console.Write("Enter password ");
            var password = Console.ReadLine();
            Console.Write("Enter name ");
            var name = Console.ReadLine();
            Console.Write("Enter email ");
            var email = Console.ReadLine();
            Console.Write("Do you allow Anonymous Question ");
            var AQ = Console.ReadLine().ToLower() == "yes" ? true: false;

            
            this.id = Convert.ToString(idSequance++);
            this.name = name;
            this.password = password;
            this.userName = userName;
            this.email = email;
            this.allowAnonymousQuestions = AQ;

            //save the new user in the file
            var path = "F:\\C#\\AskMe\\AskMe\\data\\users.txt";
            var line = $"{this.id},{this.name},{this.password},{this.userName},{this.email},{this.allowAnonymousQuestions}\n";
            File.AppendAllText(path, line);
        }
        
        /// <summary>
        /// Print all the question related to the user
        /// </summary>
        public void PrintUserFeed()
        {
            Console.WriteLine("*** User Feed ***");
            foreach (var q in Questions)
            {
                if(q.ToUserId == this.id || q.FromUserId == this.id)
                {
                    Console.WriteLine(q);
                }
            }
            Console.WriteLine("-------------------------------------");
        }
        public void PrintQuestionsToMe()
        {
            Console.WriteLine("*** Questions sent to me ***");
            foreach (var question in Questions)
            {

                if (question.ToUserId == this.id)
                {
                    if (question.Anonymous)
                    {
                        Console.Write($"Question Id({question.Id})");
                    }
                    else
                    {
                        Console.Write($"Question Id({question.Id}) from User id({question.FromUserId})");
                    }
                    Console.Write($"\tQuestion: {question.QuestionText}");
                    if (question.AnswerText.Length > 0)
                    {
                        Console.Write($"\n\t\tAnswer: {question.AnswerText}");
                    }
                    Console.WriteLine();

                    //Print threads for this question
                    foreach (var thread in question.Threads)
                    {
                        Console.Write($"\n\tThread: ");
                        if (question.Anonymous)
                        {
                            Console.Write($"Question Id({thread.Id})");
                        }
                        else
                        {
                            Console.Write($"Question Id({thread.Id}) from User id({thread.FromUserId})");
                        }
                        Console.WriteLine($"\tQuestion: {thread.QuestionText}");
                        if (question.AnswerText.Length > 0)
                        {
                            Console.Write($"\n\t\tAnswer: {thread.AnswerText}");
                        }
                        Console.WriteLine();
                    }
                }
            }
            Console.WriteLine("-------------------------------------");

        }
        public void PrintQuestionsFromMe()
        {
            Console.WriteLine("*** My Question ***");
            foreach (var question in Questions)
            {
                if(question.FromUserId == this.id)
                {
                    Console.Write($"Question Id({question.Id}) {(question.Anonymous ? "AQ" : "!AQ")} to User id({question.ToUserId})");
                    Console.Write($"\tQuestion: {question.QuestionText}");
                    Console.Write($"\n\t\tAnswer: {(question.AnswerText.Length > 0 ? question.AnswerText:"Not Ansered YET!")}\n");
                }
            }
            Console.WriteLine("-------------------------------------");
        }
        public void AnswerQuestion()
        {
            Console.WriteLine("*** Answer a question ***");
            Console.Write("Enter Question id or -1 to cancel: ");
            string questionId = Console.ReadLine();
            if (questionId == "-1") return;
            Console.Write("Enter answer ");
            string answerText = Console.ReadLine();

            //Update the question.txt file with the answer
            var path = "F:\\C#\\AskMe\\AskMe\\data\\questions.txt";
            var lines = File.ReadAllLines(path);
            bool foundQuestion = false;
            for (int i = 0; i < lines.Length; i++)
            {
                var contents = lines[i].Split(',');
                if (contents[0] == questionId && contents[3] == this.id)
                {
                    foundQuestion = true;
                    if (contents[6].Length > 0)
                    {
                        Console.WriteLine("Already Answered, Answer will be updated");
                    }
                    lines[i] = $"{contents[0]},{contents[1]},{contents[2]},{contents[3]},{contents[4]},{contents[5]},{answerText}";
                }
            }
            if (!foundQuestion)
            {
                Console.WriteLine("Can't find this question, please make sure of that this qustion sent to you");
                AnswerQuestion();
            }
            File.WriteAllLines(path, lines);
            

            foreach (var question in Questions)
            {
                if (question.Id == questionId && question.ToUserId == this.id)
                {
                    question.AnswerText = answerText;
                }
            }
        }
        public void DeleteQuestion()
        {
            Console.WriteLine("*** Delete a question ***");
            Console.Write("Enter Question id or -1 to cancel: ");
            string questionId = Console.ReadLine();
            if (questionId == "-1") return;

            //Delete the question from question.txt file
            var path = "F:\\C#\\AskMe\\AskMe\\data\\questions.txt";
            var lines = File.ReadAllLines(path);
            var index = 0;
            string[] newLines = new string[lines.Length - 1];
            bool foundQuestion = false;
            for (int i = 0; i < lines.Length; i++)
            {
                var contents = lines[i].Split(',');
                if (contents[0] != questionId && contents[2] == this.id)
                {
                    foundQuestion = true;
                    newLines[index] += lines[i];
                    index++;
                }
            }
            if (!foundQuestion)
            {
                Console.WriteLine("Can't find this question, please make sure of that this qustion id");
                DeleteQuestion();
            }
            File.WriteAllLines(path, newLines);

            Question questionToRemove = null;
            foreach (var question in Questions)
            {
                if (question.Id == questionId && question.FromUserId == this.id)
                {
                    questionToRemove = question;
                }
            }
            Questions.Remove(questionToRemove);
        }
        public void AskQuestion()
        {
            Console.WriteLine("*** Ask a question ***");
            Console.Write("Enter User id or -1 to cancel: ");
            var userId = Console.ReadLine();
            Console.Write("For thread question: Enter Question id or -1 for new Question: ");
            var questionId = Console.ReadLine();
            Console.Write("Enter question text: ");
            var questionTxt = Console.ReadLine();

            Question newQuestion = new Question(this.id, userId, questionTxt, null);
            var path = "F:\\C#\\AskMe\\AskMe\\data\\questions.txt";
            string line = null;
            if (questionId != "-1") //Thread question
            {
                bool foundThreadQuestion = false;
                foreach (var question in Questions)
                {
                    if (question.Id == questionId && question.ToUserId == userId)
                    {
                        foundThreadQuestion = true;
                        question.Threads.Add(newQuestion);
                    }
                }
                if (!foundThreadQuestion)
                {
                    Console.WriteLine("Can't find this thread question");
                    questionId = "-1";
                }
                line = $"{newQuestion.Id},{questionId},{this.id},{userId},0,{questionTxt},\n";
            }
            else //New question
            {
                Questions.AddLast(newQuestion);
                line = $"{newQuestion.Id},-1,{this.id},{userId},0,{questionTxt},\n";
            }
            File.AppendAllText(path, line);
        }
        public void PrintUsers()
        {
            //Read users info from user.txt file
            var path = "F:\\C#\\AskMe\\AskMe\\data\\users.txt";
            var lines = File.ReadAllLines(path);
            Console.WriteLine("*** List of the users: ***");
            foreach (var line in lines)
            {
                var contents = line.Split(',');
                Console.WriteLine($"ID: {contents[0]}\tName: {contents[1]}");
            }
        }

        public override string ToString()
        {
            return $"{this.id}," +
                   $"{this.name}," +
                   $"{this.password}," +
                   $"{this.userName}," +
                   $"{this.email}," +
                   $"{this.allowAnonymousQuestions}\n";
        }
    }
}
