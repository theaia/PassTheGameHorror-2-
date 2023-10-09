using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FlashingLight : MonoBehaviour {
    private Light2D light;

    [SerializeField] private float speed;

    private void Awake() {
        light = GetComponent<Light2D>();
    }

    private IEnumerator Start() {
        float _randomWait = Random.Range(0f, 3f);
        while (true) {
            Color _color = light.color;
            _color.a = Mathf.PingPong((Time.time + _randomWait) * speed, 1);
            light.color = _color;
            yield return new WaitForEndOfFrame();
        }
    }
}
