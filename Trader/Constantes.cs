using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trader
{
    class Constantes
    {
        public const int MAX_VARIATION_PROBABILITY_BOUND = 100;
        public const int USUAL_VARIATION_PROBABILITY = 49;
        public const int LOW_VALUE_VARIATION_PROBABILITY = 30;
        public const int LOWEST_POSSIBLE_PRICE = 1000;
        public const int CLOSE_TO_BORDER = 100;
        public const int HALF_HEIGHT_OF_CHART = 500;
        public const int LENGTH_OF_CHART = 60;
        public const int MONEY_START = 10000;
        public const int PRICE_START = 2500;
        public const int PRICE_START_VARIATE = 1000;
        public const int PRICE_USUAL_VARIATE = 100;
        public const int TIME_COUNTER_START = 30;
        public const int VALUE_COLOR_YELLOW = 20;
        public const int VALUE_COLOR_RED = 10;
        public const int DEAL_HIGHER = 1;
        public const int DEAL_LOWER = 0;
        public const float MULTIPLY_COEFICIENT = 2.5f;
        public const int VALUE_OF_STAKE_DEFAULT = 100;
        public const long MAX_VALUE_OF_STAKE = 100000000000;
    }
}
