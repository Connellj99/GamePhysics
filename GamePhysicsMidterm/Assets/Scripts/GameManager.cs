using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject colManager;
    public GameObject lazerPrefab;
    public Transform lazerSpawn;
    public Particle2D particle2d;
    private float pause = -1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        if (colManager.GetComponent<UIManager>().lives == 0)
        {
            //endgame
            Loss();
        }
        
    }

    
    void Loss()
    {
        SceneManager.LoadScene("Loss");

    }

    void CheckInput()
    {
        //  while (Time.deltaTime <= 60)          

        if (Input.GetKeyDown(KeyCode.Space))
        {

            Fire();

        }

    }

    void Fire()
    {
        if (Time.time >= pause)
        {
            pause += .3f;
            lazerPrefab.GetComponent<Particle2D>().position = new Vector2(lazerSpawn.position.x,lazerSpawn.position.y);
            GameObject lazer = Instantiate(lazerPrefab, lazerSpawn.position, lazerSpawn.rotation);
        }


    }
 
}
