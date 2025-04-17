using UnityEngine;
using LighthouseKeeper.GameStates;

namespace LighthouseKeeper.Environment
{
    public class FuelGauge : MonoBehaviour
    {
        [SerializeField] string fuelTag = "fuel.level";

        [SerializeField] float emptyRotation;
        [SerializeField] float fullRotation;

        int hash;

        void Awake()
        {
            if (!GameState.TryGet(fuelTag, out int fuelLevel))
            {
                fuelLevel = 0;
            }
            SetRotation(0.01f * fuelLevel);

            hash = GameState.GetHash(fuelTag) % 16;
            GameState.OnStateChangeHash += OnStateChange;
        }

        void OnDestroy()
        {
            GameState.OnStateChangeHash -= OnStateChange;
        }

        void OnStateChange(int hash)
        {
            if ((this.hash & hash) == 0) return;
            if (!GameState.TryGet(fuelTag, out int fuelLevel)) return;
            SetRotation(0.01f * fuelLevel);
        }

        void SetRotation(float fuel)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(emptyRotation, fullRotation, fuel));
        }
    }
}