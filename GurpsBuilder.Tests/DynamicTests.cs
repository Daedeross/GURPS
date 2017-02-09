using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GurpsBuilder.DataModels;

namespace GurpsBuilder.Tests
{
    [TestClass]
    public class DynamicTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            dynamic c = new Character();
            c.Age = new BaseTrait();
            c.Age.score = new ValueTag<int>(c.Age);
            c.Age.score = 3;

        }
    }
}
