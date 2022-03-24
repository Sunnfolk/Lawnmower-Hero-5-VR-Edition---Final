using UnityEngine;

public class CalculateCutGrass : MonoBehaviour
{
    [Header("Cutting Grass")]
        public RenderTexture renderTexture;
        private Texture2D newTexture2D;
        
        private float startPixels;
        private float endPixels;
        
        
        [Space(5)]
        [Header("Setting Score")]
        public float grassScore;
        public transformVariable trans;
        private Timer _timer;
        private bool _canScore;
        private scoreManager _scoreManager;

        private void Awake()
        {
            newTexture2D = ToTexture2D(renderTexture);
            _timer = GetComponent<Timer>();
            _scoreManager = GetComponent<scoreManager>();
            _canScore = true;
        }

        Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
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
            //return (startPixels / endPixels * 100f) -100f;
            return 100f - (endPixels / startPixels * 100f);
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
                newTexture2D = ToTexture2D(renderTexture);
                startPixels = readPixels(newTexture2D, Color.black);
            }

            if (!_timer.timerIsRunning && _canScore)
            { 
                newTexture2D = ToTexture2D(renderTexture);
               
                endPixels = readPixels(newTexture2D, Color.black);
                grassScore = CalculatePixels();
           
                trans.score2 += grassScore * _scoreManager.grassPoints;
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