# Change Log
## 0.4.1
Added a menu at the bottom of the vertical map to manipulate aerial and symbolic view,
as well as roads and labels.  Fixed weird bug where horizontal map sometimes couldn't
be found by its tag.
## 0.4.0
Added the ability to use the vertical map as an overview, which follows the horizontal map
at a slightly reduced zoom and displays a yellow bounding box showing the extent of the 
horizontal map.  Updated to MRTK 2.7.3.
## 0.3.0
Added the ability to long tap the map, which will spawn the street address or locality of 
the point the hand ray cursor is located at.
## 0.2.0
Added "Go To" button which takes a text input, queries Bing Maps API to see if it is a valid
location, and zooms map to the location if valid.  Known issue:  The fade effect on UI text
does not work as desired, but does not impair the "Go To" functionality.
## 0.1.0
Initial commit, minimum viable product for demo.