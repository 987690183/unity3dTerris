using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SquareSprite : MonoBehaviour{
    /// <summary>
    /// 在网格的X值
    /// </summary>
    public int gridX;
    /// <summary>
    /// 在网格的Y值
    /// </summary>
    public int gridY;
    /// <summary>
    /// 单个方格图片
    /// </summary>
    public Image sprite;
    /// <summary>
    /// 当前状态
    /// </summary>
    private int status;

    public int Status
    {
        get { return status; }
        set
        {
            if (value == 1)
            {
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = Color.black;
            }
            status = value;

        }
    }
}
