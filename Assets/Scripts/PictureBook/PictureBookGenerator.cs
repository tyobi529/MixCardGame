using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureBookGenerator : MonoBehaviour
{
    [SerializeField] GameObject PictureBookPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GeneratePictureBook();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void GeneratePictureBook()
    {
        for (int i = 0; i < 27; i++)
        {
            GameObject pictureBook = Instantiate(PictureBookPrefab, transform);

            pictureBook.GetComponent<PictureBookView>().SetView(i);

        }

    }
}
