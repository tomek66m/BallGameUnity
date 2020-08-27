using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public GameObject boxPrefab;
    public GameObject path;
    private BoxCollider pathCollider;
    public GameObject ballPrefab;
    private BallMovementManager ballMovementManager;
    [SerializeField]
    float boxSpeed = 20.0f;
    List<GameObject> boxes;

    // score
    private float score = 0;
    //public TextMesh scoreTextMesh;
    public TextMeshPro scoreTextMesh;
    public TextMeshPro resultScoreTextMesh;
    // Start is called before the first frame update
    private bool maxScoreBreaked = false;
    void Start()
    {
        maxScoreBreaked = false;
        ballMovementManager = ballPrefab.GetComponent<BallMovementManager>();
        boxes = new List<GameObject>();
        pathCollider = path.GetComponent<BoxCollider>();
        for (int i = 60; i < pathCollider.size.y; i += 20)
        {
            // count of box in 20 size
            int boxesInOneSector = Random.Range(1, 2);
            //List<GameObject> boxesToAdd = new List<GameObject>();
            for (int j = 0; j < boxesInOneSector; j++)
            {
                var tempBox = Instantiate(boxPrefab);

                tempBox.transform.position =
                    new Vector3(
                        Random.Range(
                       -pathCollider.size.x / 2 + boxPrefab.GetComponent<MeshRenderer>().bounds.size.x / 2,
                        pathCollider.size.x / 2 - boxPrefab.GetComponent<MeshRenderer>().bounds.size.x / 2),
                        2.17f,
                        Random.Range((float)(i + j), (float)(i + j + 20))
                        );

                boxes.Add(tempBox);
            }
        }

    }

    void Update()
    {
        boxSpeed = ballMovementManager.GetCurrentAngularVelocity;
        CheckIfBoxIsBehindTheCube();
        if(ballMovementManager.endStage == false)
        {
            foreach (var box in boxes)
            {
                var temp = box.transform.position;
                temp.z -= boxSpeed;
                box.transform.position = temp;
            }
            resultScoreTextMesh.text = "";
        }
        else
        {
            foreach(var box in boxes)
            {
                box.SetActive(false);
            }

            if(!PlayerPrefs.HasKey("MaxScore"))
            {
                PlayerPrefs.SetInt("MaxScore", 0);
            }

            if(PlayerPrefs.GetInt("MaxScore") < score && maxScoreBreaked == false)
            {
                PlayerPrefs.SetInt("MaxScore", (int)score);
                maxScoreBreaked = true;
            }

            if(maxScoreBreaked)
            {
                resultScoreTextMesh.text = "You earned: " + score;
                resultScoreTextMesh.faceColor = Color.red;
                resultScoreTextMesh.text += "\n YOU BREAK THE HIGHSCORE!";
            }
            else
            {
                resultScoreTextMesh.text = "You earned: " + score;
            }



        }

    }


    void CheckIfBoxIsBehindTheCube()
    {
        foreach(var box in boxes)
        {
            var temp = box.transform.position;

            if(temp.z <= ballPrefab.transform.position.z - 20.0f && ballMovementManager.endStage==false) // 10 to offset za kulką
            {
                box.transform.position = new Vector3(
                    Random.Range(
                    -pathCollider.size.x / 2 + boxPrefab.GetComponent<MeshRenderer>().bounds.size.x / 2,
                    pathCollider.size.x / 2 - boxPrefab.GetComponent<MeshRenderer>().bounds.size.x / 2),
                    2.17f,
                    400.0f
                    );

                score++;
                scoreTextMesh.text = "Score: " + score.ToString();

            }
        }
    }
}
