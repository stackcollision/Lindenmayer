using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayer {
	public class Production {
		private readonly Module leftContext;
		private readonly Module match;
		private readonly Module rightContext;
		
		/// <summary>
		/// The set of modules which will replace the matched module (not the
		/// contexts).  An empty successor means the module will be deleted.
		/// </summary>
		public List<Module> successor { get; set; }

		/// <summary>
		/// Initializes the production.  The left, match, and right modules must
		/// be given during construction.  Left and Right can be null, but a Match
		/// must be given.
		/// </summary>
		/// <param name="left">Optional left context</param>
		/// <param name="m">Module to match with</param>
		/// <param name="right">Optional right context</param>
		public Production(Module left, Module m, Module right) {
			if (m == null)
				throw new System.ArgumentNullException("m", 
					"A match module must be given");

			leftContext = left;
			match = m;
			rightContext = right;

			successor = new List<Module>();
		}

		/// <summary>
		/// Tests if the 
		/// </summary>
		/// <param name="left">The module to the left of the current one</param>
		/// <param name="m">The current module</param>
		/// <param name="right">The module to the right of the current one</param>
		/// <returns></returns>
		public bool isMatch(Module left, Module m, Module right) {
			return
				(leftContext == null || leftContext.Equals(left)) &&
				(rightContext == null || rightContext.Equals(right)) &&
				match.Equals(m);
		}
	}
}
