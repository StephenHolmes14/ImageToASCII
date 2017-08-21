using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "TrainingData.txt");
            XORTrainingData.XORTrainingData.GenerateTrainingData(filePath);
            var topology = new List<int> {3, 4, 2};
            foreach (string line in NeuralNetwork.NeuralNetwork.Start(topology, filePath))
            {
                richTextBox1.Text += line;
            }
        }
    }
}
