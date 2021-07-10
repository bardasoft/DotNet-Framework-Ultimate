using DotNet.Framework.Ultimate.Native.Constants;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	public class DragPanel : Panel {
		private bool drag = false;
		private Point MouseDownPosition;

		protected override void OnMouseDown(MouseEventArgs e) {
			this.drag = true;
			Form form = this.FindForm();

			this.MouseDownPosition = form is null ? new Point(e.X, e.Y) : form.PointToClient(this.PointToScreen(new Point(e.X, e.Y)));
		}

		protected override void OnMouseMove(MouseEventArgs e) {
			if (!this.drag)
				return;

			Form form = this.FindForm();
			form?.SetDesktopLocation(Control.MousePosition.X - this.MouseDownPosition.X, Control.MousePosition.Y - this.MouseDownPosition.Y);
		}

		protected override void OnMouseUp(MouseEventArgs e) {
			this.drag = false;
		}
	}
}