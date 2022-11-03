using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GES.Clients.Web.Extensions;

namespace GES.Clients.Web.Tests.Extensions
{
    /// <summary>
    /// Summary description for ObjectExtensionsTests
    /// </summary>
    [TestClass]
    public class ObjectExtensionsTests
    {
        public ObjectExtensionsTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void EnsureListNotNullOrAllItemsAreDefault_Should_Initialize_New_Instance_When_It_Null_Or_AllItems_Are_Null()
        {
            // Arrange
            var nullList = new List<long?>() { null, null };

            // Act
            nullList = nullList.EnsureListNotNullOrAllItemsAreDefault();

            // Assert
            Assert.IsNotNull(nullList, "The list has been initialized.");
            Assert.AreEqual(nullList.Count, 0, "The list has been initialized with empty item.");


            // Arrange
            nullList = null;

            // Act
            nullList = nullList.EnsureListNotNullOrAllItemsAreDefault();

            // Assert
            Assert.IsNotNull(nullList, "The list has been initialized.");
            Assert.AreEqual(nullList.Count, 0, "The list has been initialized with empty item.");
        }

        [TestMethod]
        public void EnsureListNotNullOrAllItemsAreDefault_Should_Not_Be_Initialized_When_List_NotNull_Or_Its_Contains_At_Least_One_NonNull_Item()
        {
            // Arrange
            var nonNullList = new List<long?>() { null, 1 };

            // Act
            nonNullList = nonNullList.EnsureListNotNullOrAllItemsAreDefault();

            // Assert
            Assert.IsNotNull(nonNullList, "The list has been not null.");
            Assert.AreEqual(nonNullList.Count, 2, "The list has not been re-initialized.");
            Assert.IsNull(nonNullList[0], "The item should not be re-ordered.");
            Assert.AreEqual(nonNullList[1], 1, "The item should not be re-ordered.");
        }
    }
}
