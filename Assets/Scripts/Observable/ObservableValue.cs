using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
[System.Serializable]
public abstract class ObservableValue<T> : ScriptableObject
{
    [SerializeField] private T value;
    public UnityEvent<T> OnValueChanged;

    public T Value
    {
        get => value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(this.value, value))
            {
                this.value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
}

public class ObservableProperty<T>
{
    private T _value;
    public event Action<T> OnValueChanged;
    public T Value
    {
        get => _value;
        set
        {
            if (!EqualityComparer<T>.Default.Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

}
