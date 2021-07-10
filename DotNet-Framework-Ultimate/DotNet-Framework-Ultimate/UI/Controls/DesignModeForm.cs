using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	/// <summary>
	/// Form that has public information about if it is currently in design mode, so running inside a WinForms editor i.e. Visual Studio.
	/// </summary>
	public class DesignModeForm : Form, IDesignModeControl {
		/// <summary>
		/// True this form is currently in design mode, so running inside a WinForms editor i.e. Visual Studio.
		/// </summary>
		public bool IsInDesginMode => this.DesignMode;
	}
}