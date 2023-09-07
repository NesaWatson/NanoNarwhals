using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;


    public GameObject player;
    public playerController playerScript;

    [SerializeField] GameObject activeMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    public GameObject playerSpawnPos;

    [SerializeField] int enemiesRemaining;

    bool isPaused;

    void Awake()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        playerSpawnPos = GameObject.FindGameObjectWithTag("Player Spawn Pos");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && activeMenu == null)
        {
            statePause();
            activeMenu = pauseMenu;
            activeMenu.SetActive(isPaused);
        }
    }
    public void statePause()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = !isPaused;
    }
    public void stateUnpause()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = !isPaused;
        activeMenu.SetActive(isPaused);
        activeMenu = null;
    }
    IEnumerator youWin()
    {
        yield return new WaitForSeconds(3);
        statePause();
        activeMenu = winMenu;
        activeMenu.SetActive(isPaused);
    }
    public void updateGameGoal(int amount)
    {
        enemiesRemaining += amount;

        if (enemiesRemaining <= 0)
        {
            StartCoroutine(youWin());
        }
    }
    public void youLose()
    {
        statePause();
        activeMenu = loseMenu;
        activeMenu.SetActive(isPaused);
    }
}
