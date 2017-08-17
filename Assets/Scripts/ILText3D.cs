using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class ILText3D : MonoBehaviour {

    // Use this for initialization
    private TextMesh textMesh;

    void Start() {
        textMesh = GetComponent<TextMesh>();

        if (ILManager.IsRtl) {
            ILManager.SetText3D(textMesh, textMesh.text);
        }
    }
}