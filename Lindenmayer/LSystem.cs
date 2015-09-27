using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayer {
	/// <summary>
	/// Base class for a Lindenmayer System
	/// </summary>
	public class LSystem {
		// Standard L-System Tuple
		private List<Production> P = null; // System productions
		private List<Module> currentState = null; // System state

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
				int stepSize = 1;

				if (i + 1 < currentState.Count)
					right = currentState[i + 1];
				else
					right = null;

				// Check the current symbol against all productions
				foreach (Production p in P) {
					if (p.isMatch(left, m, right)) {
						// Remove the old module
						currentState.RemoveAt(i);

						// Insert the successor modules
						currentState.InsertRange(i, p.successor);
						stepSize = p.successor.Count;
						break;
					}
				}

				left = m;
				i += stepSize;
			}
		}
	}
}
