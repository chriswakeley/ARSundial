using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CompassTrackableBehavior : DefaultTrackableEventHandler {

    public delegate void TrackingLost();
    public static event TrackingLost OnTrackingLostEvent;

    public delegate void TrackingFound();
    public static event TrackingFound OnTrackingFoundEvent;

    protected override void OnTrackingFound()
    {
        //base.OnTrackingFound();
        OnTrackingFoundEvent();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        OnTrackingLostEvent();
    }
}
