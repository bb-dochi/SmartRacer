using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhoisWinner : MonoBehaviour {
    Text winner,cheerup;
    GameObject background;
    public Sprite[] sprites;
	// Use this for initialization
	void Start () {
        winner = GameObject.Find("winner").GetComponent<Text>();
        cheerup = GameObject.Find("cheerup").GetComponent<Text>();
        background = GameObject.Find("background");
	}	
	// Update is called once per frame
	void Update () {
        switch (SM.winner)
        {
            case 0:
                winner.text = "TIme Out";
                cheerup.text = "Please play again, Better luck next time...";
                break;
            case 1:
                winner.text = "Player Win\n";
                cheerup.text = "Congratulations!! One more game?! ";
                break;
            case 2:
                winner.text = "Computer Win\n";
                cheerup.text = "Please play again, Better luck next time...";
                break;
            case 3:
                winner.text = "Goal Success";
                cheerup.text = "Congratulations!! One more game?";
                break;
        }

        switch (SM.prev)
        {
            case 2:
                background.transform.GetComponent<UnityEngine.UI.Image>().sprite = sprites[0];
                break;
            case 3:
                background.transform.GetComponent<UnityEngine.UI.Image>().sprite = sprites[1];
                break;
            case 4:
                background.transform.GetComponent<UnityEngine.UI.Image>().sprite = sprites[2];
                break;
        }
	}
}
