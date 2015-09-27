using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayer {
	public class StochasticProduction : Production {
		private struct OUTCOME {
			public List<Module> successor;
			public double probability;
		};

		// Hide the base successor, since there can now be more than one
		public override List<Module> successor {
			get {
				// Are there any outcomes added?
				if (outcomes == null)
					return emptySuccessor;

				// Roll and choose the result
				double roll = rand.NextDouble();
				double total = 0.0;
				foreach (OUTCOME o in outcomes) {
					total += o.probability;
					if (roll < total)
						return o.successor;
				}
				
				// If nothing was chosen, the probabilities did not sum to 1.0.
				// In this case, return the last outcome
				return outcomes.Last<OUTCOME>().successor;
			}
			set {
				// Empty
			}
		}

		// The possible set of outcomes
		private List<OUTCOME> outcomes;
		private static List<Module> emptySuccessor = new List<Module>();

		private Random rand;

		/// <summary>
		/// Initializes a context-free stochastic production
		/// </summary>
		/// <param name="m"></param>
		public StochasticProduction(Module m)
			: base(m) {
				rand = new Random();
		}

		/// <summary>
		/// Initializes a context-free stochastic production with a given seed
		/// </summary>
		/// <param name="m"></param>
		/// <param name="seed">Seed value</param>
		public StochasticProduction(Module m, int seed)
			: base(m) {
				rand = new Random(seed);
		}

		/// <summary>
		/// Initializes a context-sensitive stochastic production
		/// </summary>
		/// <param name="left"></param>
		/// <param name="m"></param>
		/// <param name="right"></param>
		public StochasticProduction(Module left, Module m, Module right)
			: base(left, m, right) {
				rand = new Random();
		}

		/// <summary>
		/// INitializes a context-sensitive stochastic production with a given seed
		/// </summary>
		/// <param name="left"></param>
		/// <param name="m"></param>
		/// <param name="right"></param>
		/// <param name="seed">Seed value</param>
		public StochasticProduction(Module left, Module m, Module right, int seed)
			: base(left, m, right) {
				rand = new Random(seed);
		}

		/// <summary>
		/// Adds a possible outcome to the system with probability p.
		/// 
		/// Added outcomes do not need to totaly to probability of 1.0.  The last
		/// inserted outcome will automatically fill 1.0 - all other p's
		/// 
		/// This function does not check for sum(p) > 1.0.  If you insert outcomes
		/// with a total probability > 1.0, results will be skewed.
		/// </summary>
		/// <param name="s">The list of successor modules for this
		/// outcome</param>
		/// <param name="p">Probability this outcome will manifest.  
		/// [0.0, 1.0]</param>
		public void addOutcome(List<Module> s, double p) {
			if (outcomes == null)
				outcomes = new List<OUTCOME>();

			outcomes.Add(new OUTCOME() {
				successor = s,
				probability = p
			});
		}
	}
}
