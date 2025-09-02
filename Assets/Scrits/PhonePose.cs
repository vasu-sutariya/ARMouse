using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Text;

public class PhonePose : MonoBehaviour
{
    public Camera arCamera;
    public Text displayText; // UI Text component to show sensor data
    public InputField ipInputField; // UI InputField for IP address input
    public Button connectButton; // Button to connect with new IP
    private UdpClient udpClient;
    private System.Net.IPEndPoint serverEndPoint;
    private string currentIP = "192.168.1.208"; // Default IP
    
    void Start()
    {
        // Initialize UI
        if (ipInputField != null)
        {
            ipInputField.text = currentIP; // Set default IP in input field
        }
        
        if (connectButton != null)
        {
            connectButton.onClick.AddListener(UpdateConnection);
        }
        
        // Initialize connection
        InitializeConnection();
        
        // Enable gyroscope and accelerometer
        Input.gyro.enabled = true;
    }
    
    void InitializeConnection()
    {
        try
        {
            // Close existing connection if any
            udpClient?.Close();
            
            // Create new UDP client and endpoint
            udpClient = new UdpClient();
            serverEndPoint = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(currentIP), 8080);
            
            Debug.Log($"Connected to server at {currentIP}:8080");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to connect to {currentIP}: {e.Message}");
        }
    }
    
    public void UpdateConnection()
    {
        if (ipInputField != null && !string.IsNullOrEmpty(ipInputField.text))
        {
            string newIP = ipInputField.text.Trim();
            if (IsValidIPAddress(newIP))
            {
                currentIP = newIP;
                InitializeConnection();
            }
            else
            {
                Debug.LogError($"Invalid IP address: {newIP}");
                // Reset input field to current valid IP
                ipInputField.text = currentIP;
            }
        }
    }
    
    bool IsValidIPAddress(string ip)
    {
        System.Net.IPAddress address;
        return System.Net.IPAddress.TryParse(ip, out address);
    }

    void Update()
    {
        Vector3 pos = arCamera.transform.position;   // phone world position
        Vector3 rot = arCamera.transform.rotation.eulerAngles; // phone world rotation
        
        // Get gyroscope and accelerometer data
        Vector3 gyro = Input.gyro.rotationRateUnbiased; // gyroscope data (rad/s)
        Vector3 accel = Input.acceleration; // accelerometer data (g-force)

        // Update display text if assigned
        UpdateDisplay(pos, rot, gyro, accel);

        // Send every frame for maximum speed - no throttling
        SendPoseDataFast(pos, rot, gyro, accel);
    }

    void SendPoseDataFast(Vector3 position, Vector3 rotation, Vector3 gyro, Vector3 accel)
    {
        // Format: x,y,z,rx,ry,rz,gx,gy,gz,ax,ay,az
        string data = $"{position.x:F2},{position.y:F2},{position.z:F2},{rotation.x:F2},{rotation.y:F2},{rotation.z:F2},{gyro.x:F2},{gyro.y:F2},{gyro.z:F2},{accel.x:F2},{accel.y:F2},{accel.z:F2}";
        byte[] bytes = Encoding.UTF8.GetBytes(data);
        
        try
        {
            udpClient.Send(bytes, bytes.Length, serverEndPoint);
        }
        catch (System.Exception e)
        {
            Debug.LogError("UDP Send failed: " + e.Message);
        }
    }
    
    void UpdateDisplay(Vector3 position, Vector3 rotation, Vector3 gyro, Vector3 accel)
    {
        if (displayText != null)
        {
            // Format all sensor data for display
            string displayString = $"SENSOR DATA\n\n" +
                                 $"Position:\n" +
                                 $"X: {position.x:F2}\n" +
                                 $"Y: {position.y:F2}\n" +
                                 $"Z: {position.z:F2}\n\n" +
                                 $"Rotation:\n" +
                                 $"X: {rotation.x:F1}°\n" +
                                 $"Y: {rotation.y:F1}°\n" +
                                 $"Z: {rotation.z:F1}°\n\n" +
                                 $"Gyroscope (rad/s):\n" +
                                 $"X: {gyro.x:F2}\n" +
                                 $"Y: {gyro.y:F2}\n" +
                                 $"Z: {gyro.z:F2}\n\n" +
                                 $"Accelerometer (g):\n" +
                                 $"X: {accel.x:F2}\n" +
                                 $"Y: {accel.y:F2}\n" +
                                 $"Z: {accel.z:F2}";
            
            displayText.text = displayString;
        }
    }
    
    void OnDestroy()
    {
        udpClient?.Close();
    }
}
