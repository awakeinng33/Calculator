using System;
using System.Windows.Forms;
using System.Linq;
namespace Calculator
{

    public partial class MainForm : Form
    {

        private double x;
        private double y;
        private bool flag;
        private int notation = 10;
        private bool DotMode;
        private double Dot;
        private double Memory;
        private string NotationCharacter(double Value)
        {
            if (Value > 9)
                return ((NumberNotation)Value).ToString();
            else return Value.ToString();
        }
        private void OutputConvertNotation()
        {
            if (notation != 10)
            {
                string notationIntegerPath = "";
                string natovationFractionalPath = "";
                bool znak = false;
                double currentValue = X;
                string[] digit = currentValue.ToString().Split(',');
                long integerPath = (long)currentValue;

                if (currentValue < 0)
                {
                    znak = true;
                    currentValue = Math.Abs(currentValue);
                }

                if (digit.Length > 1)
                {

                    double fractionalPath = currentValue - (double)integerPath;
                    natovationFractionalPath += ",";
                    int i = 0;
                    while (true && i < 8)
                    {
                        i++;
                        fractionalPath *= notation;
                        natovationFractionalPath += NotationCharacter((int)fractionalPath);
                        if (fractionalPath.ToString().Split(',').Length < 2)
                            break;
                        fractionalPath -= (int)fractionalPath;
                    }
                }
                if (Math.Abs(integerPath) >= notation)
                {
                    long ost;
                    while (true)
                    {
                        ost = integerPath % notation;
                        integerPath /= notation;
                        if (integerPath < notation)
                        {

                            notationIntegerPath += NotationCharacter(ost);
                            notationIntegerPath += NotationCharacter(integerPath);
                            break;
                        }
                        notationIntegerPath += NotationCharacter(ost);
                    }
                }
                else
                {
                    notationIntegerPath += NotationCharacter(integerPath);
                }
                if (znak)
                    notationIntegerPath += "-";
                textInidcator.Text = new string(notationIntegerPath.ToCharArray().Reverse().ToArray()) + natovationFractionalPath;
            }
            else textInidcator.Text = X.ToString();
        }

        private int Notation
        {
            get
            {
                return notation;
            }
            set
            {
                notation = value;
                for(int i = 1; i < 16; i++)
                    if(i < notation)
                        (Controls["buttonDigit" + i.ToString()] as Button).Enabled = true;
                    else
                        (Controls["buttonDigit" + i.ToString()] as Button).Enabled = false;
                OutputConvertNotation();
            }
        }

        private Operation op;
        private double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
                OutputConvertNotation();
            }
        }
        private void Clear(){
            X = 0;
            flag = false;
            op = Operation.None;
            Memory = 0;
            Notation = 10;
            DotMode = false;
            Dot = 0.1;
        }
        public MainForm()
        {
            InitializeComponent();
            Clear();

        }

        private void buttonDigit_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string number = (string)b.Tag;
                int n = int.Parse(number);
                if (flag)
                {
                    if (DotMode)
                    {
                        X = Dot * n;
                        Dot /= Notation;
                    }
                    else
                        X = n;

                    flag = false;
                }
                else
                {
                    if (DotMode)
                    {
                        X += Dot * n;
                        Dot /= Notation;
                    }
                    else
                        X = X * notation + n;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonOperation_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string s = (string)b.Tag;
                op = (Operation)Enum.Parse(typeof(Operation), s);

                y = X;
                flag = true;
                DotMode = false;
                Dot = 0.1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            try
            {
                switch (op)
                {
                    case Operation.Addition:
                        X = X + y;
                        break;
                    case Operation.Subtraction:
                        X = y - X;
                        break;
                    case Operation.Division:
                        X = y / X;
                        break;
                    case Operation.Multiplication:
                        X = X * y;
                        break;
                }
                flag = true;
                DotMode = false;
                Dot = 0.1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonOneOperation_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string s = (string)b.Tag;
                op = (Operation)Enum.Parse(typeof(Operation), s);

                flag = true;
                DotMode = false;
                Dot = 0.1;
                switch (op)
                {
                    case Operation.Sqrt:
                        X = Math.Sqrt(Math.Abs(X));
                        break;
                    case Operation.Clear:
                        X = 0;
                        flag = false;
                        op = Operation.None;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem b = (ToolStripMenuItem)sender;
                string s = (string)b.Tag;
                var op = (Menus)Enum.Parse(typeof(Menus), s);

                flag = true;

                switch (op)
                {
                    case Menus.Exit:
                        Application.Exit();
                        break;
                    case Menus.Copy:
                        Clipboard.SetData(DataFormats.Text, (Object)textInidcator.Text);
                        ставитьToolStripMenuItem.Enabled = true;
                        break;
                    case Menus.Paste:
                        X = Convert.ToDouble(Clipboard.GetText(TextDataFormat.Text));
                        flag = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void trackBarNotation_Scroll(object sender, EventArgs e)
        {
            labelCurrentNotation.Text = (trackBarNotation.Value + 2).ToString();
            Notation = trackBarNotation.Value + 2;
        }

        private void buttonDotMode_Click(object sender, EventArgs e)
        {
            Dot = 0.1;
            DotMode = true;
        }

        private void buttonMemory_Click(object sender, EventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                string s = (string)b.Tag;
                var op = (MemoryOperation)Enum.Parse(typeof(MemoryOperation), s);

                flag = true;

                switch (op)
                {
                    case MemoryOperation.AdditionMemory:
                        Memory += X;
                        break;
                    case MemoryOperation.SubtractionMemory:
                        Memory -= X;
                        break;
                    case MemoryOperation.OutMemory:
                        X = Memory;
                        break;
                    case MemoryOperation.RemoveMemory:
                        Memory = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
