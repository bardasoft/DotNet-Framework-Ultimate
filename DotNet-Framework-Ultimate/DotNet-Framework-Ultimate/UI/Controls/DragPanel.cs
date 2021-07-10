using DotNet.Framework.Ultimate.Native.Constants;

using System;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	public class DragPanel : Panel {
		protected override void WndProc(ref Message m) {
			if (m.Msg == WindowMessage.WM_NCHITTEST) {
				m.Result = new IntPtr(HitTest.HTCAPTION);
				return;
			}

			base.WndProc(ref m);
		}
	}
}