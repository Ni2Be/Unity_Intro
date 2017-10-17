# Unity_Intro
A procedural planet / star generator. TH Köln, course PGVW (procedural generation of virtual worlds)

After a one-week introduction to Unity and procedural generation we had 3 days to implement our own procedural generator.

[Windows release](https://github.com/Nice2Bee/Unity_Intro/releases/tag/v1.0)

[Unitypackage](https://1drv.ms/u/s!AtwaZYMhkRKBgrVukFEmZnbW5iU0Sg)

It takes a moment to generate something, and the screen will freeze the time it takes (unity functions can't be used parallel and I did not know when I stated work). Anyway, the assert is more that kind that you use in the editor, so I left it so.

### How it works

The program is made of three parts: [Planet_Generator.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Planet%20Gen/Planet_Generator.cs) that generates the meshes and holds information about the planet, [Perlin_Noise.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Perlin%20Noise/Perlin_Noise.cs) that generates a noise that is equal on the left and the right (not really a perlin noise) and the [Terrain_Generator.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Planet%20Gen/Terrain_Generator.cs) that generates a texture for a given heightmap. 

- First some vertices and indices form an icosahedron using the coordinates given by the formula:

![Coordinates](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/Icosahedron%20coordinates.PNG)
(phi = golden ratio)

\[Wikipedia icosahedron]

![Wikipedia icosahedron](https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/Icosahedron-golden-rectangles.svg/500px-Icosahedron-golden-rectangles.svg.png)


- There are two icosahedrons on the image, the inner one is the planet the outer one is the ocean moved to debug distance. In the program however there will only the planet icosahedron be generated and the ocean submesh will be generated later and only the parts needed.

\[Unity icosahedron]

![Unity icosahedron](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/Icosahedron.PNG)


- Then the triangles will be split and the uv's will be generated. Again: only for the planet and not for the ocean.

\[Splitting]

![splitting](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/dividing.gif)


- After that, a noise generates a heightmap.

\[Heightmap]

![Hightmap](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/hightmap.PNG)


- When the heightmap is applied you see why it would be waste to generate the ocean with the same function as the planet. The ocean submesh holds only the needed vertices and is animated. The rest of the vertices is placed on the height the heightmap represents.

\[Apply Heightmap (Debug distance down to 0)]

![Apply Hightmap](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/apply%20hightmap.gif)


- The heightmap is now given to a function that generates a texture. The look of the texture depends on the heights and the temperature variable that is set for the planet.   

\[Cold]

![ColdText](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/texture%20cold.PNG)
![Cold](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/cold.PNG)

\[Mediterranean]

![MediterraneanText](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/texture%20med.PNG)
![Mediterranean](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/med.PNG)

\[Warm]

![WarmText](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/texture%20warm.PNG)
![Warm](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/warm.PNG)

\[Hot]

![HotText](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/texture%20hot.PNG)
![Hot](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet%20texturing/hot.PNG)



- The animation allows to change the waves

\[Animation]

![Animation](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/waves.gif)


- The Stars are only generated textures out of the lava texture applied to an ocean

\[Star]

![Star](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/star%20waves.gif)



### Editor:

\[Editor control]
![Editor](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_editor.PNG)


### Random generated:


\[random cold planet]
![Cold](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_cold.PNG)


\[random warm planet 1]
![warm1](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_warm.PNG)


\[random warm planet 2]
![warm2](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_warm%202.PNG)


\[random desert planet]
![desert](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_desert.PNG)


\[random hot planet 1]
![desert](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_hot.PNG)


\[random hot planet 2]
![desert](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_hot%202.PNG)


\[random hot planet 3]
![desert](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_hot%203.PNG)


\[random star]
![desert](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_star.PNG)
