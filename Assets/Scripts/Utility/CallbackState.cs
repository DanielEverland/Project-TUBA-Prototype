using System;

[Serializable, Flags]
public enum CallbackState
{
    Awake       = 0b0001,
    Start       = 0b0010,
    Update      = 0b0100,
}