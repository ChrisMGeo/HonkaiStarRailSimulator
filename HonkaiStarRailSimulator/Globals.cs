﻿namespace HonkaiStarRailSimulator;

public static class Globals
{
    public static readonly Dictionary<CharacterId, CharacterInfo> CharacterInfo =
        new Dictionary<CharacterId, CharacterInfo>()
        {
            {
                CharacterId.March7Th,
                new CharacterInfo()
                {
                    Name = "March 7th",
                    Path = CharacterPath.Preservation,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 101, BaseAggro = 150,
                        HpBase =
                        {
                            144f, 201.6000000005588f, 259.20000000018626f, 316.80000000074506f, 374.40000000037253f,
                            432f, 489.6000000005588f,
                        },
                        HpAdd = 7.2000000001862645f,
                        AttackBase =
                        {
                            69.6000000005588f, 97.44000000040978f, 125.28000000026077f, 153.12000000011176f,
                            180.96000000089407f, 208.80000000074506f, 236.64000000059605f,
                        },
                        AttackAdd = 3.480000000447035f,
                        DefenceBase =
                        {
                            78f, 109.20000000018626f, 140.40000000037253f, 171.6000000005588f, 202.80000000074506f,
                            234f, 265.20000000018626f,
                        },
                        DefenceAdd = 3.9000000008381903f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.DanHeng,
                new CharacterInfo()
                {
                    Name = "Dan Heng",
                    Path = CharacterPath.TheHunt,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 110, BaseAggro = 75,
                        HpBase = { 120f, 168f, 216f, 264f, 312f, 360f, 408f, }, HpAdd = 6f,
                        AttackBase =
                        {
                            74.40000000037253f, 104.16000000014901f, 133.92000000085682f, 163.6800000006333f,
                            193.44000000040978f, 223.20000000018626f, 252.96000000089407f,
                        },
                        AttackAdd = 3.7200000006705523f,
                        DefenceBase =
                        {
                            54f, 75.6000000005588f, 97.20000000018626f, 118.80000000074506f, 140.40000000037253f, 162f,
                            183.6000000005588f,
                        },
                        DefenceAdd = 2.700000000651926f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.Himeko,
                new CharacterInfo()
                {
                    Name = "Himeko",
                    Path = CharacterPath.Erudition,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 96, BaseAggro = 75,
                        HpBase =
                        {
                            142.56000000052154f, 199.5840000002645f, 256.60800000000745f, 313.6320000004489f,
                            370.65600000019185f, 427.6800000006333f, 484.70400000037625f,
                        },
                        HpAdd = 7.127999999560416f,
                        AttackBase =
                        {
                            102.96000000089407f, 144.1439999998547f, 185.32799999974668f, 226.51200000033714f,
                            267.6960000002291f, 308.88000000081956f, 350.0639999997802f,
                        },
                        AttackAdd = 5.147999999579042f,
                        DefenceBase =
                        {
                            59.40000000037253f, 83.16000000014901f, 106.92000000085682f, 130.6800000006333f,
                            154.44000000040978f, 178.20000000018626f, 201.96000000089407f,
                        },
                        DefenceAdd = 2.970000000903383f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Welt,
                new CharacterInfo()
                {
                    Name = "Welt",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 102, BaseAggro = 100,
                        HpBase =
                        {
                            153.12000000011176f, 214.36799999978393f, 275.6160000001546f, 336.86400000052527f,
                            398.1119999999646f, 459.3600000003353f, 520.6080000000075f,
                        },
                        HpAdd = 7.6560000001918525f,
                        AttackBase =
                        {
                            84.48000000044703f, 118.27200000011362f, 152.0639999997802f, 185.85600000037812f,
                            219.6480000000447f, 253.44000000040978f, 287.23200000007637f,
                        },
                        AttackAdd = 4.2239999999292195f,
                        DefenceBase =
                        {
                            69.3000000002794f, 97.02000000001863f, 124.74000000068918f, 152.4600000004284f,
                            180.18000000016764f, 207.9000000008382f, 235.62000000057742f,
                        },
                        DefenceAdd = 3.465000000083819f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Kafka,
                new CharacterInfo()
                {
                    Name = "Kafka",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 100,
                        HpBase =
                        {
                            147.8400000007823f, 206.97600000048988f, 266.1119999999646f, 325.2479999996722f,
                            384.38400000007823f, 443.5200000004843f, 502.65600000019185f,
                        },
                        HpAdd = 7.39200000022538f,
                        AttackBase =
                        {
                            92.40000000037253f, 129.36000000033528f, 166.32000000029802f, 203.28000000026077f,
                            240.24000000022352f, 277.20000000018626f, 314.160000000149f,
                        },
                        AttackAdd = 4.62000000057742f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.SilverWolf,
                new CharacterInfo()
                {
                    Name = "Silver Wolf",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 107, BaseAggro = 100,
                        HpBase =
                        {
                            142.56000000052154f, 199.5840000002645f, 256.60800000000745f, 313.6320000004489f,
                            370.65600000019185f, 427.6800000006333f, 484.70400000037625f,
                        },
                        HpAdd = 7.127999999560416f,
                        AttackBase =
                        {
                            87.12000000011176f, 121.96800000034273f, 156.81600000034086f, 191.664000000339f,
                            226.51200000033714f, 261.3600000003353f, 296.2079999996349f,
                        },
                        AttackAdd = 4.355999999912456f,
                        DefenceBase =
                        {
                            62.700000000651926f, 87.78000000072643f, 112.86000000080094f, 137.94000000087544f,
                            163.02000000001863f, 188.10000000009313f, 213.18000000016764f,
                        },
                        DefenceAdd = 3.1349999997764826f,
                        MaxEnergy = 110f,
                    }
                }
            },
            {
                CharacterId.Arlan,
                new CharacterInfo()
                {
                    Name = "Arlan",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 102, BaseAggro = 125,
                        HpBase =
                        {
                            163.20000000018626f, 228.48000000044703f, 293.7600000007078f, 359.04000000003725f,
                            424.320000000298f, 489.6000000005588f, 554.8800000008196f,
                        },
                        HpAdd = 8.160000000149012f,
                        AttackBase =
                        {
                            81.6000000005588f, 114.24000000022352f, 146.88000000081956f, 179.5200000004843f,
                            212.160000000149f, 244.80000000074506f, 277.4400000004098f,
                        },
                        AttackAdd = 4.080000000074506f,
                        DefenceBase = { 45f, 63f, 81f, 99f, 117f, 135f, 153f, }, DefenceAdd = 2.2500000002328306f,
                        MaxEnergy = 110f,
                    }
                }
            },
            {
                CharacterId.Asta,
                new CharacterInfo()
                {
                    Name = "Asta",
                    Path = CharacterPath.Harmony,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 106, BaseAggro = 100,
                        HpBase =
                        {
                            139.20000000018626f, 194.88000000081956f, 250.56000000052154f, 306.2400000002235f,
                            361.9200000008568f, 417.6000000005588f, 473.28000000026077f,
                        },
                        HpAdd = 6.96000000089407f,
                        AttackBase =
                        {
                            69.6000000005588f, 97.44000000040978f, 125.28000000026077f, 153.12000000011176f,
                            180.96000000089407f, 208.80000000074506f, 236.64000000059605f,
                        },
                        AttackAdd = 3.480000000447035f,
                        DefenceBase =
                        {
                            63f, 88.20000000018626f, 113.40000000037253f, 138.6000000005588f, 163.80000000074506f, 189f,
                            214.20000000018626f,
                        },
                        DefenceAdd = 3.1500000001396984f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Herta,
                new CharacterInfo()
                {
                    Name = "Herta",
                    Path = CharacterPath.Erudition,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 75,
                        HpBase =
                        {
                            129.6000000005588f, 181.44000000040978f, 233.28000000026077f, 285.12000000011176f,
                            336.96000000089407f, 388.80000000074506f, 440.64000000059605f,
                        },
                        HpAdd = 6.480000000447035f,
                        AttackBase =
                        {
                            79.20000000018626f, 110.88000000081956f, 142.56000000052154f, 174.24000000022352f,
                            205.92000000085682f, 237.6000000005588f, 269.28000000026077f,
                        },
                        AttackAdd = 3.9600000008940697f,
                        DefenceBase =
                        {
                            54f, 75.6000000005588f, 97.20000000018626f, 118.80000000074506f, 140.40000000037253f, 162f,
                            183.6000000005588f,
                        },
                        DefenceAdd = 2.700000000651926f,
                        MaxEnergy = 110f,
                    }
                }
            },
            {
                CharacterId.Bronya,
                new CharacterInfo()
                {
                    Name = "Bronya",
                    Path = CharacterPath.Harmony,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 99, BaseAggro = 100,
                        HpBase =
                        {
                            168.96000000089407f, 236.54400000022724f, 304.1279999995604f, 371.7120000005234f,
                            439.2959999998566f, 506.88000000081956f, 574.4640000001527f,
                        },
                        HpAdd = 8.447999999858439f,
                        AttackBase =
                        {
                            79.20000000018626f, 110.88000000081956f, 142.56000000052154f, 174.24000000022352f,
                            205.92000000085682f, 237.6000000005588f, 269.28000000026077f,
                        },
                        AttackAdd = 3.9600000008940697f,
                        DefenceBase =
                        {
                            72.6000000005588f, 101.64000000059605f, 130.6800000006333f, 159.72000000067055f,
                            188.7600000007078f, 217.80000000074506f, 246.8400000007823f,
                        },
                        DefenceAdd = 3.630000000586733f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Seele,
                new CharacterInfo()
                {
                    Name = "Seele",
                    Path = CharacterPath.TheHunt,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 115, BaseAggro = 75,
                        HpBase =
                        {
                            126.72000000067055f, 177.4079999998212f, 228.0959999996703f, 278.78400000045076f,
                            329.4720000002999f, 380.160000000149f, 430.84800000023097f,
                        },
                        HpAdd = 6.335999999893829f,
                        AttackBase =
                        {
                            87.12000000011176f, 121.96800000034273f, 156.81600000034086f, 191.664000000339f,
                            226.51200000033714f, 261.3600000003353f, 296.2079999996349f,
                        },
                        AttackAdd = 4.355999999912456f,
                        DefenceBase =
                        {
                            49.50000000046566f, 69.3000000002794f, 89.10000000009313f, 108.90000000083819f,
                            128.70000000065193f, 148.50000000046566f, 168.3000000002794f,
                        },
                        DefenceAdd = 2.4750000000931323f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Serval,
                new CharacterInfo()
                {
                    Name = "Serval",
                    Path = CharacterPath.Erudition,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 104, BaseAggro = 75,
                        HpBase =
                        {
                            124.80000000074506f, 174.72000000067055f, 224.64000000059605f, 274.56000000052154f,
                            324.48000000044703f, 374.40000000037253f, 424.320000000298f,
                        },
                        HpAdd = 6.240000000223517f,
                        AttackBase =
                        {
                            88.80000000074506f, 124.32000000029802f, 159.8400000007823f, 195.36000000033528f,
                            230.88000000081956f, 266.40000000037253f, 301.9200000008568f,
                        },
                        AttackAdd = 4.440000000409782f,
                        DefenceBase =
                        {
                            51f, 71.40000000037253f, 91.80000000074506f, 112.20000000018626f, 132.6000000005588f, 153f,
                            173.40000000037253f,
                        },
                        DefenceAdd = 2.5500000005122274f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.Gepard,
                new CharacterInfo()
                {
                    Name = "Gepard",
                    Path = CharacterPath.Preservation,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 92, BaseAggro = 150,
                        HpBase =
                        {
                            190.0800000000745f, 266.1119999999646f, 342.1439999998547f, 418.1759999997448f,
                            494.2079999996349f, 570.2400000002235f, 646.2720000001136f,
                        },
                        HpAdd = 9.50400000018999f,
                        AttackBase =
                        {
                            73.92000000085682f, 103.48799999989569f, 133.05599999963306f, 162.62400000030175f,
                            192.19200000003912f, 221.7600000007078f, 251.32799999974668f,
                        },
                        AttackAdd = 3.6960000002291054f,
                        DefenceBase =
                        {
                            89.10000000009313f, 124.74000000068918f, 160.3800000003539f, 196.02000000001863f,
                            231.66000000061467f, 267.3000000002794f, 302.94000000087544f,
                        },
                        DefenceAdd = 4.455000000074506f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.Natasha,
                new CharacterInfo()
                {
                    Name = "Natasha",
                    Path = CharacterPath.Abundance,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 98, BaseAggro = 100,
                        HpBase =
                        {
                            158.40000000037253f, 221.7600000007078f, 285.12000000011176f, 348.48000000044703f,
                            411.8400000007823f, 475.20000000018626f, 538.5600000005215f,
                        },
                        HpAdd = 7.920000000856817f,
                        AttackBase =
                        {
                            64.80000000074506f, 90.72000000067055f, 116.64000000059605f, 142.56000000052154f,
                            168.48000000044703f, 194.40000000037253f, 220.32000000029802f,
                        },
                        AttackAdd = 3.2400000002235174f,
                        DefenceBase =
                        {
                            69f, 96.6000000005588f, 124.20000000018626f, 151.80000000074506f, 179.40000000037253f, 207f,
                            234.6000000005588f,
                        },
                        DefenceAdd = 3.450000000419095f,
                        MaxEnergy = 90f,
                    }
                }
            },
            {
                CharacterId.Pela,
                new CharacterInfo()
                {
                    Name = "Pela",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 105, BaseAggro = 100,
                        HpBase =
                        {
                            134.40000000037253f, 188.160000000149f, 241.92000000085682f, 295.6800000006333f,
                            349.4400000004098f, 403.20000000018626f, 456.96000000089407f,
                        },
                        HpAdd = 6.720000000670552f,
                        AttackBase =
                        {
                            74.40000000037253f, 104.16000000014901f, 133.92000000085682f, 163.6800000006333f,
                            193.44000000040978f, 223.20000000018626f, 252.96000000089407f,
                        },
                        AttackAdd = 3.7200000006705523f,
                        DefenceBase =
                        {
                            63f, 88.20000000018626f, 113.40000000037253f, 138.6000000005588f, 163.80000000074506f, 189f,
                            214.20000000018626f,
                        },
                        DefenceAdd = 3.1500000001396984f,
                        MaxEnergy = 110f,
                    }
                }
            },
            {
                CharacterId.Clara,
                new CharacterInfo()
                {
                    Name = "Clara",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 90, BaseAggro = 125,
                        HpBase =
                        {
                            168.96000000089407f, 236.54400000022724f, 304.1279999995604f, 371.7120000005234f,
                            439.2959999998566f, 506.88000000081956f, 574.4640000001527f,
                        },
                        HpAdd = 8.447999999858439f,
                        AttackBase =
                        {
                            100.32000000029802f, 140.44799999985844f, 180.57600000011735f, 220.70400000037625f,
                            260.83200000063516f, 300.96000000089407f, 341.08799999952316f,
                        },
                        AttackAdd = 5.015999999595806f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 110f,
                    }
                }
            },
            {
                CharacterId.Sampo,
                new CharacterInfo()
                {
                    Name = "Sampo",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 102, BaseAggro = 100,
                        HpBase =
                        {
                            139.20000000018626f, 194.88000000081956f, 250.56000000052154f, 306.2400000002235f,
                            361.9200000008568f, 417.6000000005588f, 473.28000000026077f,
                        },
                        HpAdd = 6.96000000089407f,
                        AttackBase =
                        {
                            84f, 117.6000000005588f, 151.20000000018626f, 184.80000000074506f, 218.40000000037253f,
                            252f, 285.6000000005588f,
                        },
                        AttackAdd = 4.2000000001862645f,
                        DefenceBase =
                        {
                            54f, 75.6000000005588f, 97.20000000018626f, 118.80000000074506f, 140.40000000037253f, 162f,
                            183.6000000005588f,
                        },
                        DefenceAdd = 2.700000000651926f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Hook,
                new CharacterInfo()
                {
                    Name = "Hook",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 94, BaseAggro = 125,
                        HpBase =
                        {
                            182.40000000037253f, 255.36000000033528f, 328.320000000298f, 401.28000000026077f,
                            474.2400000002235f, 547.2000000001863f, 620.160000000149f,
                        },
                        HpAdd = 9.120000000111759f,
                        AttackBase =
                        {
                            84f, 117.6000000005588f, 151.20000000018626f, 184.80000000074506f, 218.40000000037253f,
                            252f, 285.6000000005588f,
                        },
                        AttackAdd = 4.2000000001862645f,
                        DefenceBase =
                        {
                            48f, 67.20000000018626f, 86.40000000037253f, 105.6000000005588f, 124.80000000074506f, 144f,
                            163.20000000018626f,
                        },
                        DefenceAdd = 2.400000000372529f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Lynx,
                new CharacterInfo()
                {
                    Name = "Lynx",
                    Path = CharacterPath.Abundance,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 100,
                        HpBase =
                        {
                            144f, 201.6000000005588f, 259.20000000018626f, 316.80000000074506f, 374.40000000037253f,
                            432f, 489.6000000005588f,
                        },
                        HpAdd = 7.2000000001862645f,
                        AttackBase =
                        {
                            67.20000000018626f, 94.0800000000745f, 120.96000000089407f, 147.8400000007823f,
                            174.72000000067055f, 201.6000000005588f, 228.48000000044703f,
                        },
                        AttackAdd = 3.360000000335276f,
                        DefenceBase = { 75f, 105f, 135f, 165f, 195f, 225f, 255f, }, DefenceAdd = 3.750000000698492f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.Luka,
                new CharacterInfo()
                {
                    Name = "Luka",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 103, BaseAggro = 100,
                        HpBase =
                        {
                            124.80000000074506f, 174.72000000067055f, 224.64000000059605f, 274.56000000052154f,
                            324.48000000044703f, 374.40000000037253f, 424.320000000298f,
                        },
                        HpAdd = 6.240000000223517f,
                        AttackBase =
                        {
                            79.20000000018626f, 110.88000000081956f, 142.56000000052154f, 174.24000000022352f,
                            205.92000000085682f, 237.6000000005588f, 269.28000000026077f,
                        },
                        AttackAdd = 3.9600000008940697f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.TopazAndNumby,
                new CharacterInfo()
                {
                    Name = "Topaz and Numby",
                    Path = CharacterPath.TheHunt,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 110, BaseAggro = 75,
                        HpBase =
                        {
                            126.72000000067055f, 177.4079999998212f, 228.0959999996703f, 278.78400000045076f,
                            329.4720000002999f, 380.160000000149f, 430.84800000023097f,
                        },
                        HpAdd = 6.335999999893829f,
                        AttackBase =
                        {
                            84.48000000044703f, 118.27200000011362f, 152.0639999997802f, 185.85600000037812f,
                            219.6480000000447f, 253.44000000040978f, 287.23200000007637f,
                        },
                        AttackAdd = 4.2239999999292195f,
                        DefenceBase =
                        {
                            56.10000000009313f, 78.54000000050291f, 100.9800000009127f, 123.42000000039116f,
                            145.86000000080094f, 168.3000000002794f, 190.74000000068918f,
                        },
                        DefenceAdd = 2.8050000004004687f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.Qingque,
                new CharacterInfo()
                {
                    Name = "Qingque",
                    Path = CharacterPath.Erudition,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 98, BaseAggro = 75,
                        HpBase =
                        {
                            139.20000000018626f, 194.88000000081956f, 250.56000000052154f, 306.2400000002235f,
                            361.9200000008568f, 417.6000000005588f, 473.28000000026077f,
                        },
                        HpAdd = 6.96000000089407f,
                        AttackBase =
                        {
                            88.80000000074506f, 124.32000000029802f, 159.8400000007823f, 195.36000000033528f,
                            230.88000000081956f, 266.40000000037253f, 301.9200000008568f,
                        },
                        AttackAdd = 4.440000000409782f,
                        DefenceBase = { 60f, 84f, 108f, 132f, 156f, 180f, 204f, }, DefenceAdd = 3f,
                        MaxEnergy = 140f,
                    }
                }
            },
            {
                CharacterId.Tingyun,
                new CharacterInfo()
                {
                    Name = "Tingyun",
                    Path = CharacterPath.Harmony,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 112, BaseAggro = 100,
                        HpBase =
                        {
                            115.20000000018626f, 161.28000000026077f, 207.36000000033528f, 253.44000000040978f,
                            299.5200000004843f, 345.6000000005588f, 391.6800000006333f,
                        },
                        HpAdd = 5.760000000707805f,
                        AttackBase =
                        {
                            72f, 100.80000000074506f, 129.6000000005588f, 158.40000000037253f, 187.20000000018626f,
                            216f, 244.80000000074506f,
                        },
                        AttackAdd = 3.6000000005587935f,
                        DefenceBase =
                        {
                            54f, 75.6000000005588f, 97.20000000018626f, 118.80000000074506f, 140.40000000037253f, 162f,
                            183.6000000005588f,
                        },
                        DefenceAdd = 2.700000000651926f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.Luocha,
                new CharacterInfo()
                {
                    Name = "Luocha",
                    Path = CharacterPath.Abundance,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 101, BaseAggro = 100,
                        HpBase =
                        {
                            174.24000000022352f, 243.93600000045262f, 313.6320000004489f, 383.3279999997467f,
                            453.02399999974295f, 522.7200000006706f, 592.4159999999683f,
                        },
                        HpAdd = 8.712000000523403f,
                        AttackBase =
                        {
                            102.96000000089407f, 144.1439999998547f, 185.32799999974668f, 226.51200000033714f,
                            267.6960000002291f, 308.88000000081956f, 350.0639999997802f,
                        },
                        AttackAdd = 5.147999999579042f,
                        DefenceBase =
                        {
                            49.50000000046566f, 69.3000000002794f, 89.10000000009313f, 108.90000000083819f,
                            128.70000000065193f, 148.50000000046566f, 168.3000000002794f,
                        },
                        DefenceAdd = 2.4750000000931323f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.JingYuan,
                new CharacterInfo()
                {
                    Name = "Jing Yuan",
                    Path = CharacterPath.Erudition,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 99, BaseAggro = 75,
                        HpBase =
                        {
                            158.40000000037253f, 221.7600000007078f, 285.12000000011176f, 348.48000000044703f,
                            411.8400000007823f, 475.20000000018626f, 538.5600000005215f,
                        },
                        HpAdd = 7.920000000856817f,
                        AttackBase =
                        {
                            95.04000000003725f, 133.05599999963306f, 171.07199999992736f, 209.08799999952316f,
                            247.10399999981746f, 285.12000000011176f, 323.13599999970756f,
                        },
                        AttackAdd = 4.752000000560656f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.Blade,
                new CharacterInfo()
                {
                    Name = "Blade",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 97, BaseAggro = 125,
                        HpBase =
                        {
                            184.80000000074506f, 258.72000000067055f, 332.64000000059605f, 406.56000000052154f,
                            480.48000000044703f, 554.4000000003725f, 628.320000000298f,
                        },
                        HpAdd = 9.240000000223517f,
                        AttackBase =
                        {
                            73.92000000085682f, 103.48799999989569f, 133.05599999963306f, 162.62400000030175f,
                            192.19200000003912f, 221.7600000007078f, 251.32799999974668f,
                        },
                        AttackAdd = 3.6960000002291054f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.Sushang,
                new CharacterInfo()
                {
                    Name = "Sushang",
                    Path = CharacterPath.TheHunt,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 107, BaseAggro = 75,
                        HpBase =
                        {
                            124.80000000074506f, 174.72000000067055f, 224.64000000059605f, 274.56000000052154f,
                            324.48000000044703f, 374.40000000037253f, 424.320000000298f,
                        },
                        HpAdd = 6.240000000223517f,
                        AttackBase =
                        {
                            76.80000000074506f, 107.52000000048429f, 138.24000000022352f, 168.96000000089407f,
                            199.6800000006333f, 230.40000000037253f, 261.12000000011176f,
                        },
                        AttackAdd = 3.840000000782311f,
                        DefenceBase =
                        {
                            57f, 79.80000000074506f, 102.6000000005588f, 125.40000000037253f, 148.20000000018626f, 171f,
                            193.80000000074506f,
                        },
                        DefenceAdd = 2.850000000791624f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Yukong,
                new CharacterInfo()
                {
                    Name = "Yukong",
                    Path = CharacterPath.Harmony,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 107, BaseAggro = 100,
                        HpBase =
                        {
                            124.80000000074506f, 174.72000000067055f, 224.64000000059605f, 274.56000000052154f,
                            324.48000000044703f, 374.40000000037253f, 424.320000000298f,
                        },
                        HpAdd = 6.240000000223517f,
                        AttackBase =
                        {
                            81.6000000005588f, 114.24000000022352f, 146.88000000081956f, 179.5200000004843f,
                            212.160000000149f, 244.80000000074506f, 277.4400000004098f,
                        },
                        AttackAdd = 4.080000000074506f,
                        DefenceBase =
                        {
                            51f, 71.40000000037253f, 91.80000000074506f, 112.20000000018626f, 132.6000000005588f, 153f,
                            173.40000000037253f,
                        },
                        DefenceAdd = 2.5500000005122274f,
                        MaxEnergy = 130f,
                    }
                }
            },
            {
                CharacterId.FuXuan,
                new CharacterInfo()
                {
                    Name = "Fu Xuan",
                    Path = CharacterPath.Preservation,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 150,
                        HpBase =
                        {
                            200.64000000059605f, 280.89600000041537f, 361.15200000000186f, 441.4079999998212f,
                            521.664000000339f, 601.9200000008568f, 682.1759999997448f,
                        },
                        HpAdd = 10.031999999890104f,
                        AttackBase =
                        {
                            63.360000000335276f, 88.70400000037625f, 114.04799999948591f, 139.39200000022538f,
                            164.73600000026636f, 190.0800000000745f, 215.42400000011548f,
                        },
                        AttackAdd = 3.1679999995976686f,
                        DefenceBase =
                        {
                            82.50000000046566f, 115.50000000046566f, 148.50000000046566f, 181.50000000046566f,
                            214.50000000046566f, 247.50000000046566f, 280.50000000046566f,
                        },
                        DefenceAdd = 4.124999999767169f,
                        MaxEnergy = 135f,
                    }
                }
            },
            {
                CharacterId.Yanqing,
                new CharacterInfo()
                {
                    Name = "Yanqing",
                    Path = CharacterPath.TheHunt,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 109, BaseAggro = 75,
                        HpBase =
                        {
                            121.44000000040978f, 170.0159999995958f, 218.59200000041164f, 267.16799999959767f,
                            315.7440000004135f, 364.320000000298f, 412.89600000041537f,
                        },
                        HpAdd = 6.071999999927357f,
                        AttackBase =
                        {
                            92.40000000037253f, 129.36000000033528f, 166.32000000029802f, 203.28000000026077f,
                            240.24000000022352f, 277.20000000018626f, 314.160000000149f,
                        },
                        AttackAdd = 4.62000000057742f,
                        DefenceBase =
                        {
                            56.10000000009313f, 78.54000000050291f, 100.9800000009127f, 123.42000000039116f,
                            145.86000000080094f, 168.3000000002794f, 190.74000000068918f,
                        },
                        DefenceAdd = 2.8050000004004687f,
                        MaxEnergy = 140f,
                    }
                }
            },
            {
                CharacterId.Guinaifen,
                new CharacterInfo()
                {
                    Name = "Guinaifen",
                    Path = CharacterPath.Nihility,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 106, BaseAggro = 100,
                        HpBase = { 120f, 168f, 216f, 264f, 312f, 360f, 408f, }, HpAdd = 6f,
                        AttackBase =
                        {
                            79.20000000018626f, 110.88000000081956f, 142.56000000052154f, 174.24000000022352f,
                            205.92000000085682f, 237.6000000005588f, 269.28000000026077f,
                        },
                        AttackAdd = 3.9600000008940697f,
                        DefenceBase = { 60f, 84f, 108f, 132f, 156f, 180f, 204f, }, DefenceAdd = 3f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.Bailu,
                new CharacterInfo()
                {
                    Name = "Bailu",
                    Path = CharacterPath.Abundance,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 98, BaseAggro = 100,
                        HpBase =
                        {
                            179.5200000004843f, 251.32799999974668f, 323.13599999970756f, 394.9440000005998f,
                            466.75200000056066f, 538.5600000005215f, 610.3679999997839f,
                        },
                        HpAdd = 8.976000000489876f,
                        AttackBase =
                        {
                            76.56000000052154f, 107.18399999989197f, 137.80800000019372f, 168.43200000026263f,
                            199.05599999963306f, 229.6800000006333f, 260.3040000000037f,
                        },
                        AttackAdd = 3.8280000002123415f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 100f,
                    }
                }
            },
            {
                CharacterId.Jingliu,
                new CharacterInfo()
                {
                    Name = "Jingliu",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 96, BaseAggro = 125,
                        HpBase =
                        {
                            195.36000000033528f, 273.50400000019f, 351.6480000000447f, 429.7920000005979f,
                            507.9360000004526f, 586.0800000000745f, 664.2239999999292f,
                        },
                        HpAdd = 9.768000000156462f,
                        AttackBase =
                        {
                            92.40000000037253f, 129.36000000033528f, 166.32000000029802f, 203.28000000026077f,
                            240.24000000022352f, 277.20000000018626f, 314.160000000149f,
                        },
                        AttackAdd = 4.62000000057742f,
                        DefenceBase =
                        {
                            66f, 92.40000000037253f, 118.80000000074506f, 145.20000000018626f, 171.6000000005588f, 198f,
                            224.40000000037253f,
                        },
                        DefenceAdd = 3.3000000002793968f,
                        MaxEnergy = 140f,
                    }
                }
            },
            {
                CharacterId.DanHengImbibitorLunae,
                new CharacterInfo()
                {
                    Name = "Dan Heng • Imbibitor Lunae",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 102, BaseAggro = 125,
                        HpBase =
                        {
                            168.96000000089407f, 236.54400000022724f, 304.1279999995604f, 371.7120000005234f,
                            439.2959999998566f, 506.88000000081956f, 574.4640000001527f,
                        },
                        HpAdd = 8.447999999858439f,
                        AttackBase =
                        {
                            95.04000000003725f, 133.05599999963306f, 171.07199999992736f, 209.08799999952316f,
                            247.10399999981746f, 285.12000000011176f, 323.13599999970756f,
                        },
                        AttackAdd = 4.752000000560656f,
                        DefenceBase =
                        {
                            49.50000000046566f, 69.3000000002794f, 89.10000000009313f, 108.90000000083819f,
                            128.70000000065193f, 148.50000000046566f, 168.3000000002794f,
                        },
                        DefenceAdd = 2.4750000000931323f,
                        MaxEnergy = 140f,
                    }
                }
            },
            {
                CharacterId.PhysicalDestructionTrailblazerM,
                new CharacterInfo()
                {
                    Name = "Trailblazer (Physical/Destruction/Male)",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 125,
                        HpBase =
                        {
                            163.6800000006333f, 229.15200000000186f, 294.62400000030175f, 360.0959999996703f,
                            425.5679999999702f, 491.04000000003725f, 556.5120000003371f,
                        },
                        HpAdd = 8.183999999891967f,
                        AttackBase =
                        {
                            84.48000000044703f, 118.27200000011362f, 152.0639999997802f, 185.85600000037812f,
                            219.6480000000447f, 253.44000000040978f, 287.23200000007637f,
                        },
                        AttackAdd = 4.2239999999292195f,
                        DefenceBase =
                        {
                            62.700000000651926f, 87.78000000072643f, 112.86000000080094f, 137.94000000087544f,
                            163.02000000001863f, 188.10000000009313f, 213.18000000016764f,
                        },
                        DefenceAdd = 3.1349999997764826f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.PhysicalDestructionTrailblazerF,
                new CharacterInfo()
                {
                    Name = "Trailblazer (Physical/Destruction/Female)",
                    Path = CharacterPath.Destruction,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 100, BaseAggro = 125,
                        HpBase =
                        {
                            163.6800000006333f, 229.15200000000186f, 294.62400000030175f, 360.0959999996703f,
                            425.5679999999702f, 491.04000000003725f, 556.5120000003371f,
                        },
                        HpAdd = 8.183999999891967f,
                        AttackBase =
                        {
                            84.48000000044703f, 118.27200000011362f, 152.0639999997802f, 185.85600000037812f,
                            219.6480000000447f, 253.44000000040978f, 287.23200000007637f,
                        },
                        AttackAdd = 4.2239999999292195f,
                        DefenceBase =
                        {
                            62.700000000651926f, 87.78000000072643f, 112.86000000080094f, 137.94000000087544f,
                            163.02000000001863f, 188.10000000009313f, 213.18000000016764f,
                        },
                        DefenceAdd = 3.1349999997764826f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.FirePreservationTrailblazerM,
                new CharacterInfo()
                {
                    Name = "Trailblazer (Fire/Preservation/Male)",
                    Path = CharacterPath.Preservation,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 95, BaseAggro = 150,
                        HpBase =
                        {
                            168.96000000089407f, 236.54400000022724f, 304.1279999995604f, 371.7120000005234f,
                            439.2959999998566f, 506.88000000081956f, 574.4640000001527f,
                        },
                        HpAdd = 8.447999999858439f,
                        AttackBase =
                        {
                            81.84000000078231f, 114.57600000011735f, 147.31200000015087f, 180.0479999994859f,
                            212.78400000045076f, 245.5200000004843f, 278.2559999998193f,
                        },
                        AttackAdd = 4.091999999945983f,
                        DefenceBase =
                        {
                            82.50000000046566f, 115.50000000046566f, 148.50000000046566f, 181.50000000046566f,
                            214.50000000046566f, 247.50000000046566f, 280.50000000046566f,
                        },
                        DefenceAdd = 4.124999999767169f,
                        MaxEnergy = 120f,
                    }
                }
            },
            {
                CharacterId.FirePreservationTrailblazerF,
                new CharacterInfo()
                {
                    Name = "Trailblazer (Fire/Preservation/Female)",
                    Path = CharacterPath.Preservation,
                    Stats = new CharacterStatInfo()
                    {
                        SpeedBase = 95, BaseAggro = 150,
                        HpBase =
                        {
                            168.96000000089407f, 236.54400000022724f, 304.1279999995604f, 371.7120000005234f,
                            439.2959999998566f, 506.88000000081956f, 574.4640000001527f,
                        },
                        HpAdd = 8.447999999858439f,
                        AttackBase =
                        {
                            81.84000000078231f, 114.57600000011735f, 147.31200000015087f, 180.0479999994859f,
                            212.78400000045076f, 245.5200000004843f, 278.2559999998193f,
                        },
                        AttackAdd = 4.091999999945983f,
                        DefenceBase =
                        {
                            82.50000000046566f, 115.50000000046566f, 148.50000000046566f, 181.50000000046566f,
                            214.50000000046566f, 247.50000000046566f, 280.50000000046566f,
                        },
                        DefenceAdd = 4.124999999767169f,
                        MaxEnergy = 120f,
                    }
                }
            },
        };
}