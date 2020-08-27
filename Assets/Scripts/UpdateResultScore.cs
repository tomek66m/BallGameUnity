using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResultScore : MonoBehaviour
{
    public TextMesh scoreTextmesh;

    // Update is called once per frame
    void Start()
    {
        this.GetComponent<UnityEngine.UI.Text>().text = scoreTextmesh.text;
    }
}
