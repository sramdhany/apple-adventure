using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

    bool isFalling = false;
    float downSpeed = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            isFalling = true;
            Destroy(gameObject, 10);
        }
    }

    void Update()
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime/15;
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }
}
