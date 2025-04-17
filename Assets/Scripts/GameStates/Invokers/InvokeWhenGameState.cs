namespace LighthouseKeeper.GameStates
{
    public class InvokeWhenGameState : InvokerBehaviour
    {
        bool wasMet;
        int hash;


        void Awake()
        {
            hash = GetHash();
            wasMet = IsMet();

            if (wasMet) TryInvoke();

            GameState.OnStateChangeHash += OnStateChange;
        }


        void OnStateChange(int changedHash)
        {
            if ((hash & changedHash) == 0) return;
            if (wasMet == IsMet()) return;

            wasMet = !wasMet;
            if (wasMet) TryInvoke();
        }


        void OnDestroy()
        {
            GameState.OnStateChangeHash -= OnStateChange;
        }
    }
}