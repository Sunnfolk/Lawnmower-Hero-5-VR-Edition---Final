using UnityEngine;

public class CalculateCutGrass : MonoBehaviour
{
    [Header("Cutting Grass")]
        public RenderTexture renderTexture;
        private Texture2D newTexture2D;
        
        private float startPixels;
        private float endPixels;

        private int renderTextureWidth;
        private int renderTextureHeight;
        
        [Space(5)]
        [Header("Setting Score")]
        public float grassScore;
        private Timer _timer;
        private bool _canScore;
        private scoreManager _scoreManager;

        private void Awake()
        {
            newTexture2D = ToTexture2D(renderTexture);
            _timer = GetComponent<Timer>();
            _scoreManager = GetComponent<scoreManager>();
            _canScore = true;
            
            renderTextureWidth = renderTexture.width;
            renderTextureHeight = renderTexture.height;
        }

        Texture2D ToTexture2D(RenderTexture rTex)
        {
            //Make sure this never runs in update, or your performance will tank
            print("If this message is being spammed, you are doing something wrong");
            Texture2D tex = new Texture2D(renderTextureWidth, renderTextureHeight, TextureFormat.RGB24, false);
            // ReadPixels looks at the active RenderTexture.
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }

        float readPixels(Texture2D tex, Color clr)
        {
            var pixels = 0f;
            
            for (int x = 0; x < tex.width; x++)
            for (int y = 0; y < tex.height; y++)
                if (!tex.GetPixel(x, y).Equals(clr))
                    pixels++;

            return pixels;
        }

        float CalculatePixels()
        {
            //print("CALCULATING SHIT NOW");
            //return (startPixels / endPixels * 100f) -100f;
            //print("Start Pixels: " + startPixels);
            print("End Pixels: " + endPixels);
            print("Return value:" + ((100f - (endPixels / startPixels * 100f))*10));
            //return 100f - (endPixels / startPixels * 100f); old one that gave lower number instead of higher number for better cut job
            return (100f - (endPixels / startPixels * 100f))*10;
        }
   
        private void Update()
        {
            /*
            // Live Update
            newTexture2D = ToTexture2D(renderTexture);
            endPixels = readPixels(newTexture2D, Color.black);
            */

            if (startPixels == 0)
            {
                print("Startpixels = 0, That This should only run once i think");
                newTexture2D = ToTexture2D(renderTexture);
                startPixels = readPixels(newTexture2D, Color.black);
            }

            if (!_timer.timerIsRunning && _canScore)
            { 
                print("If this triggers then I hope you're having a wonderful day.");
                //newTexture2D = ToTexture2D(renderTexture);
                
                newTexture2D = ToTexture2D(renderTexture);
                endPixels = readPixels(newTexture2D, Color.black);
                grassScore = CalculatePixels();
                _canScore = false;
            }
        }
        
        
        float ReadTexture2DPixelsNotColor(Texture2D tex, Color clr)
        {
            var totalPixels = tex.width * tex.height;
            var pixels = 0f;
            
            for (int x = 0; x < tex.width; x++)
            for (int y = 0; y < tex.height; y++)
                if (!tex.GetPixel(x, y).Equals(clr))
                    pixels++;
            
            // return (totalPixels / pixels * 100f) -100f;
            return (pixels / totalPixels * 100f);
        }
}