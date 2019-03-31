using System;
using System.Windows.Forms;

namespace ScientificCalculator
{
    public delegate void Tran(char[] a, char[] b, char[] c);
    public partial class Calculator : Form
    {
        public static string exp = "0";
        public static int top = -1,top1 = -1;
        public Calculator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exp += "1";
            textBox1.Text += "1";
        }

        private void key2_Click(object sender, EventArgs e)
        {
            exp += "2";
            textBox1.Text += "2";
        }

        private void key3_Click(object sender, EventArgs e)
        {
            exp += "3";
            textBox1.Text += "3";
        }

        private void keyadd_Click(object sender, EventArgs e)
        {
            exp += "+";
            textBox1.Text += "+";
        }

        private void key4_Click(object sender, EventArgs e)
        {
            exp += "4";
            textBox1.Text += "4";
        }

        private void key5_Click(object sender, EventArgs e)
        {
            exp += "5";
            textBox1.Text += "5";
        }

        private void key6_Click(object sender, EventArgs e)
        {
            exp += "6";
            textBox1.Text += "6";
        }

        private void key7_Click(object sender, EventArgs e)
        {
            exp += "7";
            textBox1.Text += "7";
        }

        private void key8_Click(object sender, EventArgs e)
        {
            exp += "8";
            textBox1.Text += "8";
        }

        private void key9_Click(object sender, EventArgs e)
        {
            exp += "9";
            textBox1.Text += "9";
        }

        private void keyleft_Click(object sender, EventArgs e)
        {
            exp += "(";
            textBox1.Text += "(";
        }

        private void key0_Click(object sender, EventArgs e)
        {
            exp += "0";
            textBox1.Text += "0";
        }

        private void keyright_Click(object sender, EventArgs e)
        {
            exp += ")";
            textBox1.Text += ")";
        }

        private void keymod_Click(object sender, EventArgs e)
        {
            exp += "/";
            textBox1.Text += "/";
        }

        private void keymul_Click(object sender, EventArgs e)
        {
            exp += "*";
            textBox1.Text += "*";
        }

        private void keysub_Click(object sender, EventArgs e)
        {
            exp += "-";
            textBox1.Text += "-";

        }

        private void keycal_Click(object sender, EventArgs e)
        {
            double result = 0;
            char[] midexp = exp.ToCharArray();
            char[] sufexp = new char[midexp.Length + 30];
            char[] stack = new char[sufexp.Length];
            double[] calstack = new double[30];
            Tran Trans = new Tran(trans);
            Trans(midexp, sufexp, stack);
            Calculate(sufexp, calstack, ref result);
            Result.Items.Add(string.Format("Result: "+result.ToString()));
            exp=result.ToString();
            textBox1.Text = exp;
        }
        
        public static void trans(char[] midexp, char[] sufexp, char[] stack)
        {
            int i = 0, j = 0;
            char e = '0';
            while (i < midexp.Length)
            {
                switch (midexp[i])
                {
                    case '(':
                        Push(stack, midexp[i]);
                        i++;
                        break;
                    case ')':
                        Pop(stack, ref e);
                        while (e != '(')
                        {

                            sufexp[j++] = e;
                            Pop(stack, ref e);
                        }
                        i++;
                        break;
                    case '+':
                    case '-':
                        while (!Empty(stack))
                        {
                            Gettop(stack, ref e);
                            if (e != '(')
                            {

                                sufexp[j++] = e;
                                Pop(stack, ref e);
                            }
                            else
                                break;
                        }
                        Push(stack, midexp[i]);
                        i++;
                        break;
                    case '*':
                    case '/':
                        while (!Empty(stack))
                        {
                            Gettop(stack, ref e);
                            if (e == '*' || e == '/')
                            {

                                sufexp[j++] = e;
                                Pop(stack, ref e);
                            }
                            else
                                break;
                        }
                        Push(stack, midexp[i]);
                        i++;
                        break;
                    default:
                        while (i < midexp.Length && midexp[i] >= '0' && midexp[i] <= '9')
                        {
                            sufexp[j++] = midexp[i];
                            i++;
                        }
                        sufexp[j++] = '#';
                        break;
                }
            }
            while (!Empty(stack))
            {
                Pop(stack, ref e);
                sufexp[j++] = e;
            }
            sufexp[j] = '\0';

        }
        public static void Calculate(char[] sufexp, double[] calstack, ref double result)
        {
            double a = 0, b = 0, c;
            int i = 0;
            while (sufexp[i] != '\0')
            {
                switch (sufexp[i])
                {
                    case '+':
                        Pop1(calstack, ref a);
                        Pop1(calstack, ref b);
                        Push1(calstack, b + a);
                        i++;
                        break;
                    case '-':
                        Pop1(calstack, ref a);
                        Pop1(calstack, ref b);
                        Push1(calstack, b - a);
                        i++;
                        break;
                    case '*':
                        Pop1(calstack, ref a);
                        Pop1(calstack, ref b);
                        Push1(calstack, b * a);
                        i++;
                        break;
                    case '/':
                        Pop1(calstack, ref a);
                        Pop1(calstack, ref b);
                        if (a == 0)
                        {   MessageBox.Show("Error! Try to div 0!");
                            return;
                        }
                        Push1(calstack, b / a);
                        i++;
                        break;
                    default:
                        c = 0;
                        while (i < sufexp.Length && sufexp[i] >= '0' && sufexp[i] <= '9')
                        {
                            c = (double)(10 * c) + (double)(sufexp[i] - '0');
                            i++;
                        }
                        Push1(calstack, c);
                        i++;
                        break;
                }
            }
            Gettop1(calstack, ref result);
        }
        public static bool Push(char[] stack, char e)
        {
            if (top == stack.Length - 1)
            {
                Console.Write("full");
                return false;
            }
            stack[++top] = e;
            return true;
        }
        public static bool Empty(char[] stack)
        {
            return top == -1;
        }
        public static bool Pop(char[] stack, ref char e)
        {
            if (Empty(stack))
            {
                Console.Write("Empty");
                return false;
            }
            e = stack[top--];
            return true;
        }
        public static bool Gettop(char[] stack, ref char e)
        {
            if (Empty(stack))
            {
                Console.Write("Empty");
                return false;
            }
            e = stack[top];
            return true;
        }
        public static bool Push1(double[] stack, double e)
        {
            if (top1 == 30 - 1)
            {
                Console.Write("full");
                return false;
            }
            stack[++top1] = e;
            return true;
        }
        public static bool Empty1(double[] stack)
        {
            return top1 == -1;
        }
        public static bool Pop1(double[] stack, ref double e)
        {
            if (Empty1(stack))
            {
                Console.Write("Empty");
                return false;
            }
            e = stack[top1--];
            return true;
        }
        public static bool Gettop1(double[] stack, ref double e)
        {
            if (Empty1(stack))
            {
                Console.Write("Empty");
                return false;
            }
            e = stack[top1];
            return true;
        }

        private void result_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Result_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void C_Click(object sender, EventArgs e)
        {
            
        }

        private void CE_Click(object sender, EventArgs e)
        {
            top = -1;
            top1 = -1;
            textBox1.Text = "";
            exp = "0";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BackgroundImageLayout = ImageLayout.Stretch;
        }
    }
}
