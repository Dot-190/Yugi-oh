using System;
using TMPro;
using UnityEngine;

public class UIComponent
{
    public string Key;
}

[Serializable]
public class ButtonComponent : UIComponent
{
    public ButtonComponent Button;
}

[Serializable]
public class InputComponent : UIComponent
{
    public TMP_InputField Input;
}
