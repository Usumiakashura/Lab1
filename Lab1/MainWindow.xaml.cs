using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double x = 0;   //1-ый операнд
        char oper = 'n';  //текущая операция
        char past_oper = 'n'; //прошлая операция
        double result = 0;  //результат

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBox.Focus();
        }

        //----------------- ОБРАБОТКА КЛИКОВ ПО КНОПКАМ ------------------------

        private void ButtonNumber_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "0")
                textBox.Text = (sender as Button)?.Content.ToString();
            else
                if (textBox.Text.Length < 14)
                textBox.Text += (sender as Button)?.Content;
            textBox.Focus();
        }

        private void ButtonPoint_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.IndexOf(',') < 0)
                textBox.Text += ",";
            textBox.Focus();
        }

        private void ButtonOper_Click(object sender, RoutedEventArgs e)
        {
            oper = (sender as Button).Content.ToString()[0];
            Calculate();
            textBox.Focus();
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            string str = "", txt = textBox.Text;
            char o = (sender as Button).Content.ToString()[0];

            switch (o)
            {
                case '<':
                    for (int i = 0; i < txt.Length - 1; i++)
                        str += txt[i];
                    textBox.Text = str;
                    textBox.Focus();
                    break;
                case 'c':
                    textBox.Text = "0";
                    textBlock_info.Text = "";
                    x = 0;   //1-ый операнд
                    oper = 'n';  //текущая операция
                    past_oper = 'n';    //прошлая операция
                    result = 0; //результат
                    textBox.Focus();
                    break;
            }
        }

        //----------------- ОБРАБОТКА НАЖАТИЙ КЛАВИШ ------------------------
        
        private void TextBox_KeyEvents(object sender, KeyEventArgs e)
        {
            textBox.MaxLength = 14; //максимальное кол-во вводимых вручную символов
            if ((textBox.Text == "" || textBox.Text == "0")     //если попытаемся в начале ввести много нулей - оставить один "0"
                && (e.Key == Key.D0 || e.Key == Key.NumPad0))
                textBox.Text = "0";
            else  //если в текстбоксе есть ноль и мы пытаемся ввести другое число - убрать ноль перед числом
            if(textBox.Text == "0" && (e.Key != Key.D0 || e.Key != Key.NumPad0))
                textBox.Text = "";
            else  //если вводим с клавиатуры "," когда на текстбоксе пусто или ноль - вывести "0,"
            if ((e.Key == Key.OemComma && textBox.Text == "0") ||
                (e.Key == Key.OemComma && textBox.Text == ""))
                textBox.Text = "0,";

            //----- обработка клавиш "+", "-", "*", "/", "Enter", " " -----

            else if (e.Key == Key.Add)   // "+"
            {
                oper = '+';
                Calculate();
            }
            else if (e.Key == Key.Multiply)   // "*"
            {
                oper = '*';
                Calculate();
            }
            else if (e.Key == Key.Subtract)   // "-"
            {
                oper = '-';
                Calculate();
            }
            else if (e.Key == Key.Divide)   // "/"
            {
                oper = '/';
                Calculate();
            }
            else if (e.Key == Key.Enter)   // "Enter"
            {
                oper = '=';
                Calculate();
            }
            else if (e.Key == Key.Space)   // " "
                textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);

            //----------------------------------------------------
            
            textBox.Select(textBox.Text.Length, 0); //курсор в конец текстбокса
        }

        private void TextBox_TextInputEvent(object sender, TextCompositionEventArgs e)
        {
            if (e.Text != "," && IsNumber(e.Text) == false) //если введенный символ в текстбокс не "," и не число
                e.Handled = true;
            else if (e.Text == "0" && textBox.Text == "0")  //если введенный символ - "0", в текстбоксе - "0", то запретить дальнейший ввод "0" 
                e.Handled = true;
            else if (e.Text == ",") //если вводим ","
                if (((TextBox)sender).Text.IndexOf(e.Text) > -1)    //если "," уже есть, то запретить дальнейщий ввод ","
                    e.Handled = true;

            textBox.Select(textBox.Text.Length, 0); //курсор в конец текстбокса
        }


        //----------------- ДОП МЕТОДЫ ДЛЯ УДОБСТВА ------------------------

        private void Calculate()
        {
            x = Convert.ToDouble(textBox.Text);
            textBox.Text = "0";
            switch (past_oper)
            {
                case 'n':   //т.е. null или пустой
                    result = x;
                    past_oper = oper;
                    break;
                case '+':
                    result += x;
                    past_oper = oper;
                    break;
                case '-':
                    result -= x;
                    past_oper = oper;
                    break;
                case '*':
                    result *= x;
                    past_oper = oper;
                    break;
                case '/':
                    result /= x;
                    past_oper = oper;
                    break;
            }

            if (oper == '=')
            {
                textBlock_info.Text = "";
                textBox.Text = result.ToString();
                x = 0;   //1-ый операнд
                oper = 'n';  //текущая операция
                past_oper = 'n';    //прошлая операция
                result = 0; //результат
            } 
            else textBlock_info.Text = (result.ToString() + oper.ToString());
        }

        private bool IsNumber(string text)  //метод для проверки является ли вводимый текст числом
        {
            int output;
            return int.TryParse(text, out output);
        }
    }
}
