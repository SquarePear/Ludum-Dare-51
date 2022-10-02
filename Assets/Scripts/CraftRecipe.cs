using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class CraftRecipe : SerializedScriptableObject {
    public Dictionary<ItemType, int> requiredItems;
    public ItemType resultItem;
}