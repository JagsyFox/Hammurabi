using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hammurabi

    // Land price flucuates randomly between 17 and 26(bushels), changing year on year

    // threshold for harvested land is (1, 5) per acre

    // each person can till at most 10 acres of land, so the land sowed calculation needs
    // to take population into account

    // rats are more likely to appear the higher the bushel storage. The number of bushels consumed relates
    // to current stock, and a random chance. Rats 8can consume up to a 3rd of the current storage


   
{
    public class City
    

    {
        // Random chance declared, for use in all relevant class random methods.
        Random chance = new Random();

        // variables declared
        public int land, prices, bushels, consumed=0, farmed, highestLand, avgSowed, totalHarvested, largestHarvest, totalConsumed;

        // Escalating odds of rats consuming stock as stock increases
        public int ratInfestation(int bushels)
        {
            int infestChance;

            infestChance = chance.Next(1, 10);

            if (bushels == 0)
            {
                consumed = 0;
            }

            // escalating odds worked out 
            else if (bushels > 1 && bushels < 500)
            {
                if (infestChance > 1 && infestChance < 2)
                {
                    consumed = chance.Next(0, (bushels / 3));
                }
                
            }

            else if (bushels > 501 && bushels < 2000)
            {
                if (infestChance > 1 && infestChance < 3)
                {
                    consumed = chance.Next(0, (bushels / 3));
                }
            }

            else if (bushels > 2001 && bushels < 3500)
            {
                if (infestChance > 1 && infestChance < 4)
                {
                    consumed = chance.Next(0, (bushels / 3));
                }
            }

            else if (bushels > 3501 && bushels < 5000)
            {
                if (infestChance > 1 && infestChance < 5)
                {
                    consumed = chance.Next(0, (bushels / 3));
                }
            }
            else
            {
                if (infestChance > 1 && infestChance < 6)
                {
                    consumed = chance.Next(0, (bushels / 3));
                }
            }
           
            

            return consumed;

        }



        // deals with the random price of land
        public int landPrice()
        {
            
            prices = chance.Next(17, 26);

            return prices;
        }

        // returns the sowed amount of land based on given values
        public int landSowed(int people, int land)
        {
            int sowed;

            sowed = land / people;
            return sowed;
        }

        // returns the new land after selling or buying land
        public int landChange(int acres, int inputted)
        {
            int newLand;
            newLand = acres + inputted;
            return newLand;
        }

        // Uses the random function to work out the yielded crops
        public int landFarmed(int acres)
        {
            int yielded;

            yielded = chance.Next(1, 5);

            farmed = yielded * acres;

            return farmed;

        }

    }
}
