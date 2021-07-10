using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	public class NoFocusCuesSplitContainer : SplitContainer {
		private bool _painting;

		protected override bool ShowFocusCues => false;

		public override bool Focused {
			get { return _painting ? false : base.Focused; }
		}

		protected override void OnPaint(PaintEventArgs e) {
			_painting = true;

			try {
				base.OnPaint(e);
			} finally {
				_painting = false;
			}
		}
	}
}