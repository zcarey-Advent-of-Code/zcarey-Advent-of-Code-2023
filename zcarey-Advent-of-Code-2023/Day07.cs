using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace zcarey_Advent_of_Code_2023
{

    internal class Day07 : AdventOfCodeProblem
    {
        public object Part1(string input)
        {
            List<Hand> hands = ParseInput(input).ToList();

            hands.Sort();
            return hands
                .WithIndex()
                .Select(x => (x.Index + 1) * x.Element.Bid)
                .Sum(); 
        }

        public object Part2(string input)
        {
            return "";
        }

        static Dictionary<char, int> CardValue = new() {
            {'2', 0 },
            {'3', 1 },
            {'4', 2 },
            {'5', 3 },
            {'6', 4 },
            {'7', 5 },
            {'8', 6 },
            {'9', 7 },
            {'T', 8 },
            {'J', 9 },
            {'Q', 10 },
            {'K', 11 },
            {'A', 12 }
        };

        enum HandType
        {
            HighCard = 0,
            OnePair = 1,
            TwoPair = 2,
            ThreeOfAKind = 3,
            FullHouse = 4,
            FourOfAKind = 5,
            FiveOfAKind = 6
        }

        struct Hand : IComparable<Hand>
        {
            public HandType HandType;
            public int[] Cards; // Cards after being converted to point values
            public long Bid;

            public static Hand Parse(string cards, int bid)
            {
                Hand hand = new Hand();
                hand.Cards = new int[cards.Length];
                hand.Bid = bid;

                int[] cardCounts = new int[CardValue.Count];
                for(int i = 0; i < cards.Length; i++)
                {
                    char card = cards[i];
                    int value = CardValue[card];
                    hand.Cards[i] = value;
                    cardCounts[value]++;
                }

                // Determine the hand type
                int maxCount = cardCounts[0];
                int secondHighestCount = int.MinValue;
                for(int i = 1; i < cardCounts.Length; i++) 
                { 
                    if (cardCounts[i] > maxCount)
                    {
                        secondHighestCount = maxCount;
                        maxCount = cardCounts[i];
                    } else if (cardCounts[i] > secondHighestCount)
                    {
                        secondHighestCount = cardCounts[i];
                    }
                }

                if (maxCount == 3 && secondHighestCount == 2)
                {
                    hand.HandType = HandType.FullHouse;
                } else if (maxCount == 2 && secondHighestCount == 2)
                {
                    hand.HandType = HandType.TwoPair;
                } else if (maxCount == 2)
                {
                    hand.HandType = HandType.OnePair;
                } else if (maxCount == 5)
                {
                    hand.HandType = HandType.FiveOfAKind;
                } else if (maxCount == 4)
                {
                    hand.HandType = HandType.FourOfAKind;
                } else if (maxCount == 3)
                {
                    hand.HandType = HandType.ThreeOfAKind;
                } else if (maxCount == 1)
                {
                    hand.HandType = HandType.HighCard;
                }
                else
                {
                    throw new Exception();
                }
                return hand;
            }

            public int CompareTo(Hand other)
            {
                int typeCompare = (int)this.HandType - (int)other.HandType;
                if (typeCompare != 0)
                {
                    return typeCompare;
                }

                // Compare each card for equality
                for (int i = 0; i < Cards.Length; i++)
                {
                    int cardCompare = Cards[i] - other.Cards[i];
                    if (cardCompare != 0)
                    {
                        return cardCompare;
                    }
                }

                return 0; // Welp, they are equal I guess.
            }
        }
   
        IEnumerable<Hand> ParseInput(string input)
        {
            foreach(string line in input.GetLines())
            {
                string[] args = line.Split();
                yield return Hand.Parse(args[0], int.Parse(args[1]));
            }
        }
    }

}
