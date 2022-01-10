using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCard : MonoBehaviour
{
    
    public Button yourButton;
    public Game game; 
    public int steps;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
  	void TaskOnClick(){
        game.steps = this.steps;
        yourButton.gameObject.SetActive(false);
	}
}
