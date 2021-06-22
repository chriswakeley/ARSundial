using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour {
    private const double Deg2Rad = Math.PI / 180.0;
    private const double Rad2Deg = 180.0 / Math.PI;
    public float alignTime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator alignToTime(System.DateTime dateTime, double latitude, double longitude)
    {
        // Convert to UTC  
        dateTime = dateTime.ToUniversalTime();

        // Number of days from J2000.0.  
        double julianDate = 367 * dateTime.Year -
            (int)((7.0 / 4.0) * (dateTime.Year +
            (int)((dateTime.Month + 9.0) / 12.0))) +
            (int)((275.0 * dateTime.Month) / 9.0) +
            dateTime.Day - 730531.5;

        double julianCenturies = julianDate / 36525.0;

        // Sidereal Time  
        double siderealTimeHours = 6.6974 + 2400.0513 * julianCenturies;

        double siderealTimeUT = siderealTimeHours +
            (366.2422 / 365.2422) * (double)dateTime.TimeOfDay.TotalHours;

        double siderealTime = siderealTimeUT * 15 + longitude;

        // Refine to number of days (fractional) to specific time.  
        julianDate += (double)dateTime.TimeOfDay.TotalHours / 24.0;
        julianCenturies = julianDate / 36525.0;

        // Solar Coordinates  
        double meanLongitude = CorrectAngle(Deg2Rad *
            (280.466 + 36000.77 * julianCenturies));

        double meanAnomaly = CorrectAngle(Deg2Rad *
            (357.529 + 35999.05 * julianCenturies));

        double equationOfCenter = Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
            Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));

        double elipticalLongitude =
            CorrectAngle(meanLongitude + equationOfCenter);

        double obliquity = (23.439 - 0.013 * julianCenturies) * Deg2Rad;

        // Right Ascension  
        double rightAscension = Math.Atan2(
            Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
            Math.Cos(elipticalLongitude));

        double declination = Math.Asin(
            Math.Sin(rightAscension) * Math.Sin(obliquity));

        // Horizontal Coordinates  
        double hourAngle = CorrectAngle(siderealTime * Deg2Rad) - rightAscension;

        if (hourAngle > Math.PI)
        {
            hourAngle -= 2 * Math.PI;
        }

        double altitude = Math.Asin(Math.Sin(latitude * Deg2Rad) *
            Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
            Math.Cos(declination) * Math.Cos(hourAngle));

        // Nominator and denominator for calculating Azimuth  
        // angle. Needed to test which quadrant the angle is in.  
        double aziNom = -Math.Sin(hourAngle);
        double aziDenom =
            Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
            Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

        double azimuth = Math.Atan(aziNom / aziDenom);

        if (aziDenom < 0) // In 2nd or 3rd quadrant  
        {
            azimuth += Math.PI;
        }
        else if (aziNom < 0) // In 4th quadrant  
        {
            azimuth += 2 * Math.PI;
        }

        // Altitude  
        Console.WriteLine("Altitude: " + altitude * Rad2Deg);

        // Azimut  
        Console.WriteLine("Azimuth: " + azimuth * Rad2Deg);
        float currentTime = 0f;

        while (!Mathf.Approximately(this.transform.localEulerAngles.x, (float)(altitude * Rad2Deg)))
        {
            currentTime += Time.deltaTime;
            this.transform.localEulerAngles = new Vector3(Mathf.Lerp(this.transform.localEulerAngles.x, (float)(altitude * Rad2Deg), currentTime / alignTime), this.transform.localEulerAngles.y, 0f);
            yield return null;
        }

        currentTime = 0f;
        print((float)(azimuth * Rad2Deg) + 180f);
        while (!Mathf.Approximately(this.transform.localEulerAngles.y, ((float)(azimuth * Rad2Deg) + 180f)%360f))
        {
            currentTime += Time.deltaTime;
            this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, Mathf.Lerp(this.transform.localEulerAngles.y, ((float)(azimuth * Rad2Deg) + 180f) % 360f, currentTime / alignTime), 0f);
            yield return null;
        }
    }

    private static double CorrectAngle(double angleInRadians)
    {
        if (angleInRadians < 0)
        {
            return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
        }
        else if (angleInRadians > 2 * Math.PI)
        {
            return angleInRadians % (2 * Math.PI);
        }
        else
        {
            return angleInRadians;
        }
    }
}
