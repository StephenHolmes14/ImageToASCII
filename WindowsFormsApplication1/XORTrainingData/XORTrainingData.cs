using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XORTrainingData
{
    public static class XORTrainingData
    {
        public static void GenerateTrainingData(string filePath)
        {
            FileInfo trainingDataFile = new FileInfo(filePath);

            if (!trainingDataFile.Exists)
            {
                trainingDataFile.Delete();

                while (trainingDataFile.Exists)
                {
                    trainingDataFile.Refresh();
                    Thread.Sleep(200);
                }

                trainingDataFile.Create();

                while (!trainingDataFile.Exists)
                {
                    trainingDataFile.Refresh();
                    Thread.Sleep(200);
                }
            }

            var random = new Random();
            string fileOutputString = "";

            for (int i = 0; i < 30000; i++)
            {
                int a = random.Next(0, 2);
                int b = random.Next(0, 2);
                int output = a + b == 1 ? 1 : 0; //UNCONVENTIONAL XOR OPERATOR BITE ME

                fileOutputString += $"{a} {b} {output}\n";
            }

            fileOutputString = fileOutputString.Trim();

            using (var stream = trainingDataFile.CreateText())
            {
                stream.Write(fileOutputString);    
            }
        }
    }
}
