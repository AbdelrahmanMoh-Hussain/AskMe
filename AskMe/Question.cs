using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskMe
{
    internal class Question
    {
        //Genrate new id for each question
        private static int idSequance = 1;

        public Question(string fromUserId, string toUserId, string questionText, string? answerText, bool anonymous = false)
        {
            Id = Convert.ToString(idSequance++);
            FromUserId = fromUserId;
            ToUserId = toUserId;
            Anonymous = anonymous;
            QuestionText = questionText;
            AnswerText = answerText ?? "";
            Threads = new List<Question>();
        }

        public string Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public bool Anonymous { get; set; }
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public List<Question> Threads { get; set; }

        public override string ToString()
        {
            var str = "";
            if (this.Anonymous)
            {
                str +=$"Question Id({this.Id})";
            }
            else
            {
                str +=$"Question Id({this.Id}) from User id({this.FromUserId})";
            }
            str += $" to User id({this.ToUserId})";
            str +=$"\tQuestion: {this.QuestionText}";
            if (this.AnswerText.Length > 0)
            {
                str +=$"\n\tAnswer: {this.AnswerText}";
            }

            foreach (var thread in this.Threads)
            {
                str +=$"\n\tThread: ";
                if (thread.Anonymous)
                {
                    str +=$"Question Id({thread.Id})";
                }
                else
                {
                    str +=$"Question Id({thread.Id}) from User id({thread.FromUserId})";
                }
                str += $" to User id({thread.ToUserId})";
                str += $"\tQuestion: {thread.QuestionText}";
                if (thread.AnswerText.Length > 0)
                {
                    str +=$"\n\tAnswer: {thread.AnswerText}";
                }
            }
            return str;
        }

    }
}
