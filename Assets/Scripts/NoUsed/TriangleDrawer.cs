//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class TriangleDrawer : Graphic
//{

//    [SerializeField] GamePlayerManager hero;

//    //[SerializeField] Transform firstPos;
//    //[SerializeField] Vector2 firstPos;

//    public float defaultSize;
//    public float size;

//    //public float x;

//    /// <summary>
//    /// uGUIでメッシュ生成する際のコールバック
//    /// </summary>
//    /// <param name="vh">この引数にメッシュを設定すればOK</param>
//    protected override void OnPopulateMesh(VertexHelper vh)
//    {
//        vh.Clear();

//        var v1 = new Vector2(0, (hero.red + 1) * size);
//        var v2 = new Vector2(-(hero.yellow + 1) * size * (1.732f / 2.0f), -(hero.yellow + 1) * size / 2.0f);
//        var v3 = new Vector2((hero.green + 1) * size * (1.732f / 2.0f), -(hero.green + 1) * size / 2.0f);

//        //int max = hero.red;
//        //if (max < hero.yellow)
//        //{
//        //    max = hero.yellow;
//        //}
//        //if (max < hero.green)
//        //{
//        //    max = hero.green;
//        //}

//        //int redLack = max - hero.red;
//        //if (redLack > 3)
//        //{
//        //    redLack = 3;
//        //}
//        //int yellowLack = max - hero.yellow;
//        //if (yellowLack > 3)
//        //{
//        //    yellowLack = 3;
//        //}
//        //int greenLack = max - hero.green;
//        //if (greenLack > 3)
//        //{
//        //    greenLack = 3;
//        //}


//        //vh.Clear();
//        ////（１）座標の準備
//        //var v1 = new Vector2(0, defaultSize - redLack * size);
//        //var v2 = new Vector2((-defaultSize + yellowLack * size) * (1.732f / 2.0f), (-defaultSize + yellowLack * size) / 2.0f);
//        //var v3 = new Vector2((defaultSize - greenLack * size) * (1.732f / 2.0f), (-defaultSize + greenLack * size) / 2.0f);


//        // (２)（１）の座標に頂点を追加
//        AddVert(vh, v1);
//        AddVert(vh, v2);
//        AddVert(vh, v3);
//        // (３)（２）で追加した頂点に三角形メッシュを設定
//        vh.AddTriangle(0, 1, 2);
//    }

//    private void AddVert(VertexHelper vh, Vector2 pos)
//    {
//        var vert = UIVertex.simpleVert;
//        vert.position = pos;
//        vert.color = color;
//        vh.AddVert(vert);
//    }

//    //protected override void Awake()
//    //{
//    //    firstPos = transform.position;
//    //}

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            SetVerticesDirty();

//        }


//        if (Input.GetKeyDown(KeyCode.A))
//        {
//            //x += 100f;
//        }
//    }

    
//}
