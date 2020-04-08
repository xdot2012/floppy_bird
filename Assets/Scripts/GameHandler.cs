using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log("GameHandler Start");

        GameObject gameObject = new GameObject("Block",typeof(SpriteRenderer));
        //gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.getInstance().BlockHead;

        Debug.Log("GameHandler Done");
    }

    // Update is called once per frame
    void Update()
    {


    }
}
