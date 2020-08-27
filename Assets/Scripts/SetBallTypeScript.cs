using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBallTypeScript : MonoBehaviour
{
    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayerPrefs.SetString("ChosenBallMaterialName", gameObject.GetComponent<Renderer>().material.mainTexture.name);
        }
    }

    private void Update()
    {
        if(PlayerPrefs.GetString("ChosenBallMaterialName") == gameObject.GetComponent<Renderer>().material.mainTexture.name)
        {
            Vector3 scale = gameObject.transform.localScale;
            scale.x = 750f;
            scale.y = 750f;
            scale.z = 750f;

            gameObject.transform.localScale = scale;
        }
        else
        {
            Vector3 scale = gameObject.transform.localScale;
            scale.x = 500f;
            scale.y = 500f;
            scale.z = 500f;

            gameObject.transform.localScale = scale;
        }
    }
}
