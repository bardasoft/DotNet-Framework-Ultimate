using DotNet.Framework.Ultimate.Native.Constants;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	public class WndProcBorderFilter : NativeWindow {
		public static List<Type> TypeBlacklist { get; set; } = new List<Type>();

		private Control parent;
		private Control child;

		public int BorderThinckness { get; set; }
		public bool ResizeBorderLeft { get; set; }
		public bool ResizeBorderRight { get; set; }
		public bool ResizeBorderTop { get; set; }
		public bool ResizeBorderBottom { get; set; }

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderWndProcFilter"/>. Sets all borders to true.
		/// </summary>
		/// <param name="parent">The resizable control which should have a non blocked resize border.</param>
		/// <param name="child">The child control the filter should be applied to for not blocking the resize border of the parent control.</param>
		/// <param name="borderThinckness">The resize border thickness of the parent.</param>
		public WndProcBorderFilter(Control parent, Control child, int borderThinckness) {
			this.parent = parent;
			this.child = child;

			try {
				if (!WndProcBorderFilter.TypeBlacklist.Contains(child.GetType()))
					this.AssignHandle(child.Handle);
			} catch (Exception) { }

			this.BorderThinckness = borderThinckness;

			this.ResizeBorderLeft = true;
			this.ResizeBorderRight = true;
			this.ResizeBorderTop = true;
			this.ResizeBorderBottom = true;
		}

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderWndProcFilter"/>. Sets all borders to true.
		/// </summary>
		/// <param name="parent">The resizable control which should have a non blocked resize border.</param>
		/// <param name="child">The child control the filter should be applied to for not blocking the resize border of the parent control.</param>
		/// <param name="borderThinckness">The resize border thickness of the parent.</param>
		/// <param name="left">If true the parent has a left-side resize border which should not be blocked.</param>
		/// <param name="right">If true the parent has a right-side resize border which should not be blocked.</param>
		/// <param name="top">If true the parent has a top-side resize border which should not be blocked.</param>
		/// <param name="bottom">If true the parent has a bottom-side resize border which should not be blocked.</param>
		public WndProcBorderFilter(Control parent, Control child, int borderThinckness, bool left, bool right, bool top, bool bottom) {
			this.parent = parent;
			this.child = child;
			this.AssignHandle(child.Handle);
			this.BorderThinckness = borderThinckness;

			this.ResizeBorderLeft = left;
			this.ResizeBorderRight = right;
			this.ResizeBorderTop = top;
			this.ResizeBorderBottom = bottom;
		}

		protected override void WndProc(ref Message m) {
			Form form = this.child?.FindForm();

			if (!(form is null) && form is IDesignModeControl && (form as IDesignModeControl).IsInDesginMode) {
				base.WndProc(ref m);
				return;
			}

			if (m.Msg != WindowMessage.WM_NCHITTEST) {
				base.WndProc(ref m);
				return;
			}

			if (this.parent is null) {
				base.WndProc(ref m);
				return;
			}

			Point pos = new Point(m.LParam.ToInt32());
			Point parentGlobalPos = this.parent is Form ? this.parent.Location : this.parent.PointToScreen(this.parent.Location);

			// if on the left
			if (pos.X <= parentGlobalPos.X + this.BorderThinckness && this.ResizeBorderLeft) {
				m.Result = new IntPtr(HitTest.HTTRANSPARENT);
				return;
			}

			// if on top
			if (pos.Y <= parentGlobalPos.Y + this.BorderThinckness && this.ResizeBorderTop) {
				m.Result = new IntPtr(HitTest.HTTRANSPARENT);
				return;
			}

			// if on the right
			if (pos.X >= parentGlobalPos.X + this.parent.Width - this.BorderThinckness && this.ResizeBorderRight) {
				m.Result = new IntPtr(HitTest.HTTRANSPARENT);
				return;
			}

			// if on the bottom
			if (pos.Y >= parentGlobalPos.Y + this.parent.Height - this.BorderThinckness && this.ResizeBorderBottom) {
				m.Result = new IntPtr(HitTest.HTTRANSPARENT);
				return;
			}

			base.WndProc(ref m);
		}
	}
}