using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtAimingPoint : MonoBehaviour
{
    public GameObject target; // GameObject the player will rotate to

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("AimingPoint");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotating the player to the position of target GameObject
        Vector3 positionToRotate = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.LookAt(positionToRotate);
        transform.Rotate(0, 90, 0);
    }
}
