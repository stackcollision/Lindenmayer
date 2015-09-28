using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayer {
	/// <summary>
	/// A Lindenmayer string re-writing system.  This class performs the actual
	/// incrementing of the system with the help of its support classes (Module,
	/// Production).
	/// 
	/// Every time step is called every module in the current state is iterated over
	/// and a relevant production is searched for.  If one matches, that rule is
	/// applied to the module.
	/// 
	/// Two optional callbacks can be provided: Goals and Constraints.  If they are
	/// not provided, their slot in the flowchart is skipped.
	/// 
	/// Step:
	/// Ideal Successor -> Goals Function -> Constraints Function
	/// 
	/// After each module is processed, it is sent to an optional user TrackCallback
	/// which can be used to dynamically track the turtle's position, instead of
	/// calculating it every time from the full state.
	/// </summary>
	public class LSystem {
		/// <summary>
		/// User-defined function which can implement goals for the system.  Called
		/// immediately after the production of the ideal successor.  Cannot modify
		/// the state of the system, but can make changes to the list of successor
		/// modules before they are inserted.
		/// </summary>
		/// <param name="state">The read-only state of the system before the
		/// current module has been removed</param>
		/// <param name="pos">The index of the current module which is about to
		/// be replaced</param>
		/// <param name="idealSuccessor">The module(s) which will replace the
		/// current module</param>
		public delegate void GoalsFunction(
			IList<Module> state, int pos, List<Module> idealSuccessor);

		/// <summary>
		/// User-defined function which can implement constraints for the system.
		/// Called immediately after the goals function (if there is none provided,
		/// this is called after the ideal successor is generated).  Almost identical
		/// to the GoalsFunction, except for its calling order.
		/// </summary>
		/// <param name="state"></param>
		/// <param name="pos"></param>
		/// <param name="successor"></param>
		public delegate void ConstraintsFunction(
			IList<Module> state, int pos, List<Module> successor);

		/// <summary>
		/// Optional callback to allow the user to dynamically track the position
		/// instead of having to recalculate it every time from the full state.
		/// Not really necessary unless you are using Goals and/or Constraints.
		/// </summary>
		/// <param name="lastModule"></param>
		public delegate void TrackFunction(Module lastModule);

		// Standard L-System Tuple
		private List<Production> P = null; // System productions
		private List<Module> currentState = null; // System state
		
		// User-provided callbacks
		public GoalsFunction GoalsCallback = null;
		public ConstraintsFunction ConstraintsCallback = null;
		public TrackFunction TrackCallback = null;

		public LSystem() {
			P = new List<Production>();
		}

		/// <summary>
		/// Adds a production to this system
		/// </summary>
		/// <param name="p">Production to add</param>
		public void addProduction(Production p) {
			// TODO: Should duplicate insertion be allowed?
			P.Add(p);
		}

		/// <summary>
		/// Sets the initial state of the system
		/// </summary>
		/// <param name="omega"></param>
		public void setAxiom(List<Module> omega) {
			currentState = new List<Module>(omega);
		}

		/// <summary>
		/// Returns a read-only image of the state
		/// </summary>
		/// <returns></returns>
		public IList<Module> getState() {
			return currentState.AsReadOnly();
		}

		/// <summary>
		/// Converts the current state to a string
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			String str = "";
			foreach (Module m in currentState) {
				str += m.ToString();
			}

			return str;
		}

		/// <summary>
		/// Steps the system by one iteration
		/// </summary>
		public void step() {
			if(currentState.Count == 0)
				return;
			
			// Iterate over the current state
			int i = 0;
			Module m = null;
			Module left = null;
			Module right = null;

			if (currentState.Count > 1)
				right = currentState[1];

			while (i < currentState.Count) {
				m = currentState[i];
				int skip = 1;

				if (i + 1 < currentState.Count)
					right = currentState[i + 1];
				else
					right = null;

				// Check the current symbol against all productions
				foreach (Production p in P) {
					if (p.isMatch(left, m, right)) {
						// Get the ideal successor
						List<Module> successor = copySuccessor(p.successor);
						IList<Module> roState = currentState.AsReadOnly();
						
						// Check goals
						if(GoalsCallback != null)
							GoalsCallback(roState, i, successor);

						if(ConstraintsCallback != null)
							ConstraintsCallback(roState, i, successor);

						// Remove the old module
						currentState.RemoveAt(i);

						// Insert the successor module(s)
						currentState.InsertRange(i, successor);
						skip = successor.Count;

						break;
					}
				}

				// Call the track function for each module we inserted
				if (TrackCallback != null) {
					for (int j = i; j < i + skip; ++j) {
						TrackCallback(currentState[j]);
					}
				}

				left = m;
				i += skip;
			}
		}

		private List<Module> copySuccessor(List<Module> successor) {
			List<Module> copy = new List<Module>();
			foreach (Module m in successor)
				copy.Add(m.Clone() as Module);
			return copy;
		}
	}
}
