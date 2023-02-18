using System;
using System.Collections.Generic;
using System.Text;

namespace LibeRate.Helpers
{
    //Elo Algorithm courtesy of https://www.geeksforgeeks.org/elo-rating-algorithm/
    public class EloCalculator
    {
        private static float Probability(float rating1, float rating2)
        {
            return 1.0f * 1.0f
                / (1 + 1.0f
                    * (float)Math.Pow(
                        10, 1.0f * (rating1 - rating2) / 400));
        }

        public static float[] CalculateElo(float rating1, float rating2, int K, bool winner)
        {
            float Prob2 = Probability(rating1, rating2);
            float Prob1 = Probability(rating2, rating1);
            
            if(winner) //Book at rating1 wins
            {
                rating1 = rating1 + K * (1 - Prob1);
                rating2 = rating2 + K * (0 - Prob2);
            }
            else //Book at rating2 wins
            {
                rating1 = rating1 + K * (0 - Prob1);
                rating2 = rating2 + K * (1 - Prob2);
            }

            return new float[] { rating1, rating2 };
        }
    }
}
