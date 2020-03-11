using System;

namespace Domain.Administrators
{
    public class Results
    {
        public struct LoggedInInfo
        {
            [System.Text.Json.Serialization.JsonIgnore]
            public int Id { get; set; }
            [System.Text.Json.Serialization.JsonIgnore]
            public Guid Token { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }
    }
}
