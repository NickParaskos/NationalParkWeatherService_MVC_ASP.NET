using Capstone.Web.DAL;
using Capstone.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Transactions;

namespace CapstoneTests.DAL_Tests
{
    [TestClass]
    public class ParkSqlDALTests
    {
        private TransactionScope tran;
        private string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=NPGeek;Integrated Security=True";

        [TestInitialize]
        public void TestInitialize()
        {
            tran = new TransactionScope();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd;
                conn.Open();

                cmd = new SqlCommand("INSERT INTO park (parkCode, parkName, state, acreage, elevationInFeet, milesOfTrail, numberOfCampsites, climate, yearFounded, annualVisitorCount, inspirationalQuote, inspirationalQuoteSource, parkDescription, entryFee, numberOfAnimalSpecies) VALUES ('AAA', 'Test Park National Park', 'Ohio', 100, 1, 10, 0, 'Woodland', 2019, 7, 'To say something inspirational you must be inspired.', 'Nick Paraskos', 'Park Description', 0, 500)", conn);
                cmd.ExecuteNonQuery();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            tran.Dispose();
        }

        [TestMethod()]
        public void GetAllParksTest()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);

            List<Park> parks = parkSqlDAL.GetAllParks();

            bool found = false;
            foreach (Park park in parks)
            {
                if (park.Code == "AAA")
                    found = true;
            }

            Assert.AreEqual(true, found, "Park AAA not found in test");
            Assert.IsNotNull(parks);
        }

        [TestMethod()]
        public void GetParkTest()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);

            Park park = parkSqlDAL.GetPark("AAA");

            Assert.AreEqual("Test Park National Park", park.Name);
            Assert.IsNotNull(park);
        }

        [TestMethod()]
        public void GetParkSelectList()
        {
            ParkSqlDAL parkSqlDAL = new ParkSqlDAL(connectionString);

            List<SelectListItem> selectListItems = parkSqlDAL.GetParkSelectList();
            SelectListItem newParkSelectListItem = new SelectListItem() { Text = "Test Park National Park", Value = "AAA" };
            int index = -1;
            for(int i= 0; i < selectListItems.Count; i++)
            {
                if(selectListItems[i].Text == newParkSelectListItem.Text && selectListItems[i].Value == newParkSelectListItem.Value)
                {
                    index = i;
                }
            }

            Assert.IsNotNull(selectListItems);
            Assert.AreEqual(newParkSelectListItem.Text, selectListItems[index].Text);
            Assert.AreEqual(newParkSelectListItem.Value, selectListItems[index].Value);
        }
    }
}
