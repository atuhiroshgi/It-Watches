public class SkillBase
{
    protected int skillCost;
    protected bool isRunning = false;

    public virtual void Activate()
    {

    }

    public virtual int GetSkillCost()
    {
        return skillCost;
    }

    public virtual bool CanActivate()
    {
        return !isRunning;
    }
}
