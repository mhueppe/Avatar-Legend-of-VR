using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetPlayer : MonoBehaviour {
	public Button yourButton;
    public Game game; 
    public int playerNumber;         

	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
        game.chosenPlayer = playerNumber;
	}
}