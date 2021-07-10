using DotNet.Framework.Ultimate.Native.Constants;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	/// <summary>
	/// A Form which is resizable even with <see cref="FormBorderStyle.None"/>
	/// </summary>
	public class ResizableForm : DesignModeForm {
		private Dictionary<Control, WndProcBorderFilter> borderFilters;

		private int _resizeBorderThickness = 5;

		/// <summary>
		/// The thickness of the resize border on all sides of the window. The border is invisible. Setting it will not result in a padding.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Always)]
		[Browsable(true)]
		[DefaultValue(null)]
		[Category("Behaviour")]
		[Description("The thickness of the resize border on all sides of the window. The border is invisible. Setting it will not result in a padding.")]
		public int ResizeBorderThickness {
			get => this._resizeBorderThickness;
			set {
				if (this._resizeBorderThickness == value)
					return;

				this._resizeBorderThickness = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.BorderThinckness = value;
			}
		}

		/// <summary>
		/// Creates a new instance of the class <see cref="BorderlessResizableForm"/>
		/// </summary>
		public ResizableForm() {
			this.borderFilters = new Dictionary<Control, WndProcBorderFilter>();
			this.ControlAdded += this.Control_ControlAdded;
			this.ControlRemoved += this.Control_ControlRemoved;
		}

		private void Control_ControlAdded(object sender, ControlEventArgs e) {
			this.ApplyFiltersRecursive(this, e.Control);
		}

		private void Control_ControlRemoved(object sender, ControlEventArgs e) {
			this.RemoveFiltersRecursive(e.Control);
		}

		private void ApplyFiltersRecursive(Control parent, Control control) {
			if (control is null)
				return;

			this.borderFilters[control] = new WndProcBorderFilter(parent, control, this.ResizeBorderThickness);

			// remove them in case they were already added.
			control.ControlAdded -= this.Control_ControlAdded;
			control.ControlRemoved -= this.Control_ControlRemoved;

			control.ControlAdded += this.Control_ControlAdded;
			control.ControlRemoved += this.Control_ControlRemoved;

			foreach (Control c in control.Controls)
				if (!(c is null))
					this.ApplyFiltersRecursive(parent, c);
		}

		private void RemoveFiltersRecursive(Control control) {
			if (control is null)
				return;

			this.borderFilters.Remove(control);
			control.ControlAdded -= this.Control_ControlAdded;
			control.ControlRemoved -= this.Control_ControlRemoved;

			foreach (Control c in control.Controls)
				if (!(c is null))
					this.RemoveFiltersRecursive(c);
		}

		protected override void WndProc(ref Message m) {
			if (this.IsInDesginMode) {
				base.WndProc(ref m);
				return;
			}

			if (m.Msg != WindowMessage.WM_NCHITTEST) {
				base.WndProc(ref m);
				return;
			}

			Point pos = this.PointToClient(new Point(m.LParam.ToInt32()));

			// if in top left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOPLEFT);
				return;
			}

			// if in top right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOPRIGHT);
				return;
			}

			// if in bottom left corner
			if (pos.X <= this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOMLEFT);
				return;
			}

			// if in bottom right corner
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOMRIGHT);
				return;
			}

			// if on the left
			if (pos.X <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTLEFT);
				return;
			}

			// if on top
			if (pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOP);
				return;
			}

			// if on the right
			if (pos.X >= this.ClientSize.Width - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTRIGHT);
				return;
			}

			// if on the bottom
			if (pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOM);
				return;
			}

			base.WndProc(ref m);
		}
	}
}