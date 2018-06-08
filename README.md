# Simple Multi-Camera Rig for displaying content in the UTS Data Arena

This is a Unity3D project containing a multi-camera rig for displaying 360 degree panoramic content in the UTS Data Arena. It is also capable of rendering top/bottom stereo-3D, panoramic content.

Check out the example scenes for usage.

## Quick properties description:

 * `cameraCount` - Number of cameras to use, defaults to `6`. More cameras means a smoother-looking panorama, as they better approximate the surface of the arena. Best looking results with `36` cameras.
 * `rotationOffset` - The offset (in degrees) relative to the local transform by which the first camera is placed. Use this to adjust which camera is 'front'.
 * `viewportOffsetH` - the horizontal offset (in homogenous coords) of the created camera viewports.
 * `viewportOffsetV` - the vertical offset (in homogenous coords) of the created camera viewports.
 * `viewportHeight` - the screen height of the created camera viewports (in homogenous coords)
 * `screenHeight` - the physical height of the data arena screen in metres (4.0m)
 * `screenRadius` - the physical radius of the data arena screen in metres (4.9m)
 * `viewerHeight` - the physical vertical of the virtual cameras in the data arena in metres. (1.2f);
 * `farClip` - the far clipping plane of each camera.
 * `enableBaseCamera` - also display the base camera in addition to panoramic camera.

## Keyboard controls

 * Plus key - Increase camera count by 6
 * Minus key - Decrease camera count by 6. Minimum 6
 * PageUp key - Increase viewerHeight by 0.1m
 * PageDown key - Decrease viewerHeight by 0.1m
 
## Examples

[TODO]

### Mono Scene

[insert mono scene here]

### Stereo Scene

[insert stereo scene here]
