# An Object Tracker for HoloLens 2
This is a project inspired by my work at a logistics company, where it's valuable to be able to track important freight
in real time.  As my first foray into practical applications of MR technology, I decided to build a prototype which 
could be used for tracking valuable objects holographically.  It consists of two maps, one as an overview and the other
which can zoom in and bring up details of tracked objects.  There is also a hand menu which allows the user to adjust
map settings to their needs.

## Software Requirements
* Unity 2020 3.18.f1 or later
* MRTK 2.7.2 or later
* Bing Maps SDK for Unity 0.11.0 or later
* JSON.NET 13.0.1 for Unity or later
* Active Bing Maps developer account, with token

## MRTK Dependencies
* Mixed Reality Toolkit Extensions 2.7.2
* Mixed Reality Toolkit Foundation 2.7.2
* Mixed Reality Toolkit Standard Assets 2.7.2
* Mixed Reality Toolkit Tools 2.7.2
* Mixed Reality Scene Understanding 0.6.0
* Mixed Reality Plane Finding 1.0.0
* Mixed Reality OpenXR Plugin 1.0.1
* Microsoft Spatializer 1.0.196

## How does it work?
On launching the application, you will see a gray vertical box which wants to attach itself to the nearest wall, and 
which moves with the user's head gaze.  This is a mockup of the overview map.  When the overview map is in a convenient
place, air tap.  The actual overview map will spawn, and show a view of the world.  Currently, that is all it does, but
later, it will be able to show a transparent box which illustrates the bounds of its more detailed counterpart.

When the you have placed the overview map, a second horizontal mockup will appear.  This is the detail map.  Similarly,
you can place the detail map in a convenient spot by air tapping, whether in midair or perhaps on a table or
desk.  When the detail map spawns, it will also spawn a menu showing the latitude and longitude the map is centered on,
as well as the current zoom level.  The menu billboards to always face the user.

To adjust the zoom level of the detail map, use the slider on the menu.  To adjust the position of the rendered map
display, reach out with a hand ray, air tap, and drag the map where needed.  To adjust the position of the map object
itself, hit the "Move Map" button or air tap while putting a hand ray on it.  This will cache the current data, and
turn the map back into a gray mockup so you can move it.

To adjust the map settings, look at your open palm.  A menu will appear allowing you to toggle between aerial and
symbolic map views, add or remove roads and map labels, and toggle 3D buildings in some cities.

The map is currently hardcoded to spawn showing Greater Seattle, and there are currently six tracked objects hardcoded
in the application at various ports, airports, or loading docks throughout the region.  They are recognizable by the 
teardrop-shaped markers on the detail map.  By reaching a hand ray out and air tapping, you can spawn a detail box
that shows various characteristics of the tracked object I thought could be relevant in this use case.

## Known issues
* The arrival times of tracked objects, along with the rest of their info, is hardcoded in the file
StreamingAssets\TrackedObjects.json.  If the arrival date is in the past, the application will throw an exception and
not load the tracked objects.  I anticipate if this is ever used for anything in the real world, it would be consuming
data from a Kafka stream or other data source, so I didn't make the flat file implementation very robust.  Before
launching, edit TrackedObjects.json to ensure the arrival times are somewhere in the future.
* There is code in MapSearchText.cs which is supposed to allow a response to user input such as "Zooming . . ." or 
"Unknown Error" to fadeout after appearing and being read.  The text does not fade, but simply disappears when the 
alpha value reaches 0.

## To do
* Finish the overview map
* Add the ability to query Bing Maps to show the street address of a tracked object at a given lat/long
* Potentially add the ability to show the street address of a hand ray cursor when it hits the detail map