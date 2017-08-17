# ILText

**Please notice that currently the text can only be converted at runtime, hopefully this will be improved in the newr future**

**Important:**

>**You must add the IL Manager, this script is holding the main functions to change the unity gibberish to normal hebrew, and it holds verious settings:**

<br />

>**You can use the dedicated prefabs to make it easier for you:**

**IL_Text**: will convert text automatically if you enable the automaticallyConvertText feature in the IL manager, and add hebrew as one of the RTL languages you wish to support.

**IL_InputField**: will convert input field text automatically to readable hebrew, use the ILManager.ReverseString(String text) to revert the text, since unity only render it in a wierd way, but the text is legit.


**ILText3D**: will convert 3D text automatically if you enable the automaticallyConvertText feature in the IL manager, and add hebrew as one of the RTL languages you wish to support.

<br />

**Inspector public variables**

>**Auto Align** 

**bool _autoAlignUIText_**: wether the manager will align the text for you, it is usually nice to align the text to the right in hebrew and to the left in english.

**TextAnchor _defaultRtlAlignment_**: the anchor of the default alignment, default middle tight alignment. this will do nothing if the autoAlignUIText is false.

>**Auto changes (By System language)**

**bool _automaticallyConvertText_**: the script can detect the system language for the device running your game, and according to that change the language automatically, if true and manualRtl is false, you can safely use the manager for all text changes and it will convert the text by the system language: if the system is in english for instance, it wont do anything special.

**SystemLanguage[] _languagesToSupportAsRtl_**: here you can select supported languages, ie the system languages to refer to as RTL, for instance if you select only Hebrew, it will do nothing for any language other than hebrew.

>**Manual Changes**

This is mostly for testing, but it can give you better controll if you want the user to select his language manually for instance.

**bool _manualRtl_**: if true, the auto changes will be ignored and all languages will be refered to as RTL.

>**public variables Hidden from the inspector**

 bool IsRtl: will tell you if the script consider the device as RTL, although please use the static method IsThisDeviceLanguageRTL(), it will be safer and is used internally.
 
 
>**Public static Methods**


**public static bool SetTextUI** (Text textObject, string newText, bool shouldBeAligned -optional, TextAnchor specificAlignment -optional)
use this static method to assign RTL text to a Text object, you can use specific alignment and even make specific texts not aligned if you wish.


**public static bool SetText3D** (TextMesh textObject, string newText)
use this static method to assign RTL text to a 3D text mesh, there is no alignemt in 3D meshes.

**public static bool SetInputText** (InputField inputField, string oldText, string newText = null)
Mostly ment to be used internally by IL_Input script.
