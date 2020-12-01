using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingController : MonoBehaviour
{
    private TextMeshProUGUI textMesh;


    private void Start()
    {
        //FadeManager.FadeIn();

        StartCoroutine(Blinking());
    }

    public IEnumerator Blinking()
    {
        textMesh = transform.GetComponent<TextMeshProUGUI>();

        float alpha_Sin;

        Color _color = textMesh.color;

        while (true)
        {
            yield return new WaitForEndOfFrame();

            alpha_Sin = Mathf.Sin(Time.time * 3f) / 2 + 0.5f;

            _color.a = alpha_Sin;

            textMesh.color = _color;
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FadeManager.FadeOut(1);



        }
    }
}
