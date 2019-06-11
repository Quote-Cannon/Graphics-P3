using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using OpenTK;

namespace Template
{
	class SceneGraph
	{
		const float PI = 3.1415926535f;         // PI
		const float angle90degrees = PI / 2;
        public float xpos = 0, zpos = 0;
        Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
		public Matrix4 cameraMatrix = Matrix4.CreateTranslation(new Vector3(0, -14.5f, 0)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), angle90degrees);

        

		public void assign(Mesh Mesh, Mesh ParentMesh)
		{
			Mesh.parentMesh = ParentMesh;
		}
		public Matrix4 createMatrix(Mesh Mesh, float a)
		{
			Vector4 onevectorx = new Vector4(1, 0, 0, 0);
			Vector4 onevectory = new Vector4(0, 1, 0, 0);
			Vector4 onevectorz = new Vector4(0, 0, 1, 0);
			Vector4 onevectorw = new Vector4(0, 0, 0, 1);

			Matrix4 onematrix = new Matrix4(onevectorx, onevectory, onevectorz, onevectorw);
			Matrix4 angle, translation;
			if (Mesh.turn == new Vector3(0, 0, 0))
				angle = onematrix;
			else angle = Matrix4.CreateFromAxisAngle(Mesh.turn,a*Mesh.turningspeed);
			if (Mesh.translation == new Vector3(0, 0, 0))
				translation = onematrix;
			else translation = Matrix4.CreateTranslation(Mesh.translation);
			
			if (Mesh.parentMesh == null)
			{
				Mesh.toWorld = Mesh.modelView * translation * angle;
				return Mesh.modelView * translation * angle * cameraMatrix * Tview;
			}
			Mesh.toWorld = Mesh.modelView * translation * angle * Mesh.parentMesh.toWorld;
			return Mesh.modelView * angle * translation * createMatrix(Mesh.parentMesh, a*Mesh.turningspeed);
			
		}
	}
}
