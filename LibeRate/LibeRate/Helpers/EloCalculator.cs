using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Helpers
{
    //Elo Algorithm adapted from https://www.geeksforgeeks.org/elo-rating-algorithm/
    public class EloCalculator
    {
        private static float Probability(float rating1, float rating2)
        {
            return 1.0f * 1.0f
                / (1 + 1.0f
                    * (float)Math.Pow(
                        10, 1.0f * (rating1 - rating2) / 6.67f)); //Scale factor adjusted to fit results from 1-50
        }

        public static float[] CalculateElo(float rating1, float rating2, string result)
        {
            int K = 1; //testing found K of 1 to suit the scaled numbers
            float Prob2 = Probability(rating1, rating2);
            float Prob1 = Probability(rating2, rating1);
            
            if(result == "book1") //Book at rating1 wins
            {
                rating1 = rating1 + K * (1 - Prob1);
                rating2 = rating2 + K * (0 - Prob2);
            }
            else if (result == "book2") //Book at rating2 wins
            {
                rating1 = rating1 + K * (0 - Prob1);
                rating2 = rating2 + K * (1 - Prob2);
            }
            else //Draw
            {
                rating1 = rating1 + K * (0.5f - Prob1);
                rating2 = rating2 + K * (0.5f - Prob2);
            }

            return new float[] { rating1, rating2 };
        }
    }
}
