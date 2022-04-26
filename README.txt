Usage-
1. Once the code is loaded, click on Map Generator GameObject; you will be able to see a "Generate" button in the "Inspector" pane.
    Click on that to generate the terrain - this will generate one chunk of terrain sans shrubs
2. To view the infinite terrain click on the "Player" GameObject from the hierarchy pane and then click play
    This will launch the scripts necessary for FPS and Camera view required for this project.
    Click on Scene tab(CNTRL + 1); Zoom out entirely; Use the player GameObject and move it aroundl, you should see the chunks hiding and getting visible as the player moves along
    Click Game tab(CNTRL + 2); this is the FPS view of the terrain; use "WASD" keys to move the player around; 
    If needed you can increase the speed of the player from the Inspector pane (Walk Speed variable); recommended speed to see the endless terrain is 600(you can go upto 1000 but it will lag the game more);
    NOTE - The terrains are generated on demand so you will never reach the edge (unless the game lags), hence to view the on demand terrain generation, switch to "Scene" tab or use high walking speed
    *CAUTION - DONT GO NEAR THE SHRUB, THE PLAYER WILL DIE, THE SHRUBS ARE DEADLY*

IMPORTANT POINTS TO NOTE-
In the Scene mode, while moving the player, take care of the Y-coordinate values, sometimes the player falls off randomnly, if the values is decreasing rapidly and is below -0.06, then mode the player in positive Y direction i.e. upwards until you reach >=100 points on Y coordinate
Threading is used in this project to get a smooth movement and to load the terrain chunks smoothly.
Each thread calculates the map data and mesh data.
The game is lagging since the location of shrubs is being calculated in every thread.
To get a much smoother experience of the game (but without the shrubs), comment out "GameObject.Instantiate...." line and break from the loop (uncomment break;)


Important files-
1. Noise.cs - used to generate the NoiseMaps using Perlin noise; 5 bands of perlin noise i.e. 5 octaves are used in this project.
2. MapGenerator.cs - used to start the main thread and delegate the work on the mapData thread and meshData thread. Defines the global variables like seed, persistance, lacunarity, etc., regions and color of each region used on the map
3. InfiniteTerrain.cs - used to define the work of mapData and meshData thread. level of detail of each chunk is defined in this file and depending on the viewers position the LOD of chunk is updated along with which chunks will be visible to the viewer.
4. MeshGenerator.cs - used to generate the mesh (triangles, vertices, uvs, shrubs) from the map data (height map and color map). This is the code which is responsible to handle seams of the terrain chunks, calculates the normals and makes the union of chunks seamless and smooth
5. PlayerController.cs - used to handle FPS mode and collisions of the player on the terrain.
6. TextureGenerator.cs - used to generate textures from height and color map data

Very Basic Structure/Flow-
Generate button -> MapGenerator -> MapDataThread -> GenerateMapData -> GenerateNoiseMap 
                                -> MeshDataThread -> GenerateTerrainMesh   
                                -> DisplayMap

Things implemented-
1. 5 bands of perlin Noise
2. Smooth and seamless join of terrain chunks
3. Color map generated using height as the parameter for regions
4. On demand terrains generated (infinite terrain system, can never fall off the edge)
5. Simple plats (shrubs) placed at random points of map
6. WASD camera movement in FPS mode
7. Camera movement using player GameObject
8. Terrains visible only if within the visible range of the viewer (can be verified through "scene" tab) and different LOD for each terrain chunk according to their visibility
9. Shrub prefab used to generate shrubs on the terrain

Future scope-
1. Remove lag introduce by addition of shrubs
2. Create realistic view of the terrain
3. Add fishes in the sea/ocean

NOTE ON LAGGING-
The game lags alot due to extensive calculation involving shrubs location. Kindly be patient, the results will be generated (specially the on demand terrain generation)