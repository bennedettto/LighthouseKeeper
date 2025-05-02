using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace LighthouseKeeper
{
    public class AudioSystem : MonoBehaviour
    {
        public enum Type
        {
            Music,
            SFX,
            Voice,
        }

        public static AudioSystem Instance;


        AudioSource audioSource;

        readonly ObjectPool<AudioSource> pool = new ObjectPool<AudioSource>(Create, OnGet, OnRelease, DestroyAudioSource, true, 10, 20);


        void Awake()
        {
            Instance = this;

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 0f;
        }

        static AudioSource Create()
        {
            var go = new GameObject("AudioSource");
            var audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f;
            return audioSource;
        }

        static void OnGet(AudioSource source)
        {
            source.Stop();
            source.gameObject.SetActive(true);
        }

        static void DestroyAudioSource(AudioSource source) => Destroy(source.gameObject);

        static void OnRelease(AudioSource source)
        {
            source.Stop();
            source.transform.SetParent(null);
            source.gameObject.SetActive(false);
        }



        public void Play(AudioClip clip, Type type)
        {
            audioSource.PlayOneShot(clip);
        }

        public void Play(AudioClip clip, Type type, Transform parent)
        {
            var source = pool.Get();
            Transform transform1= source.transform;
            transform1.SetParent(parent);
            transform1.localPosition = Vector3.zero;
            source.PlayOneShot(clip);
            StartCoroutine(ReleaseCoroutine(source, clip.length + 0.2f));
        }

        IEnumerator ReleaseCoroutine(AudioSource source, float time)
        {
            yield return new WaitForSeconds(time);
            pool.Release(source);
        }

        public void Play(AudioClip clip, Type type, Vector3 position)
        {
            var source = pool.Get();
            source.transform.position = position;
            source.PlayOneShot(clip);
            StartCoroutine(ReleaseCoroutine(source, clip.length + 0.2f));
        }



    }
}