using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ValueSetter : MonoBehaviour
{
    [SerializeField]
    private BaseVariable _variable;
    [SerializeField]
    private TMP_Text _textTarget;

    private void Update()
    {
        _textTarget.text = _variable.ToString();
    }
    private void OnValidate()
    {
        if(_textTarget == null)
            _textTarget = GetComponent<TMP_Text>();
    }
}