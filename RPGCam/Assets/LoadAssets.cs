using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class NPC
{
    public string name;

    public Texture2D[] textures = new Texture2D[10];
    public GameObject obj;
    public RawImage rawImage;
}

public class Background
{
    public string name;

    public Texture2D texture;
    public GameObject obj;
    public RawImage rawImage;
}

public class LoadAssets : MonoBehaviour
{
    public GameObject pfNPC;
    public GameObject pfBackground;
    public TextureHandler textureHandlerNPC;
    public Renderer rendBackground;

    public Hider hider;

    string folderPath;
    string[] filePaths;
    public List<NPC> lstNPCs = new List<NPC>();
    public List<Background> lstBackgrounds = new List<Background>();

    public GameObject objTalkIndicator;

    public bool IsMuted;
    public bool IsNPCHidden;
    public bool IsDark;
    public bool IsBackgroundHidden;
    public bool IsFocus;

    float focus0 = 10;
    float focus1 = 2;

    public PostProcessVolume volume;
    //DepthOfField dof;

    public Light light;
    public Renderer rendBackgroundHider;
    public Renderer rendFlash;

    public Transform transParentNPC;
    public Transform transParentBackgrounds;


    string GetFileNameFromFullPath(string fullPath)
    {
        int lastPer = fullPath.LastIndexOf('/');
        int lastPer2 = fullPath.LastIndexOf("\\");

        if (lastPer2 > lastPer)
        {
            lastPer = lastPer2;
        }
        if (lastPer > 0)
        {
            return fullPath.Substring(lastPer + 1);
        }
        else
        {
            return fullPath;
        }
    }

    public void LoadBackgrounds()
    {
        foreach (Background background in lstBackgrounds)
        {
            GameObject.Destroy(background.obj);
        }
        lstBackgrounds.Clear();

        folderPath = Application.persistentDataPath + "//Backgrounds"; //Application.streamingAssetsPath;  //Get path of folder
        filePaths = Directory.GetFiles(folderPath, "*.jpg"); // Get all files of type .png in this folder

        foreach (string filePath in filePaths)
        {
            string fileName = GetFileNameFromFullPath(filePath);

            byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);

            Background background = new Background();
            lstBackgrounds.Add(background);
            background.name = fileName.Substring(0, (fileName.Length - 4));
            background.texture = tex;

            background.obj = GameObject.Instantiate(pfBackground);
            background.obj.transform.SetParent(transParentBackgrounds);
            background.obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, lstBackgrounds.IndexOf(background) * -70, 0);
            background.rawImage = background.obj.GetComponent<RawImage>();
            background.rawImage.texture = tex;

            TextMeshProUGUI txt = background.obj.transform.GetComponentInChildren<TextMeshProUGUI>();
            txt.text = background.name;
        }

        transParentBackgrounds.GetComponent<RectTransform>().sizeDelta = new Vector3(100, 70 * lstBackgrounds.Count);
    }

    public void LoadNPCs()
    {
        foreach (NPC npc in lstNPCs)
        {
            GameObject.Destroy(npc.obj);
        }
        lstNPCs.Clear();

        folderPath = Application.persistentDataPath + "//NPCs"; //Application.streamingAssetsPath;  //Get path of folder
        filePaths = Directory.GetFiles(folderPath, "*.png"); // Get all files of type .png in this folder

        foreach (string filePath in filePaths)
        {
            string fileName = GetFileNameFromFullPath(filePath);
            int i = fileName.LastIndexOf("_");
            string npcName = fileName.Substring(0, i);
            int no = int.Parse(fileName.Substring(i + 1, 1));
            bool nameFound = false;

            byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(pngBytes);

            foreach (NPC npc in lstNPCs)
            {
                if (npc.name == fileName.Substring(0, i))
                {
                    nameFound = true;
                    npc.textures[no] = tex;
                    break;
                }
            }

            if (!nameFound)
            {
                NPC npc = new NPC();
                npc.name = fileName.Substring(0, i);
                npc.textures[no] = tex;

                npc.obj = GameObject.Instantiate(pfNPC);
                npc.obj.transform.SetParent(transParentNPC);
                npc.obj.name = "btn" + npc.name;
                npc.rawImage = npc.obj.GetComponent<RawImage>();
                npc.rawImage.texture = tex;
                lstNPCs.Add(npc);
                npc.obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, lstNPCs.IndexOf(npc) * -110, 0);

                TextMeshProUGUI txt = npc.obj.transform.GetComponentInChildren<TextMeshProUGUI>();
                txt.text = npc.name;
            }
        }

        transParentNPC.GetComponent<RectTransform>().sizeDelta = new Vector3(100, 110 * lstNPCs.Count + 20);
    }

    // Start is called before the first frame update
    void Start()
    {
        hider = FindObjectOfType<Hider>();
        //volume.profile.TryGetSettings(out dof);

        LoadBackgrounds();
        LoadNPCs();
 
    }

        // Update is called once per frame
    void Update()
    {
        /*if (IsFocus)
        {
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance, focus1, Time.deltaTime);
        } else
        {
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance, focus0, Time.deltaTime);
        }*/
        if (IsDark)
        {
            light.intensity = Mathf.Lerp(light.intensity, 0, Time.deltaTime);
        }
        else
        {
            light.intensity = Mathf.Lerp(light.intensity, 1, Time.deltaTime);
        }
        if (IsBackgroundHidden)
        {
            rendBackgroundHider.material.color = Color.Lerp(rendBackgroundHider.material.color, Color.black, Time.deltaTime);
        }
        else
        {
            rendBackgroundHider.material.color = Color.Lerp(rendBackgroundHider.material.color, new Color(0, 0, 0, 0), Time.deltaTime);
        }
        textureHandlerNPC.transform.localScale = Vector3.Lerp(textureHandlerNPC.transform.localScale, Vector3.one, Time.deltaTime);
        rendFlash.material.color = Color.Lerp(rendFlash.material.color, new Color(rendFlash.material.color.r, rendFlash.material.color.g, rendFlash.material.color.b, 0), Time.deltaTime);
    }
}
