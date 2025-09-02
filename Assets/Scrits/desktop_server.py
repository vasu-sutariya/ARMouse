#!/usr/bin/env python3
"""
AR Mouse Controller - Use phone pose to control desktop mouse
Move your phone to control the mouse cursor on your desktop!
"""

import socket
import time

class ARMouseController:
    def __init__(self):
        # Get screen dimensions
        
        # Calibration values - adjust these for sensitivity
        self.sensitivity_x = 100  # Higher = more sensitive horizontal movement
        self.sensitivity_y = 100  # Higher = more sensitive vertical movement
       
        
        # Center position for relative movement
        self.center_x = 0
        self.center_y = 0
        self.center_z = 0
        self.calibrated = False
        
        
        
        print("AR Mouse Controller Ready!")
        print("=" * 40)
        print("Controls:")
        print("• Move phone left/right/up/down → Mouse cursor")
        print("=" * 40)

    def calibrate_center(self, x, y, z):
        """Set the current position as center/neutral"""
        self.center_x = x
        self.center_y = y
        self.center_z = z
        self.calibrated = True
        print(f"✓ Calibrated center position: ({x:.2f}, {y:.2f}, {z:.2f})")

    def control_mouse(self, x, y, z, rx, ry, rz, gx, gy, gz, ax, ay, az):
        # Auto-calibrate on first data
        if not self.calibrated:
            self.calibrate_center(x, y, z)
            return
        
        # Calculate relative movement from center
        rel_x = x - self.center_x
        rel_y = y - self.center_y
        
        
       
        
        
        # Check for click gesture (tilt forward)

        
        # Print both lines
        #print(f"{x},{y},{z},{rx},{ry},{rz},{gx},{gy},{gz},{ax},{ay},{az}")
        print(f"Mouse: ({mouse_x:4d}, {mouse_y:4d}) | Gyro: ({gx:5.1f},{gy:5.1f},{gz:5.1f}) | Accel: ({ax:5.1f},{ay:5.1f},{az:5.1f})")
        


      

def main():
    controller = ARMouseController()
    
    # Create UDP socket
    sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    sock.bind(('0.0.0.0', 8080))
    
    print(f"Listening on UDP port 8080...")
    print("Move your phone to control the mouse!")
    
    try:
        while True:
            # Receive pose data
            data, addr = sock.recvfrom(4096)
            #print(f"Received from {addr}: {data}")
            # Parse CSV format: x,y,z,rx,ry,rz,gx,gy,gz,ax,ay,az
            values = data.decode('utf-8').split(',')
            if len(values) == 12:
                x, y, z, rx, ry, rz, gx, gy, gz, ax, ay, az = map(float, values)
                controller.control_mouse(x, y, z, rx, ry, rz, gx, gy, gz, ax, ay, az)
            
    except KeyboardInterrupt:
        print("\n\nAR Mouse Controller stopped.")
    finally:
        sock.close()

if __name__ == '__main__':
    main()
