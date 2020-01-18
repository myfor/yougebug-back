using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Clients
{
    public class Models
    {
        public struct UserIntro
        {
            public int Id { get; set; }
            public string Account { get; set; }
            public string Avatar { get; set; }
        }
    }
}
