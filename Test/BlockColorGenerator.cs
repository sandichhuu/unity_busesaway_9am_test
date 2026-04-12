using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesAwayLogic
{
    public enum ColorType { Red, Blue, Green, Yellow, Purple, Orange }

    public class ColorBlock
    {
        public ColorType Type { get; set; }
        public int Value { get; set; }
        public override string ToString() => $"[{Type}: {Value}]";
    }

    public class Program
    {
        private static Random _random = new Random();
        private const int UNIT_SIZE = 32;
        private const int BLOCK_STEP = 4;

        public static void Main(string[] args)
        {
            // 1. Cấu hình Input: Số nhân cho từng màu
            var config = new Dictionary<ColorType, int>
            {
                { ColorType.Red, 2 },    // Tổng 64
                { ColorType.Blue, 3 },   // Tổng 96
                { ColorType.Green, 2 },  // Tổng 64
                { ColorType.Yellow, 1 }  // Tổng 32
            };

            int maxBlockValue = 12; // Khống chế độ khó

            // 2. Sinh danh sách
            List<ColorBlock> finalResult = GenerateBusesAwayList(config, maxBlockValue);

            // 3. In kết quả và kiểm chứng
            PrintAndValidate(finalResult, config);
            
            Console.WriteLine("\nNhấn phím bất kỳ để thoát...");
            Console.ReadKey();
        }

        public static List<ColorBlock> GenerateBusesAwayList(Dictionary<ColorType, int> config, int maxBlockValue)
        {
            List<ColorBlock> rawList = new List<ColorBlock>();

            // Bước 1: Sinh các block thô đảm bảo tổng và bội của 4
            foreach (var entry in config)
            {
                int targetTotal = entry.Value * UNIT_SIZE;
                int currentSum = 0;

                while (currentSum < targetTotal)
                {
                    int remaining = targetTotal - currentSum;
                    int upperLimit = Math.Min(maxBlockValue, remaining);
                    
                    int val;
                    if (remaining <= maxBlockValue)
                    {
                        val = remaining;
                    }
                    else
                    {
                        int maxSteps = upperLimit / BLOCK_STEP;
                        val = _random.Next(1, maxSteps + 1) * BLOCK_STEP;
                    }

                    rawList.Add(new ColorBlock { Type = entry.Key, Value = val });
                    currentSum += val;
                }
            }

            // Bước 2: Trộn ngẫu nhiên sơ bộ
            List<ColorBlock> shuffledList = rawList.OrderBy(x => _random.Next()).ToList();

            // Bước 3: Khử trùng lặp màu liền kề bằng thuật toán Swap
            FixAdjacentColors(shuffledList);

            return shuffledList;
        }

        private static void FixAdjacentColors(List<ColorBlock> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                // Nếu phát hiện 2 block cạnh nhau cùng màu
                if (list[i].Type == list[i + 1].Type)
                {
                    bool fixedSpot = false;
                    // Tìm một block khác trong danh sách để tráo đổi
                    for (int j = 0; j < list.Count; j++)
                    {
                        // Điều kiện tráo đổi: Block tại j không được cùng màu với i
                        // và khi đặt vào vị trí mới (i+1) không được trùng với i+2
                        if (list[j].Type != list[i].Type &&
                            (i + 2 >= list.Count || list[j].Type != list[i + 2].Type) &&
                            (j == 0 || list[j - 1].Type != list[i + 1].Type) &&
                            (j == list.Count - 1 || list[j + 1].Type != list[i + 1].Type))
                        {
                            var temp = list[i + 1];
                            list[i + 1] = list[j];
                            list[j] = temp;
                            fixedSpot = true;
                            break;
                        }
                    }
                }
            }
        }

        private static void PrintAndValidate(List<ColorBlock> list, Dictionary<ColorType, int> config)
        {
            Console.WriteLine("=== DANH SÁCH BLOCK MÀU (BUSES AWAY) ===");
            for (int i = 0; i < list.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {list[i]}");
            }

            Console.WriteLine("\n=== KIỂM CHỨNG LOGIC ===");
            
            // Kiểm tra tổng mỗi màu
            foreach (var entry in config)
            {
                int sum = list.Where(b => b.Type == entry.Key).Sum(b => b.Value);
                bool totalValid = (sum == entry.Value * UNIT_SIZE);
                Console.WriteLine($"Màu {entry.Key}: Tổng {sum}/{entry.Value * UNIT_SIZE} -> {(totalValid ? "PASS" : "FAIL")}");
            }

            // Kiểm tra trùng lặp liền kề
            bool adjacentValid = true;
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].Type == list[i + 1].Type)
                {
                    Console.WriteLine($"LỖI: Trùng màu tại vị trí {i} và {i+1} ({list[i].Type})");
                    adjacentValid = false;
                }
            }
            Console.WriteLine($"Liền kề không trùng màu: {(adjacentValid ? "PASS" : "FAIL")}");

            // Kiểm tra bội của 4
            bool stepValid = list.All(b => b.Value % BLOCK_STEP == 0);
            Console.WriteLine($"Tất cả là bội của 4: {(stepValid ? "PASS" : "FAIL")}");
        }
    }
}