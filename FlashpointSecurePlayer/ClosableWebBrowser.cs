﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static FlashpointSecurePlayer.Shared;
using static FlashpointSecurePlayer.Shared.Exceptions;

namespace FlashpointSecurePlayer {
    public partial class ClosableWebBrowser : System.Windows.Forms.WebBrowser {
        public Form Form { get; set; } = null;

        public ClosableWebBrowser() {
            InitializeComponent();
            
            this.PreviewKeyDown += ClosableWebBrowser_PreviewKeyDown;
        }

        private void ClosableWebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e) {
            e.IsInputKey = true;
        }

        protected override void OnPaint(PaintEventArgs pe) {
            base.OnPaint(pe);
        }

        protected override void WndProc(ref Message m) {
            if (!DesignMode && Form != null) {
                switch (m.Msg) {
                    case WM_PARENTNOTIFY:
                    if (m.WParam.ToInt32() == WM_DESTROY) {
                        // close the form if the browser control closes
                        // (for example, if window.close is called)
                        // needs to be done here because the event
                        // intended for this does not actually fire
                        Form.Close();
                    }

                    DefWndProc(ref m);
                    return;
                    case WM_PAINT:
                    if (Form.WindowState != FormWindowState.Maximized) {
                        // lame fix: browser hangs when window.open top attribute > control height (why?)
                        // Width, Height, and WindowState changes all work here
                        // Width/Height are less obvious and Height doesn't cause text reflow
                        Form.Height--;
                        Form.Height++;
                    }
                    break;
                }
            }

            base.WndProc(ref m);
        }
    }
}
