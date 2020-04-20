using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yougebug_back.ViewModels.Users
{
    public class UserAnswers
    {
        public Shared.UserIntro UserIntro { get; set; }
        public Domain.Paginator Paginator { get; set; }
    }
}
