using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class ILManager : MonoBehaviour {

    [Header("Auto Align")]
    [Tooltip("Will Auto Align text to the right if needed")]
    public bool autoAlignUIText = true;
    [Tooltip("The position to which the text will be anchored")]
    public TextAnchor defaultRtlAlignment = TextAnchor.MiddleRight;

    [Header("Auto changes (By System language)")]
    [Tooltip("Wether or not the language will be detected automatically")]
    public bool automaticallyConvertText = true;
    [Tooltip("The languages which will be automatically detected and get converted, other languages will not be effected")]
    public SystemLanguage[] languagesToSupportAsRtl;

    [Header("Manual Changes")]
    [Tooltip("Override the auto changes, will force text convertion")]
    public bool manualRtl = false;
    
    private static bool IsRtl;

    // since defaultRtlAlignment have to be in the inspector, and we need it to be static
    private static TextAnchor defaultAnchor;
    // we need those to be static, and yet we need them in the inspector
    private static bool staticAutoAlignUIText;
    private static bool staticManualRtl;
    private static SystemLanguage[] staticLanguagesToSupportAsRtl;

    private void Awake() {
        IsRtl = IsThisDeviceLanguageRTL();
        staticManualRtl = manualRtl;
        staticLanguagesToSupportAsRtl = languagesToSupportAsRtl;
        defaultAnchor = defaultRtlAlignment;
        staticAutoAlignUIText = autoAlignUIText;
    }

    public static bool IsThisDeviceLanguageRTL() {

        if (staticManualRtl) {
            return true;
        }

        if (IsRtl) {
            return true;
        }

        if (staticLanguagesToSupportAsRtl.Length > 0) {
            for (int i = 0, n = staticLanguagesToSupportAsRtl.Length; i < n; i++) {
                if (Application.systemLanguage == staticLanguagesToSupportAsRtl[i]) {
                    return true;
                }
            }
        }

        return false;
    }

    /* set a converted text to a UI.Text, return bool if text was affected
     * @textObject the text object to effect
     * @newText this text will be converted and put into the text object
     * Optionals
     * @shouldBeAligned:true wether the text should be aut aligned or not
     **/
    public static bool SetTextUI(Text textObject, string newText, bool shouldBeAligned = true, TextAnchor specificAlignment = TextAnchor.LowerLeft) {

        if (!IsThisDeviceLanguageRTL()) {
            return false;
        }

        //null safety
        if (newText == null) {
            Debug.Log("A null text have been passed");
            return false;
        }
        else if (textObject == null) {
            Debug.Log("the UI.Text object passed with \"" + newText + "\", is null");
            return false;
        }

        textObject.text = ReverseString(newText);

        if(shouldBeAligned == true) {
            if (staticAutoAlignUIText == true) {
                if (specificAlignment != TextAnchor.LowerLeft) {
                    textObject.alignment = specificAlignment;
                }
                else {
                    textObject.alignment = defaultAnchor;
                }
            }
        }
        return true;
    }

    /* set a converted text to a TextMesh, return bool if text was affected
    * @textObject an object of type TextMesh (3d text)
    * @newText this text will be converted and put into the text object
    **/
    public static bool SetText3D(TextMesh textObject, string newText) {

        if(!IsThisDeviceLanguageRTL()) {
            return false;
        }

        //null safety
        if (newText == null) {
            Debug.Log("A null text have been passed");
            return false;
        }
        else if (textObject == null) {
            Debug.Log("the TextMesh.Text object passed with \"" + newText + "\", is null");
            return false;
        }

        textObject.text = ReverseString(newText);
        return true;
    }

    /* Set a text in the input text
    * @inputField an object of type InputField
    * @oldText the old text of the input field
    * Optionals
    * @newText wether this should be a completely new text, if entered, the oldText will be ignored and this will be used instead
    **/
    public static bool SetInputText(InputField inputField, string oldText, string newText = null) {

        if (!IsThisDeviceLanguageRTL()) {
            return false;
        }


        //null safety
        if (inputField == null) {
            Debug.Log("the UI.InputField object passed is null");
            return false;
        }

        if (newText != null || oldText == null || oldText == "") {
            inputField.text = ReverseString(inputField.text);
            return true;
        }


        newText = inputField.text;

        char[] oldSet = oldText.ToCharArray();
        char[] newSet = newText.ToCharArray();

        string notDiff = "";
        string unReversedString;

        //to detect if a string was deleted in either side or in the middle, but not edited
        if (newSet.Count() < oldSet.Count()) {

            foreach(char c in newSet) {

                foreach (char co in oldSet) {

                    if(c == co) {
                        notDiff += co;
                    }
                }
            }

            return true;
        }

        //this is when the user is writing normal
        //1.reverse the old text (make it the default unity scrumbled text)
        //2.append the last char of the new text (hopefully the one that the user entered)
        //3.reverse the text again
        if (oldText.Length > 0 && oldText[0] == newText[0]) {
            unReversedString = ReverseString(oldText);
            unReversedString +=  newText[newText.Length - 1];
            inputField.text = ReverseString(unReversedString);
            return true;
        }
        else {
            inputField.text = newText;
            return true;
        }

    }

    /* will Reverse a multiline or normal string 
     * @s the string to reverse
    **/
    public static string ReverseString(string s) {

        String[] lines = s.Split("\n\r".ToCharArray(), StringSplitOptions.None);

        String[] newStringArr = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            newStringArr[i] = ReverseLine(lines[i]);
        }

        return String.Join("\n", newStringArr);
    }

    /* will Reverse a single line string
     * @s the string to reverse
    **/
    public static string ReverseLine(string s) {

        char[] charArray = s.ToCharArray();
        char[] reversedArray = new char[charArray.Length];

        //allocate lists to hold latin chars and indexes
        List<int> indexes = new List<int>();
        List<char> latinChars = new List<char>();

        bool wasLastCharLatin = false;

        for (int i = 0; i < charArray.Length; i++) {

            //select a char in a reversed order
            char c = charArray[charArray.Length - 1 - i];

            // add latin chars to seperate lists
            if (Regex.IsMatch(c.ToString(), "^[a-zA-Z0-9]*$")) {
                wasLastCharLatin = true;
                latinChars.Add(c);
                indexes.Add(i);
            } 
            else {
                wasLastCharLatin = false;
                if (indexes.Count > 0) {
                    //  iterate the numbers in a reversed order
                    for (int j = 0; j < indexes.Count; j++) {
                        int a = indexes[j];
                        reversedArray[a] = latinChars[indexes.Count - 1 - j];
                    }
                    //clear the numbers lists
                    indexes.Clear();
                    latinChars.Clear();
                }

                // add the next char
                reversedArray[i] = c;
            }
        }

        if (indexes.Count > 0) {
            for (int j = 0; j < indexes.Count; j++) {
                int a = indexes[j];
                reversedArray[a] = latinChars[indexes.Count - 1 - j];
            }
            //clear the numbers lists
            indexes.Clear();
            latinChars.Clear();
        }

        // Array.Reverse(charArray);
        return new string(reversedArray);
    }

}