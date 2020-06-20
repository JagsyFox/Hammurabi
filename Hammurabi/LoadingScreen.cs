using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Hammurabi
{
    public partial class LoadingScreen : Form
    {

        // 

        // This is the code for a loading screen
        public LoadingScreen()
        {
            InitializeComponent();

            // reads the score file, converts to int, then sorts them to write to the list box 
            lstScores.Items.Add("Top Scores");

            if (File.Exists("..\\Scores.txt") == false)
            {
                File.Create("..\\Scores.txt");
                onload.scoreFile = "..\\Scores.txt";
            }

  
            string[] readBack = new string[onload.scoreFile.Length];

            readBack = File.ReadAllLines(onload.scoreFile);

            int[] scores = new int[readBack.Length];

            for (int o = 0; o < readBack.Length; o++)
            {
                if (readBack[o] == "")
                {

                }
                else
                {
                    scores[o] = int.Parse(readBack[o]);
                }
            }

            // sorts the array, then puts it in highest > lowest order
            Array.Sort(scores);
            Array.Reverse(scores);
            

            foreach (int score in scores)
            {
                if (score != 0)
                {
                    lstScores.Items.Add(score);
                }
            }
        }


        Hammurabi load = new Hammurabi();
        Datastore onload = new Datastore();


        // loads the game itself
        private void btnNew_Click(object sender, EventArgs e)
        {
            this.Hide();
            load.NewGame();
            load.Show();

        }
    }
}
