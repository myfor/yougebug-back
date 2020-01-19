using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Questions
{
    public class Models
    {
        public struct QuestionItem_Admin
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
        }

        public struct QuestionDetail
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Tags { get; set; }
            public int Votes { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }
    }
}
