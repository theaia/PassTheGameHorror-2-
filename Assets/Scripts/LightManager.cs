using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    private Light2D light2DComponent;
    private Collider2D lightCollider;

    private void Start()
    {
        light2DComponent = GetComponent<Light2D>();
    }

    public void AdjustLight(bool value)
    {
        lightCollider.enabled = value;
        light2DComponent.enabled = value;
    }
}
