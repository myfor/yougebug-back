using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.Detail
{
    class DetailForReport : IGetQuestionDetail
    {
        public Task<Resp> GetDetailAsync(int questionId, int index, int size)
        {

        }
    }
}
