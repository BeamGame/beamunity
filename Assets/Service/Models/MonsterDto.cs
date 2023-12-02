using System;
using System.Collections.Generic;

public class AddMonsterDto
{
    public string Name { get; set; }
    public int Level { get; set; }
}

public class UpdateMonsterDto
{
    public int TokenId { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
}

public class TokenDto
{
    public int TokenId { get; set; }
    public int MonsterId { get; set; }
    public MonsterDto Monster { get; set; }
    public int Level { get; set; }
    public int Exp { get; set; }
}

public class MonsterMove
{
    public int MonsterMoveId { get; set; }
    public int TokenId { get; set; }
    public TokenDto Token { get; set; }
    public int MoveId { get; set; }
    public MoveDto Move { get; set; }
}

public class MonsterDto
{
    public int MonsterId { get; set; }
    public string Name { get; set; }
}

public class MoveDto
{
    public int MoveId { get; set; }
    public string Name { get; set; }
}

public class TransferMonsterDto
{
    public int TokenId { get; set; }
    public string UserName { get; set; }
}