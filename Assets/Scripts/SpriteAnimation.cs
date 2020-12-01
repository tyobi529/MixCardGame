using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation : MonoBehaviour
{
    public int Length { get; set; }
    public int No { get; set; }

    public event EventHandler Completed;

    public void Initialize(int no, int length)
    {
        Length = length;
        No = no;
    }

    public void CompleteAnimation()
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}