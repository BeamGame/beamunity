using System;
using System.Collections.Generic;

[Flags]
public enum BeamonType
{
    None = 0,
    Normal = 1,
    Fire = 2,
    Water = 4,
    Grass = 8,
    Electric = 16,
    Ice = 32,
    Fighting = 64,
    Poison = 128,
    Ground = 256,
    Flying = 512,
    Psychic = 1024,
    Bug = 2048,
    Rock = 4096,
    Ghost = 8192,
    Dragon = 16384,
    Dark = 32768,
    Steel = 65536
}
public class AddMonsterDto
{
    public string Name { get; set; }
    public int Level { get; set; }
}

public class UpdateMonsterDto
{
    public int MonsterId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
}

public class TokenDto
{
    public int BeamonId { get; set; }
    public int MonsterId { get; set; }
    public Beamon Beamon { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
}

public class MonsterMove
{
    public int MonsterMoveId { get; set; }
    public int MonsterId { get; set; }
    public TokenDto Token { get; set; }
    public int MoveId { get; set; }
    public MoveDto Move { get; set; }
}

public class Beamon
{
    public int BeamonId { get; set; }
    public string Name { get; set; }
    public int Hp { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Speed { get; set; }
    public BeamonType BeamonType { get; set; }

    
}

public class MoveDto
{
    public int MoveId { get; set; }
    public string Name { get; set; }
}

public class TransferMonsterDto
{
    public int MonsterId { get; set; }
    public string UserName { get; set; }
}

public class BalanceDto
{
    public string Address { get; set; }
    public decimal Native { get; set; }
    public decimal BeamonCoin { get; set; }
}