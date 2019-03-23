using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Transactions;
using Capstone.Web.Models;
using Capstone.Web.DAL;

namespace CapstoneTests.DAL_Tests
{
    [TestClass]
    public class Survey_ResultSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True";

        [TestInitialize]
        public void TestInitialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd;

                cmd = new SqlCommand("INSERT INTO park (parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('JSNP', 'Test Park', 'Ohio', 22222, 134, 500, 2, 'Woodland', 1980, 1, 'Quote', 'Quote Source', 'Description', 25, 5)", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO park (parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('NPNP', 'Test Park', 'Ohio', 22222, 134, 500, 2, 'Woodland', 1980, 1, 'Quote', 'Quote Source', 'Description', 25, 5)", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM survey_result", conn);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('NPNP', 'test5@email.com', 'Ohio', 'active');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('NPNP', 'test6@email.com', 'Ohio', 'active');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('NPNP', 'test@email.com', 'Ohio', 'inactive');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('JSNP', 'test2@email.com', 'Ohio', 'sedentary');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('JSNP', 'test3@email.com', 'Ohio', 'active');", conn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("INSERT INTO survey_result (parkCode, emailAddress, state, activityLevel) VALUES ('JSNP', 'test4@email.com', 'Ohio', 'active');", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            tran.Dispose();
        }

        [TestMethod]
        public void GetAllSurveysTest()
        {
            Survey_ResultSqlDAL dal = new Survey_ResultSqlDAL(connectionString);
            List<Survey_Result> results = dal.GetAllSurveys();

            CollectionAssert.AllItemsAreNotNull(results);
            Assert.IsNotNull(results);
            Assert.AreEqual(6, results.Count);
        }

        [TestMethod]
        public void AddResultTests()
        {
            Survey_ResultSqlDAL dal = new Survey_ResultSqlDAL(connectionString);
            List<Survey_Result> priorResults = dal.GetAllSurveys();

            bool addSuccessful = dal.AddResult(new Survey_Result() { ParkCode = "JSNP", Email = "test4@email.com", State = "Ohio", ActivityLevel = "Very Active" });

            List<Survey_Result> newResults = dal.GetAllSurveys();

            Assert.AreEqual(true, addSuccessful);
            Assert.AreEqual(priorResults.Count + 1, newResults.Count);
        }

        [TestMethod]
        public void GetTopRankedParksTest()
        {
            Survey_ResultSqlDAL dal = new Survey_ResultSqlDAL(connectionString);

            List<SurveyResultViewModel> surveys = dal.GetTopRankedParks();

            bool result = false;

            if (surveys[0].ParkCode == "JSNP")
            {
                result = true;
            }


            Assert.IsTrue(result);
        }
    }
}
