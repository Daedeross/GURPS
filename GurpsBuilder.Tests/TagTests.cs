using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GurpsBuilder.DataModels;
using System.Dynamic;

namespace GurpsBuilder.Tests
{
    [TestClass]
    public class TagTests
    {
        [TestMethod]
        public void TestLiteralValues()
        {
            Character c = new Character();
            dynamic bt = new BaseTrait(c);
            ITag ti = new ValueTag<int>(bt);
            ti.Text = "11";
            bt.TestInt = ti;

            ITag td = new ValueTag<double>(bt);
            td.Text = "12";
            bt.TestDouble = td;

            ITag tb = new ValueTag<bool>(bt);
            tb.Text = "true";
            bt.TestBool = tb;

            ITag ts = new ValueTag<string>(bt);
            ts.Text = "\"testing 1 2 3\"";
            bt.TestString = ts;

            Assert.AreEqual(11, ((IValueTag<int>)ti).FinalValue);
            Assert.AreEqual(12d, ((IValueTag<double>)td).FinalValue);
            Assert.AreEqual(true, ((IValueTag<bool>)tb).FinalValue);
            Assert.AreEqual("testing 1 2 3", ((IValueTag<string>)ts).FinalValue);
        }

        [TestMethod]
        public void TestRefPlusLiteral()
        {
            Character c = new Character();
            dynamic bt = new BaseTrait(c);
            ITag ti = new ValueTag<int>(bt);
            ti.Text = "11";
            bt.TestInt1 = ti;

            IValueTag<int> result = new ValueTag<int>(bt);
            result.Text = "owner.TestInt1 + 2";
            bt.ResultInt = result;

            Assert.AreEqual(13, result.FinalValue);
        }
    }
}
