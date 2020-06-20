using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;


// this class deals with storing the data as each year passes, and turning the data into
// meaningful information for the player
// This class also handles the saving and loading of data to the local file, for the listBox at the start
// of the game. 


/* 
 * During a win condition, the following is evaluated - 
 * Land per person (start with 10, attempt to keep it above this number)
 * % of people who starved per year - keep strack of the starved figure per year, and average it. This is then rounded up to an int.
 * this is then added to the total NOT starved (100 is the max you can starve before failing, if 0 starve you get a 100 point bonus)
 * it then takes into account if your current ending bushel stock is enough to feed your population in the next year, and takes away a point for 
 * every starvation that would occur
 * 
 */

namespace Hammurabi
{
    public class Datastore
    {

        // variables declared
        public int[,] yearScore = new int[10,7];
        
        public int finalScore;



        //Score file to write to

        public string scoreFile = @"..\Scores.txt";




        // Function taht runs in tandom with the year pass, to hold data
        public int[,] KeepCount(int year, int stvd, int imm, int crnt, int lnd, int frm, int infest, int stk)
        {
                 // array is called from list above
              yearScore[year, 0] = stvd; // stvd is starved population
              yearScore[year, 1] = imm; // imm is new population
              yearScore[year, 2] = crnt;  // crnt is current population
              yearScore[year, 3] = lnd;   // lnd is current land
              yearScore[year, 4] = frm; // frm is farmed bushels
              yearScore[year, 5] = infest;  // infest is rat-eaten bushels
              yearScore[year, 6] = stk;  // stk is current bushels in stock

            return yearScore;

        }

       // Declaring variables that will be used in the final score calculation
        int popStarved;
        public double popPerAcre;
        public int[] harvest = new int[10];
        public int[] pop = new int[10];
        public int[] land = new int[10];


        // final score function - calculates the final score, and also creates 3 new arrays that are used in displaying end game data.
        public int FinalTotal(int[,] yearscore)
        {

            for (int i = 0; i < 10; i++)
            {
                harvest[i] = yearScore[i, 4];
                pop[i] = yearScore[i, 2];
                land[i] = yearScore[i, 3];
                popStarved += yearscore[i, 0];
            }

            try
            {
                popPerAcre = (yearscore[9, 3] / yearscore[9, 2]);
            }
            catch (DivideByZeroException)
            {
                popPerAcre = 0;
            }

            // Calls the population class, so the starvedPopulation function can be called to calculate the potential starved population next year
            Population popl = new Population();

            popl.starvedPopulation(yearScore[9, 2], yearScore[9, 6]);


            // Final score calculated based on rules set at head of class
            finalScore = (int.Parse(Math.Round(popPerAcre).ToString()) + (100 - popStarved) - (popl.starved));


            return finalScore;

        }

        // Writes score to file
        public void WriteScore(int finalScore)
        {

            File.AppendAllText(scoreFile, $"\n{finalScore}");

        }
        


      

    }
}
