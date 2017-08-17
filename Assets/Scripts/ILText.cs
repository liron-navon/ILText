using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ILText : MonoBehaviour {

    private Text uiText;

    void Start() {

        uiText = GetComponent<Text>();

        if (ILManager.IsRtl) {
            ILManager.SetTextUI(uiText, uiText.text);
        }
    }
}