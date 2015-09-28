using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lindenmayer;

namespace LindenmayerTests {
	[TestClass]
	public class ProductionTests {
		/// <summary>
		/// Test of the default comparisons
		/// </summary>
		[TestMethod]
		public void DefaultProduction() {
			Module A = new Module('A');
			Module B = new Module('B');
			Module C = new Module('C');

			Production P = new Production(A, B, C);

			Assert.IsTrue(P.isMatch(A, B, C));
			Assert.IsFalse(P.isMatch(B, B, C));
			Assert.IsFalse(P.isMatch(A, B, B));
			Assert.IsFalse(P.isMatch(B, C, A));

		}

		/// <summary>
		/// Test a greater-than comparator for a parameterized B module
		/// </summary>
		[TestMethod]
		public void CustomComparators() {
			Module A = new Module('A');
			DerivedModule B9 = new DerivedModule('B', 9);
			DerivedModule B10 = new DerivedModule('B', 10);
			DerivedModule B11 = new DerivedModule('B', 11);
			Module C = new Module('C');

			Production P = new Production(A, B10, C);
			P.matchCompare = GreaterThan;

			Assert.IsFalse(P.isMatch(A, B9, C));
			Assert.IsTrue(P.isMatch(A, B11, C));

			P = new Production(B10, A, C);
			P.leftCompare = GreaterThan;

			Assert.IsFalse(P.isMatch(B9, A, C));
			Assert.IsTrue(P.isMatch(B11, A, C));

			P = new Production(A, C, B10);
			P.rightCompare = GreaterThan;

			Assert.IsFalse(P.isMatch(A, C, B9));
			Assert.IsTrue(P.isMatch(A, C, B11));
		}

		public bool GreaterThan(Module expected, Module actual) {
			DerivedModule dExp = expected as DerivedModule;
			DerivedModule dAct = actual as DerivedModule;

			if (dExp == null || dAct == null)
				return false;

			return dAct.signature == dExp.signature && dAct.param > dExp.param;
		}
	}
}
