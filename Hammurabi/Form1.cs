using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace Hammurabi


// Game is ended if you starve (100) people total, across multiple years

/* the starved calculation takes place BEFORE the check on how many bushels you've fed the population
and before new people are taken into account. this is important for working out how the year is scored. */

{ 
    public partial class Hammurabi : Form
    {
        public Hammurabi()
        {
            InitializeComponent();
        }



        // class calls 
        Population Pop = new Population();
        City Res = new City();
        Datastore Data = new Datastore();

        // game variables declared
        int year;
        int bushelsGiven;
        int acresBought;
        int seedPlanted;
        int totalStarved;
        int bushelsTest;
        int landTest;
        double landPerPop;
        int currentSowed;
        bool gameEnded = false;

        // chart data 
        Series harvested;
        Series consumed;
        Series population;
        Series bushels;
        Series acres;



        // new game method, resets all variables to standard.
        public void NewGame()
        {    
            year = 1;
            Res.land = 1000;
            Res.bushels = 2800;
            Pop.population = 100;
            Pop.highestPop = Pop.population;
            Pop.lowestPop = Pop.population;
            Res.landPrice();
            landPerPop = Res.land / Pop.population;
            Res.highestLand = Res.land;
            totalStarved = 0;
            Data.KeepCount((year - 1), Pop.starved, Pop.newPeople, Pop.population, Res.land, Res.farmed, Res.consumed, Res.bushels);

            chartHarvest.Titles.Clear();
            chartHarvest.Titles.Add("Harvested Bushels");

            chartHarvest.Series.Clear();
            harvested = chartHarvest.Series.Add("Harvested");
            harvested.Color = Color.Red;

            consumed = chartHarvest.Series.Add("Consumed");
            consumed.Color = Color.Black;

            picBushels.Size = new Size((Res.bushels / 30), 60);
            picLand.Size = new Size((Res.land / 4), 60);
            picPopulation.Size = new Size((Pop.population * 2), 60);

            chartTotals.Size = new Size(0, 0);




        }

        // Starts a new game on form load, as this form is only accessed from the loading screen
        private void Hammurabi_Load(object sender, EventArgs e)
        {
            NewGame();
            Display();
        }

        
        // New game button 
        private void BtnNew_Click(object sender, EventArgs e)
        {
            NewGame();
            Display();
        }

        // function used when a year is successfully passed, calling all class methods.
        private int YearPass(int bushels, int acres, int seeded)
        {

            // Used on the display of the 'land worth' pictures, will be reset to show a value in the function
            pic17.Visible = false;
            pic18.Visible = false;
            pic19.Visible = false;
            pic20.Visible = false;
            pic21.Visible = false;
            pic22.Visible = false;
            pic23.Visible = false;
            pic24.Visible = false;
            pic25.Visible = false;
            pic26.Visible = false;


            // checks if years aren't fully passed
            if (year != 10)
            {
                // Data store array update
                Data.KeepCount(year, Pop.starved, Pop.newPeople, Pop.population, Res.land, seedPlanted, Res.consumed, Res.bushels);

                //incriments
                year++;

                //Land class method that calculates random price of land
                Res.landPrice();
     
                // starved method is called, calculated, then applied to population
                Pop.starvedPopulation(Pop.population, bushelsGiven);
                Pop.population = Pop.population - Pop.starved;
                totalStarved += Pop.starved;
                

                // Method call to calculate new people coming in
                Pop.newPopulation();
                Pop.population = Pop.population + Pop.newPeople;

                if (Pop.population == 0)
                {
                    EndGame();
                }


                // calculates how many bushels were used up previous turn
                Res.bushels = Res.bushels - (bushelsGiven + seedPlanted);

                // calls the resource class methods on how many bushels were farmed & how many rats consumed
                Res.landFarmed(seedPlanted);
                Res.ratInfestation(Res.bushels);

                // takes the figures returned from the two methods above and adds to bushel total
                Res.bushels = Res.bushels + (Res.farmed - Res.consumed);

                // Updates land value
                Res.land += acresBought;


                /* ------ visual components ------*/

                // Updates the chart
                harvested.Points.Add(Res.farmed);
                consumed.Points.Add(Res.consumed);

                // Moves the year progress bar along
                progYear.PerformStep();

                // Changes the size of the picturebox to a multiple of 50, to display new people
                picNewPop.Size = new Size((Pop.newPeople * 50), 50);

                // Plague check
                Pop.plague();
                if (Pop.cityPlague == true)
                {
                    Pop.population = Pop.population / 2;
                    Pop.plagueVictims += Pop.population;
                    MessageBox.Show("A plague struck your city, killing half the population!");
                }




                // modifies the highest population count depending on the current population
                if (Pop.population > Pop.highestPop)
                {
                    Pop.highestPop = Pop.population;
                }
                else if (Pop.population < Pop.lowestPop)
                {
                    Pop.lowestPop = Pop.population;
                }

                // modifies other score-keeping variables based on current values
                currentSowed += seedPlanted;
                Res.avgSowed = currentSowed /( year-1);

                Res.totalConsumed += Res.consumed;
                Res.totalHarvested += Res.farmed;

                if (Res.farmed > Res.largestHarvest)
                {
                    Res.largestHarvest = Res.farmed;
                }

                try
                {
                    landPerPop = Res.land / Pop.population;
                }
                catch (DivideByZeroException)
                {
                    EndGame();
                }


                // Below functions modify picture boxes to reflect current totals, with a sanity check to ensure the picture boxes 
                // do not exceed 300 pixels.
                if ((Res.bushels / 30) > 300)
                {
                    picBushels.Size = new Size(300, 60);
                }
                else
                {
                    picBushels.Size = new Size((Res.bushels / 30), 60);
                }

                if ((Pop.population * 2) > 300)
                {
                    picPopulation.Size = new Size(300, 60);
                }
                else
                {
                    picPopulation.Size = new Size((Pop.population * 2),60);
                }

                if ((Res.land / 4) > 300)
                {
                    picLand.Size = new Size(300, 60);
                }
                else
                {
                    picLand.Size = new Size((Res.land / 4), 60);
                }
                
                if (Res.bushels < 0)
                {
                    Res.bushels = 0;
                }
                

                // shows the bushel prices as a picture display, case switch on value.
                switch (Res.prices)
                {
                    case 17:
                        pic17.Visible = true;
                        break;
                    case 18:
                        pic18.Visible = true;
                        goto case 17;
                    case 19:
                        pic19.Visible = true;
                        goto case 18;
                    case 20:
                        pic20.Visible = true;
                        goto case 19;
                    case 21:
                        pic21.Visible = true;
                        goto case 20;
                    case 22:
                        pic22.Visible = true;
                        goto case 21;
                    case 23:
                        pic23.Visible = true;
                        goto case 22;
                    case 24:
                        pic24.Visible = true;
                        goto case 23;
                    case 25:
                        pic25.Visible = true;
                        goto case 24;
                    case 26:
                        pic26.Visible = true;
                        goto case 25;
                }


            }

            
            // ends game when 10 years have passed
            else
            {
                EndGame();
                Data.FinalTotal(Data.yearScore);
            }

            // Ends the game if over 100 people starved total.
            if (totalStarved >= 100)
            {

                string lossMsg = "You starved over 100 people, and were booted from office!";
                string lossMessage = "Would you like to restart?";
                var result = MessageBox.Show(lossMsg, lossMessage, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    NewGame();
                }
                else
                {
                    Close();
                }
                
            }

            
            return year;


        }


        // Updates the labels with the game variables
        private void Display()
        {
            lblYear.Text = $"in year {year}, ";
            lblStarved.Text = $"{Pop.starved} people starved";
            lblInflux.Text = $"{Pop.newPeople} people entered the city";
            lblPopulation.Text = $"{Pop.population} is the current population";
            lblAcres.Text = $"The city currently owns {Res.land} acres";
            lblFarmed.Text = $"You farmed {Res.farmed} bushels";
            lblRats.Text = $"Rats ate {Res.consumed} bushels";
            lblBushels.Text = $"You now have {Res.bushels} bushels";
            lblLandVal.Text = $"land is currently trading at {Res.prices}";
            lblBushDisplay.Text = $"You will have {Res.bushels} remaining";
            lblLandDisplay.Text = $"You will have {Res.land} remaining";
        }


        // Function to move the game through the years
        private void BtnMakeItSo_Click(object sender, EventArgs e)
        {
            // Converts the bushelsTest into the current bushels, so user can confirm what they are using visually.
            Res.bushels = bushelsTest;
            Res.land = landTest;
            txtSellLand.Text = "0";

            // Godmode cheat - used for debugging purposes
            if (txtSellLand.Text == "Midas")
            {
                Res.land = 99999;
                Res.bushels = 99999;
                Pop.population = 100;
                txtSellLand.Text = "0"; // this stops the usual catch validation failing
            }

                try
                {
                    acresBought = int.Parse(txtSellLand.Text);
                    bushelsGiven = int.Parse(txtBushes.Text);
                    seedPlanted = int.Parse(txtSeed.Text);
                }
                catch
                {
                    MessageBox.Show("You must enter a whole number");
                    return;
                }


            /* ---------- Validation! ---------- */

            // below if statements control the inputs vs the current variable values
            if (bushelsGiven > Res.bushels)
                {
                    MessageBox.Show($"Hammurabi, you do not have that many bushels! Current stock is {Res.bushels} ");
                    return;
                }

                if (seedPlanted > Res.land)
                {
                    MessageBox.Show($"Hammurabi, you do not have that much land to farm! Currently, we own {Res.land}");
                    return;
                }

                if ((seedPlanted + bushelsGiven) > Res.bushels)
                {
                    MessageBox.Show($"Hammurabi, you do not have that many bushels! Current stock is {Res.bushels} ");
                    return;
                }

                if (acresBought + Res.land < 0)
                {
                    MessageBox.Show($"Hammurabi, you do not have that much land to sell! Currently, we own {Res.land}");
                    return;
                }

                if ((seedPlanted / 10) > Pop.population)
                {
                    MessageBox.Show($"Hammurabi, you don't have enough people to till that much land!");
                    return;
                }
                




            // calls the yearPass function, to check year is not 10
            YearPass(bushelsGiven, acresBought, seedPlanted);

            // Resets bushelsTest to current bushels, after yearPass has returned it's calculations.
            bushelsTest = Res.bushels;


            // Updates the labels
            Display();



        }

        // allows the user to test the outcome of their decisions without confirming actions
        // modifies the variable bushelsTest, rather than actual bushels



        // form closing check 
        private void Hammurabi_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (gameEnded == false)
            {
                string closeMessage = "Would you like to close? Current game will be lost";
                var result = MessageBox.Show(closeMessage, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

            else
            {
                string closeMessage = "Would you like to restart?";
                var result = MessageBox.Show(closeMessage, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    NewGame();
                    return;
                }
                else if (result == DialogResult.No)
                {
                    Close();
                }
            }

        }

        /*---------methods for calling current info relevant to each resource when image clicked----------*/

        private void PicPopulation_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Pop.starved} people have starved so far.\n{Pop.highestPop} is the highest your population has been.\n{Pop.lowestPop} is the lowest your population has been.\n{Pop.plagueVictims} people have succumbed to the plague");
            return;
                }

        private void PicLand_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"{Res.highestLand} is the most land you have owned.\n{Res.avgSowed} is the amount of land you have sowed per year on average.\nYou currently have {landPerPop} land per person.");
            return;
        }

        private void PicBushels_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"You have harvested {Res.totalHarvested} total.\n{Res.largestHarvest} is your largest harvest.\n{Res.totalConsumed} bushels have been eaten by rats.\n");
        }

        // Function that ends the game, used on failed years or after 10 years
        private void EndGame()
        {
            gameEnded = true;

            endPanel.Visible = true;
            lblPopPerAcre.Visible = true;

            // Functions for turning the gathered data into a final score, and writing to file
            Data.FinalTotal(Data.yearScore);
            Data.WriteScore(Data.finalScore);

            // Shows a score on the label
            lblPopPerAcre.Text = $"You had {Data.popPerAcre} per person";

            // turns the input area into the graph area
            chartTotals.Size = new Size(657, 226);



            // Fills the chart with data 
            chartTotals.Titles.Clear();
            chartTotals.Titles.Add("People, land and bushels");

            chartTotals.Series.Clear();
            population = chartTotals.Series.Add("Population");
            population.Color = Color.Blue;

            bushels = chartTotals.Series.Add("Bushels (x100)");
            bushels.Color = Color.Yellow;

            acres = chartTotals.Series.Add("Acres (x10)");
            acres.Color = Color.Green;

            chartTotals.ChartAreas[0].AxisX.Maximum = 11;
            chartTotals.ChartAreas[0].AxisX.Minimum = 0;

            chartTotals.ChartAreas[0].AxisY.Maximum = 220;
            chartTotals.ChartAreas[0].AxisY.Minimum = 0;


            // Adds data to the 3 populating series
            for (int i = 0; i < 10; i++)
            {

                population.Points.Add(Data.pop[i]);

                bushels.Points.Add((Data.harvest[i] / 10));

                acres.Points.Add((Data.land[i] / 10));

            }

            // highlights the chart's highs and lows
            population.Points.FindMinByValue().Color = Color.DarkBlue;
            population.Points.FindMinByValue().Label = "Min";
            population.Points.FindMaxByValue().Color = Color.LightBlue;
            population.Points.FindMaxByValue().Label = "Max";

            bushels.Points.FindMinByValue().Color = Color.Orange;
            bushels.Points.FindMinByValue().Label = "Min";
            bushels.Points.FindMaxByValue().Color = Color.LightYellow;
            bushels.Points.FindMaxByValue().Label = "Max";

            acres.Points.FindMinByValue().Color = Color.DarkGreen;
            acres.Points.FindMinByValue().Label = "Min";
            acres.Points.FindMaxByValue().Color = Color.LightGreen;
            acres.Points.FindMaxByValue().Label = "Max";



        }

        // Radio buttons for manipulating the colour of the harvested/consumed chart
        private void radChartOne_CheckedChanged(object sender, EventArgs e)
        {
            harvested.Color = Color.Red;
            consumed.Color = Color.Black;
            radChartTwo.Checked = false;

        }

        private void radChartTwo_CheckedChanged(object sender, EventArgs e)
        {
            harvested.Color = Color.DarkBlue;
            consumed.Color = Color.Gray;
            radChartOne.Checked = false;
        }


        // Radio buttons for modifying the shown values on the end game chart
        private void radPopulation_CheckedChanged(object sender, EventArgs e)
        {
            population.Enabled = true;
            bushels.Enabled = false;
            acres.Enabled = false;
            radHarvest.Checked = false;
            radLand.Checked = false;


        }

        private void radHarvest_CheckedChanged(object sender, EventArgs e)
        {
            population.Enabled = false;
            bushels.Enabled = true;
            acres.Enabled = false;
            radPopulation.Checked = false;
            radLand.Checked = false;
        }

        private void radLand_CheckedChanged(object sender, EventArgs e)
        {
            population.Enabled = false;
            bushels.Enabled = false;
            acres.Enabled = true;
            radHarvest.Checked = false;
            radPopulation.Checked = false;
        }

        private void radReset_CheckedChanged(object sender, EventArgs e)
        {
            population.Enabled = true;
            bushels.Enabled = true;
            acres.Enabled = true;
            radHarvest.Checked = false;
            radPopulation.Checked = false;
            radLand.Checked = false;
        }


        private void TxtSellLand_Leave(object sender, EventArgs e)
        {
            bushelsTest = Res.bushels;
            landTest = Res.land;
            lblBushDisplay.Text = $"You will have {bushelsTest} remaining";
            lblLandDisplay.Text = $"You will have {landTest} remaining";

            if (txtSellLand.Text == "Midas")
            {
                lblBushDisplay.Text = "God mode enabled";
                bushelsTest = 99999;
                landTest = 99999;
                txtSellLand.Text = "0"; // this stops the usual catch validation failing
            }

            else
            {
                if (int.TryParse(txtSellLand.Text, out int tst) == true && int.Parse(txtSellLand.Text) <= 0)
                {
                    bushelsTest = Res.bushels - (int.Parse(txtSellLand.Text) * Res.prices);
                    landTest = Res.land + int.Parse(txtSellLand.Text);
                    lblBushDisplay.Text = $"You will have {bushelsTest} remaining";
                    lblLandDisplay.Text = $"You will have {landTest} remaining";

                }


                else if (int.TryParse(txtSellLand.Text, out int tt) == true && int.Parse(txtSellLand.Text) > 0)
                {
                    bushelsTest = Res.bushels - (int.Parse(txtSellLand.Text) * Res.prices);
                    landTest = Res.land + int.Parse(txtSellLand.Text);
                    lblBushDisplay.Text = $"You will have {bushelsTest} remaining";
                    lblLandDisplay.Text = $"You will have {landTest} remaining";

                }

                else
                {
                    MessageBox.Show("Invalid land input, must be a whole number");
                    return;
                }
            }
        }
        // Function created to alert user when invalid input is present on leaving a text box

        private void txtBushes_Leave(object sender, EventArgs e)
        {

            lblBushDisplay.Text = $"You will have {bushelsTest} remaining";
            lblLandDisplay.Text = $"You will have {landTest} remaining";


            if (String.IsNullOrWhiteSpace(txtBushes.Text))
            {
                MessageBox.Show("Invalid bushels input, must be a whole number");
                return;
            }
            else if (int.TryParse(txtBushes.Text, out int feedTest) == false)
            {
                MessageBox.Show("Invalid bushels input, must be a whole number");
                return;
            }
            else if (feedTest < 0)
            {
                MessageBox.Show("Invalid bushels input, must be a whole number");
                txtBushes.Text = "0";
                return;
            }
            else
            {
                if (int.TryParse(txtSeed.Text, out int seed) == true && int.Parse(txtSeed.Text) > 0)
                {
                    lblBushDisplay.Text = $"You will have {bushelsTest - feedTest - seed} remaining";
                }
                else
                {
                    lblBushDisplay.Text = $"You will have {bushelsTest - feedTest} remaining";
                }
                
            }

        }

        private void txtSeed_Leave(object sender, EventArgs e)
        {


            lblBushDisplay.Text = $"You will have {bushelsTest} remaining";
            lblLandDisplay.Text = $"You will have {landTest} remaining";

            if (String.IsNullOrWhiteSpace(txtSeed.Text))
            {
                MessageBox.Show("Invalid planting input, must be a whole number");
                return;
            }
            else if (int.TryParse(txtSeed.Text, out int seedTest) == false)
            {
                MessageBox.Show("Invalid planting input, must be a whole number");
                return;
            }
            else if (seedTest < 0)
            {
                MessageBox.Show("Invalid planting input, must be a whole number");
                txtSeed.Text = "0";
                return;
            }
            else
            {
                if (int.TryParse(txtBushes.Text, out int feed) == true && int.Parse(txtBushes.Text) > 0)
                {
                    lblBushDisplay.Text = $"You will have {bushelsTest - seedTest - feed} remaining";
                }
                else
                {
                    lblBushDisplay.Text = $"You will have {bushelsTest - seedTest} remaining";
                }
            }

        }
    }



    }

    


    


