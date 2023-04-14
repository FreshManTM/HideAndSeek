# Hide and Seek

Our goal is to experience the release process of a game by building the simplest impossible that other people will actually play. 

To do this we will duplicate exactly a game that has already been made by us on the platform Ylands years ago. 

https://youtu.be/9JnTScoKKyk

This is our design shortcut, as this game has been painstakingly balanced over the course of a few hundred iterations. 

Moreover, rather than building art we are going to use a Synty Polygon pack.

The game will be multiplayer but we will be using Photon Unity networking and integrate it using: https://assetstore.unity.com/packages/templates/packs/mfp-multiplayer-first-person-38333

I have made a multiplayer game on a weekend with this asset. It’s easy to customize. It already has the lobby feature we need and networked pickups and moving platforms which is really all we need. 

Finally, to avoid the challenges of level design we are going to use the demo level of the synty pack and just customize it.

Deadline: 30 days. 

Gameplay: 
https://youtu.be/x7fkqAu4ypI 
The gameplay is a twist on hide and seek. Hide and seek is really boring in the end because hiding is boring. In our game there are some fundamental differences. Firstly, there are glowing glowing eggs all around the map. The more you pick up the more points you get. These glowing eggs are always out in the open. The hider who gets the most glowing eggs before the time runs out wins the game. The amount of glowing eggs obtained by each player is seen on a scoreboard in the top right. 

Glowing eggs respawn 45 seconds after being picked up. 

The length of the timer should correlate with the size of the map and the number of hiding places. 8 min is my estimate based on the pack. 

If a hider gets tagged by a seeker they turn into a seeker they get turned into a seeker (they should visibly change clothing/color). 

Seekers run 10% faster than hiders. 

There are power-ups around the map that act as speed bonuses allowing seekers to get away. 

Items are placed specifically so that you can get on top of buildings and such to out-parkour seekers. 

A seeker can win by tagging the last hider incentivizing a strong action by seekers. 

Bonus: 
A statue or special item in the middle of the map has an arrow which points to the location of the person in the lead. 

# Good to knows: 

LTS Release 2021.3.12f1
https://unity3d.com/unity/qa/lts-releases

Other assets: 
Synty art: 
https://assetstore.unity.com/packages/3d/environments/urban/polygon-town-pack-low-poly-3d-art-by-synty-121115
For testing: 
https://assetstore.unity.com/packages/essentials/starter-assets-third-person-character-controller-196526

# Future ideas: 

Make the characters all 1 person with in Perston's family from Preston Plays (they love hide and seek) and pitch it to them a a video idea. 

Have people do everything in the game with a spoon in their mouth and an egg on it and they lose if it drops and release it as an easter update. 

