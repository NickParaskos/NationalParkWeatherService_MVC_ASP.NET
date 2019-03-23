using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAL.Interfaces
{
    public interface ISurvey_ResultSqlDAL
    {
        List<Survey_Result> GetAllSurveys(); 
        bool AddResult(Survey_Result result);
        List<SurveyResultViewModel> GetTopRankedParks();
    }
}
