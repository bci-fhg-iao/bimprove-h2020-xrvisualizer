using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class DiscrepancySyncModel
{
    [RealtimeProperty(3, true, true)]
    private string _discrepancyString;

    [RealtimeProperty(4, true, true)]
    private int _discrepancyInt;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class DiscrepancySyncModel : RealtimeModel {
    public string discrepancyString {
        get {
            return _discrepancyStringProperty.value;
        }
        set {
            if (_discrepancyStringProperty.value == value) return;
            _discrepancyStringProperty.value = value;
            InvalidateReliableLength();
            FireDiscrepancyStringDidChange(value);
        }
    }
    
    public int discrepancyInt {
        get {
            return _discrepancyIntProperty.value;
        }
        set {
            if (_discrepancyIntProperty.value == value) return;
            _discrepancyIntProperty.value = value;
            InvalidateReliableLength();
            FireDiscrepancyIntDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(DiscrepancySyncModel model, T value);
    public event PropertyChangedHandler<string> discrepancyStringDidChange;
    public event PropertyChangedHandler<int> discrepancyIntDidChange;
    
    public enum PropertyID : uint {
        DiscrepancyString = 3,
        DiscrepancyInt = 4,
    }
    
    #region Properties
    
    private ReliableProperty<string> _discrepancyStringProperty;
    
    private ReliableProperty<int> _discrepancyIntProperty;
    
    #endregion
    
    public DiscrepancySyncModel() : base(null) {
        _discrepancyStringProperty = new ReliableProperty<string>(3, _discrepancyString);
        _discrepancyIntProperty = new ReliableProperty<int>(4, _discrepancyInt);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _discrepancyStringProperty.UnsubscribeCallback();
        _discrepancyIntProperty.UnsubscribeCallback();
    }
    
    private void FireDiscrepancyStringDidChange(string value) {
        try {
            discrepancyStringDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireDiscrepancyIntDidChange(int value) {
        try {
            discrepancyIntDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _discrepancyStringProperty.WriteLength(context);
        length += _discrepancyIntProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _discrepancyStringProperty.Write(stream, context);
        writes |= _discrepancyIntProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.DiscrepancyString: {
                    changed = _discrepancyStringProperty.Read(stream, context);
                    if (changed) FireDiscrepancyStringDidChange(discrepancyString);
                    break;
                }
                case (uint) PropertyID.DiscrepancyInt: {
                    changed = _discrepancyIntProperty.Read(stream, context);
                    if (changed) FireDiscrepancyIntDidChange(discrepancyInt);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _discrepancyString = discrepancyString;
        _discrepancyInt = discrepancyInt;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
