# Simple Multi-Camera Rig for displaying content in the UTS Data Arena

This is a Unity3D project containing a multi-camera rig for displaying 360 degree content in the UTS Data Arena. It is also capable of rendering top/bottom stereo-3D content.

Check out the example scenes for usage.

Quick properties description:

 * cameraCount - Number of cameras to use, defaults to 6. More cameras means a smoother transition bewteen cameras
 * rotationOffset - The offset (in degrees) relative to the local transform by which the first camera is placed. Use this to adjust which camera is 'front'
 * viewportOffsetH - the horizontal offset (in homogenous coords) of the created camera viewports
 * viewportOffsetV - the vertical offset (in homogenous coords) of the created camera viewports
 * hOblique - the amount (in homogenous coords) by which to horizontally offset the camera's projection matrix. Useful for stereo
 * vOblique - the amount (in homogenous coords) by which to vertically offset the camera's projection matrix.
 * farClip - the far clipping plane of each camera.
 * enableBaseCamera - also display the base camera in addition to panoramic camera
 * enableObliqueControls - allow camera projection matrix to be changed during playback

