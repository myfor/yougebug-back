using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions
{
    /// <summary>
    /// 提问的举报
    /// </summary>
    public class Reports
    {
        public static ReportQuestion GetReportQuestion(int questionId)
        {
            return new ReportQuestion(questionId);
        }

    }
}
