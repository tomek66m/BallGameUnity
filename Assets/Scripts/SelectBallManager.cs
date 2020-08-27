using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBallManager : MonoBehaviour
{
    public GameObject ballModelFire, ballModelGrass, ballModelWater;
    private float rotateSpeed = 100.0f;
    public float currentRotationX = 0.0f;

    // Update is called once per frame
    void Update()
    {
        currentRotationX += rotateSpeed * Time.deltaTime;
        ballModelFire.transform.rotation = Quaternion.Euler
            (currentRotationX,
            0.0f, 0.0f);
        ballModelGrass.transform.rotation = Quaternion.Euler
            (currentRotationX,
            0.0f, 0.0f);
        ballModelWater.transform.rotation = Quaternion.Euler
            (currentRotationX,
            0.0f, 0.0f);
    }

    private void OnMouseOver()
    {
        
    }
}
