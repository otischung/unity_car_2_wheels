// using System.Collections;
// using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class LidarSensor : MonoBehaviour
{
    public float RangeMetersMin = 0.10f;      // 0
    public float RangeMetersMax = 100;        // 1000
    float ScanAngleStartDegrees = 0;          // -45
    float ScanAngleEndDegrees = -359;         // 45
    public int NumMeasurementsPerScan = 180;  // 10  // 可能是控制lidar的射線數量
    private Color[] lineColors = new Color[] {
        Color.red,
        Color.green,
        Color.blue,
        Color.yellow,
        Color.cyan,
        Color.magenta,
        Color.white,
        Color.black
    };

    List<float> ranges = new List<float>();
    List<float> range_tmp = new List<float>();
    float m_CurrentScanAngleStart;
    float m_CurrentScanAngleEnd;
    int m_NumMeasurementsTaken;
    bool isScanning = false;

    float GetMinRangeFlag = 0;

    List<Vector3> directionVectors = new List<Vector3>();
    List<Vector3> directionVectors_tmp = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentScanAngleStart = ScanAngleStartDegrees;
        m_CurrentScanAngleEnd = ScanAngleEndDegrees;
    }

    void BeginScan()
    {
        isScanning = true;
        m_NumMeasurementsTaken = 0;
    }

    public void EndScan()
    {
        if (ranges.Count == 0)
        {
            Debug.LogWarning($"Took {m_NumMeasurementsTaken} measurements but found no valid ranges");
        }
        else if (ranges.Count != m_NumMeasurementsTaken || ranges.Count != NumMeasurementsPerScan)
        {
            Debug.LogWarning($"Expected {NumMeasurementsPerScan} measurements. Actually took {m_NumMeasurementsTaken}" +
                             $"and recorded {ranges.Count} ranges.");
        }
        range_tmp = new List<float>(ranges);
        directionVectors_tmp = new List<Vector3>(directionVectors);
        m_NumMeasurementsTaken = 0;
        ranges.Clear();
        directionVectors.Clear();
        isScanning = false;

    }

    // Update is called once per frame
    void Update()
    {
        NumMeasurementsPerScan = 360;  // Must declaration at here. Don't know why.
        if (!isScanning)
        {
            BeginScan();
        }

        var measurementsSoFar = NumMeasurementsPerScan;  //180
        var yawBaseDegrees = transform.rotation.eulerAngles.y;
        while (m_NumMeasurementsTaken < measurementsSoFar)
        {
            var t = m_NumMeasurementsTaken / (float)NumMeasurementsPerScan;
            var yawSensorDegrees = Mathf.Lerp(m_CurrentScanAngleStart, m_CurrentScanAngleEnd, t);
            var yawDegrees = yawBaseDegrees + yawSensorDegrees;  // rotate lidar
            var directionVector = Quaternion.Euler(0f, yawDegrees, 0f) * Vector3.forward;  // Scanning direction
            var measurementStart = RangeMetersMin * directionVector + transform.position;  // ray scan in z axis
            var measurementRay = new Ray(measurementStart, directionVector);  // Simulate laser light from the starting point in a specific direction
            var foundValidMeasurement = Physics.Raycast(measurementRay, out var hit, RangeMetersMax);  // Returns whether an object was detected

            int colorIndex = (m_NumMeasurementsTaken / 45) % lineColors.Length;
            Color lineColor = lineColors[colorIndex];
            // Only record measurement if it's within the sensor's operating range
            if (foundValidMeasurement)
            {
                if (hit.distance > 15)  // 檢測大於15公尺就等於100，因為真實lidar最高也只到15m
                {
                    ranges.Add(100.0f);
                }
                else
                {
                    ranges.Add(hit.distance); // object distance
                }
                // Debug.DrawLine(measurementStart, hit.point, lineColor, 0.1f);
            }
            else
            {
                var rayEnd = measurementStart + directionVector * RangeMetersMax;
                // Debug.DrawLine(measurementStart, rayEnd, lineColor, 0.1f);
                ranges.Add(100.0f);
            }

            // Even if Raycast didn't find a valid hit, we still count it as a measurement
            ++m_NumMeasurementsTaken;

            directionVectors.Add(directionVector);
        }

        if (m_NumMeasurementsTaken >= NumMeasurementsPerScan)
        {
            if (m_NumMeasurementsTaken > NumMeasurementsPerScan)
            {
                Debug.LogError($"LaserScan has {m_NumMeasurementsTaken} measurements but we expected {NumMeasurementsPerScan}");
            }
            EndScan();
        }
    }
    public List<float> GetRange()
    {
        // return minRange;
        return range_tmp;
    }
    public List<Vector3> GetRangeDirection()
    {
        // return minDirectionVector;
        return directionVectors_tmp;
    }
    public int GetRangeSize()
    {
        return m_NumMeasurementsTaken;
    }
}
