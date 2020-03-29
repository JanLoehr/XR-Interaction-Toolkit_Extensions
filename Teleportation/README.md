This is an extension to the Locomotion System.

Teleporting is pretty basic in th XR Interaction Toolkit. With this scripts you'll be able to implement an Oculus Style Teleportation Flow.

Push the stick on your controller forward -> Rotate to chose the direction to look in after teleporting -> Release the stick.
Teleportation can be canceled by pressing the stick.

To see it working, drag the sample rig into your scene and put a "XRDirectionTeleportationArea" component on an object with a collider in your scene.

![alt text](http://www.jan-loehr.de/wp-content/uploads/2020/03/Rhn6iorft2.gif "Directed Teleporting")

There is one adjustment to make in the XR Interaction Toolkit package.

Search for the XRController.cs file (also in packages, that's where you'll find it). Add "protected" in the two marked lines. This is necessary for the my scripts to hook into XR Interaction Toolkit.

![alt text](https://github.com/JanLoehr/XR-Interaction-Toolkit_Extensions/blob/master/x_ReadmeSources/TeleportationAdjustments.png "XR Interaction Toolkit Adjustments")

If you find this usefull and woulkd like to support be, feel free to buy me a coffee =)

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/R5R31JY3V)
