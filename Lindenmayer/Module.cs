using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lindenmayer {

	/// <summary>
	/// At the most basic level a Module is just a character in the string.
	/// This struct can be extended to produce complex modules which can contain
	/// other data.  A simple example:
	/// 
	/// public class MyModule : Module {
	///		public int otherField;
	///		// ...  snip ...
	/// }
	/// 
	/// With a properly implemented ToString method, a module with the signature 'A'
	/// and a value of 4 in otherField would print as: A(4)
	/// 
	/// Note that the ToString method does not need to be implemented in this way,
	/// as it has no affect on functionality, but keeping it uniform helps with
	/// clarity.
	/// 
	/// You should also override the Equals method for your types to make sure they
	/// are being compared correctly
	/// </summary>
	public class Module {
		public char signature;

		public Module(char sig) {
			signature = sig;
		}

		/// <summary>
		/// Prints this module in an easily readable format
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			return signature.ToString();
		}

		/// <summary>
		/// Override to check for equivalence to your own derived types
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) {
			Module other = obj as Module;
			if (other != null)
				return this.signature == other.signature;
			else
				return false;
		}

		public override int GetHashCode() {
			return signature.GetHashCode();
		}
	}
}
