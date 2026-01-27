using UnityEngine;

public class LoginCircleAnim : MonoBehaviour
{
    [SerializeField] private float delay = 0;


    private void Start() {
         LeanTween.scale(gameObject, Vector3.one, 0.001f);
         LeanTween.scale(gameObject, new Vector3(0.3f,0.3f,0.3f), 0.5f)
            .setDelay(delay)
            .setEase(LeanTweenType.easeInOutCirc)
            .setLoopPingPong();
    }
}
