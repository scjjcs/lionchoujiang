using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class WaitForm : Form
    {
        public WaitForm()
        {
            InitializeComponent();
            SetText("asdfsadf");
        }
        private delegate void SetTextHandler(string text);
        public void SetText(string text)
        {
            if (this.label2.InvokeRequired)
            {
                this.Invoke(new SetTextHandler(SetText), text);
            }
            else
            {
                this.label2.Text = text;
            }
            
        }

    }
}
