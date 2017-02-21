using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        GameSetting.isLocalServer = true;
        SceneManager.LoadScene("GamePlay");
    }

    void OnJoinGameClick()
    {
        Debug.Log("join");
        //remote host
        GameSetting.isLocalServer = false;
        //ip
        SceneManager.LoadScene("GamePlay");
    }
    
}
