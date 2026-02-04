using UnityEngine;

public class LoginCircleAnim : MonoBehaviour {
    [SerializeField] private float delay = 0f;

    private void OnEnable() {
        // Reset scale first
        transform.localScale = Vector3.one;

        // Start fresh tween
        LeanTween.scale(gameObject, new Vector3(0.3f, 0.3f, 0.3f), 0.5f)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInOutCirc)
            .setLoopPingPong();
    }

    private void OnDisable() {
        // Stop ALL tweens on this object
        LeanTween.cancel(gameObject);

        // Optional: reset scale when disabled
        transform.localScale = Vector3.one;
    }
}

