using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace NatureSim
{
    public class Logic
    {
        public const int IN_DATA_PIXELS = 576; // 24 * 24
        public const int PIXELS = 24;
        public const int N_CHARACTERS = 11;

        private Form1 window;
        private Graphics g, userG, pcG, barG, viewerG;
        private Label opdracht;
        private NetworkViewerForm viewForm;

        private SolidBrush black, white, red, yellow, green;

        public bool userPaint = false;
        private int prevX = 0, prevY = 0;

        private float[,] map;
        private int numberToDraw = 0;
        private double[][] maps;
        private double[][] extramaps;

        NeuralNetwork network = new NeuralNetwork(IN_DATA_PIXELS, 10, N_CHARACTERS);
        NeuralNetwork extranet = new NeuralNetwork(11, 10, 11);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        private double[][] optimal_results = new double[][] { 
            new double[] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new double[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            new double[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
            new double[] { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            new double[] { 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0 },
            new double[] { 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 },
            new double[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0 },
            new double[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
            new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
            new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
            new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 },
        };

        public Logic(Form1 form, Panel p, Panel pcp, Label l, Panel pProg, NetworkViewerForm viewF)
        {
            window = form;
            g = window.CreateGraphics();
            userG = p.CreateGraphics();
            pcG = pcp.CreateGraphics();
            barG = pProg.CreateGraphics();
            viewForm = viewF;
            viewerG = viewForm.canvas.CreateGraphics();
            opdracht = l;

            black = new SolidBrush(Color.Black);
            white = new SolidBrush(Color.White);
            red = new SolidBrush(Color.Red);
            yellow = new SolidBrush(Color.Yellow);
            green = new SolidBrush(Color.Green);

            map = new float[PIXELS, PIXELS];
            maps = new double[11][];
            extramaps = new double[11][];

            network.loadNetwork("network.ann");
        }

        public void ClearUserCanvas()
        {
            userG.FillRectangle(black, new Rectangle(0, 0, 960, 980));
        }
        public void ClearPcCanvas()
        {
            pcG.FillRectangle(black, new Rectangle(0, 0, 960, 980));
            pcG.FillRectangle(white, new Rectangle(0, 0, 20, 980));
        }
        public void ClearMap()
        {
            for (int x = 0; x < PIXELS; x++)
                for (int y = 0; y < PIXELS; y++)
                    map[x, y] = 0;
            Console.WriteLine("Cleared Map");
        }

        public void DrawMapOnPC()
        {
            ClearPcCanvas();
            for (int x = 0; x < PIXELS; x++)
                for (int y = 0; y < PIXELS; y++)
                {
                    if(map[x, y] == 1f)
                        pcG.FillRectangle(white, new Rectangle(x * 40, y * 40, 40, 40));
                }
        }
        public void UserDrawOnCanvas(int x, int y)
        {
            if (!userPaint || x < 0 || y < 0 || x > 960 - 1 || y > 960 - 1)
                return;
            
            userG.FillEllipse(white, new Rectangle(x - 10, y - 10, 20, 20));
            map[x / 40, y / 40] = 1f;

            prevX = x;
            prevY = y;
        }
        public void DrawNetwork()
        {
            SendMessage(viewForm.Handle, 11, true, 0);
            viewerG.FillRectangle(black, 0, 0, 1900, 1160);

            for(int i = 0; i < IN_DATA_PIXELS; i++)
            {
                byte r = 0, g = 0, b = 0;

                if (network.inputLayer[i].bias > 0)
                    g = (byte)(network.inputLayer[i].bias * 255);
                else if (network.inputLayer[i].bias < 0)
                    r = (byte)(-network.inputLayer[i].bias * 255);
                else
                    b = 255;

                SolidBrush br = new SolidBrush(Color.FromArgb(255, r, g, b));
                viewerG.FillRectangle(br, 50, i * 2, 50, 1);
            }
            for (int i = 0; i < network.hiddenLayer.Count; i++)
            {
                byte r = 0, g = 0, b = 0;

                if (network.hiddenLayer[i].bias > 0)
                    g = (byte)(network.hiddenLayer[i].bias * 255);
                else if (network.hiddenLayer[i].bias < 0)
                    r = (byte)(-network.hiddenLayer[i].bias * 255);
                else
                    b = 255;

                SolidBrush br = new SolidBrush(Color.FromArgb(255, r, g, b));
                viewerG.FillRectangle(br, 1200, i * (1160 / network.hiddenLayer.Count), 75, 75);
            }
            for (int i = 0; i < network.outputLayer.Count; i++)
            {
                byte r = 0, g = 0, b = 0;

                if (network.outputLayer[i].bias > 0)
                    g = (byte)(network.outputLayer[i].bias * 255);
                else if (network.outputLayer[i].bias < 0)
                    r = (byte)(-network.outputLayer[i].bias * 255);
                else
                    b = 255;

                SolidBrush br = new SolidBrush(Color.FromArgb(255, r, g, b));
                viewerG.FillRectangle(br, 1750, i * (1160 / network.outputLayer.Count), 75, 75);
            }
            ///////////////////////////
            for (int i = 0; i < network.inputLayer.Count; i++)
            {
                for (int j = 0; j < network.inputLayer[i].outputSynapses.Count; j++)
                {
                    byte r = 0, g = 0, b = 0;
                    if (network.inputLayer[i].outputSynapses[j].weight > 0)
                        g = (byte)(network.inputLayer[i].outputSynapses[j].weight * 255);
                    else if (network.inputLayer[i].outputSynapses[j].weight < 0)
                        r = (byte)(network.inputLayer[i].outputSynapses[j].weight * 255);
                    else
                        b = 255;
                    Pen p = new Pen(Color.FromArgb(75, r, g, b));
                    viewerG.DrawLine(p, 110, i * 2, 1200, j * (1160 / network.hiddenLayer.Count) + 30);
                }
            }
            for (int i = 0; i < network.hiddenLayer.Count; i++)
            {
                for (int j = 0; j < network.hiddenLayer[i].outputSynapses.Count; j++)
                {
                    byte r = 0, g = 0, b = 0;
                    if (network.hiddenLayer[i].outputSynapses[j].weight > 0)
                        g = (byte)(network.hiddenLayer[i].outputSynapses[j].weight * 255);
                    else if (network.hiddenLayer[i].outputSynapses[j].weight < 0)
                        r = (byte)(network.hiddenLayer[i].outputSynapses[j].weight * 255);
                    else
                        b = 255;
                    Pen p = new Pen(Color.FromArgb(255, r, g, b), 2);
                    viewerG.DrawLine(p, 1275, i * (1160 / network.hiddenLayer.Count) + 30, 1750, j * (1160 / network.outputLayer.Count) + 30);
                }
            }
            Console.WriteLine("DONE RENDERING");
            SendMessage(viewForm.Handle, 11, false, 0);
        }
        
        public void NextMission()
        {
            numberToDraw++;
            if (numberToDraw > 10)
                numberToDraw = 0;
            if (numberToDraw != 10)
                opdracht.Text = "Please draw a " + numberToDraw;
            else
                opdracht.Text = "Please draw a Smile";
        }
        public void AddCurrentChar()
        {
            int sumCount = 0, sum0 = 0, sum1 = 0, sum2 = 0, sum3 = 0, particleCount = 0;
            double avX = 0d, avY = 0d, aX0 = 0d, aY0 = 0d, aX1 = 0d, aY1 = 0d, aX2 = 0d, aY2 = 0d, aX3 = 0d, aY3 = 0d;

            double[] useData = new double[IN_DATA_PIXELS];
            for (int x = 0; x < PIXELS; x++)
            {
                for (int y = 0; y < PIXELS; y++)
                {
                    useData[PIXELS * x + y] = map[x, y];
                    if(map[x, y] == 1d)
                    {
                        if(x < 12 && y < 12)
                        {
                            sum0++;
                            aX0 += x;
                            aY0 += y;
                        }
                        else if (x >= 12 && y < 12)
                        {
                            sum1++;
                            aX1 += x;
                            aY1 += y;
                        }
                        else if (x < 12 && y >= 12)
                        {
                            sum2++;
                            aX2 += x;
                            aY2 += y;
                        }
                        else if (x >= 12 && y >= 12)
                        {
                            sum3++;
                            aX3 += x;
                            aY3 += y;
                        }
                        avX += x;
                        avY += y;
                        sumCount++;
                        particleCount++;
                    }
                }
            }
            maps[numberToDraw] = useData;

            avX /= sumCount;
            avY /= sumCount;

            aX0 /= sum0;
            aY0 /= sum0;
            aX1 /= sum1;
            aY1 /= sum1;
            aX2 /= sum2;
            aY2 /= sum2;
            aX3 /= sum3;
            aY3 /= sum3;

            extramaps[numberToDraw] = new double[] {particleCount, avX, avY, aX0, aY0, aX1, aY1, aX2, aY2, aX3, aY3};

            pcG.FillRectangle(red, new Rectangle((int)avX * 40, (int)avY * 40, 40, 40));
            pcG.FillRectangle(yellow, new Rectangle((int)aX0 * 40, (int)aY0 * 40, 40, 40));
            pcG.FillRectangle(yellow, new Rectangle((int)aX1 * 40, (int)aY1 * 40, 40, 40));
            pcG.FillRectangle(yellow, new Rectangle((int)aX2 * 40, (int)aY2 * 40, 40, 40));
            pcG.FillRectangle(yellow, new Rectangle((int)aX3 * 40, (int)aY3 * 40, 40, 40));
        }
        public void LearnCurrentChar()
        {
            double[] trainData = new double[IN_DATA_PIXELS];
            for (int x = 0; x < PIXELS; x++)
            {
                for (int y = 0; y < PIXELS; y++)
                {
                    trainData[PIXELS * x + y] = map[x, y];
                }
            }
            for (int i = 0; i < 2000; i++)
            {
                network.train(trainData);
                network.backPropagate(optimal_results[numberToDraw]);
            }
            Console.WriteLine("DONE TRAINING CHAR! " + network.calculateError(optimal_results[numberToDraw]));
        }
        public void TestChar()
        {
            double[] usedata = new double[IN_DATA_PIXELS];
            for (int x = 0; x < PIXELS; x++)
            {
                for (int y = 0; y < PIXELS; y++)
                {
                    usedata[PIXELS * x + y] = map[x, y];
                }
            }
            double[] output = network.compute(usedata);

            double max = 0d;
            int element = 0;
            
            for(int i = 0; i < output.Length; i++)
            {
                Console.WriteLine("__out: " + output[i]);
                if (output[i] > max)
                {
                    max = output[i];
                    element = i;
                } 
            }

            if (element != 10)
                DrawStringPC("" + element);
            else
                DrawStringPC(":)");

            //////////////////////////////////////////

            int sumCount = 0, sum0 = 0, sum1 = 0, sum2 = 0, sum3 = 0, particleCount = 0;
            double avX = 0d, avY = 0d, aX0 = 0d, aY0 = 0d, aX1 = 0d, aY1 = 0d, aX2 = 0d, aY2 = 0d, aX3 = 0d, aY3 = 0d;
            
            for (int i = 0; i < IN_DATA_PIXELS; i++)
            {
                int x = i / PIXELS;
                int y = i - x;

                if (usedata[i] == 1d)
                {
                    if (x < 12 && y < 12)
                    {
                        sum0++;
                        aX0 += x;
                        aY0 += y;
                    }
                    else if (x >= 12 && y < 12)
                    {
                        sum1++;
                        aX1 += x;
                        aY1 += y;
                    }
                    else if (x < 12 && y >= 12)
                    {
                        sum2++;
                        aX2 += x;
                        aY2 += y;
                    }
                    else if (x >= 12 && y >= 12)
                    {
                        sum3++;
                        aX3 += x;
                        aY3 += y;
                    }
                    avX += x;
                    avY += y;
                    sumCount++;
                    particleCount++;
                }
            }

            avX /= sumCount;
            avY /= sumCount;

            aX0 /= sum0;
            aY0 /= sum0;
            aX1 /= sum1;
            aY1 /= sum1;
            aX2 /= sum2;
            aY2 /= sum2;
            aX3 /= sum3;
            aY3 /= sum3;

            double[] extramap = new double[] { particleCount, avX, avY, aX0, aY0, aX1, aY1, aX2, aY2, aX3, aY3 };
            double[] extraOutput = extranet.compute(extramap);

            double extramax = 0d;
            int extraelement = 0;

            for (int i = 0; i < 11; i++)
            {
                if(extraOutput[i] > extramax)
                {
                    extramax = extraOutput[i];
                    extraelement = i;
                }
            }

            if (extraelement != 10)
                Console.WriteLine("EXTRA NET GUESS: " + extraelement);
            else
                Console.WriteLine("EXTRA NET GUESS: :)");
        }
        public void LearnAllChars()
        {
            for(int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if(maps[j] != null)
                    {
                        network.train(maps[j]);
                        network.backPropagate(optimal_results[j]);
                    }
                }
            }
            for (int i = 0; i < 400; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (extramaps[j] != null)
                    {
                        extranet.train(extramaps[j]);
                        extranet.backPropagate(optimal_results[j]);
                    }
                }
            }

            network.saveNetwork("network.ann");
            for(int i = 0; i < 11; i++)
            {
                network.compute(maps[i]);
                Console.WriteLine("ERROR ON MAIN NET: " + network.calculateError(optimal_results[i]));
                extranet.compute(maps[i]);
                Console.WriteLine("ERROR ON EXTRA NET: " + extranet.calculateError(optimal_results[i]));
            }
        }

        public void DrawStringPC(string s)
        {
            ClearPcCanvas();
            Font f = new Font("Times New Roman", 512);
            pcG.DrawString(" " + s, f, white, new Rectangle(0,0,960,960));
            Console.WriteLine("Displayed on PC: " + s);
        }
    }
}