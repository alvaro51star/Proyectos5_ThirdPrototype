using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSizeAddapt : MonoBehaviour
{
    [SerializeField] private RectTransform buttonPanel;
    private GridLayoutGroup gridLayoutGroup;

    // Start is called before the first frame update
    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        int childCount = buttonPanel.childCount;
        float cellSizeY = gridLayoutGroup.cellSize.y;
        float spacingY = gridLayoutGroup.cellSize.y;
        buttonPanel.sizeDelta = new Vector2(buttonPanel.sizeDelta.x, (170 * childCount));
    }
}
