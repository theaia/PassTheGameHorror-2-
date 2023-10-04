using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSprite : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int v = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == v);
        }
        //transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360.0f));
    }

}
