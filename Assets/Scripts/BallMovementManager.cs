using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;
using static LoginRegisterScript;

public class BallMovementManager : MonoBehaviour
{

    // forward X rotation
    public float currentRotateSpeed = 50.0f;
    private float angularVelocity = 0;
    public float GetCurrentAngularVelocity
    {
        get
        {
            return angularVelocity;
        }
    }
    public float GetCurrentRotateSpeed
    {
        get
        {
            return currentRotateSpeed;
        }
    }
    private float RotateSpeedMultiplier = 600.0f;
    private float minRotateSpeed = 500.0f;
    private float maxRotateSpeed = 1000.0f;
    private float currentXRotation = 0f;


    // turning left/right position
    private float currentHorizontalSpeed;
    private float HorizontalSpeedMultiplier = 20.0f;
    private float MaxNegativeHorizontalSpeed = -5.0f;
    private float MaxPositiveHorizontalSpeed = 5.0f;
    //private float currentXPosition = 0f;

    // sideward Y rotation
    private float currentSidewardRotation = 0.0f;
    //private float SideWardRotationMultiplier = 20.0f;
    private float minSideWardRotation = -60.0f;
    private float maxSideWardRotation = 60.0f;

    public float accelx, accely, accelz;

    // collided
    public bool IsCollided = false;
    public Canvas resultCanvas;

    // materials
    private Renderer Renderer;

    public GameObject path1, path2;

    public bool endStage = false;

    // android
    [SerializeField]
    public float verticalSpeedAndroid;
    [SerializeField]
    public float horizontalSpeedAndroid;

    LoginRegisterScript.ResponseModel postResult;

    public bool timeToUpdateScore;


    void Start()
    {
        timeToUpdateScore = true;
        currentXRotation = 0.0f;

        var currentMaterial = this.gameObject.GetComponent<Material>();
        string textureName = PlayerPrefs.GetString("ChosenBallMaterialName");

        Renderer = gameObject.GetComponent<Renderer>();

        // if ball type was chosen
        if (textureName != null || textureName != "")
        {
            Renderer.material.mainTexture = Resources.Load(textureName) as Texture;

        }
        // if ball type wasnt chosen
        else
        {
            Renderer.material.mainTexture = Resources.Load("BallNewUVTextureGrass") as Texture;
        }

    }

    private void Awake()
    {
        currentXRotation = 0.0f;
        timeToUpdateScore = true;

        var currentMaterial = this.gameObject.GetComponent<Material>();
        var textureName = PlayerPrefs.GetString("ChosenBallMaterialName");


    }

    public IEnumerator UnityUpdateUserScore(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("UnityUpdateUserScore: " + request.error);
        }
        else
        {
            Debug.Log("UnityUpdateUserScore: " + request.responseCode);
            postResult = JsonUtility.FromJson<ResponseModel>(request.downloadHandler.text);
            if (postResult.status == "ScoreUpdated")
            {
                Debug.Log("Score updated");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (IsCollided || gameObject.transform.position.y < 0.0f)
        {
            var temp = resultCanvas.GetComponent<Canvas>();
            temp.enabled = true;

            path1.SetActive(false);
            path2.SetActive(false);
            this.gameObject.SetActive(false);

            endStage = true;

            // ustawic score na result
            int score=0;
            if(PlayerPrefs.HasKey("MaxScore"))
            {
                score = PlayerPrefs.GetInt("MaxScore");
            }
            // UPDATE SCORE TO DO
            if(timeToUpdateScore)
            {
                var tempVM = new LoginRegisterScript.UnityUpdateScoreVM { MacAddress = SystemInfo.deviceUniqueIdentifier, UniqueCode = LoginRegisterScript.uniqueCode, HighScore= PlayerPrefs.GetInt("MaxScore") };
                string jsonData = JsonUtility.ToJson(tempVM);
                this.gameObject.SetActive(true);
                StartCoroutine(UnityUpdateUserScore(LoginRegisterScript.serverAdressWithControllerPath+"AccountManager/UpdatePlayerScore", jsonData));
                Debug.Log("updated");
                timeToUpdateScore = false;
            }
            else
            {
                Debug.Log("not update");
            }

        }
        else
        {
            HandleMovement();
        }

    }


    private void HandleMovement()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            accelx = Input.acceleration.x;
            accely = Mathf.Abs(Input.acceleration.y);
            accelz = Mathf.Abs(Input.acceleration.z); // rotation X
            accelz = Mathf.Clamp(accelz, 0.0f, 1.0f);
            verticalSpeedAndroid = accelz * 1000.0f;
            ForwardRolling(verticalSpeedAndroid);

            accelx = Mathf.Clamp(accelx, -0.6f, 0.6f);
            horizontalSpeedAndroid = accelx * 6.5f;
            SidewardRolling(horizontalSpeedAndroid, horizontalSpeedAndroid);
        }


        // other platforms
        else
        {
            float verticalSpeed = Input.GetAxis("Vertical");

            ForwardRolling(verticalSpeed);

            float horizontalSpeed = Input.GetAxis("Horizontal");

            SidewardRolling(horizontalSpeed, horizontalSpeed);

        }
    }
    // przy androidzie prawdopodbnie wystarczy podzielic wartosci inputu sensorow przez 10 ( sensor da wartosci od -10 do 10)
    private void ForwardRolling(float forwardRotationSpeed) // argument is INPUT value
    {
        // rotation WORKING
        currentRotateSpeed = minRotateSpeed + forwardRotationSpeed * RotateSpeedMultiplier;

        currentRotateSpeed = Mathf.Clamp(currentRotateSpeed, minRotateSpeed, maxRotateSpeed);
        currentRotateSpeed *= Time.deltaTime;

        // applying the rotation
        if(currentXRotation > 360.0f)
        {
            currentXRotation = 0.0f;
        }
        currentXRotation += currentRotateSpeed;
        transform.rotation = Quaternion.Euler(currentXRotation, 0.0f, 0.0f); // apply rotation

        // policzyc predkosc katowa i przekazac do offsetu materiału Patha i skrzynek

        angularVelocity = currentRotateSpeed * this.gameObject.GetComponent<SphereCollider>().radius * 100 * Time.deltaTime;
    }

    private void SidewardRolling(float sideWardMoveSpeed, float sideWardRotationSpeed)
    {
        // rotation NOT WORKING
        currentSidewardRotation = 0.0f;
        currentSidewardRotation -= sideWardRotationSpeed * 20.0f * Time.deltaTime;
        currentSidewardRotation = Mathf.Clamp(currentSidewardRotation, minSideWardRotation, maxSideWardRotation);
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentSidewardRotation, 0.0f);

        // movement WORKING
        currentHorizontalSpeed = 0.0f;
        currentHorizontalSpeed += sideWardMoveSpeed * HorizontalSpeedMultiplier * Time.deltaTime;
        currentHorizontalSpeed = Mathf.Clamp(currentHorizontalSpeed, MaxNegativeHorizontalSpeed, MaxPositiveHorizontalSpeed);
        transform.position = new Vector3(transform.position.x + currentHorizontalSpeed, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        IsCollided = true;
    }



}
