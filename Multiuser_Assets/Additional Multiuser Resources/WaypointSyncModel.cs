using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class WaypointSyncModel
{
    [RealtimeProperty(40, true, true)]
    private int _routeIndex;
    [RealtimeProperty(41, true, true)]
    private int _waypointIndex;
}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class WaypointSyncModel : RealtimeModel {
    public int routeIndex {
        get {
            return _routeIndexProperty.value;
        }
        set {
            if (_routeIndexProperty.value == value) return;
            _routeIndexProperty.value = value;
            InvalidateReliableLength();
            FireRouteIndexDidChange(value);
        }
    }
    
    public int waypointIndex {
        get {
            return _waypointIndexProperty.value;
        }
        set {
            if (_waypointIndexProperty.value == value) return;
            _waypointIndexProperty.value = value;
            InvalidateReliableLength();
            FireWaypointIndexDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(WaypointSyncModel model, T value);
    public event PropertyChangedHandler<int> routeIndexDidChange;
    public event PropertyChangedHandler<int> waypointIndexDidChange;
    
    public enum PropertyID : uint {
        RouteIndex = 40,
        WaypointIndex = 41,
    }
    
    #region Properties
    
    private ReliableProperty<int> _routeIndexProperty;
    
    private ReliableProperty<int> _waypointIndexProperty;
    
    #endregion
    
    public WaypointSyncModel() : base(null) {
        _routeIndexProperty = new ReliableProperty<int>(40, _routeIndex);
        _waypointIndexProperty = new ReliableProperty<int>(41, _waypointIndex);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _routeIndexProperty.UnsubscribeCallback();
        _waypointIndexProperty.UnsubscribeCallback();
    }
    
    private void FireRouteIndexDidChange(int value) {
        try {
            routeIndexDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    private void FireWaypointIndexDidChange(int value) {
        try {
            waypointIndexDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _routeIndexProperty.WriteLength(context);
        length += _waypointIndexProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _routeIndexProperty.Write(stream, context);
        writes |= _waypointIndexProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.RouteIndex: {
                    changed = _routeIndexProperty.Read(stream, context);
                    if (changed) FireRouteIndexDidChange(routeIndex);
                    break;
                }
                case (uint) PropertyID.WaypointIndex: {
                    changed = _waypointIndexProperty.Read(stream, context);
                    if (changed) FireWaypointIndexDidChange(waypointIndex);
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
        _routeIndex = routeIndex;
        _waypointIndex = waypointIndex;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */
