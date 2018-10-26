using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextIndexSetter : MonoBehaviour
{
    [SerializeField]
    private Text _targetText = null;
    [SerializeField]
    private BaseVariable _variable = null;
    [SerializeField]
    private BaseRuntimeSet _collection = null;
    [SerializeField]
    private bool _updateEveryFrame = false;

    private void Awake()
    {
        if (!_variable.Type.IsAssignableFrom(_collection.Type))
            throw new System.ArgumentException("Type mismatch");
    }
    private void OnValidate()
    {
        if (_targetText == null)
            _targetText = GetComponent<Text>();
    }
    private void Update()
    {
        if (_updateEveryFrame)
            UpdateText();
    }
    public void UpdateText()
    {
        _targetText.text = _collection.Items.IndexOf(_variable.BaseValue).ToString();
    }
}