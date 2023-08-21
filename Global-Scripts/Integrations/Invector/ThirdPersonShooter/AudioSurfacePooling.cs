using UnityEngine.Pool;
using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace Invector
{
    public class AudioSurfacePooling : MonoBehaviour
    {
        [BoxGroup("Pool Prefabs")]
        public GameObject audioSourcePrefab;
        [BoxGroup("Pool Prefabs")]
        public GameObject particleFxPrefab;
        [BoxGroup("Pool Prefabs")]
        public GameObject stepPrefabHuman;
        [BoxGroup("Pool Prefabs")]
        public GameObject stepPrefabHorse;

        [FoldoutGroup("Debugging")]
        public bool debug = false;
        [FoldoutGroup("Debugging")]
        public int audioPoolSize;
        [FoldoutGroup("Debugging")]
        public int particlePoolSize;
        [FoldoutGroup("Debugging")]
        public int stepPoolSize;

        [BoxGroup("Lifetime Settings")]
        public static float audioSourceDelay = 2.0f;
        [BoxGroup("Lifetime Settings")]
        public static float particleDelay = 2.0f;
        [BoxGroup("Lifetime Settings")]
        public static float stepDelay = 5.0f;

        [FoldoutGroup("Containers")]
        public static GameObject AudioSourceContainer;
        [FoldoutGroup("Containers")]
        public static GameObject ParticleContainer;
        [FoldoutGroup("Containers")]
        public static GameObject StepHumanContainer;
        [FoldoutGroup("Containers")]
        public static GameObject StepHorseContainer;

        // Added for Object Pooling
        public static ObjectPool<GameObject> AudioSourcePool;
        public static ObjectPool<GameObject> ParticlePool;
        public static ObjectPool<GameObject> StepMarkHumanPool;
        public static ObjectPool<GameObject> StepMarkHorsePool;

        private static MonoBehaviour _instance;

        private void Awake()
        {
            _instance = this;

            AudioSourcePool = new ObjectPool<GameObject>(AudioSourceCreatePoolItem, AudioSourceOnTakeFromPool, AudioSourceOnReturnToPool, AudioSourceOnDestroyPoolObject, false, 100, 200);
            ParticlePool = new ObjectPool<GameObject>(ParticleCreatePoolItem, ParticleOnTakeFromPool, ParticleOnReturnToPool, ParticleOnDestroyPoolObject, false, 20, 50);
            StepMarkHumanPool = new ObjectPool<GameObject>(StepMarkHumanCreatePoolItem, StepMarkHumanOnTakeFromPool, StepMarkHumanOnReturnToPool, StepMarkHumanOnDestroyPoolObject, false, 20, 50);
            StepMarkHorsePool = new ObjectPool<GameObject>(StepMarkHorseCreatePoolItem, StepMarkHorseOnTakeFromPool, StepMarkHorseOnReturnToPool, StepMarkHorseOnDestroyPoolObject, false, 20, 50);

            AudioSourceContainer = new GameObject("Audio Source Pool");
            AudioSourceContainer.transform.SetParent(transform, false);

            ParticleContainer = new GameObject("Particle Pool");
            ParticleContainer.transform.SetParent(transform, false);

            StepHumanContainer = new GameObject("Step Human Pool");
            StepHumanContainer.transform.SetParent(transform, false);

            StepHorseContainer = new GameObject("Step Horse Pool");
            StepHorseContainer.transform.SetParent(transform, false);
            
        }

        /// <summary>
        /// Calculate pool sizes, if debugging
        /// </summary>
        private void Update()
        {
            if (!debug)
            {
                return;
            }

            audioPoolSize = AudioSourcePool.CountAll;
            stepPoolSize = StepMarkHumanPool.CountAll;
            particlePoolSize= ParticlePool.CountAll;

        }

        private GameObject AudioSourceCreatePoolItem()
        {
            GameObject newAudioSourceGameObject = Instantiate(audioSourcePrefab);
            newAudioSourceGameObject.name = "AudioSourcePool-New";
            newAudioSourceGameObject.transform.SetParent(AudioSourceContainer.transform, false);
            return newAudioSourceGameObject;
        }

        private void AudioSourceOnTakeFromPool(GameObject audioSourceGameObject)
        {
            audioSourceGameObject.name = "AudioSourcePool-Taken";
            audioSourceGameObject.SetActive(true);
        }

        private void AudioSourceOnReturnToPool(GameObject audioSourceGameObject)
        {
            if (!audioSourceGameObject)
            {
                Debug.Log("AudioSourceOnReturnToPool: AudioSourceGameObject is null!");
                return;
            }

            audioSourceGameObject.name = "AudioSourcePool-Returned";

            if (audioSourceGameObject.transform)
            {
                audioSourceGameObject.transform.position = Vector3.zero;
                audioSourceGameObject.gameObject.transform.rotation = Quaternion.identity;
            }
            if (audioSourceGameObject)
            {
                audioSourceGameObject.SetActive(false);
            }
        }

        private void AudioSourceOnDestroyPoolObject(GameObject audioSourceGameObject)
        {
            Destroy(audioSourceGameObject);
        }

        public static void AudioSourceReturnToPool(GameObject audioSourceGameObject)
        {
            _instance.StartCoroutine(AudioSourceReturnToPoolAsync(audioSourceGameObject, audioSourceDelay));
        }

        public static void StepMarkHumanReturnToPool(GameObject stepMarkGameObject)
        {
            _instance.StartCoroutine(StepMarkHumanReturnToPoolAsync(stepMarkGameObject, stepDelay));
        }

        public static void StepMarkHorseReturnToPool(GameObject stepMarkGameObject)
        {
            _instance.StartCoroutine(StepMarkHorseReturnToPoolAsync(stepMarkGameObject, stepDelay));
        }
        
        public static void ParticleReturnToPool(GameObject particleGameObject)
        {
            _instance.StartCoroutine(ParticleReturnToPoolAsync(particleGameObject, particleDelay));
        }

        public static IEnumerator AudioSourceReturnToPoolAsync(GameObject audioSourceGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            AudioSourcePool.Release(audioSourceGameObject);
        }

        public static IEnumerator ParticleReturnToPoolAsync(GameObject particleGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            ParticlePool.Release(particleGameObject);
        }

        public static IEnumerator StepMarkHumanReturnToPoolAsync(GameObject stepMarkGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            StepMarkHumanPool.Release(stepMarkGameObject);
        }

        public static IEnumerator StepMarkHorseReturnToPoolAsync(GameObject stepMarkGameObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            StepMarkHorsePool.Release(stepMarkGameObject);
        }
        
        private GameObject ParticleCreatePoolItem()
        {
            GameObject gameObject = Instantiate(particleFxPrefab);
            gameObject.name = "ParticlePool-New";
            gameObject.transform.SetParent(ParticleContainer.transform, false);
            return gameObject;
        }

        private void ParticleOnTakeFromPool(GameObject particleGameObject)
        {
            particleGameObject.name = "ParticlePool-Taken";
            particleGameObject.SetActive(true);
        }

        private void ParticleOnReturnToPool(GameObject particleGameObject)
        {
            particleGameObject.name = "ParticlePool-Returned";
            particleGameObject.transform.position = Vector3.zero;
            particleGameObject.transform.rotation = Quaternion.identity;
            particleGameObject.SetActive(false);
        }

        private void ParticleOnDestroyPoolObject(GameObject particleGameObject)
        {
            Destroy(particleGameObject);
        }

        private GameObject StepMarkHorseCreatePoolItem()
        {
            GameObject gameObject = Instantiate<GameObject>(stepPrefabHorse);
            gameObject.name = "StepMarkHorsePool-New";
            gameObject.transform.SetParent(StepHorseContainer.transform, false);
            return gameObject;
        }

        private void StepMarkHorseOnTakeFromPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.name = "StepMarkHorsePool-Taken";
            stepMarkGameObject.SetActive(true);
        }

        private void StepMarkHorseOnReturnToPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.name = "StepMarkHorsePool-Returned";
            stepMarkGameObject.transform.position = Vector3.zero;
            stepMarkGameObject.transform.rotation = Quaternion.identity;
            stepMarkGameObject.SetActive(false);
        }

        private void StepMarkHorseOnDestroyPoolObject(GameObject stepMarkGameObject)
        {
            Destroy(stepMarkGameObject);
        }
        
        private GameObject StepMarkHumanCreatePoolItem()
        {
            GameObject gameObject = Instantiate<GameObject>(stepPrefabHuman);
            gameObject.name = "StepMarkHumanPool-New";
            gameObject.transform.SetParent(StepHumanContainer.transform, false);
            return gameObject;
        }

        private void StepMarkHumanOnTakeFromPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.name = "StepMarkHumanPool-Taken";
            stepMarkGameObject.SetActive(true);
        }

        private void StepMarkHumanOnReturnToPool(GameObject stepMarkGameObject)
        {
            stepMarkGameObject.name = "StepMarkHumanPool-Returned";
            stepMarkGameObject.transform.position = Vector3.zero;
            stepMarkGameObject.transform.rotation = Quaternion.identity;
            stepMarkGameObject.SetActive(false);
        }

        private void StepMarkHumanOnDestroyPoolObject(GameObject stepMarkGameObject)
        {
            Destroy(stepMarkGameObject);
        }
        
    }
}