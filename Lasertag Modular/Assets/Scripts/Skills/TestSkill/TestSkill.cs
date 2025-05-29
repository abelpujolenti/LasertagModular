using UnityEngine;
using Agent;

public class TestSkill : BaseSkill
{
    public override void UseAbility()
    {
        Debug.Log("USING");
    }

    protected override void OnAbilityFailed()
    {
        Debug.Log("FAILURE");
    }

    protected override void OnAbilityUsed()
    {
        Debug.Log("SUCCESS");
    }
}
