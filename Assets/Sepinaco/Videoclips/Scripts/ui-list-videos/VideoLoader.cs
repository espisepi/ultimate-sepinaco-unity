using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VideoLoader : MonoBehaviour
{
    private GameObject videoItemPrefab; // Prefab for displaying each video
    private Transform contentPanel; // UI parent where video items will be added

    void Awake()
    {
        // Create the UI Components
        CreateUIComponents();
    }

    void Start()
    {
        StartCoroutine(GetVideoDataCoroutine());
    }

    private void CreateUIComponents()
{
    // Create Canvas
    GameObject canvasObject = new GameObject("Canvas");
    Canvas canvas = canvasObject.AddComponent<Canvas>();
    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    canvasObject.AddComponent<CanvasScaler>();
    canvasObject.AddComponent<GraphicRaycaster>();

    // Create ScrollView and its components
    GameObject scrollViewObject = new GameObject("ScrollView", typeof(Image), typeof(ScrollRect));
    scrollViewObject.transform.SetParent(canvas.transform);
    scrollViewObject.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 600);
    ScrollRect scrollRect = scrollViewObject.GetComponent<ScrollRect>();

    // Create Scrollbar
    GameObject scrollbarObject = new GameObject("Scrollbar", typeof(Scrollbar));
    scrollbarObject.transform.SetParent(scrollViewObject.transform);
    scrollbarObject.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 600);
    scrollbarObject.GetComponent<Scrollbar>().direction = Scrollbar.Direction.BottomToTop;

    // Link Scrollbar to ScrollRect
    scrollRect.verticalScrollbar = scrollbarObject.GetComponent<Scrollbar>();

    // Create Content Panel for ScrollView
    GameObject contentObject = new GameObject("Content", typeof(RectTransform), typeof(VerticalLayoutGroup), typeof(ContentSizeFitter));
    contentPanel = contentObject.transform;
    contentPanel.SetParent(scrollViewObject.transform);
    contentPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 600);
    contentPanel.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    contentPanel.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
    contentPanel.GetComponent<VerticalLayoutGroup>().childControlWidth = true;
    scrollRect.content = (RectTransform)contentPanel;

    // Create Video Item Prefab
    videoItemPrefab = new GameObject("VideoItemPrefab", typeof(RectTransform), typeof(Image), typeof(Button));
    videoItemPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(380, 50);
    videoItemPrefab.GetComponent<Image>().color = Color.gray; // Color for visibility
    Text text = new GameObject("Text").AddComponent<Text>();
    text.transform.SetParent(videoItemPrefab.transform);
    text.rectTransform.sizeDelta = new Vector2(360, 30);
    text.alignment = TextAnchor.MiddleCenter;
    text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"); // Update to the correct font
    videoItemPrefab.SetActive(false); // Set prefab to inactive as it is just a template
}


    IEnumerator GetVideoDataCoroutine()
    {
        string apiUrl = "https://sepinaco.com:3000/media-list";
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching data: " + request.error);
            }
            else
            {
                ProcessVideos(request.downloadHandler.text);
            }
        }
    }

 void ProcessVideos(string jsonData)
    {
        VideoData videoData = JsonUtility.FromJson<VideoData>(jsonData);
        foreach (string videoFile in videoData.mediaFiles)
        {
            Debug.Log("OYEEEEEEEEEE" + videoFile);
            GameObject item = Instantiate(videoItemPrefab, contentPanel);
            item.SetActive(true);
            Text videoTitle = item.GetComponentInChildren<Text>();
            if (videoTitle != null)
            {
                videoTitle.text = videoFile;
            }
            Button videoButton = item.GetComponent<Button>();
            if (videoButton != null)
            {
                videoButton.onClick.AddListener(() => Debug.Log("Selected Video: " + videoFile));
            }
        }
    }

    [System.Serializable]
    public class VideoData
    {
        public string[] mediaFiles;
    }

}
