using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class ILInput : MonoBehaviour {

    private InputField inputField;
    private string lastInput = "";

    // Use this for initialization
    void Start() {
        inputField = GetComponent<InputField>();
        Listen(inputField);
    }

    private void ChangeListener() {

        // remove old listener and create a new one when were done manipulating the text
        // otherwise we will get stackOverflow (recursive change)
        inputField.onValueChanged.RemoveAllListeners();

        bool didSet = ILManager.SetInputText(inputField, lastInput);

        Listen(inputField);

        //if the manager didnt manipulate the text, we dont want to save the last text
        if (didSet) {
            lastInput = inputField.text;
        }
    }

    private void Listen(InputField i) {
        i.onValueChanged.AddListener(delegate { ChangeListener(); });
    }

}