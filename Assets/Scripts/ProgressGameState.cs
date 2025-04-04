using System.Collections;
using UnityEngine;

namespace LighthouseKeeper
{
    public class ProgressGameStateToPlaying : MonoBehaviour
    {
        [SerializeField, Min(0)] float delay;

        void OnEnable() => StartCoroutine(ProgressCoroutine());

        IEnumerator ProgressCoroutine()
        {
            yield return new WaitForSeconds(delay);

            GameManager.Instance.SetState(GameManager.State.Playing);
            Destroy(this);
        }
    }
}