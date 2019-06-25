using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Input;
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
		Texture wood, horde;                    // texture to use for rendering
		SceneGraph sceneGraph;
        Light light1, light2;
        Matrix4 zmin, xplus, xmin, CW, CCW;
        Matrix4 identitycamera;
        float x = 1, y = 1, z = 1, rotation = 1;

        // initialize
        public void Init()
		{
			// load teapot
			mesh = new Mesh( "../../assets/table.obj" );
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
			horde = new Texture( "../../assets/horde.jpg" );
			sceneGraph = new SceneGraph();
			sceneGraph.assign(mesh, floor);
			sceneGraph.assign(meshI, mesh);
			sceneGraph.assign(meshII, meshI);
			sceneGraph.assign(meshIII, meshII);
			floor.modelView = Matrix4.CreateScale(4.0f);
			mesh.modelView = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)Math.PI) * Matrix4.CreateScale(0.1f);
			meshI.modelView = Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), (float)Math.PI) * Matrix4.CreateScale(0.5f);
			//mesh.
			meshI.turn = new Vector3(0, 1, 0);
			meshI.translation = new Vector3(0, 0f, 20f);
			meshII.modelView = Matrix4.CreateScale(0.8f);
			meshII.turn = new Vector3(0, 1, 0);
			meshII.translation = new Vector3(0, 0f, 20f);
			meshIII.modelView = Matrix4.CreateScale(0.8f);
			meshIII.turn = new Vector3(0, 1, 0);
			meshIII.translation = new Vector3(0, 0f, 20f);
			//floor.turn = new Vector3(0, 1, 0);
			floor.translation = new Vector3(0f, 0f, 0);
			mesh.translation = new Vector3(0f, 0f, 0f);
			mesh.turn = new Vector3(0, 0, 0);
            // set the light
            light1 = new Light(shader, new Vector3(0.0f, 5f, 15.0f), 0);
            light2  = new Light(shader, new Vector3(5.0f, 10.0f, 0.0f), 0);
            //define modifiers
            zmin = Matrix4.CreateTranslation(new Vector3(0, 0, -0.2f));
            xplus = Matrix4.CreateTranslation(new Vector3(0.2f, 0, 0));
            xmin = Matrix4.CreateTranslation(new Vector3(-0.2f, 0, 0));
            CW = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), -0.1f);
            CCW = Matrix4.CreateFromAxisAngle(new Vector3(0, 1, 0), 0.1f);
            identitycamera = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), (float)Math.PI/2);
        }


		// tick for background surface
		public void Tick()
		{
			screen.Clear( 0 );
			screen.Print( "hello there", 2, 2, 0xffff00 );
            screen.Print("general kenobi", 2, 12, 0xffff00);
            sceneGraph.cameraMatrix = identitycamera * Matrix4.CreateFromAxisAngle(new Vector3(0, 0, 1), rotation) * Matrix4.CreateTranslation(new Vector3(x, y, z));
            //TODO: Multiply translations with a variable that stores the rotated angle
            // implement controls
            if (y > -7f)
            {
                if (Keyboard.GetState()[Key.W])
                {
                    y -= 0.1f;
                    Console.WriteLine("Y: " + y);
                }
            }
            else y = -7f;
            if (y < 7f)
            {
                if (Keyboard.GetState()[Key.S])
                {
                    y += 0.1f;
                    Console.WriteLine("Y: " + y);
                }
            }
            else y = 7f;
            if (x < 9f)
            {
                if (Keyboard.GetState()[Key.A])
                {
                    x += 0.1f;
                    Console.WriteLine("X: " + x);
                }
            }
            else x = 9f;
            if (x > -9f)
            {
                if (Keyboard.GetState()[Key.D])
                {
                    x -= 0.1f;
                    Console.WriteLine("X: " + x);
                }
            }
            else x = -9f;
            if (Keyboard.GetState()[Key.Q])
            {
                rotation += 0.05f;
            }
            if (Keyboard.GetState()[Key.E])
            {
                rotation -= 0.05f;
            }
            if (z < 22f)
            {
                if (Keyboard.GetState()[Key.ShiftLeft])
                {
                    z+= 0.1f;
                    Console.WriteLine("Z: " + z);
                }
            }
            else z = 22f;
            if (Keyboard.GetState()[Key.Space])
            {
                z -= 0.1f;
                Console.WriteLine("Z: " + z);
            }
        }

		// tick for OpenGL rendering code
		public void RenderGL()
		{
			// measure frame duration
			float frameDuration = timer.ElapsedMilliseconds;
			timer.Reset();
			timer.Start();

			// update rotation
			a += 0.001f * frameDuration;
			if( a > 2 * PI ) a -= 2 * PI;

            // render scene
            floor.Render( shader, sceneGraph.createMatrix(floor,a), floor.toWorld, wood );
			mesh.Render( shader, sceneGraph.createMatrix(mesh,a), mesh.toWorld, horde );
			meshI.Render(shader, sceneGraph.createMatrix(meshI, a), meshI.toWorld, wood);
			meshII.Render(shader, sceneGraph.createMatrix(meshII, a), meshII.toWorld, wood);
			meshIII.Render(shader, sceneGraph.createMatrix(meshIII, a), meshIII.toWorld, wood);
			//mesh.Render(shader, Tpot * Tcamera * Tview, wood);
			//floor.Render(shader, Tfloor * Tcamera * Tview, wood);
		}
	}

    class Light
    {
        public Light(Shader s, Vector3 color, int posMod)
        {
            int lightID = GL.GetUniformLocation(s.programID, "lightPos");
            GL.UseProgram(s.programID);
            GL.Uniform3(lightID, 5.0f, 5f, 0.0f);
        }
    }
}