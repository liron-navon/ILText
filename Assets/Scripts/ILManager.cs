using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ILManager : MonoBehaviour {

    public bool supporRtl = true;
    public static bool autoAlign = true;
    public bool supportMultiline = true;
    public bool manualRtl = true;
    public TextAnchor defaultRtlAlignment = TextAnchor.MiddleRight;
    public SystemLanguage[] languagesToSupportAsRtl;

    private static SystemLanguage language;
    public static bool isRtl;

    public bool useTestingTools = false;
    public SystemLanguage testLanguage;
    private static TextAnchor defaultAnchor;

    private void Awake() {
        language = Application.systemLanguage;
        isRtl = IsThisDeviceLanguageRTL();
        defaultAnchor = defaultRtlAlignment;
    }


    public bool IsThisDeviceLanguageRTL() {

        if (manualRtl) {
            return true;
        }

        if (languagesToSupportAsRtl.Length > 0) {
            for (int i = 0, n = languagesToSupportAsRtl.Length; i < n; i++) {
                if (language == languagesToSupportAsRtl[i]) {
                    return true;
                }
            }
        }

        return false;
    }

    public static void SetText(Text textObject, string newText, bool shouldBeAligned = false, TextAnchor specificAlignment = TextAnchor.LowerLeft) {

        //Debug.Log("numOfCharsPerLine " + numOfCharsPerLine);
        //Debug.Log("sizeOfChar " + sizeOfChar);
        //Debug.Log("rectWidth " + rectWidth);
        //Debug.Log("isWrapping " + isWrapping);

        //null safety
        if (newText == null) {
            Debug.Log("A null text have been passed");
            return;
        }
        else if (textObject == null) {
            Debug.Log("the UI.Text object passed with \"" + newText + "\", is null");
            return;
        }




        bool isWrapping = textObject.horizontalOverflow == HorizontalWrapMode.Wrap;

        if (isWrapping) {
            float rectWidth = Math.Abs(textObject.rectTransform.rect.width);
            float sizeOfChar = textObject.fontSize;
            int numOfCharsPerLine = Convert.ToInt32((rectWidth / sizeOfChar) * 2);
            textObject.text = ReverseString(newText, numOfCharsPerLine);
        }
        else {
            textObject.text = ReverseString(newText);
        }








        if (autoAlign == true) {
            if (specificAlignment != TextAnchor.LowerLeft) {
                textObject.alignment = specificAlignment;
            }
            else {
                textObject.alignment = defaultAnchor;
            }

        }

    }

    public static string ReverseString(string s, int numOfCharsPerLine = 0) {

        String[] lines = s.Split("\n\r".ToCharArray(), StringSplitOptions.None);

        String[] newStringArr = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            newStringArr[i] = ReverseLine(lines[i]);
        }

        return String.Join("\n", newStringArr);
    }

    private static string ReverseLine(string s) {

        char[] charArray = s.ToCharArray();
        char[] reversedArray = new char[charArray.Length];

        //allocate lists to hold digits
        List<int> nArr = new List<int>();
        List<char> cArr = new List<char>();

        for (int i = 0; i < charArray.Length; i++) {

            //select a char in a reversed order
            char c = charArray[charArray.Length - 1 - i];

            // add numbers to seperate lists
            if (Char.IsNumber(c)) {
                cArr.Add(c);
                nArr.Add(i);
            }
            else {

                if (nArr.Count > 0) {
                    //  iterate the numbers in a reversed order
                    for (int j = 0; j < nArr.Count; j++) {
                        int a = nArr[j];
                        reversedArray[a] = cArr[nArr.Count - 1 - j];
                    }
                    //clear the numbers lists
                    nArr.Clear();
                    cArr.Clear();
                }

                // add the next char
                reversedArray[i] = c;
            }
        }

        // Array.Reverse(charArray);
        return new string(reversedArray);
    }
    private static string ReverseLineWithWidth(string s) {

        char[] charArray = s.ToCharArray();
        char[] reversedArray = new char[charArray.Length + charArray.Length];

        //allocate lists to hold digits
        List<int> nArr = new List<int>();
        List<char> cArr = new List<char>();

        for (int i = 0; i < charArray.Length; i++) {




            //select a char in a reversed order
            char c = charArray[charArray.Length - 1 - i];

            // add numbers to seperate lists
            if (Char.IsNumber(c)) {
                cArr.Add(c);
                nArr.Add(i);
            }
            else {

                if (nArr.Count > 0) {
                    //  iterate the numbers in a reversed order
                    for (int j = 0; j < nArr.Count; j++) {
                        int a = nArr[j];
                        reversedArray[a] = cArr[nArr.Count - 1 - j];
                    }
                    //clear the numbers lists
                    nArr.Clear();
                    cArr.Clear();
                }

                // add the next char
                reversedArray[i] = c;
            }
        }

        // Array.Reverse(charArray);
        return new string(reversedArray);
    }


}
