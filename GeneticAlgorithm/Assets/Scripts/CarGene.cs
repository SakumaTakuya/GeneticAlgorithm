﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CarGene
{
    [SerializeField] private float _time;
    [Range(-1,1)] public int Front;
    [Range(-1,1)] public int Right;
    [HideInInspector] public float CurrentTime;
    
    public CarGene(float time, int front, int right)
    {
        _time = time;
        Front = front;
        Right = right;
        CurrentTime = time;
    }

    public void Reset()
    {
        CurrentTime = _time;
    }
}
