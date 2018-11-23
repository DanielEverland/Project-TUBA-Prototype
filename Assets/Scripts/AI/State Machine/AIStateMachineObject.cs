using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateMachineObject : ScriptableObject
{
    private static readonly Dictionary<AIDataTypes, object> _defaultValues = new Dictionary<AIDataTypes, object>()
    {

    };
    private static T GetDefaultValue<T>(AIDataTypes index)
    {
        if (_defaultValues.ContainsKey(index))
        {
            return (T)_defaultValues[index];
        }
        else
        {
            return default(T);
        }
    }

    private Dictionary<object, DataContainer> _containers = new Dictionary<object, DataContainer>();
    
    public T GetData<T>(object owner, AIDataTypes index)
    {
        EnsureContainerExists(owner);

        return _containers[owner].GetData<T>(index);
    }
    public void SetData<T>(object owner, AIDataTypes index, T value)
    {
        EnsureContainerExists(owner);

        _containers[owner].SetData<T>(index, value);
    }
    private void EnsureContainerExists(object owner)
    {
        if(!_containers.ContainsKey(owner))
        {
            _containers.Add(owner, new DataContainer());
        }
    }

    private class DataContainer
    {
        public T GetData<T>(AIDataTypes index)
        {
            if(!_data.ContainsKey(index))
            {
                _data.Add(index, GetDefaultValue<T>(index));
            }

            return (T)_data[index];
        }
        public void SetData<T>(AIDataTypes index, T value)
        {
            if(!_data.ContainsKey(index))
            {
                _data.Add(index, GetDefaultValue<T>(index));
            }

            _data[index] = value;
        }

        public Dictionary<AIDataTypes, object> _data = new Dictionary<AIDataTypes, object>();
    }
}
