using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CatType", menuName = "CatCafeProject/Cats", order = 0)]
public class CatDataSO : ScriptableObject
{
    public CatDifficulty difficulty;
    public List<FurnitureTheme> likes;
    public List<FurnitureTheme> dislikes;
}
