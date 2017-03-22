using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trader
{
    public partial class Form1 : Form
    {
        private Random rand = new Random();
        private int currentValue;
        private int savedValue;
        private int min;
        private int max;
        private int timeCounter;
        private bool moveDone;
        private long money;
        private int typeOfDeal;
        private long dealValue;
        private static Timer myTimer = new Timer();
        
        public Form1()
        {
            InitializeComponent();
            this.putData("cur");
            this.createLine("line", 0);
            this.chart1.Series["line"].Enabled = false;
            this.chart1.Series["cur"].Color = Color.Blue;
            this.chart1.Series["line"].Color = Color.Blue;
            this.moveDone = false;
            this.money = Constantes.MONEY_START;
            this.label2.Visible = false;
            this.label3.Visible = false;

            myTimer.Tick += new EventHandler(gameUpdate);
            myTimer.Interval = 1000;
            myTimer.Start();
        }

        //function to generate values to the given series of the chart
        private void putData(string name)
        {
            this.currentValue = Constantes.PRICE_START + this.rand.Next(Constantes.PRICE_START_VARIATE);
            this.max = this.currentValue + Constantes.HALF_HEIGHT_OF_CHART;
            this.min = this.currentValue - Constantes.HALF_HEIGHT_OF_CHART;
            this.chart1.ChartAreas[name].AxisY.Minimum = this.min;
            this.chart1.ChartAreas[name].AxisY.Maximum = this.max;

            for (int i = 0; i < Constantes.LENGTH_OF_CHART; i++)
            {
                this.chart1.Series[name].Points.AddY(this.currentValue);
                this.newCurrentValue(Constantes.PRICE_USUAL_VARIATE);
                this.updateBorders();
            }            
        }

        //function that initialize a line with zeroes
        public void createLine(string name, int value)
        {
            for (int i = 0; i < Constantes.LENGTH_OF_CHART; i++)
            {
                this.chart1.Series[name].Points.AddY(value);
            }
        }

        //function that makes given line with a given value
        private void updateLine(string name, int value)
        {
            for (int i = 0; i < Constantes.LENGTH_OF_CHART; i++)
            {
                this.chart1.Series[name].Points.RemoveAt(0);
                this.chart1.Series[name].Points.AddY(value);
            }
        }

        //function that generate new current price every tick of the timer
        private void newCurrentValue(int difference)
        {
            //trying to be sure that price will not be negative
            int probability;
            if (this.currentValue < Constantes.LOWEST_POSSIBLE_PRICE)
            {
                probability = Constantes.LOW_VALUE_VARIATION_PROBABILITY;
            } else
            {
                probability = Constantes.USUAL_VARIATION_PROBABILITY;
            }

            //change current price of share
            if (rand.Next(Constantes.MAX_VARIATION_PROBABILITY_BOUND) > probability)
            {
                this.currentValue += this.rand.Next(difference);
            }
            else
            {
                this.currentValue -= this.rand.Next(difference);
            }
        }

        //function that updates the borders of the chart 
        //so the current value is always in the center of the screen
        private void updateBorders() {
            if ((this.currentValue - this.min) < Constantes.CLOSE_TO_BORDER || (this.max - this.currentValue) < Constantes.CLOSE_TO_BORDER)
            {
                this.max = this.currentValue + Constantes.HALF_HEIGHT_OF_CHART;
                this.min = this.currentValue - Constantes.HALF_HEIGHT_OF_CHART;
                this.chart1.ChartAreas["cur"].AxisY.Minimum = this.min;
                this.chart1.ChartAreas["cur"].AxisY.Maximum = this.max;
            }
        }

        //function that makes a deal of given type
        private void makeDeal(int type)
        {
            if (this.dealValue > 0 && this.money >= this.dealValue) //if it is possible to play
            {
                //set all the data of the deal
                this.money -= this.dealValue;
                this.timeCounter = Constantes.TIME_COUNTER_START; //start the timer
                this.moveDone = true;
                this.typeOfDeal = type; //set the type of the deal to understand the result
                this.savedValue = this.currentValue;

                //paint the line and data of the deal
                this.updateLine("line", this.savedValue);
                this.chart1.Series["line"].Enabled = true;
                if (type == Constantes.DEAL_HIGHER) //indicate what kind of deal player has done
                {
                    this.richTextBox1.Text += "H";
                }
                else //the deal is of type lower
                {
                    this.richTextBox1.Text += "L";
                }
                this.richTextBox1.Text += "\t" + this.savedValue + "\t\t$" + this.dealValue + "\t\t";
            }
            else if (this.money < this.dealValue)
            {
                MessageBox.Show("Sorry, but you do not have enough money to do this...");
            }
        }

        //click hanler of the "HIGHER" button of the game
        private void button2_Click(object sender, EventArgs e)
        {
            //get the stack of the player
            this.getStake();

            //make a deal of given type
            this.makeDeal(Constantes.DEAL_HIGHER);
        }

        //click hanler of the "LOWER" button of the game
        private void button3_Click(object sender, EventArgs e)
        {
            //get the stack of the player
            this.getStake();

            //make a deal of given type
            this.makeDeal(Constantes.DEAL_LOWER);
        }

        //function that takes a stake of the player
        private void getStake()
        {
            this.dealValue = 0;
            try
            {
                this.dealValue = Convert.ToInt64(this.textBox1.Text);
            } catch (System.FormatException)
            {
                MessageBox.Show("Sorry, your input is incorrect! Please, try again with some integer value that greater than zero.");            
            }
        }

        //function that calculates the result of the deal depending on type of deal and prices
        private long getResult(int current, int saved, int key, long deal)
        {
            long result = 0;
            if (key == Constantes.DEAL_HIGHER && current > saved)
            {
                return (long)(deal * 1.9f) + 1;
            }
            if (key == Constantes.DEAL_LOWER && current < saved)
            {
                return (long)(deal * 1.9f) + 1;
            }
            return result;
        }

        //function that updates all labels in the game
        private void updateLabels()
        {
            this.label1.Text = "Current price is " + this.currentValue;
            this.label2.Text = "Deal price is " + this.savedValue;
            this.label3.Text = "Deal ends in " + this.timeCounter;
            this.label4.Text = "$" + this.money;
        }

        //function that changes the color of the timer depending on time that left
        private void timerColor()
        {
            this.label3.ForeColor = Color.Green; //initially => green

            if (this.timeCounter <= Constantes.VALUE_COLOR_RED) //if it is too little time => red
            {
                this.label3.ForeColor = Color.Red;
            } else if (this.timeCounter <= Constantes.VALUE_COLOR_YELLOW) // so-so time => yellow
            {
                this.label3.ForeColor = Color.Orange;
            }
        }

        //function that calculates the color of the price line
        private void lineColor()
        {
            if (this.savedValue > this.currentValue)
            {
                this.chart1.Series["cur"].Color = Color.Red;
            }
            else
            {
                this.chart1.Series["cur"].Color = Color.Green;
            }
        }

        //function that executes every tick of the programm
        //the main logic is here
        private void gameUpdate(Object myObject, EventArgs myEventArgs)
        {
            //updating the data of chart every tick
            this.chart1.Series["cur"].Points.RemoveAt(0);
            this.newCurrentValue(Constantes.PRICE_USUAL_VARIATE);
            this.chart1.Series["cur"].Points.AddY(this.currentValue);
            this.updateBorders();

            //if player put his prediction
            if (this.moveDone)
            {
                //handle visualization of timer and data
                this.label2.Visible = true;
                this.label3.Visible = true;
                this.button2.Enabled = false;
                this.button3.Enabled = false;
                this.timerColor();                
                this.lineColor();

                //timer update
                this.timeCounter--;

                if (timeCounter == 0) //the end of a deal
                {
                    //change game state to usual
                    this.moveDone = false;
                    this.label2.Visible = false;
                    this.label3.Visible = false;
                    this.button2.Enabled = true;
                    this.button3.Enabled = true;
                    this.chart1.Series["line"].Enabled = false;
                    this.chart1.Series["cur"].Color = Color.Blue;

                    //calculate the result and print it to the history box
                    long result = this.getResult(this.currentValue, this.savedValue, this.typeOfDeal, this.dealValue);
                    this.money += result;
                    this.richTextBox1.Text += this.currentValue + "\t\t$" + result + "\n";
                }
            }
            //labels apdating
            this.updateLabels();
        }

        //button that decreases the value of a stake of the player
        private void button4_Click(object sender, EventArgs e)
        {
            long value = 0;
            try
            {
                value = Convert.ToInt64(this.textBox1.Text);
                value = (long)((float)value / Constantes.MULTIPLY_COEFICIENT);
                this.textBox1.Text = value.ToString();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Sorry, your input is incorrect! Please, try again with some integer value that greater than zero.");
            }
        }

        //button that increases the value of a stake of the player
        private void button5_Click(object sender, EventArgs e)
        {
            long value = 0;
            try
            {
                value = Convert.ToInt64(this.textBox1.Text);
                if (value > 0) {
                    if (value < Constantes.MAX_VALUE_OF_STAKE) //to avoid overflowing
                    {
                        value = (long)(value * Constantes.MULTIPLY_COEFICIENT);
                    }                    
                } else // if stake is 0 ake a default value of the stake
                {
                    value = Constantes.VALUE_OF_STAKE_DEFAULT;
                }                
                this.textBox1.Text = value.ToString();
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Sorry, your input is incorrect! Please, try again with some integer value that greater than zero.");
            }
        }
        
    }
}
