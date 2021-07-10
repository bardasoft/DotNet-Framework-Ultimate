namespace DotNet.Framework.Ultimate.UI.Controls {
	/// <summary>
	/// Interface for any control that should have public information about if it is currently in design mode, so running inside a WinForms editor i.e. Visual Studio.
	/// </summary>
	public interface IDesignModeControl {
		/// <summary>
		/// True this control is currently in design mode, so running inside a WinForms editor i.e. Visual Studio.
		/// </summary>
		bool IsInDesginMode { get; }
	}
}