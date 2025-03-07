using UnityEngine;

public class SkillUISpawner : MonoBehaviour
{
    [SerializeField] private SkillUI skillUIPrefab;
    [SerializeField] private Transform skillParent;
    [SerializeField] private SkillDataSO tempSkillData;

    public void TestSkilUISpawn()
    {
        SpawnSkillUI(tempSkillData, skillParent);
    }

    public SkillUI SpawnSkillUI(SkillDataSO skillDataSO)
    {
        SkillUI newSkillUI = Instantiate(skillUIPrefab, Vector3.zero, Quaternion.identity);
        newSkillUI.transform.SetParent(skillParent);
        newSkillUI.transform.localScale = Vector3.one;
        newSkillUI.Init(skillDataSO);
        return newSkillUI;
    }

    public SkillUI SpawnSkillUI(SkillDataSO skillDataSO, Transform parentTransform)
    {
        SkillUI newSkillUI = Instantiate(skillUIPrefab, Vector3.zero, Quaternion.identity);
        newSkillUI.transform.SetParent(parentTransform);
        newSkillUI.transform.localScale = Vector3.one;
        newSkillUI.Init(skillDataSO);
        return newSkillUI;
    }

    public void SetSkillUIParentToSkillParent(SkillUI skillUI)
    {
        skillUI.transform.parent = skillParent;
        skillUI.transform.localScale = Vector3.one;
    }
}
