using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ControlsHandler : MonoBehaviour
{

	Event keyEvent;
	Text buttonText;
	KeyCode newKey;

	bool waitingForKey;


	void OnEnable()
	{
		//Assign menuPanel to the Panel object in our Canvas
		//Make sure it's not active when the game starts
		waitingForKey = false;

		/*iterate through each child of the panel and check
		 * the names of each one. Each if statement will
		 * set each button's text component to display
		 * the name of the key that is associated
		 * with each command. Example: the ForwardKey
		 * button will display "W" in the middle of it
		 */

		Debug.Log(CustomInputManager.cim.down.ToString());

		Text[] button_texts = GetComponentsInChildren<Text>();

		for (int i = 0; i < button_texts.Length; i++)
		{

            switch (button_texts[i].gameObject.name)
            {
				case "upkey":
					button_texts[i].text = CustomInputManager.cim.up.ToString().ToUpper();
					break;
				case "downkey":
					button_texts[i].text = CustomInputManager.cim.down.ToString().ToUpper();
					break;
				case "leftkey":
					button_texts[i].text = CustomInputManager.cim.left.ToString().ToUpper();
					break;
				case "rightkey":
					button_texts[i].text = CustomInputManager.cim.right.ToString().ToUpper();
					break;
				case "backkey":
					button_texts[i].text = CustomInputManager.cim.back.ToString().ToUpper();
					break;
				case "selectkey":
					button_texts[i].text = CustomInputManager.cim.select.ToString().ToUpper();
					break;
			}
		}
	}


	void Update()
	{

	}

	void OnGUI()
	{
		/*keyEvent dictates what key our user presses
		 * bt using Event.current to detect the current
		 * event
		 */
		keyEvent = Event.current;

		//Executes if a button gets pressed and
		//the user presses a key
		if (keyEvent.isKey && waitingForKey)
		{
			newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
			waitingForKey = false;
		}
	}

	/*Buttons cannot call on Coroutines via OnClick().
	 * Instead, we have it call StartAssignment, which will
	 * call a coroutine in this script instead, only if we
	 * are not already waiting for a key to be pressed.
	 */
	public void StartAssignment(string keyName)
	{
		if (!waitingForKey)
			StartCoroutine(AssignKey(keyName));
	}

	//Assigns buttonText to the text component of
	//the button that was pressed
	public void SendText(Text text)
	{
		buttonText = text;
	}

	//Used for controlling the flow of our below Coroutine
	IEnumerator WaitForKey()
	{
		while (!keyEvent.isKey)
			yield return null;
	}

	/*AssignKey takes a keyName as a parameter. The
	 * keyName is checked in a switch statement. Each
	 * case assigns the command that keyName represents
	 * to the new key that the user presses, which is grabbed
	 * in the OnGUI() function, above.
	 */
	public IEnumerator AssignKey(string keyName)
	{
		waitingForKey = true;

		yield return WaitForKey(); //Executes endlessly until user presses a key

		switch (keyName)
		{
			case "up":
				CustomInputManager.cim.up = newKey; //Set forward to new keycode
				buttonText.text = CustomInputManager.cim.up.ToString().ToUpper(); //Set button text to new key
				PlayerPrefs.SetString("upkey", CustomInputManager.cim.up.ToString()); //save new key to PlayerPrefs
				break;
			case "down":
				CustomInputManager.cim.down = newKey; //set backward to new keycode
				buttonText.text = CustomInputManager.cim.down.ToString().ToUpper(); //set button text to new key
				PlayerPrefs.SetString("downkey", CustomInputManager.cim.down.ToString()); //save new key to PlayerPrefs
				break;
			case "left":
				CustomInputManager.cim.left = newKey; //set left to new keycode
				buttonText.text = CustomInputManager.cim.left.ToString().ToUpper(); //set button text to new key
				PlayerPrefs.SetString("leftkey", CustomInputManager.cim.left.ToString()); //save new key to playerprefs
				break;
			case "right":
				CustomInputManager.cim.right = newKey; //set right to new keycode
				buttonText.text = CustomInputManager.cim.right.ToString().ToUpper(); //set button text to new key
				PlayerPrefs.SetString("rightkey", CustomInputManager.cim.right.ToString()); //save new key to playerprefs
				break;
			case "back":
				CustomInputManager.cim.back = newKey; //set jump to new keycode
				buttonText.text = CustomInputManager.cim.back.ToString().ToUpper(); //set button text to new key
				PlayerPrefs.SetString("backkey", CustomInputManager.cim.back.ToString()); //save new key to playerprefs
				break;
			case "select":
				CustomInputManager.cim.select = newKey; //set jump to new keycode
				buttonText.text = CustomInputManager.cim.select.ToString().ToUpper(); //set button text to new key
				PlayerPrefs.SetString("selectkey", CustomInputManager.cim.select.ToString()); //save new key to playerprefs
				break;
		}

		yield return null;
	}
}
