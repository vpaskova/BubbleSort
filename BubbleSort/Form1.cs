using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BubbleSort
{
    public partial class Form1 : Form
    {
        //Списък на всички бутончета за всяко число
        List<Button> buttons = new List<Button>();
        public Form1()
        {
            InitializeComponent();
        }

        //Тук влиза когато се натисне бутона "Сортирай"
        private void button1_Click(object sender, EventArgs e)
        {
            //Зачистваме за всеки случай надписа 
            label1.BeginInvoke((MethodInvoker)delegate () { label1.Text = ""; });

            //Създаваме си един масив от стрингове за да вземем въведените данни в textbox-a
            string[] arrString = textBox1.Text.Split(',');
            //Проверка ако случайно нищо не е въведено да не гърми, а да покаже съобщение
            if (arrString.Length <= 1)
            {
                MessageBox.Show("Моля въведете коректен масив от числа!");
                return;
            }
            //Конвертираме си данните в масив от int За да работим с числа 
            int[] arr = Array.ConvertAll(arrString, s => int.Parse(s));

            //Обхождаме всяко число и му създаваме бутонче извиквайки за всяко число метода CreateButton
            for (int i = 0; i < arr.Length; i++)
                CreateButton(arr[i].ToString(), i);

            //След като сме начертали бутончетата сортираме масива
            BubbleSort(arr);
        }

        //Метод за създаване на бутонче за всяко число, за да може да се оцветяват и да показват как се извършва алгоритъма
        private void CreateButton(string text, int index)
        {
            //Създаваме бутон, даваме му височина и ширина и за локация изполваме индекса, за да го умножим по 70 и бутончетата да не се застъпват
            Button btn = new Button();
            btn.Height = 40;
            btn.Width = 60;
            btn.Location = new Point(index * 70, 0);
            btn.Text = text;
            btn.Name = index.ToString();
            //Тук добавяме бутона в панела
            //BeginInvoke((MethodInvoker)delegate () това се използва понеже добавяме в този панел много пъти и основната нишка по този начин се достъпва много пъти
            //а не може да се и гърми и по този начин се изчаква да се завърши добавянето на бутона в панела преди да се премине към следващо добавяне
            // това с BeginInvoke го има и надолу логиката е същата - ползва се за да се завърши дадено действие във визуалната част
            panel1.BeginInvoke((MethodInvoker)delegate () { panel1.Controls.Add(btn); });
            //Добавяме си бутона в списък, за да го ползваме и в другите методи като се оцветяват
            buttons.Add(btn);
        }

        //Това е самият алгоритъм
        public void BubbleSort(int[] array)
        {
            int temp;
            //Алгоритъма го стартираме в отделна нишка с този ред като идеята е да може да я приспиваме
            //, за да се създаде тази анимация при обхождането и да се види точно какво прави алгоритъма
            Task.Factory.StartNew(() =>
            {
                //Приспиваме нишката за 2 секунди
                Thread.Sleep(2000);
                for (int j = 0; j <= array.Length - 2; j++)
                {
                    for (int i = 0; i <= array.Length - 2; i++)
                    {
                        //с тези редове оцветяваме в кафяво полетата, които се преглеждат в момента от алгоритъма
                        buttons[i].BackColor = Color.Chocolate;
                        Thread.Sleep(200);
                        buttons[i + 1].BackColor = Color.Chocolate;
                        Thread.Sleep(500);

                        //Тук влиза ако трябва да размени двете стойности (т.е. ако първото число е по-голямо от второто)
                        if (array[i] > array[i + 1])
                        {
                            temp = array[i + 1];
                            array[i + 1] = array[i];
                            array[i] = temp;

                            //Щом ще ги разменя ги оцветяваме в червено, за да си личи кога се случва нещо
                            buttons[i].BeginInvoke((MethodInvoker)delegate () { buttons[i].Text = temp.ToString(); });
                            buttons[i].BackColor = Color.Red;
                            buttons[i + 1].BeginInvoke((MethodInvoker)delegate () { buttons[i + 1].Text = array[i + 1].ToString(); });
                            buttons[i + 1].BackColor = Color.Red;
                            Thread.Sleep(500);

                        }
                        //След като е свършило с проверката на двете числа ги оцветяваме пак в бяло, за да преминем към следващите
                        buttons[i].BackColor = Color.AntiqueWhite;
                        buttons[i + 1].BackColor = Color.AntiqueWhite;
                    }
                }
                //Когато е готово сортирането изписваме надпис отдолу със вече сортирания масив
                label1.BeginInvoke((MethodInvoker)delegate () { label1.Text = "Сортираният масив е :\n" + string.Join(",", array); });
            });
        }        
    }
}
