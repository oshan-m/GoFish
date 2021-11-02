using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace GoFish
{
    public class CardComparerByValue: IComparer<Card>
    {
        public CardComparerByValue()
        {
        }

        public int Compare(Card x, Card y)
        {
            if (x.Suit < y.Suit)
                return -1;
            else if (x.Suit > y.Suit)
                return 1;
            else
                if (x.Value < y.Value)
                    return -1;
                else if (x.Value > y.Value)
                    return 1;
                else
                    return 0;
        }
    }
}
