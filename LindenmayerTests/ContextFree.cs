using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lindenmayer;

namespace LindenmayerTests {
	[TestClass]
	public class ContextFree {

		/// <summary>
		/// Test a simple context-free grammar
		/// </summary>
		[TestMethod]
		public void CF1() {
			LSystem LS = new LSystem();

			// Rule 1: A -> AB
			Production P1 = new Production(null, new Module('A'), null);
			P1.successor.Add(new Module('A'));
			P1.successor.Add(new Module('B'));

			// Rule 2: B -> A
			Production P2 = new Production(null, new Module('B'), null);
			P2.successor.Add(new Module('A'));

			LS.addProduction(P1);
			LS.addProduction(P2);

			//
			// Axiom: A
			List<Module> axiom = new List<Module>();
			axiom.Add(new Module('A'));

			LS.setAxiom(axiom);

			Assert.IsTrue(Utility.compareStates(axiom, LS.getState()));

			List<Module> expected = null;

			//
			// First step: AB
			LS.step();
			expected = new List<Module>() {
				new Module('A'),
				new Module('B')
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Second step: ABA
			LS.step();
			expected.Add(new Module('A'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Third step:ABAAB
			LS.step();
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Fourth step: ABAABABA
			LS.step();
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Fifth step: ABAABABAABAAB
			LS.step();
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Sixth step: ABAABABAABAABABAABABA
			//             ABAABABAABAABABAABABAABAABABAABAAB
			LS.step();
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));
			
			//
			// Seventh step: ABAABABAABAABABAABABAABAABABAABAAB
			LS.step();
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			expected.Add(new Module('A'));
			expected.Add(new Module('A'));
			expected.Add(new Module('B'));
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));
		}

		/// <summary>
		/// Testing a context-free grammar with a derived module
		/// Demonstrates delaying growth of a module
		/// </summary>
		[TestMethod]
		public void CF2() {
			LSystem LS = new LSystem();

			// Rule 1: A(0) -> A(1)
			Production P1 = new Production(null, new DerivedModule('A', 0), null);
			P1.successor.Add(new DerivedModule('A', 1));

			// Rule 2: A(1) -> A(0)B
			Production P2 = new Production(null, new DerivedModule('A', 1), null);
			P2.successor.Add(new DerivedModule('A', 0));
			P2.successor.Add(new Module('B'));

			// Rule 3: B -> A(0)
			Production P3 = new Production(null, new Module('B'), null);
			P3.successor.Add(new DerivedModule('A', 0));

			LS.addProduction(P1);
			LS.addProduction(P2);
			LS.addProduction(P3);

			//
			// Axiom: A(0)
			List<Module> axiom = new List<Module>();
			axiom.Add(new DerivedModule('A', 0));

			LS.setAxiom(axiom);

			Assert.IsTrue(Utility.compareStates(axiom, LS.getState()));

			List<Module> expected = null;

			//
			// Step one: A(1)
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 1)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));


			//
			// Step two: A(0)B
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 0),
				new Module('B')
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Step three: A(1)A(0)
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 1),
				new DerivedModule('A', 0)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Step four: A(0)BA(1)
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 0),
				new Module('B'),
				new DerivedModule('A', 1)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Step five: A(1)A(0)A(0)B
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 1),
				new DerivedModule('A', 0),
				new DerivedModule('A', 0),
				new Module('B')
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Step six: A(0)BA(1)A(1)A(0)
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 0),
				new Module('B'),
				new DerivedModule('A', 1),
				new DerivedModule('A', 1),
				new DerivedModule('A', 0)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Step seven: A(1)A(0)A(0)BA(0)BA(1)
			LS.step();
			expected = new List<Module>() {
				new DerivedModule('A', 1),
				new DerivedModule('A', 0),
				new DerivedModule('A', 0),
				new Module('B'),
				new DerivedModule('A', 0),
				new Module('B'),
				new DerivedModule('A', 1)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));
		}
	}
}
