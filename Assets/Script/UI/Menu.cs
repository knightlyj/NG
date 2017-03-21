using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

struct defaultScreen
{
    public static float width = 1280;
    public static float height = 720;

    public static float damageFontSize = 26f;
}

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Button btnNewGame = GameObject.Find("NewGame").GetComponent<Button>();
        btnNewGame.onClick.AddListener(this.OnNewGameClick);

        Button btnJoinGame = GameObject.Find("JoinGame").GetComponent<Button>();
        btnJoinGame.onClick.AddListener(this.OnJoinGameClick);
        
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void OnNewGameClick()
    {
        Debug.Log("new");
        //local host
        GameSetting.isHost = true;
        SceneManager.LoadScene("GamePlay");
    }

    void OnJoinGameClick()
    {
        Debug.Log("join");
        //remote host
        GameSetting.isHost = false;
        //ip
        SceneManager.LoadScene("GamePlay");
    }
    
}
