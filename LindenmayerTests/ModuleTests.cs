using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Lindenmayer;

namespace LindenmayerTests {
	[TestClass]
	public class ModuleTests {

		[TestMethod]
		public void ModuleEquals() {
			Module m1 = new Module('A');
			Module m2 = new Module('A');
			Module m3 = new Module('B');

			Assert.AreEqual(m2, m1);
			Assert.AreNotEqual(m3, m1);
		}
	}
}
