using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dump2Code
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void convertBtn_Click(object sender, EventArgs e)
        {
            var output = Reconstructor.ParseInput(inputText.Text);
            outputText.Clear();
            output.ForEach(delegate(String line)
                               {
                                   outputText.AppendText(line);
                                   outputText.AppendText(Environment.NewLine);
                               });
        }
    }
}
