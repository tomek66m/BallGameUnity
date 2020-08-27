using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSectorManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public List<GameObject> boxPrefabsList;
    public float boxMaxCount = 10;

    public GameObject ballPrefab;
    private BallMovementManager ballMovementManager;

    // Start is called before the first frame update
    void Start()
    {
        ballMovementManager = ballPrefab.GetComponent<BallMovementManager>();
        boxPrefabsList = new List<GameObject>();
        RandomizeBoxPosition();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var box in boxPrefabsList)
        {
            if (box.transform.position.z <= -10.0f)
            {
                // set sector to end of path
                transform.position = new Vector3(transform.position.x, transform.position.y, 500.0f);
            }
            else
            {
                Vector3 newPosition = box.transform.position;
                newPosition.z -= ballMovementManager.GetCurrentAngularVelocity;
                box.transform.position = newPosition;
            }
        }
    }

    private void RandomizeBoxPosition()
    {
        for(int i =0;i<boxMaxCount;i++)
        {
            var tempBox = Instantiate(boxPrefab);

            tempBox.transform.position =
                new Vector3(
                    Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                    2.17f,
                    Random.Range(0, transform.localScale.z)
                    );

            boxPrefabsList.Add(tempBox);
        }
    }
}
