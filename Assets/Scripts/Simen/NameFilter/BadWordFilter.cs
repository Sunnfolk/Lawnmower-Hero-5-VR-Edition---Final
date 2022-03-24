using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class BadWordFilter : MonoBehaviour
{
    public TMP_InputField inFieldText;
    private string _myString;
    public badWordVariable badWordVariable;

    private void Start()
    {
        inFieldText = GetComponent<TMP_InputField>();
    }

    public void ChangeName(TMP_InputField field)
    {
        _myString = field.text;
        BadWordParser();
    }

    private void BadWordParser()
    {
        for (int i = 0; i < badWordVariable.badWords.Length; i++)
        {
            if (_myString.ToLower().Contains(badWordVariable.badWords[i]))
            {
                for (int j = 0; j < _myString.Length; j++)
                {
                    if (_myString.ToLower()[j] == badWordVariable.badWords[i][0])
                    {
                        string temp = _myString.Substring(j, badWordVariable.badWords[i].Length);
                        if (temp.ToLower() == badWordVariable.badWords[i])
                        {
                            _myString = _myString.Remove(j, badWordVariable.badWords[i].Length);
                            if (_myString != null)
                            {
                                inFieldText.text = _myString.ToString();
                            }
                            else
                            {
                                inFieldText.text = "";
                            }
                            return;
                        }
                        
                    }
                }
            }
        }
    }
}
