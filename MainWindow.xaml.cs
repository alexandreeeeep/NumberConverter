using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

namespace ConvertionsApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();//sets up the GUI

            //Sets up the comboBoxes (also prevents null errors)
            ToComboBox.Text = "Denary";
            FromComboBox.Text = "Binary";
            Conversion.Content = "Conversion";
        }

        /// <summary>
        /// -----------------------------------------------------------Validation and convertions-----------------------------------------------------
        /// This is the validation and checks to convert the number to the correct format.
        /// These subrotines refer to eachother where RunProgram() validates and converts all the numbers.
        /// Then calls a function called OutputConverted converting it to the desired format.
        /// The subrotines use the Convertion functions and are called by the GUI subrotines.
        /// </summary>

        private void RunProgram() {
            ErrorLable.Content = "";
            if (Input.Text.ToString() == "No Value entered")
            {
                Input.Text = "";
            }
            if (Input.Text.ToString().Any(" ".Contains))//catches if spaces were entered
            {
                OutPut.Content= "Input must not contain spaces";
            }
            else if (Input.Text == "") { OutPut.Content = "No Value entered"; }//catches if the used didnt input a value
            else if (ToValue.Content.ToString() == "Denary to" && FromDenaryValidator() != "e")
            { OutPut.Content = OutputConvert(FromDenaryValidator()); }
            else if (ToValue.Content.ToString() == "Binary to" && FromBinaryValidator() != "e")
            { OutPut.Content = OutputConvert(FromBinaryValidator()); }
            else if (ToValue.Content.ToString() == "Hexidecimal to" && FromHexValidator() != "e")
            { OutPut.Content = OutputConvert(FromHexValidator()); }
            else { OutPut.Content = "Invalid Input format"; }//returns if the input is not valid
        }
        private string OutputConvert(string Value)//converts the value to any other value and outputs it
        {
            if (FromValue.Content.ToString() == "Denary") { return Value; }
            else if (FromValue.Content.ToString() == "Binary") { return DenaryToBinary(Value); }//Convert.ToString(int.Parse(Value), 2); }//converts to binary
            else { return DenaryToHex(int.Parse(Value)); }//Convert.ToString(int.Parse(Value), 16).ToUpper(); }//converts to hexidecimal and capitalises it
        }
        string FromDenaryValidator()//valudates if denary value is correct
        {
            string Den = Convert.ToString(Input.Text);
            string TempNumber = "";
            int Total = 0;
            if (Den.All("0123456789".Contains) && Den.Length <= 9 || Den.All("0123456789+".Contains) && Den.Any("+".Contains) && Den.Length <= 10)//validates if its a int
            {
                for (int i = 0; i < Den.Length; i++)
                {
                    if (Den.Substring(i, 1) == "+")
                    {
                        if (TempNumber.Length > 3)//input validation
                        {
                            ErrorLable.Content = "Number(s) too large";
                            return "e";
                        }
                        if (TempNumber != "")//input validation
                        {
                            Total += Convert.ToInt32(TempNumber);
                        }
                        TempNumber = "";
                    }
                    else
                    {
                        TempNumber += Den.Substring(i, 1);//adds value to substring
                    }
                }
                if (TempNumber != "")//Input validation
                {
                    Total += Convert.ToInt32(TempNumber);
                }
                if (Total >= 0) { return Convert.ToString(Total); }//returns the int as a string
            }
            return "e";//returns E if its not a string
        }

        string FromBinaryValidator()//valudates if binary value is correct and adds binary numbers together
        {
            string Bin = Convert.ToString(Input.Text);
            string TempNumber = "";
            int Total = 0;
            if (Bin.All("01".Contains) && Bin.Length <= 8 || Bin.All("01+".Contains) && Bin.Any("+".Contains) && Bin.Length <= 27)//validates if its a int
            {
                for (int i = 0; i < Bin.Length; i++)//ittarates through each letter
                {
                    if (Bin.Substring(i, 1) == "+")//Checks if addition is occuring
                    {
                        if (TempNumber.Length > 8)//validation
                        {
                            ErrorLable.Content = "Number too large";
                            return "e";
                        }
                        Total += Convert.ToInt32(BinaryToDenary(TempNumber));//adds number to total
                        TempNumber = "";
                    }
                    else
                    {
                        TempNumber += Bin.Substring(i, 1);//adds value to new stirng
                    }

                    if (TempNumber.Length > 8){ErrorLable.Content = "Number too large";return "e"; }//returns e and an error message if the value is to large

                }
                Total += Convert.ToInt32(BinaryToDenary(TempNumber));
                if (Total>255)
                {ErrorLable.Content = "This would cause a\n bit overflow error"; }//checks for a bit overflow
                return Convert.ToString(Total);
            }
            return "e";//returns E if its not a string
        }

        string FromHexValidator()//valudates if hex value is correct
        {
            string hex = Convert.ToString(Input.Text.ToUpper());
            if (hex.All("0123456789ABCDEF".Contains) && hex.Length <= 2)//validates if its a int
            { //return Convert.ToString(Convert.ToInt32(hex, 16)); }
                return HexToDenary(hex);//returns the int as a Denary string
            }
            return "e";//returns E if its not a string
        }


        /// <summary>
        /// ----------------------------------------------------GUI subrotines----------------------------------------------------------------------------
        /// These perform button functions for the interface.
        /// These functions detect user input and updatw the interface
        /// they also use the procedure RunProgram() to update the converted value when buttons are pressed.
        /// </summary>

        
        private void CalculateResult(object sender, RoutedEventArgs e)
        {
            RunProgram();
        }
        private void ToComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//updates everything when this combobox is changed
        {
            string newvalue = (e.AddedItems[0] as ComboBoxItem).Content as string;
            if (FromComboBox.Text.ToString() == newvalue)//checks if this value is already being used
            {
               SwapButton("", e);//swaps the values if the same value is inputed twice
            }
            else
            {   if (Conversion.Content.ToString() == "Convertion")//prevents errors from occuring by 
                {
                    ToComboBox.Text = newvalue;
                    SwapButton("", e);//swaps the values if the same value is inputed twice
                    string Temp = OutPut.Content.ToString();
                    //RunProgram();
                    SwapButton("", e);
                    Input.Text = Temp;
                }
                if (Input.Text.ToString().Any(" ".Contains) || Input.Text.ToString().All(" 0".Contains))
                {
                    Input.Text = "";
                }
            }
            ToValue.Content = newvalue + " to";//gets the new value in the combo box
            RunProgram();//runs new convertion
        }
        private void FromComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//updates everything when this combobox is changed
        {
            string newvalue = (e.AddedItems[0] as ComboBoxItem).Content as string;//gets the new value in the combo box
            if (ToComboBox.Text.ToString() == newvalue)
            {
                //Swapbutton cant be used instead because it causes inexplicable errors this does it the other way around fixing a major issue I had
                ToComboBox.Text = FromComboBox.Text;
                FromComboBox.Text = newvalue;
                if (Input.Text.ToString().Any(" ".Contains) || Input.Text.ToString().All(" 0".Contains))
                {
                    Input.Text = OutPut.Content.ToString();//swaps the input and outputvalue
                }
            }
            FromValue.Content = newvalue;
            RunProgram();//runs new convertion
            
        }
        //Additional function buttons 
        private void SwapButton(object sender, RoutedEventArgs e)//swaps the values and the convertions
        {
            if (Input.Text.ToString() != null)//somehow prevents errors
            {
                string TempResult = OutPut.Content.ToString();
                string Temp = FromComboBox.Text;//stores value as a temp to swap
                                                //swaps the combobox selections
                FromComboBox.Text = ToComboBox.Text;
                ToComboBox.Text = Temp;
                Input.Text = TempResult;//swaps the input and outputvalue
                if (Input.Text.ToString().Any(" ".Contains) || Input.Text.ToString().All(" 0".Contains))
                {
                    Input.Text = "";
                }
            }
        }
        private void ClearButton(object sender, RoutedEventArgs e)
        {
            Input.Text = "";//clears the text in the input box
        }



        /// <summary>
        /// ------------------------------------------------------------Convertion functions----------------------------------------------------------
        /// the manual convertions that convert values to and from Den hex and bin.
        /// They are all in functions and used in the rest of the code.
        /// </summary>


        static string BinaryToDenary(string Bin)//converts dennary to binary manualy
        {
            double Result = 0;
            for (int i = 0; i < Bin.Length; i++)//ittarates through each letter
            {
                if (Bin.Substring(i, 1) == "1")//checks if there is a one in every row
                {
                    Result += Math.Pow(2, Bin.Length - i - 1);//adds the relevent amount to the string
                }
            }
            return Result.ToString();
        }

        string DenaryToBinary(string Den)//converts dennary to binary manualy
        {
            if (int.Parse(Den) > 255)
            {
                ErrorLable.Content = "Number(s) too large";
                return "Input a smaller value";
            }
            string Result = "";
            Double DenNum = int.Parse(Den)+1;
            for (int i = 128; i > 0; i/= 2)//ittarates through each letter
            {
                if (DenNum-i >0)//checks if there is a one in every row
                {
                    DenNum  -= i;
                    Result += "1";//adds the relevent amount to the string
                }
                else 
                { Result += "0"; }
            }
            return Result;
        }
        string DenaryToHex(int Den)
        {
            if (Den == 0) { return "0"; }//checks if value is zero returns zero to prevent errors
            if (Den >255)
            {
                ErrorLable.Content = "Number(s) too large";
                return "Input a smaller value";
                    }
            int Remander = Den % 16;//this will be the didget valued from 0-15
            int val = (Den - Remander) / 16;//this will be the number didget from 16-240
            if (val < 0) { val=0; }//prevents erros that occur if val <0

            return Convert16bitToHex(val)+Convert16bitToHex(Remander);//converts each number to Hex
        }
        static string Convert16bitToHex(int den)
        {
            if (den>9)
            {
               switch (den)//converts numbers to letters
                {
                    case 10:
                        return "A";
                    case 11:
                        return "B";
                    case 12:
                        return "C";
                    case 13:
                        return "D";
                    case 14:
                        return "E";
                    case 15:
                        return "F";
                }
            }
            return den .ToString();//otherwise returns numbers
        }

        static string HexToDenary(string hex)
        {
            int dec = 0;
            int length = hex.Length;
            for (int i = 0; i < length + 1; i++)//cycles through each letter
            {
                 if (hex.Substring(i,1).Any("ABCDEF".Contains))//checks if the value is a letter
                 {
                    if (hex[i] == 'A')//check the value
                        dec += 10 * Convert.ToInt32(Math.Pow(16, length - 1));//adds the correct ammount
                    if (hex[i] == 'B')
                        dec += 11 * Convert.ToInt32(Math.Pow(16, length - 1));
                    if (hex[i] == 'C')
                        dec += 12 * Convert.ToInt32(Math.Pow(16, length - 1));
                    if (hex[i] == 'D')
                        dec += 13 * Convert.ToInt32(Math.Pow(16, length - 1));
                    if (hex[i] == 'E')
                        dec += 14 * Convert.ToInt32(Math.Pow(16, length - 1));
                    if (hex[i] == 'F')
                        dec += 15 * (Convert.ToInt32(Math.Pow(16, length - 1)));
                    }
                    else
                        dec += (int)char.GetNumericValue(hex[i]) * (int)Math.Pow(16, length - 1);//converts the value if its a number
                    length--;

            //outputs the result
            }
            return dec.ToString();

        }
    }
}