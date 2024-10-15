using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapeCreator : MonoBehaviour
{
    public GameObject grapePrefab;
    public float rayDistance = 100.0f; 
    public LayerMask targetLayer; // Layer for targets
    public int FrogId;
    public Material[] GrapeMaterials;
    public bool CanEat;
    LevelManager levelManager;
    void Start()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
    void CreateGrape()
    {
        // Send the ray from the frog
        if (gameObject.transform.parent != null)
        {
            FrogId = gameObject.transform.parent.GetComponent<Cell>().color;
            FrogId = gameObject.transform.parent.GetComponent<Cell>().color;
        }

        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.back));
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, targetLayer);

        if (hits.Length > 0)
        {
            // Sort hits by distance
            System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            for (int i = 0; i < Mathf.Min(Mathf.Max(levelManager.levels[PlayerPrefs.GetInt("Level")].GridSizeX - 1, levelManager.levels[PlayerPrefs.GetInt("Level")].GridSizeY) - 1, hits.Length); i++)
            {
                if (hits[i].transform.childCount == 0 && !(gameObject.GetComponent<Frog>().CanEat))
                {
                    hits[i].transform.gameObject.GetComponent<Cell>().color = FrogId;
                    GameObject grape = Instantiate(grapePrefab, hits[i].transform.position, Quaternion.identity);
                    grape.GetComponent<Renderer>().material = GrapeMaterials[FrogId];
                    grape.transform.parent = hits[i].transform;

                }

            }
        }
    }
    private void Update()
    {
        StartCoroutine(CreatingGrapes());
    }
    IEnumerator CreatingGrapes()
    {
        yield return new WaitForSeconds(.11f);
        CreateGrape();
    }

}