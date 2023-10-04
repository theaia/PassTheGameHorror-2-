using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSprayer : MonoBehaviour
{
    public GameObject bloodSprite;
    public float maxSprayRange = 1f;
    public int minBloodAmnt = 2;
    public int maxBloodAmnt = 4;
    public LayerMask layerMask;

    public void SprayBlood()
    {
        int bloodAmnt = Random.Range(minBloodAmnt, maxBloodAmnt);
        for (int i = 0; i < bloodAmnt; i++)
        {
            float sprayRange = Random.Range(0.0f, maxSprayRange);
            float sprayAngle = Random.Range(0.0f, 360f) * Mathf.Deg2Rad;
            Vector2 sprayDir = new Vector2(Mathf.Cos(sprayAngle), Mathf.Sin(sprayAngle));
            Vector2 bloodPos = (Vector2)transform.position + sprayDir;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, sprayDir, sprayRange, layerMask);
            if (hit.collider != null)
                bloodPos = hit.point;

            Instantiate(bloodSprite, bloodPos, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        }
    }
}
