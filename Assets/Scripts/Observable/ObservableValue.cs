using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
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
