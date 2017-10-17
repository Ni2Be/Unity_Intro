# Unity_Intro
#### A procedural planet / star generator. TH Köln, course PGVW (procedural generation of virtual worlds)
![Planet](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/planet.gif)

After a one-week introduction to Unity and procedural generation we had 3 days to implement our own procedural generator.

[Windows release](https://github.com/Nice2Bee/Unity_Intro/releases/tag/v1.0)

[Unitypackage](https://1drv.ms/u/s!AtwaZYMhkRKBgrVukFEmZnbW5iU0Sg)

It takes a moment to generate something, and the screen will freeze the time it takes (unity functions are not thread safe ..one of those things you better know before you start work). Anyway, the assert is more that kind you use in the editor, so that's not that big of a problem.

### How it works

The program is made of three parts: [Planet_Generator.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Planet%20Gen/Planet_Generator.cs) that generates the meshes and holds information about the planet, [Perlin_Noise.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Perlin%20Noise/Perlin_Noise.cs) that generates a noise that is equal on the left and the right (not really a perlin noise) and the [Terrain_Generator.cs](https://github.com/Nice2Bee/Unity_Intro/blob/master/Planet%20Gen/Assets/Planet%20Generator/Planet%20Gen/Terrain_Generator.cs) that generates a texture for a given heightmap. 

- First some vertices and indices form an icosahedron using the coordinates given by the formula:

![Coordinates](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/Icosahedron%20coordinates.PNG)
(phi = golden ratio)

\[Wikipedia icosahedron]

![Wikipedia icosahedron](https://upload.wikimedia.org/wikipedia/commons/thumb/9/9c/Icosahedron-golden-rectangles.svg/500px-Icosahedron-golden-rectangles.svg.png)


- There are two icosahedrons on the image, the inner one is the planet, the outer one is the ocean scaled up to debug-distance *(debug-distance is used to seperate the ocean from the planet, will be clearer in a moment, image: [Apply Heightmap (Debug distance down to 0)])*. In the program however there will be only the planet icosahedron generated and the ocean submesh will be generated later and only the parts needed.

\[Unity view icosahedron]

![Unity view icosahedron](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/Icosahedron.PNG)


- Then the triangles will be split and the uv's will be generated. Again: only for the planet and not for the ocean.

\[Splitting]

![splitting](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/dividing.gif)


- After that, a noise generates a heightmap.

\[Heightmap]

![Heightmap](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/heightmap.PNG)


- When the heightmap is applied you see why it would be a waste to generate the ocean with the same function as the planet. The ocean submesh holds only the needed vertices and is animated. The rest of the vertices is placed on the height the heightmap represents.

\[Apply Heightmap (Debug distance down to 0)]

![Apply Heightmap](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/apply%20heightmap.gif)


- The heightmap is now passed to a function that generates a texture. The look of the texture depends on the heights and the temperature variable that is set for the planet.   

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



- The animation menu allows to adjust the waves

\[Animation]

![Animation](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/waves.gif)


- The stars are only generated textures out of the lava texture applied to an ocean

\[Star]

![Star](https://github.com/Nice2Bee/Unity_Intro/blob/technique/technique/star%20waves.gif)



### Editor:

\[Editor control]
![Editor](https://github.com/Nice2Bee/Unity_Intro/blob/technique/screenshot_editor.PNG)


### Randomly generated:


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
