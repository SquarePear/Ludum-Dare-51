using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave", order = 1)]
public class Wave : ScriptableObject {
    [SerializeField]
    public List<WaveItemSpot> waveItems = new List<WaveItemSpot>();
    [SerializeField]
    public List<WaveItemCraft> waveItemCrafts = new List<WaveItemCraft>();
}

[Serializable]
public class WaveItemSpot {
    public GameObject spotPrefab;
    public ItemType itemType;
}

[Serializable]
public class WaveItemCraft {
    public GameObject craftPrefab;
    public CraftRecipe recipe;
}