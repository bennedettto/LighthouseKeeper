namespace LighthouseKeeper.GameStates
{
    public interface IConditionNode
    {
        bool IsMet();
        int GetHash();
    }
}