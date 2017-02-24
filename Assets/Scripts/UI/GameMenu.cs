using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameMenu : MonoBehaviour {

	public void OnQuitClick()
    {
        //find network manager and start as single player game
        GameObject obj = GameObject.FindGameObjectWithTag("NetworkManager");
        if (obj)
        {
            NetworkManager netMan = obj.GetComponent<NetworkManager>();
            if (netMan)
                netMan.StopHost();
        }
    }
}
