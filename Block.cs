using StereoKit;

class Block {
	Tex texture;
	Material material = Default.Material.Copy();
	Material cursorMaterial;
	static float cursorOpacity = 0.25f;

	public Material Material { get{ return material; } }
	public Material CursorMaterial { get { return cursorMaterial; } }

	public Block(string texturePath) {
		texture = Tex.FromFile(texturePath, false);
		texture.SampleMode = TexSample.Point;
		material["diffuse"] = texture;

		cursorMaterial = material.Copy();
		cursorMaterial.Transparency = Transparency.Blend;
		cursorMaterial["color"] = new Color(1, 1, 1, cursorOpacity);
		cursorMaterial.DepthTest = DepthTest.Always;
		cursorMaterial.DepthWrite = false;
	}
}
