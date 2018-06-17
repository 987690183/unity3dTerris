using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StartGame : MonoBehaviour {
    /// <summary>
    /// 开始和再次UI界面 再次按钮
    /// </summary>
    public RectTransform startRect, againRect,againBtn;
    /// <summary>
    /// 得分 计时
    /// </summary>
    public Text socre, time;
    /// <summary>
    /// 计时
    /// </summary>
    private float fTime=0.0f;
    private int iMinute=0, iSecond=0;
    /// <summary>
    /// 预设
    /// </summary>
    public GameObject goPrefab;
	// Use this for initialization
	void Start () {
        FunctionTeris.getInstance().InitGame(againRect, goPrefab);
	}
	
	// Update is called once per frame
	void Update () {
        FunctionTeris.getInstance().DealGameFunction();
        if (FunctionTeris.getInstance().isGameStop)
        {
            if (!againBtn.gameObject.activeSelf)
                againBtn.gameObject.SetActive(true);
        }
        else
        {
            fTime += Time.fixedDeltaTime;
            iSecond = (int)fTime;
            if (iSecond == 60)
            {
                iMinute++;
                iSecond = 0;
                fTime -= 60;
            }
            socre.text = "Score: " + FunctionTeris.getInstance().iScore;
            time.text = "Time: " + iMinute + "m " + iSecond + "s";
        }
	}

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void BtnStartGame()
    {
        againRect.gameObject.SetActive(true);
        Destroy(startRect.gameObject);
        againBtn.gameObject.SetActive(false);
        FunctionTeris.getInstance().BeginGame();

    }
    /// <summary>
    /// 再次游戏
    /// </summary>
    public void BtnAgainGame()
    {
        fTime = 0.0f;
        iMinute = 0;
        iSecond = 0;
        againBtn.gameObject.SetActive(false);
        FunctionTeris.getInstance().BeginGame();
    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void BtnQuitGame()
    {
        Application.Quit();
    }
}
