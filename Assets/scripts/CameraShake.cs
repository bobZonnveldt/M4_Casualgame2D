using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake _instance;
    private Coroutine _shakeRoutine;

    public static void Shake(float duration = 0.12f, float magnitude = 0.12f)
    {
        if (Camera.main == null)
        {
            return;
        }

        if (_instance == null)
        {
            _instance = Camera.main.GetComponent<CameraShake>();
            if (_instance == null)
            {
                _instance = Camera.main.gameObject.AddComponent<CameraShake>();
            }
        }

        _instance.StartShake(duration, magnitude);
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    private void StartShake(float duration, float magnitude)
    {
        if (_shakeRoutine != null)
        {
            StopCoroutine(_shakeRoutine);
        }

        _shakeRoutine = StartCoroutine(ShakeRoutine(duration, magnitude));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }
}
