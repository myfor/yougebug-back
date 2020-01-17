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
    }
}
