using BA.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BA.Level
{
    public class LevelConfig
    {
        private const int UNIT_SIZE = 32;
        private const int BLOCK_STEP = 4;

        public int stationCapacity;
        public int maxSpawnEachBlock;
        public int maxBusAtSameTime;

        public Dictionary<PassengerColor, int> lane1Config; // Key: Color, Value: SpawnAmountMultiplier
        public Dictionary<PassengerColor, int> lane2Config; // Key: Color, Value: SpawnAmountMultiplier
        public Dictionary<PassengerColor, int> lane3Config; // Key: Color, Value: SpawnAmountMultiplier

        public List<List<PassengerBlockData>> spawnQueues;

        public void RegenerateSpawnQueues()
        {
            this.stationCapacity = 100;
            this.maxSpawnEachBlock = 12; // Higher mean more hard, because player have to calculate before tap.
            this.maxBusAtSameTime = 2;

            this.lane1Config = new()
            {
                { PassengerColor.Red, 1 },
                { PassengerColor.Green, 1 },
                { PassengerColor.Blue, 1 },
                { PassengerColor.Purple, 1 },
                { PassengerColor.Yellow, 1 },
            };

            this.lane2Config = new()
            {
                { PassengerColor.Red, 1 },
                { PassengerColor.Green, 1 },
                { PassengerColor.Blue, 1 },
                { PassengerColor.Purple, 1 },
                { PassengerColor.Yellow, 1 },
            };

            this.lane3Config = new()
            {
                { PassengerColor.Red, 1 },
                { PassengerColor.Green, 1 },
                { PassengerColor.Blue, 1 },
                { PassengerColor.Purple, 1 },
                { PassengerColor.Yellow, 1 },
            };

            var lane1 = GenerateBusesAwayList(this.lane1Config, this.maxSpawnEachBlock);
            var lane2 = GenerateBusesAwayList(this.lane2Config, this.maxSpawnEachBlock);
            var lane3 = GenerateBusesAwayList(this.lane3Config, this.maxSpawnEachBlock);
            this.spawnQueues = new() { lane1, lane2, lane3 };
        }

        public List<PassengerBlockData> GenerateBusesAwayList(Dictionary<PassengerColor, int> config, int maxVal)
        {
            var rawList = new List<PassengerBlockData>();

            foreach (var entry in config)
            {
                int targetTotal = entry.Value * UNIT_SIZE;
                int currentSum = 0;

                while (currentSum < targetTotal)
                {
                    int remaining = targetTotal - currentSum;
                    int upperLimit = Mathf.Min(maxVal, remaining);

                    int val;
                    if (remaining <= maxVal)
                    {
                        val = remaining;
                    }
                    else
                    {
                        int maxSteps = upperLimit / BLOCK_STEP;
                        val = Random.Range(1, maxSteps + 1) * BLOCK_STEP;
                    }

                    rawList.Add(new PassengerBlockData { color = entry.Key, amount = val });
                    currentSum += val;
                }
            }

            ShuffleList(rawList);
            FixAdjacentColors(rawList);

            return rawList;
        }

        private void ShuffleList(List<PassengerBlockData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        private void FixAdjacentColors(List<PassengerBlockData> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].color == list[i + 1].color)
                {
                    for (int j = 0; j < list.Count; j++)
                    {
                        if (list[j].color != list[i].color &&
                            (i + 2 >= list.Count || list[j].color != list[i + 2].color) &&
                            (j == 0 || list[j - 1].color != list[i + 1].color) &&
                            (j == list.Count - 1 || list[j + 1].color != list[i + 1].color))
                        {
                            var temp = list[i + 1];
                            list[i + 1] = list[j];
                            list[j] = temp;
                            break;
                        }
                    }
                }
            }
        }
    }
}