using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmIK : MonoBehaviour
{
	public Transform handPosMarker;
	public Transform shoulder;
	public Transform elbow;
	public Transform hand;

	public bool elbowFlipped = true;

	float upperArmLen = 0.0f;
	float lowerArmLen = 0.0f;
	float maxLen = 0.0f;

    private void Start()
    {
		UpdateArmLens();
    }
    void UpdateArmLens()
    {
		upperArmLen = Vector3.Distance(shoulder.position, elbow.position);
		lowerArmLen = Vector3.Distance(hand.position, elbow.position);
		maxLen = upperArmLen + lowerArmLen;
	}

    private void Update()
    {
		//float dist = Vector2.Distance(hand.position, handPosMarker.position);
		//if (dist < 0.01f)
		//	return;
		Vector2 goalHandPos = handPosMarker.position;
		Vector2 handPos = getHandPos(handPosMarker.position);
		Vector2 elbowPos = getElbowPos(goalHandPos);

		float shoulderAngle = Vector2.SignedAngle(Vector2.right, elbowPos - (Vector2)transform.position) + 180;
		//float shoulderAngle = Vector2.SignedAngle(Vector2.right, handPos - (Vector2)transform.position) + 180;
		shoulder.rotation = Quaternion.Euler(0, 0, shoulderAngle);
		elbow.position = elbowPos;

		float elbowAngle = Vector2.SignedAngle(Vector2.right, handPos - elbowPos) + 180;
		elbow.rotation = Quaternion.Euler(0, 0, elbowAngle);
	}

	Vector2 getHandPos(Vector2 goalHandPos)
    {
		Vector2 handPos = goalHandPos;
		if (Vector2.Distance(shoulder.position, handPos) > maxLen)
        {
			Vector2 pos = transform.position;
			Vector2 dir = (handPos - pos).normalized;
			handPos = pos + dir * maxLen;
		}
		return handPos;
    }
	Vector2 getElbowPos(Vector2 goalHandPos)
	{
		Vector2 localHandPos = transform.InverseTransformPoint(goalHandPos);
		
		float handXSQ = localHandPos.x * localHandPos.x;
		float handYSQ = localHandPos.y * localHandPos.y;
		float n = upperArmLen * upperArmLen + handXSQ + handYSQ - lowerArmLen * lowerArmLen;
		float d = 2 * upperArmLen * Mathf.Sqrt(handXSQ + handYSQ);
		float relativeElbowAngle = Mathf.Acos(n / d) * Mathf.Rad2Deg;
		
		if (float.IsNaN(relativeElbowAngle))
			relativeElbowAngle = 0.0f;
		if (elbowFlipped)
			relativeElbowAngle *= -1;

		float angle = relativeElbowAngle + Vector2.SignedAngle(Vector2.right, localHandPos);
		angle *= Mathf.Deg2Rad;
		Vector2 localPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * upperArmLen;
		return transform.TransformPoint(localPos);
	}


	
}
