using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lindenmayer;

namespace LindenmayerTests {
	/// <summary>
	/// A simple parametric module for testing
	/// </summary>
	public class DerivedModule : Module {
		public int param;

		public DerivedModule(char sig, int p)
			: base(sig) {
			param = p;
		}

		public override string ToString() {
			return signature.ToString() + "(" + param + ")";
		}

		public override bool Equals(object obj) {
			DerivedModule other = obj as DerivedModule;
			if (other != null) {
				if (base.Equals(obj))
					if (this.param == other.param)
						return true;
			}

			return false;
		}

		public override int GetHashCode() {
			return signature.GetHashCode() * 7 + param;
		}
	}
}
