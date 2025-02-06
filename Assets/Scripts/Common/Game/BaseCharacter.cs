using UnityEngine;

public enum CharacterType
{
    Knight,
    Rogue,
    Magician,
    Priest
}

public class BaseCharacter :BaseTarget
{
    public CharacterType CharacterType; 
    public void Init()
    {
        base.Init();

    }
}
