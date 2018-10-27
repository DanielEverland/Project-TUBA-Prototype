using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextIndexSetter : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _targetText = null;
    [SerializeField]
    private BaseVariable _variable = null;
    [SerializeField]
    private BaseRuntimeSet _collection = null;
    [SerializeField, EnumFlags]
    private UpdateState _updateState = UpdateState.Start;

    private void Awake()
    {
        if (!_variable.Type.IsAssignableFrom(_collection.Type))
            throw new System.ArgumentException("Type mismatch");

        if (_updateState.HasFlag(UpdateState.Awake))
            UpdateText();
    }
    private void Start()
    {
        if (_updateState.HasFlag(UpdateState.Start))
            UpdateText();
    }
    private void OnValidate()
    {
        if (_targetText == null)
            _targetText = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        if (_updateState.HasFlag(UpdateState.Update))
            UpdateText();
    }
    public void UpdateText()
    {
        _targetText.text = _collection.Items.IndexOf(_variable.BaseValue).ToString();
    }

    [System.Flags, System.Serializable]
    private enum UpdateState
    {
        Awake = 0,
        Start = 2,
        Update = 4,
    }
}