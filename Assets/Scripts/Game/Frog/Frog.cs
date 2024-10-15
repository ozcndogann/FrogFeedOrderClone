using System.Collections;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public float rayDistance = 100.0f; 
    public LayerMask[] targetLayers;
    public float shrinkSpeed = 5f; 
    public float moveSpeed = 10.0f;
    public int FrogId;
    public bool CanEat;
    public bool Eated;
    private GameObject tongue;
    private LevelManager levelManager;

    private LevelUI levelUI;
    private GameObject movesTextObject;

    public Vector3 stretchDirection = Vector3.forward; 
    public float stretchSpeed = 0.5f;

    private Vector3 originalScale;
    private Coroutine stretchCoroutine;
    private Coroutine eatCoroutine;
    private int tongueHitCount;
    private bool tongueStop;
    private int hitsCount;
    private bool hitWrong;
    Collider frogCollider;
    private void Start()
    {
        if (gameObject.tag == "Frog")
        {
            tongue = gameObject.transform.GetChild(2).gameObject;
        }
        else
        {
            tongue = gameObject.transform.GetChild(0).gameObject;
        }
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
        levelUI = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<LevelUI>();
        movesTextObject = GameObject.FindGameObjectWithTag("Moves");
        if (tongue != null)
        {
            originalScale = tongue.transform.localScale;
        }
        frogCollider = gameObject.GetComponent<Collider>();
    }

    void OnMouseDown()
    {
        // Send Ray from the frog
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.back));
        RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, targetLayers[0]);
        RaycastHit[] hitsFrog = Physics.RaycastAll(ray, rayDistance, targetLayers[1]);
        RaycastHit[] hitsEmptyCell = Physics.RaycastAll(ray, rayDistance, targetLayers[2]);

        levelManager.levels[PlayerPrefs.GetInt("Level")].MoveAmount -= 1;
        
        movesTextObject.GetComponent<Animator>().SetBool("MadeMove", true);
        movesTextObject.GetComponent<Animator>().SetBool("Normal", true);

        if (hitsFrog.Length > 0)
        {
            CanEat = false;
            stretchCoroutine = StartCoroutine(StretchRoutine());
        }
        else
        {
            CanEat = true;
        }
        hitsCount = hits.Length;
        

        Debug.Log("Raycast hit count: " + hits.Length);
        if (hits.Length > 0)
        {
            Debug.Log("Raycast hit count: " + hits.Length);
            // Sort hits array by distance
            System.Array.Sort(hits, (hit1, hit2) => hit1.distance.CompareTo(hit2.distance));

            // First, we check whether the condition is generally satisfied.
            bool allMatch = true;
            Cell parentCell = gameObject.transform.parent.GetComponent<Cell>();

            if (parentCell == null)
            {
                Debug.LogError("Frog is not a direct child of a Cell object.");
                return;
            }

            int frogColor = parentCell.color;

            for (int j = 0; j < hits.Length; j++)
            {
                
                Cell hitCell = hits[j].transform.parent.GetComponent<Cell>();

                if (hits[j].transform.parent == null || hitCell == null || hitCell.color != frogColor)
                {
                    allMatch = false;
                    CanEat = false;
                    break;
                }
            }

            // If the condition is met, perform the operation for all elements.
            if (allMatch && CanEat)
            {
                for (int j = 0; j < hits.Length; j++)
                {
                    eatCoroutine = StartCoroutine(MoveAndShrink(hits[j].collider.gameObject));
                }
                
            }
            else
            {
                stretchCoroutine = StartCoroutine(StretchRoutine());
                Debug.Log("Color mismatch or another condition failed.");
            }
        }
    }
    private IEnumerator StretchRoutine()
    {
        float elapsed = 0.0f;
        frogCollider.enabled = false;
        while (!tongueStop)
        {
            Vector3 newScale = tongue.transform.localScale;
            newScale += stretchDirection * stretchSpeed * Time.deltaTime * 2;
            tongue.transform.localScale = newScale;

            elapsed += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.PlaySFX("Wrong");
        Vector3 currentScale = tongue.transform.localScale;

        while (tongueStop && tongue.transform.localScale.y > 0.0035f)
        {
            Vector3 newScale = tongue.transform.localScale;
            newScale -= stretchDirection * stretchSpeed * Time.deltaTime * 2;
            tongue.transform.localScale = newScale;
        }
        hitWrong = false;
        tongueHitCount = 0;
        
        tongue.transform.localScale = originalScale;
        frogCollider.enabled = true;
        // End coroutine (so that the tongue cannot come back out from behind the frog)

        stretchCoroutine = null;
    }
    public void OnTongueTriggerEnter(Collider other)
    {
        if (gameObject.transform.parent != null && other.transform.parent != null)
        {
            if (tongue != null && other.CompareTag("Grape") && !Eated)//After eating, the following if statement becomes null
            {
                if (other.transform.parent.GetComponent<Cell>().color != gameObject.transform.parent.GetComponent<Cell>().color)
                {
                    hitWrong = true;
                }
                else
                {
                    tongueHitCount += 1;
                    AudioManager.Instance.PlaySFX("Grape");
                    Debug.Log("Tongue object collided with a Grape object.");
                }

            }
            if (tongue != null && other.CompareTag("Frog"))
            {
                if (other.transform.parent.name != gameObject.transform.parent.name)
                {
                    hitWrong = true;
                    Debug.Log("Tongue object collided with a wrong object.");
                }

            }
        }
        

    }
    IEnumerator MoveAndShrink(GameObject target)
    {
        frogCollider.enabled = false;
        // Perform stretching
        while (!tongueStop)
        {
            Vector3 newScale = tongue.transform.localScale;
            newScale += stretchDirection * stretchSpeed * Time.deltaTime;
            tongue.transform.localScale = newScale;
            yield return null; // Bir sonraki frame'e kadar bekle
        }

        // Returning to original size when stretching process is completed
        Vector3 currentScale = tongue.transform.localScale;
        while (tongueStop && CanEat && tongue.transform.localScale.y > 0.0035f)
        {
            target.transform.position = Vector3.MoveTowards(target.transform.position, tongue.transform.position, Time.deltaTime * moveSpeed);
            target.transform.parent = gameObject.transform;
            gameObject.transform.parent = null;
            Vector3 newScale = tongue.transform.localScale;
            newScale -= stretchDirection * stretchSpeed * Time.deltaTime * 4/tongueHitCount;
            tongue.transform.localScale = newScale;
            //Eated = true;
            yield return null;
        }
        
        tongue.transform.localScale = originalScale;
        
        
        eatCoroutine = null;
        Destroy(target);
        Eated = true;
        yield return new WaitForSeconds(.1f);
        levelUI.EndLevel();
        Destroy(gameObject);
    }
    void Update()
    {
        if (hitWrong || tongue.transform.localScale.y > 3)
        {
            tongueStop = true;
        }
        else
        {
            if (tongueHitCount == hitsCount && tongueHitCount > 0)
            {
                tongueStop = true;
            }
            else if (!CanEat)
            {
                tongueStop = false;
            }
        }
        
    }
}