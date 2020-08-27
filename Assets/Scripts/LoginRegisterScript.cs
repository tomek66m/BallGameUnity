using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginRegisterScript : MonoBehaviour
{
    public TMP_InputField loginField;
    public TMP_Text infoField;

    public static string serverAdressWithControllerPath = "http://176.122.224.135:5000/";
    //public static string serverAdressWithControllerPath = "http://localhost:52487/";
    public static string uniqueCode = "!zw90.232adsa;!";


    //RequestsManager requestsManager;

    ResponseModel postResult;
    [Serializable]
    public class UnityUpdateScoreVM
    {
        public string MacAddress;
        public int HighScore;
        public string UniqueCode;
    }

    [Serializable]
    public class UnityCheckRegisterVM
    {
        public string MacAddress;
        public string UniqueCode;
    }

    [Serializable]
    public class UnityRegisterPlayerVM
    {
        public string Nick;
        public string MacAddress;
        public string UniqueCode;
    }

    [Serializable]
    public class ResponseModel
    {
        public string status;
    }


    public IEnumerator UnityCheckMacRegistered(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("CheckMacRegistered: " + request.error);
        }
        else
        {
            Debug.Log("CheckMacRegistered:: " + request.responseCode);
            postResult = JsonUtility.FromJson<ResponseModel>(request.downloadHandler.text);
            if (postResult.status == "MacAlreadyAssigned")
                SceneManager.LoadScene("Menu");


            Debug.Log(postResult);
        }
    }

    public IEnumerator UnityRegisterNewUser(string url, string bodyJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("UnityRegisterNewUser: " + request.error);
        }
        else
        {
            Debug.Log("UnityRegisterNewUser: " + request.responseCode);
            postResult = JsonUtility.FromJson<ResponseModel>(request.downloadHandler.text);
            if (postResult.status == "UserAccountCreated")
            {
                PlayerPrefs.SetString("PlayerNickName", loginField.text);
                SceneManager.LoadScene("Menu");
            }


            Debug.Log(postResult);
        }
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
    private void Start()
    {
        UnityCheckRegisterVM tempVM = new UnityCheckRegisterVM { MacAddress = SystemInfo.deviceUniqueIdentifier, UniqueCode = uniqueCode };
        string jsonData = JsonUtility.ToJson(tempVM);

        StartCoroutine(UnityCheckMacRegistered(serverAdressWithControllerPath+"AccountManager/CheckIfMacRegistered", jsonData));

    }


    public void ConfirmButton()
    {
        //infoField.text = "Waiting for confirm...";

        string userLogin = loginField.text;
        if(ValidateLogin(userLogin))
        {
            //Tuple<string, string> GetMICResult = GetNetworkMacAddress();
            UnityRegisterPlayerVM request = new UnityRegisterPlayerVM { MacAddress = SystemInfo.deviceUniqueIdentifier, Nick = userLogin, UniqueCode = LoginRegisterScript.uniqueCode };
            var jsonData = JsonUtility.ToJson(request);
            StartCoroutine(UnityRegisterNewUser(serverAdressWithControllerPath+"AccountManager/AddPlayer", jsonData));
        }
        else
        {
            infoField.text = "Nick must contains less or exatly 8 signs(mixed letters and numbers)!";
        }
    }

    private bool ValidateLogin(string loginFromInputField)
    {
        bool result = true;
        result = Regex.IsMatch(loginFromInputField, @"[a-z0-9]{4,8}");
        return result;
    }

}
