Aight so there's a few things you'll want to know.

CameraFollow.cs - Just some script I copied from the internet to follow the car better

collisionScript.cs - Created by myself, simply uses short rays to see if the car has it the wall and resets it if so. You'll likely
		need to mess with this one because it determines when the AI has "Failed"

RayCastScript.cs - Probably the most important. This creates the three visable rays and determine when they overlap the walls. they have
		a public variable that has true for each of the rays when they cross the wall. I set the rays to a length that seemed appropriate
		for the track.

CarUserControl.cs - this is where the AI will change the accel and steering variables from 0 to 1 to accelerate, brake, and turn. These are
		also public variables and currently the only way to drive the car. (manualy change the variables in the inspecter in the car object)



I'm going to be sending a scene along with the rest of this stuff so hopefully that works. If not, I've made a prefab for the car, camera,
and map so that you can easily place each one back in.


*****IF THE BEAMS DON'T APPEAR VISIBLY AT FIRST, OPEN THE GIZMOS DROPDOWN ABOVE THE SCENE WINDOW AND CHECK "SELECTION WIRE"*****