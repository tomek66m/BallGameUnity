using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathOffseting : MonoBehaviour
{

    public GameObject ballPrefab;
    private BallMovementManager ballMovementManager;


    // Start is called before the first frame update
    void Start()
    {
        ballMovementManager = ballPrefab.GetComponent<BallMovementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // moving path after disappearing from camera viewport
        if(this.transform.position.z < -510.0f)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, 490.0f);
        }
        // moving path
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - ballMovementManager.GetCurrentAngularVelocity);
    }

}
