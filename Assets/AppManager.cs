#define TEST

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using CustomExtensions;

namespace CustomExtensions
{
    //Extension methods must be defined in a static class
    public static class EnumExtension
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                           Attribute.GetCustomAttribute(field,
                             typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
    }
}

public class AppManager : MonoBehaviour {

    public delegate void StateChange();
    public static event StateChange OnStateChange;

    public GameObject earthModel;
    public GameObject sunModel;
    public GameObject StatusText;
    public GameObject MaskingBox;
    public GameObject sunDial;

    public float latitude;
    public float longitude;

    public enum LocationState {
        [Description("Unfortunately something went wrong when tryng to find your phone's location, try restarting the app.")] NotSearching,
        [Description("Your phone is trying to find its location...")] Initializing,
        [Description("Your phone is trying to find its location...")] Querying,
        [Description("Your phone's location is turned off, the sundial will only work if your phone's location is known.")] NotEnabled,
        [Description("Unfortunately something went wrong when tryng to find your phone's location, try restarting the app.")] Failed,
        [Description("Your phone reported it's location, and we can use it to position your sundial on the globe!")] Found };

    public LocationState locationState;

    public enum AppState {NoTarget, Intro, FindingLocation, Viewing};

    public AppState appState;

    private DateTime currentDateTime;


    // Use this for initialization
    void Start () {
        appState = AppState.NoTarget;
        earthModel.SetActive(false);
        sunModel.SetActive(false);
        sunDial.SetActive(false);
        MaskingBox.SetActive(false);
        locationState = LocationState.NotSearching;
        StartCoroutine(RunLocation());
        
        CompassTrackableBehavior.OnTrackingFoundEvent += targetFound;
        CompassTrackableBehavior.OnTrackingLostEvent += targetLost;
    }
	
	// Update is called once per frame
	void Update () {
        switch (appState)
        {
            case AppState.NoTarget:
                break;
            case AppState.Intro:
                break;
            case AppState.FindingLocation:
                break;
            case AppState.Viewing:
                break;
        }
    }

    IEnumerator RunLocation()
    {
#if TEST
        updateLocationStatus(LocationState.Found);
        latitude = 37.23f;
        longitude = -80.42f;
        Input.location.Stop();
        currentDateTime = new DateTime(2018, 8, 24, 14, 36, 55, DateTimeKind.Local);
        yield break;
#endif
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            updateLocationStatus(LocationState.NotEnabled);
            yield break;
        }            

        // Start service before querying location
        Input.location.Start();
        updateLocationStatus(LocationState.Querying);
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            updateLocationStatus(LocationState.Failed);
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            updateLocationStatus(LocationState.Failed);
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            updateLocationStatus(LocationState.Found);
            latitude = Input.location.lastData.latitude;
            longitude = Input.location.lastData.longitude;
        }

        // Stop service if there is no need to query location updates continuously
        Input.location.Stop();
    }

    private void updateLocationStatus(LocationState newState)
    {
        if(newState != locationState)
        {
            locationState = newState;
            if (appState == AppState.FindingLocation)
            {
                StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText(locationState.GetDescription()));
            }
        }
    }

    public void targetFound()
    {
        if(appState == AppState.NoTarget) {
            appState = AppState.Intro;
            earthModel.SetActive(true);
            MaskingBox.SetActive(true);
            sunModel.SetActive(true);
            StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText(""));
            StartCoroutine(IntroSequence());
        }
        
    }
    private void targetLost()
    {

    }

    private IEnumerator IntroSequence()
    {
#if TEST
        yield return StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText(""));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().AlignLongitude(longitude));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().AlignLatitude(latitude));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().Descend());

        yield return StartCoroutine(sunModel.GetComponent<SunBehavior>().alignToTime(currentDateTime, latitude, longitude));

        sunDial.SetActive(true);
        sunDial.GetComponent<sunDialBehavior>().setAngle(latitude);
        yield break;
#endif
        yield return StartCoroutine(MaskingBox.GetComponent<MaskingBoxBehavior>().Fade());
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().Fade());
        StartCoroutine(earthModel.GetComponent<earthBehavior>().StartSpin());
        yield return StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText("This is a model of your planet."));
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText("Let's find where your sundial is on the model."));
        yield return new WaitForSeconds(1);


        yield return StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText(locationState.GetDescription()));
        appState = AppState.FindingLocation;

        while (locationState != LocationState.Found)
        {
            yield return null;
        }
        yield return StartCoroutine(StatusText.GetComponent<StatusTextBehavior>().ChangeText(""));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().AlignLongitude(longitude));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().AlignLatitude(latitude));
        yield return StartCoroutine(earthModel.GetComponent<earthBehavior>().Descend());

        yield return StartCoroutine(sunModel.GetComponent<SunBehavior>().alignToTime(currentDateTime, latitude, longitude));

        sunDial.SetActive(true);
        sunDial.GetComponent<sunDialBehavior>().setAngle(latitude);
    }
}
