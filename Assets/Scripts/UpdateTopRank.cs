using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Networking;

[Serializable]
public class PlayerRowModel
{
    [SerializeField]
    public string nick;
    [SerializeField]
    public int highScore;

}

[Serializable]
public class PlayerRowModelList
{
    [SerializeField]
    public PlayerRowModel[] list;
}

public class UpdateTopRank : MonoBehaviour
{
    public ScrollView scrollView;
    public TMP_Text rowPrefab;

    List<PlayerRowModel> topRankList;

    // Start is called before the first frame update
    void Start()
    {
        
        topRankList = new List<PlayerRowModel>();
        StartCoroutine(UnityGetTopRank(LoginRegisterScript.serverAdressWithControllerPath+ "AccountManager/GetAllPlayersScores"));

    }

    public IEnumerator UnityGetTopRank(string url)
    {
        var request = new UnityWebRequest(url, "GET");
        // byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        //request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        //request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("UnityUpdateUserScore: " + request.error);
        }
        else
        {
            Debug.Log("UnityGetTopRank: " + request.responseCode);
            var temp = JsonUtility.FromJson<PlayerRowModelList>(request.downloadHandler.text);

            foreach(var xx in temp.list)
            {
                topRankList.Add(xx);
            }

            int counter = 1;
            foreach (var tempp in topRankList)
            {
                TMP_Text row = Instantiate(rowPrefab, rowPrefab.transform);
                row.text = counter + " | " + tempp.nick + " | Score:" + tempp.highScore;
                row.alignment = TextAlignmentOptions.Left;
                row.transform.parent = transform;
            }

        }
    }

}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
