using UnityEngine;

public enum CharacterType
{
    Knight = 0,
    Rogue = 1,
    Magician = 2,
    Priest = 3,
    Neutral = 4,
}

public class BaseCharacter :BaseTarget
{
    public CharacterType CharacterType; 
    public void Init()
    {
        base.Init();

    }
}
