using UnityEngine;
using UnityEngine.UI;

public class ILText : MonoBehaviour {

    // Use this for initialization
    private Text t;

    public bool alignTextOnStart = true;

    void Start() {

        if (alignTextOnStart) {

            t = GetComponent<Text>();

            if (!t) {
                Debug.Log("Please put me on a UI.Text object, i'm currently on: " + this.gameObject.name);
            }

            if (ILManager.isRtl) {
                ILManager.SetText(t, t.text);
            }

        }
    }
}
