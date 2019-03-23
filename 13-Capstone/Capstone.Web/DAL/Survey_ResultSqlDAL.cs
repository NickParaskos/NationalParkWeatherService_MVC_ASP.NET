using Capstone.Web.DAL.Interfaces;
using Capstone.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Capstone.Web.DAL
{
    public class Survey_ResultSqlDAL : ISurvey_ResultSqlDAL
    {
        private const string SQL_GetAllSurveys = "SELECT * FROM survey_result JOIN park on park.parkCode = survey_result.parkCode;";
        private const string SQL_AddResult = "INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES (@parkCode, @email, @state, @activityLevel);";
        private const string SQL_GetTopRankedParks = "SELECT COUNT(survey_result.parkCode) as numVotes, park.parkName, survey_result.parkCode FROM survey_result JOIN park ON park.parkCode = survey_result.parkCode GROUP BY parkName, survey_result.parkCode ORDER BY numVotes DESC, parkName ASC, parkCode";

        private string connectionString;

        public Survey_ResultSqlDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Survey_Result> GetAllSurveys()
        {
            List<Survey_Result> output = new List<Survey_Result>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetAllSurveys, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Survey_Result sr = new Survey_Result();

                    sr.Id = Convert.ToInt32(reader["surveyId"]);
                    sr.ParkCode = Convert.ToString(reader["parkCode"]);
                    sr.Email = Convert.ToString(reader["emailAddress"]);
                    sr.State = Convert.ToString(reader["state"]);
                    sr.ActivityLevel = Convert.ToString(reader["activityLevel"]);

                    output.Add(sr);
                }
            }

            return output;
        }

        public bool AddResult(Survey_Result result)
        {
            bool output;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_AddResult, conn);
                    cmd.Parameters.AddWithValue("@parkCode", result.ParkCode);
                    cmd.Parameters.AddWithValue("@email", result.Email);
                    cmd.Parameters.AddWithValue("@state", result.State);
                    cmd.Parameters.AddWithValue("@activityLevel", result.ActivityLevel);

                    cmd.ExecuteNonQuery();
                }
                output = true;
            }
            catch
            {
                output = false;
            }

            return output;
        }

        public List<SurveyResultViewModel> GetTopRankedParks()
        {
            List<SurveyResultViewModel> output = new List<SurveyResultViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SQL_GetTopRankedParks, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    SurveyResultViewModel srvm = new SurveyResultViewModel();

                    srvm.ParkCode = Convert.ToString(reader["parkCode"]);
                    srvm.ParkName = Convert.ToString(reader["parkName"]);
                    srvm.VoteCount = Convert.ToInt32(reader["numVotes"]);


                    output.Add(srvm);
                }
            }

            return output;
        }
    }
}
