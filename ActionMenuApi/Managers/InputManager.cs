using ActionMenuApi.Helpers;
using MelonLoader;
using UnityEngine;
using UnityEngine.XR;

namespace ActionMenuApi.Managers
{
    internal static class InputManager
    {
        private static Vector2 mouseAxis;
        public static Vector2 LeftInput
        {
            get
            {
                if (XRDevice.isPresent) 
                    return new Vector2(Input.GetAxis(Constants.LEFT_HORIZONTAL), Input.GetAxis(Constants.LEFT_VERTICAL)) * 16;
                mouseAxis.x = Mathf.Clamp(mouseAxis.x+Input.GetAxis("Mouse X"), -16f, 16f);
                mouseAxis.y = Mathf.Clamp(mouseAxis.y+Input.GetAxis("Mouse Y"), -16f, 16f);
                var translatedHit = Utilities.GetIntersection(mouseAxis.x, mouseAxis.y, Mathf.Max(Mathf.Abs(mouseAxis.x),Mathf.Abs(mouseAxis.y)));
                if (translatedHit.x1 > 0 && mouseAxis.x > 0)
                    return new Vector2((float)translatedHit.x1, (float)translatedHit.y1);
                return new Vector2((float)translatedHit.x2, (float)translatedHit.y2);;
            }
        }
        public static Vector2 RightInput
        {
            get
            {
                if (XRDevice.isPresent) 
                    return new Vector2(Input.GetAxis(Constants.RIGHT_HORIZONTAL), Input.GetAxis(Constants.RIGHT_VERTICAL)) * 16;
                mouseAxis.x = Mathf.Clamp(mouseAxis.x+Input.GetAxis("Mouse X"), -16f, 16f);
                mouseAxis.y = Mathf.Clamp(mouseAxis.y+Input.GetAxis("Mouse Y"), -16f, 16f);
                var translatedHit = Utilities.GetIntersection(mouseAxis.x, mouseAxis.y, Mathf.Max(Mathf.Abs(mouseAxis.x),Mathf.Abs(mouseAxis.y)));
                if (translatedHit.x1 > 0 && mouseAxis.x > 0)
                    return new Vector2((float)translatedHit.x1, (float)translatedHit.y1);
                return new Vector2((float)translatedHit.x2, (float)translatedHit.y2);;
            }
        }

        public static void ResetMousePos()
        {
            mouseAxis.x = 0f;
            mouseAxis.y = 0f;
        }
        
    }
}