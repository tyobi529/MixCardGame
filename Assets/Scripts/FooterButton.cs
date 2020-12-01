using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEditor;

public class FooterButton : MonoBehaviour
{
    [SerializeField] int pageNum;

    [SerializeField] Transform canvasTransform; 

    public void OnFooterButton()
    {

        GameObject origin = canvasTransform.GetChild(0).gameObject;
               
        origin.GetComponent<SimpleScrollSnap>().enabled = false;


        GameObject clone = Instantiate(origin, canvasTransform);
        clone.transform.SetAsFirstSibling();


        Destroy(origin);
        clone.GetComponent<SimpleScrollSnap>().startingPanel = pageNum;
        clone.GetComponent<SimpleScrollSnap>().enabled = true;

    }

}
