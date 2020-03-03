using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yougebug_back.ViewModels.Questions
{
    public class SearchModel
    {
        public string Search { get; set; }
        public Domain.Paginator Page { get; set; }
    }
}
