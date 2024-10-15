using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUI : MonoBehaviour
{
    public LevelManager LevelManager;
    public GameObject NextPanel, RestartPanel, FinalPanel;
    public TMP_Text MoveCount;
    bool allCellsEmpty;
    void Start()
    {
    }


    void Update()
    {
        MoveCount.text = LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount.ToString() + " MOVES";
    }

    public void NextLevelButton()
    {
        AudioManager.Instance.PlaySFX("Button");
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
        NextPanel.SetActive(false);
        allCellsEmpty = true;
        LevelManager.SetLevel(PlayerPrefs.GetInt("Level"));
    }
    public void RestartLevelButton()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (!allCellsEmpty)
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                if (cell.gameObject.transform.childCount > 0)
                {
                    //allCellsEmpty = false;
                    Destroy(cell.gameObject.transform.GetChild(0).gameObject);
                }
            }
            RestartPanel.SetActive(false);
            LevelManager.SetLevel(PlayerPrefs.GetInt("Level"));
            LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount = LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmountInitial;
        }
    }
    public void RestartAllLevelSButton()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (!allCellsEmpty)
        {
            Cell[] cells = FindObjectsOfType<Cell>();
            foreach (Cell cell in cells)
            {
                if (cell.gameObject.transform.childCount > 0)
                {
                    Destroy(cell.gameObject.transform.GetChild(0).gameObject);
                }
            }
            
        }
        FinalPanel.SetActive(false);
        PlayerPrefs.SetInt("Level", 0);
        LevelManager.SetLevel(PlayerPrefs.GetInt("Level"));
        LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount = LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmountInitial;
    }
    IEnumerator EndPanelOpen()
    {
        yield return new WaitForSeconds(1f);
        Cell[] cells = FindObjectsOfType<Cell>();

        allCellsEmpty = true;
        foreach (Cell cell in cells)
        {
            if (cell.transform.childCount != 0)
            {
                allCellsEmpty = false;
                break;
            }
        }
        
        if (allCellsEmpty)
        {
            if (LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount >= 0)
            {
                
                if (PlayerPrefs.GetInt("Level") == LevelManager.levels.Count - 1)
                {
                    FinalPanel.SetActive(true);
                }
                else
                {
                    NextPanel.SetActive(true);
                    RestartPanel.SetActive(false);
                }
                Debug.Log("end1");
            }

        }
        else
        {
            if (LevelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount <= 0)
            {
                for (int i = 0; i < LevelManager.frogs.Count; i++)
                {
                    LevelManager.frogs[i].GetComponent<Collider>().enabled = false;
                }
                if (PlayerPrefs.GetInt("Level") == LevelManager.levels.Count - 1)
                {
                    FinalPanel.SetActive(true);
                }
                else
                {
                    RestartPanel.SetActive(true);
                    NextPanel.SetActive(false);
                }
            }
            Debug.Log("end2");
        }

    }
    public void EndLevel()
    {
        StartCoroutine(EndPanelOpen());
    }
}
