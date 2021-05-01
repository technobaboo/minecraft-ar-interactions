using System;
using System.Collections.Generic;
using StereoKit;
using StereoKit.Framework;

class Program {
	static Dictionary<string, Block> blockDict = new Dictionary<string, Block> {
		{"air", null},
		{"bedrock", new Block("Assets/bedrock.png")},
		{"oak_planks", new Block("Assets/oak_planks.png")},
		{"obsidian", new Block("Assets/obsidian.png")},
		{"snow", new Block("Assets/snow.png")},
		{"stone", new Block("Assets/stone.png")},
	};

	static BlockDraw blocks = new BlockDraw();
	static Block selectedBlock = blockDict["bedrock"];
	static Pose windowPose = new Pose(0, 0.5f, 0, Quat.LookDir(1, 0, 0));

	static void BlocksWindow() {
		UI.WindowBegin("Block Picker", ref windowPose, new Vec2(20, 0) * U.cm, UIWin.Normal);
		UI.HSlider("blockSizeSlider", ref BlockDraw.blockSize, 0.025f, 0.1f, 0.005f, 0.18f);
		foreach(KeyValuePair<string, Block> block in blockDict) {
			if(UI.Button(block.Key)) {
				selectedBlock = block.Value;
			}
		}
		UI.WindowEnd();
	}

	static void Main(string[] args) {
		if (!SK.Initialize(new SKSettings{ appName = "Minecraft AR Test" }))
			Environment.Exit(1);
		Renderer.EnableSky = false;

		while(SK.Step(() => {
			Hand rightHand = Input.Hand(Handed.Right);
			if(rightHand.pinch.IsActive() && !UI.IsInteracting(Handed.Right))
				blocks.AddBlock(selectedBlock, rightHand.Get(1, 4).position);

			blocks.Step();

			if(!UI.IsInteracting(Handed.Right))
				blocks.DrawCursor(selectedBlock, rightHand.Get(1, 4).position);
			BlocksWindow();
		}));
		SK.Shutdown();
	}
}
