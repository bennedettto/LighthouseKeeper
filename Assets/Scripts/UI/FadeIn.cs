using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LighthouseKeeper.UI
{
    public class FadeInOut : MonoBehaviour
    {
        enum FadeType
        {
            Out,
            In,
        }

        [SerializeField] FadeType fadeType = FadeType.Out;

        [SerializeField] GameObject target;

        [SerializeField, Range(0, 10)] float duration;

        float time = 0;


        void Start()
        {
            time = 0f;
            SetColor(fadeType == FadeType.Out ? 1 : 0);
            StartCoroutine(UpdateCoroutine());
        }


        IEnumerator UpdateCoroutine()
        {
            for (;;)
            {
                time += Time.deltaTime;
                if (time > duration) break;

                SetColor(fadeType == FadeType.Out
                             ? 1 - Mathf.Pow(time / duration, 2)
                             :     Mathf.Pow(time / duration, 2));
                yield return null;
            }

            gameObject.SetActive(false);
            target.SetActive(false);
        }

        void SetColor(float alpha)
        {
            var images = target.GetComponentsInChildren<Image>();
            for (int i = 0; i < images.Length; i++)
            {
                var color = images[i].color;
                color.a = alpha;
                images[i].color = color;

            }

            var tmps = target.GetComponentsInChildren<TextMeshPro>();
            for (int i = 0; i < tmps.Length; i++)
            {
                var color = tmps[i].color;
                color.a = alpha;
                tmps[i].color = color;
            }
        }


    }
}