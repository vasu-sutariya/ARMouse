<div align="center">

# ARMouse - AR-Based Robot Controller

![ARMouse Demo](ARMouse.gif)

</div>

Transform your smartphone into a precision 3D controller for robotic manipulators! This app uses AR tracking combined with Kalman-filtered sensor fusion to for real-time control of robot end-effector.

## Features

### Core Functionality
- **3D Position Control**: Phone movement translates to precise robot end-effector positioning
- **Sensor Fusion**: Kalman filter combines AR, gyroscope and accelerometer data for better accuracy
- **Real-time Communication**: High-frequency (60 - 120 Hz) UDP data transmission for responsive control of Robot
- **URAlight Integration**: Direct integration with [URAlight robotics platform](https://github.com/vasu-sutariya/Uralight) for manipulator control

### Technical Features
- **Sensor Fusion Options**: Complementary and Kalman filter options for noise reduction
- **Adaptive Calibration**: Auto-calibration with gyro bias correction
- **Configurable Mappings**: Flexible axis mapping between phone and robot coordinate systems
- **Live Sensor Visualization**: Real-time display of all sensor data and fusion results
- **Precision Control**: Individual axis sensitivity and offset adjustments

## System Architecture

The system consists of three main components:

### Unity Mobile App (`PhonePose.cs`)
- **AR Tracking**: Captures precise 6DOF pose using Unity AR Foundation
- **Multi-Sensor Input**: Combines gyroscope, accelerometer and camera data
- **Real-time Streaming**: Sends 12-channel sensor data at maximum frame rate
- **User Interface**: IP configuration and live sensor data visualization (With future plans for URALight integration to provide more robot features directly to smartphone)

### Advanced Pose Controller (`ARPoseController.cs`)
- **Kalman Filter Fusion**: Sensor fusion with configurable noise parameters
- **Robot Integration**: Direct communication with URAlight robot controllers
- **Inverse Kinematics**: Automatic IK calculation for end-effector positioning
- **Coordinate Mapping**: Flexible transformation between coordinate systems

### URAlight Robot Platform
- **Manipulator Control**: Compatible with UR series and other industrial robots
- **Motion Planning**: Integrated path planning and collision avoidance
- **Real-time Simulation**: Live visualization of robot movements
- **Safety Systems**: Built-in joint limits and workspace constraints

## Getting Started

### Prerequisites

#### For Robot Control (URAlight Integration)
- **URAlight Platform**: Clone and set up [URAlight](https://github.com/vasu-sutariya/Uralight/Traj)
- **Android/iOS device** with AR capabilities (ARCore/ARKit support)
- **Robotic Manipulator**: UR series or compatible robot (physical or simulated)
- Both devices on the **same Wi-Fi network**

#### For Basic Mouse Control (Legacy)
- **Python 3.7+** on desktop
- **Android/iOS device** with gyroscope and accelerometer

### Robot Control Setup (Primary Use Case)

1. **Set up URAlight Platform**:
```bash
git clone https://github.com/vasu-sutariya/Uralight.git
# Follow URAlight setup instructions
```

2. **Clone this repository**:
[Download the app on your Andriod phone] (https://github.com/vasu-sutariya/ARMouse/releases/tag/Pre-release)
```bash
git clone https://github.com/yourusername/ARMouse.git
cd ARMouse
```

3. **Unity Setup**:
   - Open Unity and load the URalight project
   - Ensure AR Foundation package is installed
   - Import `ARPoseController.cs` script into your URAlight project
   - Attach `ARPoseController` to a GameObject in your URAlight scene

4. **Configure Robot Integration**:
   - Assign robot components in ARPoseController inspector
   - Set up coordinate system mappings
   - Adjust sensitivity and offset parameters

5. **Mobile App Deployment**:
   - Launch app and configure network connection
   - Connect to the URAlight system


## 🎮 Usage

### Robot Control Mode

1. **Start URAlight** - Launch the URAlight platform with ARPoseController attached
2. **Launch mobile app** and connect to URAlight system IP
3. **Calibrate system** - Press Space key or let auto-calibration complete
4. **Control robot**:
   - Move phone in 3D space → Robot end-effector follows
   - Smooth, filtered motion with Kalman fusion
   - Real-time inverse kinematics calculation
   - Fixed end-effector orientation (currently 180°, 0°, 0°)

### Advanced Configuration

- **Sensor Fusion**: Choose between Complementary or Kalman filtering
- **Axis Mapping**: Configure coordinate system transformations
- **Sensitivity Tuning**: Adjust individual axis sensitivities
- **Offset Calibration**: Set workspace center and boundaries

## Configuration

### ARPoseController Parameters

#### Sensor Fusion Settings
```csharp
[Header("Sensor Fusion")]
public SensorFusionType fusionType = SensorFusionType.Kalman;
public bool useSensorFusion = true;
public float arPoseWeight = 0.7f;        // Trust level for AR data
public float imuWeight = 0.3f;           // Trust level for IMU data
```

#### Kalman Filter Tuning
```csharp
[Header("Kalman Filter Parameters")]
public float processNoise = 0.01f;           // System noise
public float arPoseMeasurementNoise = 0.1f;  // AR measurement noise
public float imuMeasurementNoise = 0.5f;     // IMU measurement noise
```

#### Position Control
```csharp
[Header("Position Sensitivity")]
public float positionSensitivityX = 1.0f;
public float positionSensitivityY = 1.0f;
public float positionSensitivityZ = 1.0f;

[Header("Position Offsets")]
public float positionOffsetX = 0.0f;
public float positionOffsetY = 0.0f;
public float positionOffsetZ = 0.0f;
```

#### Coordinate System Mapping
```csharp
[Header("Axis Mapping")]
public AxisMapping inputToRobotMapping = AxisMapping.Direct;
// Options: Direct, SwapXY, SwapXZ, SwapYZ, Custom
```

### Network Settings
- **Default UDP Port**: `8080`
- **Data Format**: 12-channel (position, rotation, gyro, accelerometer)
- **Update Rate**: Unity frame rate dependent (typically 60-120 Hz)
- **Thread Safety**: Lock-free data exchange between network and main threads

## Data Processing

### Sensor Data Pipeline
```
Phone Sensors → AR Tracking → UDP Transmission → Kalman Filtering → Robot IK → Joint Control
```

### Data Format
```csv
x,y,z,rx,ry,rz,gx,gy,gz,ax,ay,az
```

**Channel Descriptions**:
- `x,y,z`: AR camera world position (meters)
- `rx,ry,rz`: Phone rotation Euler angles (degrees)
- `gx,gy,gz`: Gyroscope angular velocity (rad/s)
- `ax,ay,az`: Accelerometer linear acceleration (g-force)

### Kalman Filter Implementation
- **State Estimation**: 3D position and rotation tracking
- **Noise Modeling**: Separate process and measurement noise parameters
- **Sensor Fusion**: Weighted combination of AR and IMU data
- **Drift Correction**: Automatic gyroscope bias calibration

## Technical Implementation

### Key Features
- **Real-time Performance**: Sub-10ms latency from phone to robot
- **Robust Filtering**: Kalman and complementary filter options
- **Thread-Safe Communication**: Dedicated UDP listener thread
- **Automatic Calibration**: Center position and gyro bias correction
- **Flexible Integration**: Modular design for easy URAlight attachment

### Robot Integration
- **Inverse Kinematics**: Automatic IK solving for target positions
- **Joint Control**: Direct communication with URAlight joint controllers
- **Safety Features**: Workspace limits and collision avoidance
- **Fixed Orientation**: Currently uses fixed end-effector rotation (180°, 0°, 0°)

## Troubleshooting

### Network & Connection Issues
- **WiFi Connection**: Ensure both devices are on the same network
- **Firewall Settings**: Allow UDP port 8080 through system firewall
- **IP Configuration**: Verify correct IP address in mobile app
- **URAlight Connection**: Check URAlight platform is running and accessible

### Robot Control Issues
- **IK Solutions**: No solution found - check workspace limits and target reachability
- **Joint Limits**: Verify robot joint constraints in URAlight configuration
- **Coordinate Mapping**: Try different axis mapping options if movement seems inverted
- **Sensitivity**: Adjust position sensitivity if movements are too large/small

### Sensor Performance Issues
- **AR Tracking**: Ensure good lighting and textured environment for AR tracking
- **Sensor Fusion**: Try switching between Kalman and Complementary filtering
- **Calibration**: Recalibrate if drift is observed over time
- **Update Rate**: Reduce Unity quality settings if frame rate is low

### Advanced Diagnostics
- **Debug Logging**: Enable `showDebugInfo` in ARPoseController for detailed logs
- **Kalman Tuning**: Adjust noise parameters if filtering is too aggressive/loose
- **Thread Issues**: Check Unity console for UDP thread errors
