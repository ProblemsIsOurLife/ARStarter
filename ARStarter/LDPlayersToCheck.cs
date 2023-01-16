using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARStarter
{
    public partial class LDPlayersToCheck : Form
    {
        public LDPlayersToCheck()
        {
            InitializeComponent();
            textBox1.Font = new Font("Segoe UI", 13, FontStyle.Regular);
        }
        private List<string> _dataToReturn = new List<string>();
        public List<string> dataToReturn
        {
            get
            {
                return _dataToReturn;
            }
        }
        private void addButton_Click(object sender, EventArgs e)
        {
            foreach (string str in textBox1.Lines)
            {
                if(str != "\r\n" && str != "\n" && str != "\t" && str!="")
                {
                    _dataToReturn.Add(str);
                }
            }
            this.Close();
        }
    }
}
