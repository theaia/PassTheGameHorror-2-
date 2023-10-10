using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandheldCamera : MonoBehaviour {
    public float moveSpeed = 1.0f;        // RectTransform movement speed.
    public float maxMoveDistance = 10.0f; // Maximum distance the RectTransform can move.
    public float scaleChangeSpeed = 0.1f; // Scale change speed.
    public float uniformScale = 1.0f;     // Uniform scale.
    
    private Vector3 targetPosition;
    private RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        SetNewTargetPosition();
        StartCoroutine(MoveAndScaleCoroutine());
    }

    private void SetNewTargetPosition()
    {
        // Generate a random target position for RectTransform movement.
        float randomX = Random.Range(-maxMoveDistance, maxMoveDistance);
        float randomY = Random.Range(-maxMoveDistance, maxMoveDistance);
        targetPosition = new Vector3(randomX, randomY, 0f);
    }

    private IEnumerator MoveAndScaleCoroutine()
    {
        while (true)
        {
            // Smoothly move the RectTransform to the target position.
            float startTime = Time.time;
            Vector3 startPosition = rectTransform.anchoredPosition;
            float journeyLength = Vector3.Distance(startPosition, targetPosition);

            while (Time.time < startTime + moveSpeed)
            {
                float distanceCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distanceCovered / journeyLength;
                rectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, fractionOfJourney);
                yield return null;
            }

            // Ensure that the RectTransform reaches the exact target position.
            rectTransform.anchoredPosition = targetPosition;

            // Set a new target position.
            SetNewTargetPosition();

            // Uniformly scale the RectTransform.
            Vector3 targetScale = new Vector3(uniformScale, uniformScale, 1f);

            while (Vector3.Distance(rectTransform.localScale, targetScale) > 0.01f)
            {
                rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, scaleChangeSpeed);
                yield return null;
            }

            yield return null;
        }
    }
}
