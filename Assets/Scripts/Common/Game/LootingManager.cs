using UnityEngine;

public class LootingManager : SingletonBehaviour<LootingManager>
{
    public SkillUISpawner SkillUiSpawner;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillUiSpawner = GetComponent<SkillUISpawner>();
    }

    
}

