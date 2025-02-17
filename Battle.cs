﻿using static SpartaDungeonBattle.Common;

namespace SpartaDungeonBattle
{
    class Battle
    {
        // 몬스터 하드코딩
        public class Monster
        {
            public string Name { get; set; }
            public int Level { get; set; }
            public int Hp { get; set; }
            public int Atk { get; set; }
            public bool IsLive => Hp > 0;
            public Monster(string name, int level, int hp, int atk)
            {
                Name = name;
                Level = level;
                Hp = hp;
                Atk = atk;
            }
        }

        public static Monster[] monsters =
        {
            new Monster("미니언",2,15,5)
            , new Monster("대포미니언",5,25,10)
            , new Monster("공허충",3,10,10)
        };

        public static List<Monster> monsterList;


        /// <summary>전투 시작 화면 출력</summary>
        public static void DisplayBattle()
        {

            monsterList = new List<Monster>();
            Random rand = new Random();
            // monsters에서 monster 랜덤 뽑기
            for (int i = 0; i < rand.Next(1, 4); i++)
            {
                monsterList.Add(monsters[rand.Next(0, 3)]);
            }

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Battle!!");
            Console.ResetColor();
            Console.WriteLine();
            foreach (Monster monster in monsterList)
            {
                if(monster.IsLive)
                {
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP {monster.Hp}");
                }
                else
                {
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name} Dead");
                }
               
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp}/100");
            Console.WriteLine();
            Console.WriteLine("1. 공격");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(1, 1);
            switch (input)
            {
                case 1:
                    DisplayBattleAttack("");
                    break;
            }
        }

        /// <summary>전투 공격 화면 출력</summary>
        public static void DisplayBattleAttack(string msg)
        {
            int num = 1;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Battle!!");
            Console.ResetColor();
            Console.WriteLine();

            foreach (Monster monster in monsterList)
            {
                if (monster.IsLive)
                {
                    Console.WriteLine($"{num++} Lv.{monster.Level} {monster.Name} HP {monster.Hp}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{num++} Lv.{monster.Level} {monster.Name} Dead");
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
            Console.WriteLine($"HP {player.Hp}/100");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0. 취소");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("대상을 선택해주세요.");
            Console.WriteLine(msg);
            int input = CheckValidInput(0, monsterList.Count);
            switch (input)
            {
                case 0:
                    DisplayBattle();
                    break;
                default:
                    Attack(input);
                    break;
            }
        }

        /// <summary>전투 공격</summary>
        private static void Attack(int index)
        {
            index--;
            string msg = "";
            if (monsterList[index].IsLive)
            {
                // 데미지 계산
                Random rand = new Random();
                int atk = player.Atk + myAddStat[(int)Abilitys.공격력];
                int error = (int)(atk * 0.1);
                int damage = rand.Next(atk - error, atk + error);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Battle!!");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine($"{player.Name} 의 공격!");
                Console.WriteLine($"Lv.{monsterList[index].Level} {monsterList[index].Name} 을(를) 맞췄습니다. [데미지: {damage}]");
                Console.WriteLine();
                Console.WriteLine($"Lv.{monsterList[index].Level} {monsterList[index].Name}");
                if (monsterList[index].Hp - damage > 0)
                {
                    Console.WriteLine($"HP {monsterList[index].Hp}->{monsterList[index].Hp - damage}");
                    monsterList[index].Hp -= damage;
                }
                else
                {
                    Console.WriteLine($"HP {monsterList[index].Hp}->Dead");
                    monsterList[index].Hp = 0;
                }
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                Console.WriteLine();
                int input = CheckValidInput(0, 0);
                switch (input)
                {
                    case 0:
                        Defend(0);
                        break;
                }
            }
            else 
            {
                msg = "잘못된 입력입니다.";
                DisplayBattleAttack(msg);
            }
        }

        /// <summary>전투 방어</summary>
        private static void Defend(int index)
        {
            //플레이어 사망 여부
            if(player.Hp == 0)
            {
                //사망
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Battle!! - Result");
                Console.WriteLine();
                Console.WriteLine("You Lose");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("0. 다음");
                Console.WriteLine();
                int input = CheckValidInput(0, 0);
                switch (input)
                {
                    case 0:
                        DisplayGameIntro();
                        break;
                }
            }
            else
            {
                //몬스터 유무
                if(index == monsterList.Count)
                {
                    DisplayBattleAttack("");
                }
                else
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Battle!!");
                    Console.ResetColor();
                    Console.WriteLine();
                    //몬스터 사망 우유
                    if (monsterList[index].IsLive)
                    {
                        // 데미지 계산
                        Random rand = new Random();
                        int atk = monsterList[index].Atk;
                        int error = (int)(atk * 0.1);
                        int damage = rand.Next(atk - error, atk + error);

                        Console.WriteLine($"Lv.{monsterList[index].Level} {monsterList[index].Name} 의 공격!");
                        Console.WriteLine($"Lv.{player.Name} 을(를) 맞췄습니다. [데미지: {damage}]");
                        if (player.Hp - damage > 0)
                        {
                            Console.WriteLine($"HP {player.Hp}->{player.Hp - damage}");
                            player.Hp -= damage;
                        }
                        else
                        {
                            Console.WriteLine($"HP {player.Hp}->0");
                            player.Hp = 0;
                        }

                        Console.WriteLine();
                        Console.WriteLine("0. 다음");
                        Console.WriteLine();
                        int input = CheckValidInput(0, 0);
                        switch (input)
                        {
                            case 0:
                                Defend(++index);
                                break;
                        }
                    }
                    else
                    {
                        index++;
                        if (index == monsterList.Count)
                        {
                            //사망
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Battle!! - Result");
                            Console.WriteLine();
                            Console.WriteLine("Victory");
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine($"던전에서 몬스터 {monsterList.Count}를 잡았습니다.");
                            Console.WriteLine();
                            Console.WriteLine("0. 다음");
                            Console.WriteLine();
                            int input = CheckValidInput(0, 0);
                            switch (input)
                            {
                                case 0:
                                    DisplayGameIntro();
                                    break;
                            }
                        }
                        else
                        {
                            Defend(index);
                        }
                    }
                }
            }
        }
    }
}