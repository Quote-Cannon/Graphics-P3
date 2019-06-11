using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;


namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;                  // background surface for printing etc.
		Mesh mesh, floor, meshI, meshII, meshIII;                       // a mesh to draw using OpenGL
		const float PI = 3.1415926535f;         // PI
		float a = 0;                            // teapot rotation angle
		Stopwatch timer;                        // timer for measuring frame duration
		Shader shader;                          // shader to use for rendering
		Texture wood;                           // texture to use for rendering
		SceneGraph sceneGraph;

		// initialize
		public void Init()
		{

			// load teapot
			mesh = new Mesh( "../../assets/teapot.obj" );
			meshI = new Mesh("../../assets/teapot.obj");
			meshII = new Mesh("../../assets/teapot.obj");
			meshIII = new Mesh("../../assets/teapot.obj");
			floor = new Mesh( "../../assets/floor.obj" );
			// initialize stopwatch
			timer = new Stopwatch();
			timer.Reset();
			timer.Start();
			// create shaders
			shader = new Shader( "../../shaders/vs.glsl", "../../shaders/fs.glsl" );
			// load a texture
			wood = new Texture( "../../assets/wood.jpg" );
			sceneGraph = new SceneGraph();
			sceneGraph.assign(mesh, floor);
			sceneGraph.assign(meshI, mesh);
			sceneGraph.assign(meshII, meshI);
			sceneGraph.assign(meshIII, meshII);
			floor.modelView = Matrix4.CreateScale(4.0f);
			mesh.modelView = Matrix4.CreateScale(0.1f);
			meshI.modelView = Matrix4.CreateScale(0.8f);
			meshI.turn = new Vector3(0, 1, 0);
			meshI.translation = new Vector3(0, 0f, 20f);
			meshII.modelView = Matrix4.CreateScale(0.8f);
			meshII.turn = new Vector3(0, 1, 0);
			meshII.translation = new Vector3(0, 0f, 20f);
			meshIII.modelView = Matrix4.CreateScale(0.8f);
			meshIII.turn = new Vector3(0, 1, 0);
			meshIII.translation = new Vector3(0, 0f, 20f);
			floor.turn = new Vector3(0, 1, 0);
			floor.translation = new Vector3(0f, 0f, 0);
			mesh.translation = new Vector3(0f, 0f, 0f);
			mesh.turn = new Vector3(0, 1, 0);
			// set the light
			int lightID = GL.GetUniformLocation(shader.programID, "lightPos");
			GL.UseProgram(shader.programID);
			GL.Uniform3(lightID, 0.0f, 0.0f, 0.0f);

		}


		// tick for background surface
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello world", 2, 2, 0xffff00 );
		}

		// tick for OpenGL rendering code
		public void RenderGL()
		{
			// measure frame duration
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();

			// prepare matrix for vertex shader
			float angle90degrees = PI / 2;
			//Matrix4 Tpot = sceneGraph.createMatrix(mesh);           //Matrix4.CreateScale( 0.5f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			//Matrix4 Tfloor = sceneGraph.createMatrix(floor);        //Matrix4.CreateScale( 4.0f ) * Matrix4.CreateFromAxisAngle( new Vector3( 0, 1, 0 ), a );
			//Matrix4 Tcamera = sceneGraph.cameraMatrix;			//Matrix4.CreateTranslation( new Vector3( 0, -14.5f, 0 ) ) * Matrix4.CreateFromAxisAngle( new Vector3( 1, 0, 0 ), angle90degrees );
			//Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView( 1.2f, 1.3f, .1f, 1000 );

			// update rotation
			a += 0.001f * frameDuration;
			if( a > 2 * PI ) a -= 2 * PI;

			// render scene
			mesh.Render( shader, sceneGraph.createMatrix(mesh,a), mesh.toWorld, wood );
			floor.Render( shader, sceneGraph.createMatrix(floor,a), floor.toWorld, wood );
			meshI.Render(shader, sceneGraph.createMatrix(meshI, a), meshI.toWorld, wood);
			meshII.Render(shader, sceneGraph.createMatrix(meshII, a), meshII.toWorld, wood);
			meshIII.Render(shader, sceneGraph.createMatrix(meshIII, a), meshIII.toWorld, wood);
			//mesh.Render(shader, Tpot * Tcamera * Tview, wood);
			//floor.Render(shader, Tfloor * Tcamera * Tview, wood);
		}
	}
}