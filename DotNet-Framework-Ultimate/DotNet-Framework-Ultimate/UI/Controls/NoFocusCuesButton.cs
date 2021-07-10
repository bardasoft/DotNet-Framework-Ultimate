﻿using System.ComponentModel;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	/// <summary>
	/// Modified button which has no focus rectangles when the form which contains this button loses focus while the button was focused.
	/// </summary>
	[ToolboxItem(true)]
	public class NoFocusCuesButton : Button {
		protected override bool ShowFocusCues => false;

		/// <summary>
		/// Creates a new instance of a <see cref="NoFocusCueButton"/>
		/// </summary>
		public NoFocusCuesButton() { }

		public override void NotifyDefault(bool value) {
			base.NotifyDefault(false);
		}
	}
}
