using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lindenmayer;

namespace LindenmayerTests {
	[TestClass]
	public class StochasticTests {
		/// <summary>
		/// Checks that the last production returns the last outcome the correct
		/// number of times
		/// </summary>
		[TestMethod]
		public void StochLessThan1() {
			List<Module> outcome1 = new List<Module>();
			List<Module> outcome2 = new List<Module>();

			StochasticProduction P = new StochasticProduction(new Module('A'));

			P.addOutcome(outcome1, 0.5);
			P.addOutcome(outcome2, 0.25);

			long o1 = 0;
			long o2 = 0;

			for (long i = 0; i < 10000000; ++i) {
				List<Module> result = P.successor;
				if(result == outcome1)
					o1++;
				else if(result == outcome2)
					o2++;
				else
					Assert.IsTrue(false);
			}

			double p1 = o1 / 10000000.0;
			double p2 = o2 / 10000000.0;

			Assert.IsTrue(p1 > 0.48 && p1 < 0.52);
			Assert.IsTrue(p2 > 0.48 && p2 < 0.52);
		}

		[TestMethod]
		public void StochGreaterThan1() {
			List<Module> outcome1 = new List<Module>();
			List<Module> outcome2 = new List<Module>();

			StochasticProduction P = new StochasticProduction(new Module('A'));

			P.addOutcome(outcome1, 0.5);
			P.addOutcome(outcome2, 0.75);

			long o1 = 0;
			long o2 = 0;

			for (long i = 0; i < 10000000; ++i) {
				List<Module> result = P.successor;
				if (result == outcome1)
					o1++;
				else if (result == outcome2)
					o2++;
				else
					Assert.IsTrue(false);
			}

			double p1 = o1 / 10000000.0;
			double p2 = o2 / 10000000.0;

			Assert.IsTrue(p1 > 0.48 && p1 < 0.52);
			Assert.IsTrue(p2 > 0.48 && p2 < 0.52);
		}
	}
}
