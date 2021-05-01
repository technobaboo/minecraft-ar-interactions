using StereoKit;
using System;

class BlockDraw {
	public static float blockSize = 0.05f;
	Vec3 origin {get {return -new Vec3(bounds[0], 0, bounds[2]) * blockSize / 2;}}
	static int[] bounds = new int[3]{16, 256, 16};
	Block[,,] blocks = new Block[bounds[0], bounds[1], bounds[2]];

	static Mesh cubeMesh = Mesh.GenerateCube(Vec3.One);

	public void Step() {
		DrawBounds();
		for(int x = 0; x<bounds[0]; x++) {
			for(int y = 0; y<bounds[1]; y++) {
				for(int z = 0; z<bounds[2]; z++) {
					Block block = blocks[x,y,z];
					if(block != null) {
						cubeMesh.Draw(block.Material, blockPosMatrix(x,y,z));
					}
				}
			}
		}
		// cubeModel.Draw(Matrix.T(origin+()));
	}

	private void DrawBounds() {
		Vec3 XP = Vec3.Right * bounds[0] * blockSize;
		Vec3 YP = Vec3.Up * bounds[1] * blockSize;
		Vec3 ZP = -Vec3.Forward * bounds[2] * blockSize;

		Vec3 XYP = XP + YP;
		Vec3 XZP = XP + ZP;
		Vec3 ZYP = ZP + YP;

		Vec3 XYZP = XP + YP + ZP;

		// Bottom Square
		Lines.Add(origin, origin + XP, Color32.White, 0.01f);
		Lines.Add(origin, origin + ZP, Color32.White, 0.01f);
		Lines.Add(origin + XP, origin + XZP, Color32.White, 0.01f);
		Lines.Add(origin + ZP, origin + XZP, Color32.White, 0.01f);

		// Vertical Lines
		Lines.Add(origin, origin + YP, Color32.White, 0.01f);
		Lines.Add(origin + XP, origin + XYP, Color32.White, 0.01f);
		Lines.Add(origin + ZP, origin + ZYP, Color32.White, 0.01f);
		Lines.Add(origin + XZP, origin + XYZP, Color32.White, 0.01f);

		// Top Square
		Lines.Add(origin + YP, origin + XYP, Color32.White, 0.01f);
		Lines.Add(origin + XYP, origin + XYZP, Color32.White, 0.01f);
		Lines.Add(origin + XYZP, origin + ZYP, Color32.White, 0.01f);
		Lines.Add(origin + ZYP, origin + YP, Color32.White, 0.01f);
	}

	public void AddBlock(Block block, int[] coords) {
		blocks[coords[0],coords[1],coords[2]] = block;
	}
	public void AddBlock(Block block, Vec3 pos) {
		int[] blockPos = WorldToBlockPos(pos);
		if(blockPos != null)
			AddBlock(block, blockPos);
	}

	public void DrawCursor(Block block, Vec3 handPos) {
		if(block == null)
			return;
		int[] blockPos = WorldToBlockPos(handPos);
		if(blockPos == null)
			return;
		cubeMesh.Draw(block.CursorMaterial, blockPosMatrix(blockPos[0], blockPos[1], blockPos[2]));
	}

	public int[] WorldToBlockPos(Vec3 pos) {
		Vec3 adjustedPos = pos - origin;
		adjustedPos /= blockSize;
		int[] blockPos = new int[3] {
			(int) Math.Floor(adjustedPos.x),
			(int) Math.Floor(adjustedPos.y),
			(int) Math.Floor(adjustedPos.z)
		};
		if (blockPos[0] < 0 || blockPos[0] >= bounds[0])
			return null;
		if (blockPos[1] < 0 || blockPos[1] >= bounds[1])
			return null;
		if (blockPos[2] < 0 || blockPos[2] >= bounds[2])
			return null;
		return blockPos;
	}
	public Vec3 BlockToWorldPos(int[] blockPos) {
		Vec3 worldPos = new Vec3(blockPos[0] + 0.5f, blockPos[1] + 0.5f, blockPos[2] + 0.5f) * blockSize;
		return origin + worldPos;
	}

	public bool IsBlockPresent(Vec3 worldPos) {
		int[] blockPos = WorldToBlockPos(worldPos);
		return blockPos != null && blocks[blockPos[0], blockPos[1], blockPos[2]] != null;
	}
	public bool IsBlockPresent(int[] blockPos) {
		return blocks[blockPos[0], blockPos[1], blockPos[2]] != null;
	}

	private Matrix blockPosMatrix(int x, int y, int z) {
		return Matrix.TS(BlockToWorldPos(new int[3]{x, y, z}), blockSize);
	}
}
