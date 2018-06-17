using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FunctionTeris  {

    //// Use this for initialization
    //void Start () {
    //    DrawGridBG(transform);
    //}
	
    //// Update is called once per frame
    //void Update () {
    //    DealGameFunction();
    //}

    /// <summary>
    /// 对象
    /// </summary>
    private static FunctionTeris functionTeris;
    /// <summary>
    /// 定义一个二维数组 大的网格
    /// </summary>
    public SquareSprite[,] BigGridBG;
    /// <summary>
    /// 小网格
    /// </summary>
    public SquareSprite[,] SmallGridBG;
    /// <summary>
    /// 方格宽度
    /// </summary>
    private int iSpriteWidth = 30;
    /// <summary>
    /// 大网格的起始位置
    /// </summary>
    private int iBigPosX = -100, iBigPosY = 300,iBigStartX=-100;
    /// <summary>
    /// 小网格的起始位置
    /// </summary>
    private int iSmallPosX = -300, iSmallPosY = 300,iSmallStartX=-300;
    /// <summary>
    /// 单个的方格
    /// </summary>
    private GameObject goSquareSprite;
    /// <summary>
    /// 形状  方向
    /// </summary>
    private int iShape, iDirection, iNewShape, iNewDirection;
    /// <summary>
    /// iCurrY表示纵向Y轴的增加值，iCurrX表示横向X轴的增加值
    /// </summary>
    private int iCurrY=0, iCurrX = 3,iCurrXStart=3;
    /// <summary>
    /// 当前移动的白色方格列表
    /// </summary>
    private List<SquareSprite> listSquare = new List<SquareSprite>();
    /// <summary>
    /// 游戏是否停止
    /// </summary>
    public bool isGameStop = true;
    /// <summary>
    /// 游戏的刷新时间
    /// </summary>
    private float fUpdateTime = 0.0f;
    /// <summary>
    /// 按住下键的刷新时间
    /// </summary>
    private float fDownTime = 0.0f;
    /// <summary>
    /// 得分
    /// </summary>
    public int iScore = 0;
    /// <summary>
    /// 返回本类的对象
    /// </summary>
    /// <returns></returns>
    public static FunctionTeris getInstance()
    {
        if (functionTeris == null)
        {
            functionTeris = new FunctionTeris();
        }
        return functionTeris;
    }
    /// <summary>
    /// 初始化
    /// </summary>
    public void InitGame(Transform parent,GameObject go)
    {
        goSquareSprite = go;
        if (goSquareSprite == null)
            return;

        DrawBigGridBG(parent);
        DrawSmallGridBG(parent);
        isGameStop = false;
    }
    /// <summary>
    /// 绘制大网格
    /// </summary>
    private void DrawBigGridBG(Transform parent)
    {
        BigGridBG = new SquareSprite[20, 10];
        for (int i = 0; i < BigGridBG.GetLength(0); i++)
        {
            for (int j = 0; j < BigGridBG.GetLength(1); j++)
            {
                GameObject _gos = GameObject.Instantiate(goSquareSprite) as GameObject;
                BigGridBG[i,j] = _gos.GetComponent<SquareSprite>();
                BigGridBG[i, j].gridX= i;
                BigGridBG[i, j].gridY = j;
                BigGridBG[i, j].sprite= _gos.GetComponent<Image>();
                //BigGridBG[i, j].sprite.transform.parent = parent;
                BigGridBG[i, j].sprite.transform.SetParent(parent);
                BigGridBG[i, j].sprite.transform.localScale = Vector3.one;
                BigGridBG[i, j].sprite.transform.localPosition = new Vector3(iBigPosX, iBigPosY);
                BigGridBG[i, j].Status = 0;
                iBigPosX += iSpriteWidth;
            }
            iBigPosY -= iSpriteWidth;
            iBigPosX = iBigStartX;
        }
    }
    /// <summary>
    /// 绘制小网格
    /// </summary>
    private void DrawSmallGridBG(Transform parent)
    {
        SmallGridBG = new SquareSprite[4, 4];
        for (int i = 0; i < SmallGridBG.GetLength(0); i++)
        {
            for (int j = 0; j < SmallGridBG.GetLength(1); j++)
            {
                GameObject _gos = GameObject.Instantiate(goSquareSprite) as GameObject;
                SmallGridBG[i, j] = _gos.GetComponent<SquareSprite>();
                SmallGridBG[i, j].gridX = i;
                SmallGridBG[i, j].gridY = j;
                SmallGridBG[i, j].sprite = _gos.GetComponent<Image>();
                //SmallGridBG[i, j].sprite.transform.parent = parent;
                SmallGridBG[i, j].sprite.transform.SetParent(parent);
                SmallGridBG[i, j].sprite.transform.localScale = Vector3.one;
                SmallGridBG[i, j].sprite.transform.localPosition = new Vector3(iSmallPosX, iSmallPosY);
                SmallGridBG[i, j].Status = 0;
                iSmallPosX += iSpriteWidth;
            }
            iSmallPosY -= iSpriteWidth;
            iSmallPosX = iSmallStartX;
        }
    }
    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void BeginGame()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                BigGridBG[i, j].Status = 0;
            }
        }
        CreateSquare(Random.Range(0, 7), Random.Range(0, 4));
        CreatSmallSquare();
        iScore = 0;
        isGameStop = false;
    }

   /// <summary>
   /// 创建白色方块
   /// </summary>
   /// <param name="iShape">形状</param>
   /// <param name="iDirection">方向</param>
    private void CreateSquare(int iShape,int iDirection)
    {
        this.iShape = iShape;
        this.iDirection = iDirection;
        listSquare = new List<SquareSprite>();
        iCurrY = 0;
        iCurrX = iCurrXStart;
        //遍历Y轴  横向
        for (int y = 0; y < 4; y++)
        {
            //遍历X轴 纵向
            for (int x = 0; x < 4; x++)
            {
                //表示这个方格即将为白色
                if (Tricks[iShape, iDirection, y, x] == 1)
                {
                    //添加到列表中
                    listSquare.Add(BigGridBG[y + iCurrY, x + iCurrX]);
                    //判断网格的里面是否已经为白色了
                    if (BigGridBG[y + iCurrY, x + iCurrX].Status == 1)
                    {
                        listSquare.Clear();
                        isGameStop = true;
                        Debug.Log("游戏结束");
                        return;
                    }
                    else
                    {
                        //将不是白色的方格变为白色
                        BigGridBG[y + iCurrY, x + iCurrX].Status = 1;
                    }
                }
            }
        }
    }
    /// <summary>
    /// 创建小地图的白色方块
    /// </summary>
    private void CreatSmallSquare()
    {
        this.iNewShape = Random.Range(0, 7);
        this.iNewDirection = Random.Range(0, 4);
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                SmallGridBG[y, x].Status = Tricks[iNewShape, iNewDirection, y, x];
            }
        }
    }
    /// <summary>
    /// 变型
    /// </summary>
    /// <returns></returns>
    private bool NextShape()
    {
        List<SquareSprite> listTemp = new List<SquareSprite>();
        foreach (SquareSprite item in listSquare)
        {
            //如果物体变型的位置在范围之外  则返回false
            if (item.gridY < 0 || item.gridY > 9 || item.gridX > 19)
            {
                return false;
            }
        }
        //旋转方向
        if (iDirection < 3)
        {
            iDirection++;
        }
        else
        {
            iDirection = 0;
        }
        for (int y = 0; y < 4; y++)
        {
            for (int x = 0; x < 4; x++)
            {
                if (Tricks[iShape, iDirection, y, x] == 1)
                {
                   //超出范围则返回false
                    if (x + iCurrX < 0 || x + iCurrX > 9 || y + iCurrY > 19)
                    {
                        return false;
                    }

                    listTemp.Add(BigGridBG[y + iCurrY, x + iCurrX]);
                }
            }
        }
        //将原列表的所有变为黑色
        foreach (SquareSprite item in listSquare)
        {
            item.Status = 0;
        }
        foreach (var item in listTemp)
        {
            //如果原地有白块 
            if (item.Status == 1)
            {
                //将原来的恢复为白块
                foreach (var temp in listSquare)
                {
                    temp.Status = 1;
                }
                return false;
            }
        }
        //临时列表的方格变为白色
        foreach (var item in listTemp)
        {
            item.Status = 1;
        }
        listSquare = listTemp;
        return true;
    }
    /// <summary>
    /// 方块向下移动
    /// </summary>
    private bool MoveDown()
    {
        //临时列表变量
        List<SquareSprite> listTemp = new List<SquareSprite>();
        foreach (SquareSprite temp in listSquare)
        {
            //如果有方块到底部  则返回
            if (temp.gridX == 19)
                return false;
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原list的内容全下一格 赋值给临时列表
            listTemp.Add(BigGridBG[temp.gridX + 1, temp.gridY]);
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原列表的值 全变为黑色
            temp.Status = 0;
        }
        foreach (SquareSprite temp in listTemp)
        {
            //如果下面已经有白色方块了
            if (temp.Status == 1)
            {
                //将原列表的方块变为白色
                foreach (SquareSprite item in listSquare)
                {
                    item.Status = 1;
                }
                return false;
            }
        }
        foreach (SquareSprite temp in listTemp)
        {
            //将临时列表的方块变为白色
            temp.Status = 1;
        }
        iCurrY++;
        //将临时列表的值 重新赋值给原来的列表
        listSquare = listTemp;
        return true;
    }
    /// <summary>
    /// 左移
    /// </summary>
    /// <returns></returns>
    private bool MoveLeft()
    {
        //临时列表变量
        List<SquareSprite> listTemp = new List<SquareSprite>();
        foreach (SquareSprite temp in listSquare)
        {
            //如果有方块到左边  则返回
            if (temp.gridY == 0)
                return false;
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原list的内容全左移一格 赋值给临时列表
            listTemp.Add(BigGridBG[temp.gridX , temp.gridY-1]);
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原列表的值 全变为黑色
            temp.Status = 0;
        }
        foreach (SquareSprite temp in listTemp)
        {
            //如果下面已经有白色方块了
            if (temp.Status == 1)
            {
                //将原列表的方块变为白色
                foreach (SquareSprite item in listSquare)
                {
                    item.Status = 1;
                }
                return false;
            }
        }
        foreach (SquareSprite temp in listTemp)
        {
            //将临时列表的方块变为白色
            temp.Status = 1;
        }
        //将临时列表的值 重新赋值给原来的列表
        listSquare = listTemp;
        iCurrX--;
        return true;
    }
    /// <summary>
    /// 右移
    /// </summary>
    /// <returns></returns>
    private bool MoveRight()
    {
        //临时列表变量
        List<SquareSprite> listTemp = new List<SquareSprite>();
        foreach (SquareSprite temp in listSquare)
        {
            //如果有方块到左边  则返回
            if (temp.gridY == 9)
                return false;
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原list的内容全左移一格 赋值给临时列表
            listTemp.Add(BigGridBG[temp.gridX, temp.gridY + 1]);
        }
        foreach (SquareSprite temp in listSquare)
        {
            //将原列表的值 全变为黑色
            temp.Status = 0;
        }
        foreach (SquareSprite temp in listTemp)
        {
            //如果下面已经有白色方块了
            if (temp.Status == 1)
            {
                //将原列表的方块变为白色
                foreach (SquareSprite item in listSquare)
                {
                    item.Status = 1;
                }
                return false;
            }
        }
        foreach (SquareSprite temp in listTemp)
        {
            //将临时列表的方块变为白色
            temp.Status = 1;
        }
        //将临时列表的值 重新赋值给原来的列表
        listSquare = listTemp;
        iCurrX++;
        return true;
    }

    /// <summary>
    /// 判断一行是否为白色 并清除
    /// </summary>
    private void ClearWhiteLine()
    {
        //一次消多行加分
        int rows = 0;
        //临时列表
        List<SquareSprite> tempList = new List<SquareSprite>();
        //总共20行
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if(BigGridBG[i,j].Status==1)
                    tempList.Add(BigGridBG[i, j]);
            }
            if (tempList.Count == 10)
            {
                //将那一行全变为黑色
                foreach (SquareSprite temp in tempList)
                {
                    temp.Status = 0;
                }
                rows++;
                DealClearMoveDown(i);
            }
            tempList.Clear();
        }
        //计算得分
        iScore += 100 * rows * rows;
        Debug.Log("得分：" + iScore);
    }
    /// <summary>
    /// 处理清楚一行白块的下移
    /// </summary>
    /// <param name="iLine">行数</param>
    private void DealClearMoveDown(int iLine)
    {
        for (int i = iLine - 1; i > 0; i--)
        {
            for (int j = 0; j < 10; j++)
            {
                if (BigGridBG[i, j].Status == 1)
                {
                    BigGridBG[i, j].Status = 0;
                    BigGridBG[i + 1, j].Status = 1;
                }
            }
        }
    }

    /// <summary>
    /// 处理游戏运行
    /// </summary>
    public void DealGameFunction()
    {
        if (isGameStop)
            return;
        if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S))
        {
            if (fDownTime < 0.05f)
            {
                fDownTime += Time.deltaTime;
            }
            else
            {
                if (!MoveDown())
                {
                    ClearWhiteLine();
                    CreateSquare(iNewShape, iNewDirection);
                    CreatSmallSquare();
                }
                fDownTime = 0.0f;
            }
               
        }
        //变型
        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W))
            NextShape();
        //左移动
        if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))
            MoveLeft();
        //右移动
        if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))
            MoveRight();

        if (fUpdateTime < 0.5f)
        {
            fUpdateTime+=Time.deltaTime;
            return;
        }
        fUpdateTime = 0;
        if (!MoveDown())
        {
            ClearWhiteLine();
            CreateSquare(iNewShape, iNewDirection);
            CreatSmallSquare();
        }
    }


    /// <summary>
    /// 四维数组   -- 形状 方向  x，y
    /// </summary>
    private int[, , ,] Tricks = {{  
                                     {  
                                         {0,0,0,0},  
                                         {1,1,1,1},  
                                         {0,0,0,0},  
                                         {0,0,0,0}  
                                     },  
                                     {  
                                         {0,1,0,0},  
                                         {0,1,0,0},  
                                         {0,1,0,0},  
                                         {0,1,0,0}  
                                     },  
                                     {  
                                         {0,0,0,0},  
                                         {1,1,1,1},  
                                         {0,0,0,0},  
                                         {0,0,0,0} 
                                     },  
                                     {  
                                         {0,1,0,0},  
                                         {0,1,0,0},  
                                         {0,1,0,0},  
                                         {0,1,0,0}  
                                     }  
                                },  
                                {  
                                      {  
                                          {0,0,0,0},  
                                          {0,1,1,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,0,0,0},  
                                          {0,1,1,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0} 
                                      },  
                                      {  
                                          
                                          {0,0,0,0},  
                                          {0,1,1,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0}   
                                      },  
                                      {  
                                          
                                          {0,0,0,0},  
                                          {0,1,1,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {  
                                      {  
                                          {0,1,0,0},  
                                          {0,1,1,0},  
                                          {0,0,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,1,1,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}
                                            
                                      },  
                                      {  
                                          {0,1,0,0},  
                                          {0,1,1,0},  
                                          {0,0,1,0},  
                                          {0,0,0,0}   
                                      },  
                                      {  
                                          {0,1,1,0},  
                                          {1,1,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {
                                       {  
                                          {0,0,1,0},  
                                          {0,1,1,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,0,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,0,1,0},  
                                          {0,1,1,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}   
                                      },  
                                      {  
                                          {1,1,0,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}   
                                      }  
                                  },
                                  {  
                                      {  
                                          {0,1,1,0},  
                                          {0,0,1,0},  
                                          {0,0,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,0,1,0},  
                                          {1,1,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,1,0,0},  
                                          {0,1,0,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,1,0},  
                                          {1,0,0,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {  
                                      {  
                                          {0,1,1,0},  
                                          {0,1,0,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,1,1,0},  
                                          {0,0,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,0,1,0},  
                                          {0,0,1,0},  
                                          {0,1,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {1,0,0,0},  
                                          {1,1,1,0},  
                                          {0,0,0,0},  
                                          {0,0,0,0}  
                                      }  
                                  },  
                                  {  
                                      {  
                                          {0,0,0,0},  
                                          {0,1,0,0},  
                                          {1,1,1,0},  
                                          {0,0,0,0}  
                                      },  
                                      {  
                                          {0,1,0,0},  
                                          {0,1,1,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0}

                                            
                                      },  
                                      {  
                                          {0,0,0,0},  
                                          {1,1,1,0},  
                                          {0,1,0,0},  
                                          {0,0,0,0} 
                                      },  
                                      {  
                                          {0,0,1,0},  
                                          {0,1,1,0},  
                                          {0,0,1,0},  
                                          {0,0,0,0}  
                                      }  
                                  }
                                };
}
