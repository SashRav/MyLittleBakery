using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class SC_LevelManager : MonoBehaviour
{

    [SerializeField] private GameObject[] BackgroundPosistions;

    [SerializeField] private GameObject BackgroundPrefab;
    [SerializeField] private GameObject PiePrefab;
    [SerializeField] private GameObject CurrentGoldText;
    [SerializeField] private GameObject IncomeGoldText;

    [SerializeField] private int BackgroundsCount;
    [SerializeField] private int PlayablePieCount;

    private static List<GameObject> BackgroundObjects = new List<GameObject>();
    private static List<GameObject> PieObjects = new List<GameObject>();

    private static GameObject GoldCurrentTextObject;
    private static GameObject GoldIncomeTextObject;
    private float currentGold;
    private float goldIncome;


    // Start is called before the first frame update
    void Start()
    {
        GoldCurrentTextObject = CurrentGoldText;
        GoldIncomeTextObject = IncomeGoldText;

        for (int currentBackground = 0; currentBackground < BackgroundsCount; currentBackground++)
        {
            GameObject backgroundObj = Instantiate(BackgroundPrefab);
            BackgroundObjects.Add(backgroundObj);
            backgroundObj.transform.position = new Vector3(BackgroundPosistions[currentBackground].transform.position.x, BackgroundPosistions[currentBackground].transform.position.y, 10);

            GameObject pieObj = Instantiate(PiePrefab);
            PieObjects.Add(pieObj);
            pieObj.transform.position = new Vector3(BackgroundPosistions[currentBackground].transform.position.x, BackgroundPosistions[currentBackground].transform.position.y, 5);
            pieObj.GetComponent<SC_PieItem>().ClearPie();
        }

        for (int currentPie = 0; currentPie < PlayablePieCount; currentPie++)
        {
            PieObjects[currentPie].GetComponent<SC_PieItem>().UpdatePieLevel();
        }
        InvokeRepeating("UpdateCurrentGold", 1.0f, 1.0f);
    }

    public void SpanwNewPie()
    {
        foreach (GameObject SinglePie in PieObjects)
        {
            print(SinglePie.GetComponent<SC_PieItem>().GetPieLevel());
            if (SinglePie.GetComponent<SC_PieItem>().GetPieLevel() == -1)
            {
                SinglePie.GetComponent<SC_PieItem>().UpdatePieLevel();
                break;
            }
        }
    }
    private void UpdateGoldIncome()
    {
        goldIncome = 0;
        foreach (GameObject SinglePie in PieObjects)
        {
            if (SinglePie.GetComponent<SC_PieItem>().GetPieLevel() >= 0)
                goldIncome += Mathf.Pow(2, SinglePie.GetComponent<SC_PieItem>().GetPieLevel());// 2 * (SinglePie.GetComponent<SC_PieItem>().GetPieLevel() + 1);
        }
        GoldIncomeTextObject.GetComponent<Text>().text = goldIncome.ToString();
    }

    private void UpdateCurrentGold()
    {
        UpdateGoldIncome();
        currentGold += goldIncome;
        GoldCurrentTextObject.GetComponent<Text>().text = currentGold.ToString();
    }
}

/*
 # #
 # #

0 = 1 
1 = 2
2 = 4
3 = 8
4 = 16
 # # #
 # # 

 # # #
 # # #

   # 
 # # #
 # # #

 # # 
 # # #
 # # #

 # # #
 # # # 
 # # #
 */