using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class ScriptVideoclip : MonoBehaviour
{
    public Material videoMaterial; // Asigna aquí el material del video

    // AVISO: !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    // Ruta al video dentro de los Assets del proyecto
    // Ruta donde poner los videos:
    // MiNombreProyecto/Assets/StreamingAssets/Sepinaco/Videoclips/DELAOSSA - JUDAS (PROD. JMOODS).mp4
    // Ruta para poner aqui en el codigo:
    // Sepinaco/Videoclips/DELAOSSA - JUDAS (PROD. JMOODS).mp4
    public string[] videoPaths = {
        "Sepinaco/Videoclips/DELAOSSA - JUDAS (PROD. JMOODS).mp4",
        "Sepinaco/Videoclips/Soto Asa y La Zowi - Smartphone   GALLERY SESSION.mp4"
    };

    private VideoPlayer videoPlayer;

    void Start()
    {
        ReplaceSceneMaterials(videoMaterial);
        StartVideoPlayer();
        InitializeVideoObjects();
    }

    void ReplaceSceneMaterials(Material newMaterial)
    {
        Renderer[] renderers = FindObjectsOfType<Renderer>(); // Encuentra todos los objetos con componente Renderer en la escena
        foreach (Renderer renderer in renderers)
        {
            Material[] mats = new Material[renderer.sharedMaterials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = newMaterial; // Reemplaza cada material por el material de video
            }
            renderer.sharedMaterials = mats; // Asigna el nuevo arreglo de materiales al renderer
        }
    }

    void StartVideoPlayer() {
        // Obtén el componente VideoPlayer ya existente en este GameObject
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("No se encontró el componente VideoPlayer en el GameObject.");
            return;
        }

        // Configura el VideoPlayer
        // videoPlayer.playOnAwake = false;
        // videoPlayer.renderMode = VideoRenderMode.MaterialOverride;
        // videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
        // videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        // videoPlayer.SetTargetAudioSource(0, GetComponent<AudioSource>());

        // Establece el clip de video
        SetVideoClip(videoPaths[0]);

        // Opcional: Reproducir automáticamente
        videoPlayer.Play();
    }

    void SetVideoClip(string path)
    {
        // https://docs.unity3d.com/Manual/StreamingAssets.html
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, path);
    }

    void InitializeVideoObjects() {
        MeshRenderer parentRenderer = GetComponent<MeshRenderer>();
        MeshFilter parentFilter = GetComponent<MeshFilter>();
        for (int i = 0; i < videoPaths.Length; i++)
        {
            // Crea un nuevo GameObject para cada video
            GameObject videoObject = new GameObject(videoPaths[i]);
            videoObject.transform.SetParent(transform); // Establece el GameObject padre
            videoObject.transform.localPosition = Vector3.right * i * 2.0f; // Ajusta la posición relativa según sea necesario

            // Copia los componentes de renderizado del padre al hijo
            if (parentRenderer != null && parentFilter != null)
            {
                MeshRenderer childRenderer = videoObject.AddComponent<MeshRenderer>();
                MeshFilter childFilter = videoObject.AddComponent<MeshFilter>();
                childRenderer.material = parentRenderer.material;
                childFilter.mesh = parentFilter.mesh;
            }

            // Añade un Collider para detectar colisiones
            BoxCollider collider = videoObject.AddComponent<BoxCollider>();
            collider.isTrigger = true; // Hacer el collider un trigger si quieres evitar físicas reales

            // Añade el script de manejo de trigger a cada hijo
            videoObject.AddComponent<ChildTriggerHandler>();

            // Añade y configura el script VideoTrigger
            // VideoTrigger trigger = videoObject.AddComponent<VideoTrigger>();
            // trigger.videoPath = videoPaths[i];
            // trigger.videoPlayer = videoPlayer;
        }
    }

     // Esta función será llamada cuando ocurra una colisión con un trigger
    private void OnTriggerEnter(Collider other)
    {
        // string urlVideo = other.name;
        // SetVideoClip(urlVideo);
        // Debug.Log("Colisiono con el objeto: " + urlVideo);
        // VideoTrigger trigger = other.GetComponent<VideoTrigger>();
        // if (trigger != null)
        // {
        //     trigger.PlayVideo();
        // }
    }

    // Este método será llamado por los hijos cuando detecten una colisión
    public void ChildCollided(GameObject child, Collider other)
    {
        Debug.Log(child.name + " colisionó con " + other.gameObject.name);
        string urlVideo = child.name;
        SetVideoClip(urlVideo);
        // Aquí puedes añadir más lógica basada en la colisión
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}




// OLD CODE
/*
    // public RenderTexture videoTexture; // Asigna aquí tu Render Texture


    // Start is called before the first frame update
    void Start()
    {
        ReplaceSceneMaterials(videoMaterial);
        // Material videoMaterial = new Material(Shader.Find("Unlit/Texture"));
        // videoMaterial.mainTexture = videoTexture; // Usa la Render Texture como la textura principal del material
        // ReplaceSceneMaterials(videoMaterial);
    }
*/