using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Lindenmayer;

namespace LindenmayerTests {
	[TestClass]
	public class OtherSystemTests {
		/// <summary>
		/// Tests that the modules created by a successor can be changed
		/// without affecting others
		/// </summary>
		[TestMethod]
		public void ModuleMutability() {
			DerivedModule A10 = new DerivedModule('A', 10);
			Module B = new Module('B');

			// Rule 1: B -> A(10)
			Production P = new Production(B);
			P.successor.Add(A10);

			LSystem LS = new LSystem();
			LS.addProduction(P);

			List<Module> axiom = new List<Module>() {
				B, B, B
			};
			LS.setAxiom(axiom);

			//
			// Step one: A(10)A(10)A(10)
			LS.step();
			List<Module> expected = new List<Module>() {
				new DerivedModule('A', 10),
				new DerivedModule('A', 10),
				new DerivedModule('A', 10)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));

			// Change the original A10 to see if it affects the state
			A10.param = 15;
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));
		}

		/// <summary>
		/// Tests Goal function's ability to change successor
		/// </summary>
		[TestMethod]
		public void Goals() {
			// Rule 1: A -> B(10)
			Production P1 = new Production(new Module('A'));
			P1.successor.Add(new DerivedModule('B', 10));
			P1.SuccessorCallback = GoalsTest;

			LSystem LS = new LSystem();
			LS.addProduction(P1);

			List<Module> axiom = new List<Module>() {
				new Module('A'),
				new Module('A'),
				new Module('A')
			};

			LS.setAxiom(axiom);
			
			LS.step();
			List<Module> expected = new List<Module>() {
				new DerivedModule('B', 21),
				new DerivedModule('B', 22),
				new DerivedModule('B', 23)
			};
			Assert.IsTrue(Utility.compareStates(expected, LS.getState()));
		}

		private int goalState = 0;
		private void GoalsTest
			(IList<Module> state, int pos, List<Module> successor) {
				switch (goalState) {
					case 0:
						(successor[0] as DerivedModule).param = 21;
						break;

					case 1:
						(successor[0] as DerivedModule).param = 22;
						break;

					case 2:
						(successor[0] as DerivedModule).param = 23;
						break;
				}

				goalState++;
		}

		/// <summary>
		/// Tests keeping track of position with the tracking callback
		/// </summary>
		[TestMethod]
		public void Tracking() {
			// Rule 1: B(5) -> B(5)F(10)
			Production P1 = new Production(new DerivedModule('B', 5));
			P1.successor.Add(new DerivedModule('B', 5));
			P1.successor.Add(new DerivedModule('F', 10));

			LSystem LS = new LSystem();
			LS.addProduction(P1);
			LS.TrackCallback = TrackCallback;

			List<Module> axiom = new List<Module>() {
				new DerivedModule('B', 5),
				new DerivedModule('B', 5),
				new DerivedModule('B', 5)
			};
			LS.setAxiom(axiom);

			LS.step();

			Assert.AreEqual(15, trackedPos);
		}

		private int trackedPos = 0;
		private void TrackCallback(Module lastModule) {
			DerivedModule m = lastModule as DerivedModule;
			if (m == null)
				return;

			if (m.signature == 'F')
				trackedPos += m.param;
			else if (m.signature == 'B')
				trackedPos -= m.param;

		}
	}
}
