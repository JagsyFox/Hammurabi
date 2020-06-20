using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//  Each person has to be fed 20 bushels in order to survive, or they starve.
//  
//


namespace Hammurabi
{

    public class Population
    {

        // These variables are used with population methods, and in main class calls.
        public int population;

        public int starved = 0, newPeople = 0, highestPop =0, lowestPop = 0, plagueVictims = 0;


        public bool cityPlague = false;


        /* Starvation - method that calculates if anybody starved based on current population and bushels given
         Condition checks population vs bushels given, if below threshold calculates amount of people
         starved, then modifies the class variable 'starved' */

        //Random class is used to calculate influx of people, between (0, 10)

        // A plague can occur, killing half the population. Since working this backwards can be 
        // difficult, a customer calculation is used in this verion of Hammurabi (12%)




        // Below method calculates bushels entered vs current population. 
        // returned value is total starved population, which will be applied in main class
        public int starvedPopulation(int people, int bushelsGiven)
        {
            if (bushelsGiven == 0)
            {
                starved = people;
            }

            if ((bushelsGiven / people) < 20)
            {
                starved = (people - (bushelsGiven / 20));
            }
            else
            {
                starved = 0;
            }
            return starved;
            
        }


        // This method uses a random function to generate a random influx of new people

        public int newPopulation()
        {
            Random infx = new Random();
            newPeople = infx.Next(1, 10);
            
            return newPeople;
        }


        // This method creates a 10% chance of the city succumbing to a plague, returning a boolean 
        public bool plague()
        {
            int plagueCheck;

            Random plague = new Random();
            plagueCheck = plague.Next(1, 100);

            if (plagueCheck / 10 == 0)
            {
                cityPlague = true;
            }
            else
            {
                cityPlague = false;
            }


            return cityPlague;


        }





    }

    


}

