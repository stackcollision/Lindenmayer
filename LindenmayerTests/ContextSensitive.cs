using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lindenmayer;

namespace LindenmayerTests {

	/*
	 * 
	 * Notes on notation for context sensitive L-Systems
	 * 
	 * The following notation is used in these tests:
	 * 
	 * LeftContext < CurrentModule > RightContext : Condition -> successor
	 * 
	 * The simplest possible rule is
	 * CurrentModule -> successor
	 * However this is not context sensitive.
	 * 
	 * A more relevant example:
	 * A > B -> C
	 * This translates to "If a module A is followed by a B, replace the A with a C
	 * 
	 * Another example:
	 * A(param) > B : param == 0 -> C
	 * If a parametric module A is followed by a B, and A's 'param' value 
	 * is equal to 0 replace the A with a C.
	 * 
	 */

	[TestClass]
	public class ContextSensitive {
		/// <summary>
		/// A simple context sensitive system with the base Module
		/// </summary>
		[TestMethod]
		public void CS1() {
			LSystem LS = new LSystem();

			// Rule 1: D -> B
			Production P1 = new Production(null, new Module('D'), null);
			P1.successor.Add(new Module('B'));

			// Rule 2: A > B -> DC
			Production P2 = new Production(null, new Module('A'), new Module('B'));
			P2.successor.Add(new Module('D'));
			P2.successor.Add(new Module('C'));

			// Rule 3: C < B -> A
			Production P3 = new Production(new Module('C'), new Module('B'), null);
			P3.successor.Add(new Module('A'));

			LS.addProduction(P1);
			LS.addProduction(P2);
			LS.addProduction(P3);

			//
			// Axiom: AD
			List<Module> axiom = new List<Module>();
			axiom.Add(new Module('A'));
			axiom.Add(new Module('D'));

			LS.setAxiom(axiom);

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
			// Second step: DCB
			LS.step();
			expected = new List<Module>() {
				new Module('D'),
				new Module('C'),
				new Module('B')
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Third step: BCA
			LS.step();
			expected = new List<Module>() {
				new Module('B'),
				new Module('C'),
				new Module('A')
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			//
			// Fourth step: BCA
			LS.step();
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

		}
	}
}
