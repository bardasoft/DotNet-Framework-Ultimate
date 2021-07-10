﻿using DotNet.Framework.Ultimate.Native.Constants;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DotNet.Framework.Ultimate.UI.Controls {
	public class ResizablePanel : Panel {
		private int _resizeBorderThickness = 5;
		private bool _resizeBorderLeft = false;
		private bool _resizeBorderRight = false;
		private bool _resizeBorderTop = false;
		private bool _resizeBorderBottom = false;

		private Dictionary<Control, WndProcBorderFilter> borderFilters;

		public int ResizeBorderThickness {
			get => this._resizeBorderThickness;
			set {
				if (this._resizeBorderThickness == value)
					return;

				this._resizeBorderThickness = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.BorderThinckness = this._resizeBorderThickness;
			}
		}

		public bool ResizeBorderLeft {
			get => this._resizeBorderLeft;
			set {
				if (this._resizeBorderLeft == value)
					return;

				this._resizeBorderLeft = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.ResizeBorderLeft = this._resizeBorderLeft;
			}
		}

		public bool ResizeBorderRight {
			get => this._resizeBorderRight;
			set {
				if (this._resizeBorderRight == value)
					return;

				this._resizeBorderRight = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.ResizeBorderRight = this._resizeBorderRight;
			}
		}

		public bool ResizeBorderTop {
			get => this._resizeBorderTop;
			set {
				if (this._resizeBorderTop == value)
					return;

				this._resizeBorderTop = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.ResizeBorderTop = this._resizeBorderTop;
			}
		}

		public bool ResizeBorderBottom {
			get => this._resizeBorderBottom;
			set {
				if (this._resizeBorderBottom == value)
					return;

				this._resizeBorderBottom = value;

				foreach (WndProcBorderFilter filter in this.borderFilters.Values)
					filter.ResizeBorderBottom = this._resizeBorderBottom;
			}
		}

		public ResizablePanel() {
			this.SetStyle(ControlStyles.ResizeRedraw, true);
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

			this.borderFilters[control] = new WndProcBorderFilter(parent, control, this.ResizeBorderThickness, this._resizeBorderLeft, this._resizeBorderRight, this._resizeBorderTop, this._resizeBorderBottom);

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
			if (m.Msg != WindowMessage.WM_NCHITTEST) {
				base.WndProc(ref m);
				return;
			}

			Point pos = this.PointToClient(new Point(m.LParam.ToInt32()));

			// if in top left corner
			if (this.ResizeBorderLeft && this.ResizeBorderTop && pos.X <= this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOPLEFT);
				return;
			}

			// if in top right corner
			if (this.ResizeBorderRight && this.ResizeBorderTop && pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOPRIGHT);
				return;
			}

			// if in bottom left corner
			if (this.ResizeBorderLeft && this.ResizeBorderBottom && pos.X <= this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOMLEFT);
				return;
			}

			// if in bottom right corner
			if (this.ResizeBorderRight && this.ResizeBorderBottom && pos.X >= this.ClientSize.Width - this.ResizeBorderThickness && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOMRIGHT);
				return;
			}

			// if on the left
			if (this.ResizeBorderLeft && pos.X <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTLEFT);
				return;
			}

			// if on top
			if (this.ResizeBorderTop && pos.Y <= this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTTOP);
				return;
			}

			// if on the right
			if (this.ResizeBorderRight && pos.X >= this.ClientSize.Width - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTRIGHT);
				return;
			}

			// if on the bottom
			if (this.ResizeBorderBottom && pos.Y >= this.ClientSize.Height - this.ResizeBorderThickness) {
				m.Result = new IntPtr(HitTest.HTBOTTOM);
				return;
			}

			base.WndProc(ref m);
		}
	}
}