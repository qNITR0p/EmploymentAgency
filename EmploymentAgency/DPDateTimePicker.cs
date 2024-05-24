using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EmploymentAgency
{
    internal class DPDateTimePicker : DateTimePicker
    {
        //Контрол для DPDateTimePicker

        private bool selectionComplete = false;
        private bool numberKeyPressed = false;

        private const int WM_KEYUP = 0x0101;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_REFLECT = 0x2000;
        private const int WM_NOTIFY = 0x004e;

        [StructLayout(LayoutKind.Sequential)]
        private struct NMHDR
        {
            public IntPtr hwndFrom;
            public IntPtr idFrom;
            public int Code;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            numberKeyPressed = (e.Modifiers == Keys.None && ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode != Keys.Back && e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)));
            selectionComplete = false;
            base.OnKeyDown(e);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_REFLECT + WM_NOTIFY)
            {
                var hdr = (NMHDR)m.GetLParam(typeof(NMHDR));
                if (hdr.Code == -759) //date chosen (by keyboard)
                    selectionComplete = true;
            }
            base.WndProc(ref m);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (numberKeyPressed && selectionComplete &&
                (e.Modifiers == Keys.None && ((e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9) || (e.KeyCode != Keys.Back && e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9))))
            {
                Message m = new Message();
                m.HWnd = this.Handle;
                m.LParam = IntPtr.Zero;
                m.WParam = new IntPtr((int)Keys.Right); //right arrow key
                m.Msg = WM_KEYDOWN;
                base.WndProc(ref m);
                m.Msg = WM_KEYUP;
                base.WndProc(ref m);
                numberKeyPressed = false;
                selectionComplete = false;
            }
        }

    }
}
