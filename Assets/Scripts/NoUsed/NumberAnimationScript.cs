using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberAnimationScript : MonoBehaviour
{


    void DestroyNumber()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
