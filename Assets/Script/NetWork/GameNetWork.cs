using UnityEngine;
using System.Collections;

public class GameNetWork : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (!GameSetting.isMultiPlayer)
        {
            this.gameObject.SetActive(false);
        }

    }

    bool prepared = true;
    bool initialized = false;
	// Update is called once per frame
	void Update () {
        if (!initialized)
        {
            if (GameSetting.isHost)
            {
                if (prepared)
                {
                    Transform server = this.transform.FindChild("ServerAgent");
                    server.gameObject.SetActive(true);
                    initialized = true;
                }
            }
            else
            {
                Transform client = this.transform.FindChild("ClientAgent");
                client.gameObject.SetActive(true);
                initialized = true;
            }
        }
    }
}
