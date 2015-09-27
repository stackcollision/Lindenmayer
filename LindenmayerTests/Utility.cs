using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lindenmayer;

namespace LindenmayerTests {
	class Utility {
		public static bool compareStates(List<Module> expected, IList<Module> actual) {
			if (expected.Count != actual.Count) {
				Console.WriteLine("compareStates: States have different numbers of elements");
				return false;
			}

			for (int i = 0; i < expected.Count; ++i) {
				if (!expected[i].Equals(actual[i])) {
					Console.WriteLine("compareStates: States differ on element " + i);
					return false;
				}
			}

			return true;
		}
	}
}
