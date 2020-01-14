using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Administrators
{
    public class Results
    {
        public struct LoggedInInfo
        {
            public int Id { get; set; }
            public Guid Token { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }
    }
}
