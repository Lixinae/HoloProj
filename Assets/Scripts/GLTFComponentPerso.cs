using HoloToolkit.Unity;
using System.Collections;
using System.IO;
using UnityEngine;

using UnityGLTF;

public class GLTFComponentPerso : Singleton<GLTFComponentPerso> {
    public string Url = "";
    public bool Multithreaded = false;
    public bool UseStream = true;

    public int MaximumLod = 300;

    public Shader GLTFStandard;
    public Shader GLTFStandardSpecular;
    public Shader GLTFConstant;

    public bool addColliders = true;

    public Stream GLTFStream = null;

    public bool IsLoaded { get; set; }

    private void Awake() {
        GLTFStandard = Shader.Find("GLTF/GLTFStandard");
        GLTFStandardSpecular = Shader.Find("GLTF/GLTFStandard");
        GLTFConstant = Shader.Find("GLTF/GLTFConstant");
    }


    public IEnumerator CreateComponentFromFile(string filename, GameObject parent) {
        GLTFSceneImporter loader = null;
        Url = filename;
        if (UseStream) {
            string fullPath = "";

            if (GLTFStream == null) {
                fullPath = Path.Combine(Application.streamingAssetsPath, Url);
                GLTFStream = File.OpenRead(fullPath);
            }
            loader = new GLTFSceneImporter(
                fullPath,
                GLTFStream,
                parent.transform,
                addColliders
                );
        }
        else {
            loader = new GLTFSceneImporter(
                Url,
                parent.transform,
                addColliders
                );
        }
        loader.SetShaderForMaterialType(GLTFSceneImporter.MaterialType.PbrMetallicRoughness, GLTFStandard);
        loader.SetShaderForMaterialType(GLTFSceneImporter.MaterialType.KHR_materials_pbrSpecularGlossiness, GLTFStandardSpecular);
        loader.SetShaderForMaterialType(GLTFSceneImporter.MaterialType.CommonConstant, GLTFConstant);
        loader.MaximumLod = MaximumLod;
        yield return loader.Load(-1, Multithreaded);
        if (GLTFStream != null) {
#if WINDOWS_UWP
            GLTFStream.Dispose();
#else
            GLTFStream.Close();
#endif

            GLTFStream = null;
        }

        IsLoaded = true;
    }


    public IEnumerator WaitForModelLoad() {
        while (!IsLoaded) {
            yield return null;
        }
    }
}
